using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using CardGame;
using UnityEngine;
using CardGame.logic;

namespace DummyClient
{

    // OnConnected후에 세션 매니저 같은 곳에 기억하게 만들어줘야한다
    class ServerSession : PacketSession
    {
        // unsafe C++처럼 포인터를 사용할 수 있게됨
        static unsafe void ToBytes(byte[] array, int offset, ulong value)
        {
            fixed (byte* ptr = &array[offset])
                *(ulong*)ptr = value;
        }

        public override void OnConnected(EndPoint endPoint)
        {
            // Console.WriteLine($"OnConnected : {endPoint}");
            // Debug.Log("On Connected : {endPoint}");
            // LogicManager.instance.Connected(true);
            C_PlayerConnect pkt = new C_PlayerConnect();
            pkt.playerNickname = LogicManager.instance.playergear.GetPlayername();
            Debug.Log(pkt.playerNickname);
            HoldemClientMain.instance.NetworkManager.Send(pkt.Write());
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer, (s, p) => PacketQueue.Instance.Push(p));
        }

        public override void OnSend(int numOfBytes)
        {
            //Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
