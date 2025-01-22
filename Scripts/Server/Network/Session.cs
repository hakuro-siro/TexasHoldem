using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerCore
{
    public abstract class PacketSession : Session
    {
        public static readonly int HeaderSize = 2;

        // sealed 다른 클래스가 PacketSession을 상속받고 OnRecv를 재정의할 수 없게 함
        public sealed override int OnRecv(ArraySegment<byte> buffer)
        {
            int processLen = 0;
            int packetCount = 0;

            while (true)
            {
                // 최소한 헤더는 파싱할 수 있는지 확인
                if (buffer.Count < HeaderSize)
                {
                    break;
                }

                // 패킷이 완전체로 도착했는지 확인
                // 헤더에서 나오는 크기 확인
                ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
                if (buffer.Count < dataSize)
                {
                    break;
                }

                // 여기까지 왔으면 패킷 조립 가능
                OnRecvPacket(new ArraySegment<byte>(buffer.Array, buffer.Offset, dataSize));
                packetCount++;

                processLen += dataSize;
                buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize);
            }

            if (packetCount > 1)
            {
                Console.WriteLine($"패킷 모아보내기 : {packetCount}");
            }

            return processLen;
        }

        public abstract void OnRecvPacket(ArraySegment<byte> buffer);
    }

    public abstract class Session
    {
        Socket _socket;
        int _disconnected = 0;

        RecvBuffer _recvBuffer = new RecvBuffer(65535);

        object _lock = new object();
        Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>();
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
        SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();

        public abstract void OnConnected(EndPoint endPoint);
        public abstract int OnRecv(ArraySegment<byte> buffer);
        public abstract void OnSend(int numOfBytes);
        public abstract void OnDisconnected(EndPoint endPoint);

        void Clear()
        {
            lock (_lock)
            {
                _sendQueue.Clear();
                _pendingList.Clear();
            }
        }

        public void Start(Socket socket)
        {
            _socket = socket;
            _recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);
            _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);

            RegisterRecv();
        }

        public void Send(List<ArraySegment<byte>> sendBuffList)
        {
            if (sendBuffList.Count == 0)
            {
                return;
            }

            lock (_lock)
            {
                foreach (ArraySegment<byte> sendBuff in sendBuffList)
                {
                    _sendQueue.Enqueue(sendBuff);
                }
                // Send하는 중인얘가 없어서 내가 Send를 호출할거다
                if (_pendingList.Count == 0)
                {
                    RegisterSend();
                }
            }
        }

        public void Send(ArraySegment<byte> sendBuff)
        {
            lock (_lock)
            {
                _sendQueue.Enqueue(sendBuff);
                // Send하는 중인얘가 없어서 내가 Send를 호출할거다
                if (_pendingList.Count == 0)
                {
                    RegisterSend();
                }
            }
        }

        public void Disconnect()
        {
            // Disconnect를 두번하면 안되기때문에 Interlocked를 사용한 멀티쓰레드까지 고려한
            // 연산으로 처리한다.
            if(Interlocked.Exchange(ref _disconnected, 1) == 1)
            {
                return;
            }
            OnDisconnected(_socket.RemoteEndPoint);
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            Clear();
        }

        #region 네트워크 통신

        // RegisterSend를 호출하는 Send함수가 lock처리가 되어있기 때문에 얘는
        // lock을 안걸어줘도 된다.
        void RegisterSend()
        {
            if (_disconnected == 1)
            {
                return;
            }

            // 이렇게하면 1 send당 1 sendAsync이기 때문에 좀더 효율적인 BufferList를 사용한다.
            // byte[] buff = _sendQueue.Dequeue();
            // _sendArgs.SetBuffer(buff, 0, buff.Length);

            // 무조건 모든 정보를 보내는게 아니라 지금까지 몇 byte를 보냈는지 추적해서
            // 심하게 많이 보내면 조절하는 기능도 있어야 한다.
            // 의미없는 쓰레기 패킷을 계속 보내면 OnRecvCompleted쪽에서 체크하여 킥해야 한다.
            while (_sendQueue.Count > 0)
            {
                ArraySegment<byte> buff = _sendQueue.Dequeue();
                _pendingList.Add(buff);
            }

            _sendArgs.BufferList = _pendingList;

            try
            {
                bool peding = _socket.SendAsync(_sendArgs);
                if (peding == false)
                {
                    OnSendCompleted(null, _sendArgs);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"RegisterSend Failed {e}");
            }

        }

        // 얘는 RegisterSend와 위에서 등록한 이벤트를 통해서 총 2곳에서
        // 호출될 수 있으므로 lock을 걸어준다.
        void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            lock (_lock)
            {
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    try
                    {
                        _sendArgs.BufferList = null;
                        _pendingList.Clear();

                        OnSend(_sendArgs.BytesTransferred);

                        // 방금 Send를 하는동안 누군가가 또 Queue에 값을 넣었다면
                        if (_sendQueue.Count > 0)
                        {
                            RegisterSend();
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"OnSendCompleted Failed {e}");
                    }
                }
                else
                {
                    Disconnect();
                }
            }
        }

        void RegisterRecv()
        {
            if (_disconnected == 1)
            {
                return;
            }

            _recvBuffer.Clean();
            ArraySegment<byte> segment = _recvBuffer.WriteSegment;
            _recvArgs.SetBuffer(segment.Array, segment.Offset, segment.Count);

            try
            {
                bool pending = _socket.ReceiveAsync(_recvArgs);
                if (pending == false)
                {
                    OnRecvCompleted(null, _recvArgs);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"RegisterRecv Failed {e}");
            }

        }

        void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
        {
            // args.BytesTransferred 내가 몇바이트를 받았는지 출력해줌
            // 0바이트를 받은경우는 상대가 연결을 끊은경우임
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                try
                {
                    // Write 커서 이동
                    if (_recvBuffer.OnWrite(args.BytesTransferred) == false)
                    {
                        Disconnect();
                        return;
                    }

                    // 컨텐츠 쪽으로 데이터를 넘겨주고 얼마나 처리했는지 받는다
                    int processLen = OnRecv(_recvBuffer.ReadSegment);
                    if (processLen < 0 || _recvBuffer.DataSize < processLen)
                    {
                        Disconnect();
                        return;
                    }

                    // Read 커서 이동
                    if (_recvBuffer.OnRead(processLen) == false)
                    {
                        Disconnect();
                        return;
                    }

                    // args.Buffer 소켓의 버퍼
                    // args.Offset 시작지점
                    // args.BytesTransferred 받은 크기
                    // string recvData = Encoding.UTF8.GetString(args.Buffer, args.Offset, args.BytesTransferred);
                    // Console.WriteLine($"[From Client] {recvData}");

                    RegisterRecv();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"OnRecvCompleted Failed {e}");
                }
            }
            else
            {
                Disconnect();
            }
        }
        #endregion
    }
}
