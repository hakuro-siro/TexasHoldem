﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ServerCore
{
    public class SendBufferHelper
    {
        // 원래 전역 변수로 만들면 멀티쓰레드 환경에서 다른 쓰레드가 접근하기 때문에 위험하다.
        // 그래서 전역이긴하지만 내 쓰레드에서만 접근할 수 있게 ThreadLocal을 사용한다.
        public static ThreadLocal<SendBuff> CurrentBuffer = new ThreadLocal<SendBuff>(() => { return null; });

        public static int ChunkSize { get; set; } = 65535;

        public static ArraySegment<byte> Open(int reserveSize)
        {
            if (CurrentBuffer.Value == null)
            {
                CurrentBuffer.Value = new SendBuff(ChunkSize);
            }
            if (CurrentBuffer.Value.FreeSize < reserveSize)
            {
                CurrentBuffer.Value = new SendBuff(ChunkSize);
            }

            return CurrentBuffer.Value.Open(reserveSize);
        }

        public static ArraySegment<byte> Close(int usedSize)
        {
            return CurrentBuffer.Value.Close(usedSize);
        }

    }

    public class SendBuff
    {
        byte[] _buffer;
        int _usedSize = 0;

        // 남은 공간
        // [][][][][u][][][][][]
        // u 부터 끝까지
        public int FreeSize { get { return _buffer.Length - _usedSize; } }

        public SendBuff(int chunkSize)
        {
            _buffer = new byte[chunkSize];
        }

        public ArraySegment<byte> Open(int reserveSize)
        {
            if (reserveSize > FreeSize)
            {
                return null;
            }
            return new ArraySegment<byte>(_buffer, _usedSize, reserveSize);
        }

        public ArraySegment<byte> Close(int usedSize)
        {
            ArraySegment<byte> segment = new ArraySegment<byte>(_buffer, _usedSize, usedSize);
            _usedSize += usedSize;
            return segment;
        }
    }
}
