
using ServerCore;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Net;
using System.Text;

public enum PacketID
{
    S_PlayerConnect = 1,
	C_PlayerConnect = 2,
	S_BroadcastEnterGame = 3,
	S_LeaveGame = 4,
	C_LeaveGame = 5,
	S_BroadcastLeaveGame = 6,
	S_PlayerList = 7,
	S_BroadcastMove = 8,
	S_GameRoomList = 9,
	C_GameRoomList = 10,
	S_GameChat = 11,
	C_GameChat = 12,
	S_CreateGameRoom = 13,
	C_CreateGameRoom = 14,
	S_GameRoomInfo = 15,
	C_GameRoomInfo = 16,
	S_GameRoomConnectCall = 17,
	C_GameRoomConnectCall = 18,
	S_GameRoomPlayerList = 19,
	C_GameRoomPlayerList = 20,
	S_GameRoomInfoRefresh = 21,
	C_GameRoomInfoRefresh = 22,
	C_InGameReady = 23,
	S_GameStart = 24,
	C_GameStart = 25,
	S_DealHoleCards = 26,
	C_DealHoleCards = 27,
	S_ChipStackSync = 28,
	C_ChipStackSync = 29,
	S_UserDataSync = 30,
	C_UserDataSync = 31,
	S_DealerButton = 32,
	C_DealerButton = 33,
	S_Bet = 34,
	C_Bet = 35,
	S_DealHoleCardsTrigger = 36,
	C_DealHoleCardsTrigger = 37,
	S_InGameLoadingComplete = 38,
	C_InGameLoadingComplete = 39,
	S_CommunityCards = 40,
	C_CommunityCards = 41,
	S_ErrorCode = 42,
	C_ErrorCode = 43,
	S_EnterOk = 44,
	C_EnterOk = 45,
	S_Fold = 46,
	C_Fold = 47,
	S_ShowDown = 48,
	C_ShowDown = 49,
	S_Winner = 50,
	C_Winner = 51,
	S_LeaveInGame = 52,
	C_LeaveInGame = 53,
	
}

public interface IPacket
{
	ushort Protocol { get; }
	void Read(ArraySegment<byte> segment);
	ArraySegment<byte> Write();
}


public class S_PlayerConnect : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.S_PlayerConnect; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_PlayerConnect), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_PlayerConnect : IPacket
{
    public string playerNickname;

    public ushort Protocol { get { return (ushort)PacketID.C_PlayerConnect; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        ushort playerNicknameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.playerNickname = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerNicknameLen);
		count += playerNicknameLen;
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_PlayerConnect), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        ushort playerNicknameLen = (ushort)Encoding.Unicode.GetBytes(this.playerNickname, 0, this.playerNickname.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(playerNicknameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += playerNicknameLen;

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_BroadcastEnterGame : IPacket
{
    public string playerName;
	public int sessionId;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastEnterGame; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        ushort playerNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.playerName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerNameLen);
		count += playerNameLen;
		this.sessionId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastEnterGame), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        ushort playerNameLen = (ushort)Encoding.Unicode.GetBytes(this.playerName, 0, this.playerName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(playerNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += playerNameLen;
		Array.Copy(BitConverter.GetBytes(this.sessionId), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_LeaveGame : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.S_LeaveGame; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_LeaveGame), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_LeaveGame : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_LeaveGame; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_LeaveGame), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_BroadcastLeaveGame : IPacket
{
    public int playerName;
	public int SessionId;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastLeaveGame; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.playerName = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.SessionId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastLeaveGame), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.playerName), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.SessionId), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_PlayerList : IPacket
{
    
	public struct Player
	{
	    public bool isSelf;
		public int playerId;
		public float posX;
		public float posY;
		public float posZ;
	
	    public void Read(ArraySegment<byte> segment , ref ushort count)
	    {
	        this.isSelf = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
			count += sizeof(bool);
			this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
			this.posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
			count += sizeof(float);
			this.posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
			count += sizeof(float);
			this.posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
			count += sizeof(float);
	    }
	
	    public bool Write(ArraySegment<byte> segment, ref ushort count)
	    {
	        bool success = true;
	        Array.Copy(BitConverter.GetBytes(this.isSelf), 0, segment.Array, segment.Offset + count, sizeof(bool));
			count += sizeof(bool);
			Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
			Array.Copy(BitConverter.GetBytes(this.posX), 0, segment.Array, segment.Offset + count, sizeof(float));
			count += sizeof(float);
			Array.Copy(BitConverter.GetBytes(this.posY), 0, segment.Array, segment.Offset + count, sizeof(float));
			count += sizeof(float);
			Array.Copy(BitConverter.GetBytes(this.posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
			count += sizeof(float);
	        return success;
	    }
	
	}
	
	public List<Player> players = new List<Player>();
	

    public ushort Protocol { get { return (ushort)PacketID.S_PlayerList; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.players.Clear();
		ushort playerLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		for (int i = 0; i < playerLen; i++)
		{
		    Player player = new Player();
		    player.Read(segment, ref count);
		    players.Add(player);
		}
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_PlayerList), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)this.players.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		foreach (Player player in this.players)
		{
		    player.Write(segment, ref count);
		}

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_BroadcastMove : IPacket
{
    public int playerId;
	public float posX;
	public float posY;
	public float posZ;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastMove; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastMove), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.posX), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.posY), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_GameRoomList : IPacket
{
    
	public struct Room
	{
	    public int roomId;
		public string roomName;
		public int roomLimit;
		public int roomPlayers;
		public bool isStartGame;
	
	    public void Read(ArraySegment<byte> segment , ref ushort count)
	    {
	        this.roomId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
			ushort roomNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
			count += sizeof(ushort);
			this.roomName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, roomNameLen);
			count += roomNameLen;
			this.roomLimit = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
			this.roomPlayers = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
			this.isStartGame = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
			count += sizeof(bool);
	    }
	
	    public bool Write(ArraySegment<byte> segment, ref ushort count)
	    {
	        bool success = true;
	        Array.Copy(BitConverter.GetBytes(this.roomId), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
			ushort roomNameLen = (ushort)Encoding.Unicode.GetBytes(this.roomName, 0, this.roomName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
			Array.Copy(BitConverter.GetBytes(roomNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
			count += sizeof(ushort);
			count += roomNameLen;
			Array.Copy(BitConverter.GetBytes(this.roomLimit), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
			Array.Copy(BitConverter.GetBytes(this.roomPlayers), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
			Array.Copy(BitConverter.GetBytes(this.isStartGame), 0, segment.Array, segment.Offset + count, sizeof(bool));
			count += sizeof(bool);
	        return success;
	    }
	
	}
	
	public List<Room> rooms = new List<Room>();
	

    public ushort Protocol { get { return (ushort)PacketID.S_GameRoomList; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.rooms.Clear();
		ushort roomLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		for (int i = 0; i < roomLen; i++)
		{
		    Room room = new Room();
		    room.Read(segment, ref count);
		    rooms.Add(room);
		}
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_GameRoomList), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)this.rooms.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		foreach (Room room in this.rooms)
		{
		    room.Write(segment, ref count);
		}

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_GameRoomList : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_GameRoomList; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_GameRoomList), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_GameChat : IPacket
{
    public string playerName;
	public int playerId;
	public string chat;

    public ushort Protocol { get { return (ushort)PacketID.S_GameChat; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        ushort playerNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.playerName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerNameLen);
		count += playerNameLen;
		this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		ushort chatLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.chat = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, chatLen);
		count += chatLen;
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_GameChat), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        ushort playerNameLen = (ushort)Encoding.Unicode.GetBytes(this.playerName, 0, this.playerName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(playerNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += playerNameLen;
		Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		ushort chatLen = (ushort)Encoding.Unicode.GetBytes(this.chat, 0, this.chat.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(chatLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += chatLen;

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_GameChat : IPacket
{
    public string playerName;
	public string chat;

    public ushort Protocol { get { return (ushort)PacketID.C_GameChat; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        ushort playerNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.playerName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerNameLen);
		count += playerNameLen;
		ushort chatLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.chat = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, chatLen);
		count += chatLen;
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_GameChat), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        ushort playerNameLen = (ushort)Encoding.Unicode.GetBytes(this.playerName, 0, this.playerName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(playerNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += playerNameLen;
		ushort chatLen = (ushort)Encoding.Unicode.GetBytes(this.chat, 0, this.chat.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(chatLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += chatLen;

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_CreateGameRoom : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.S_CreateGameRoom; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_CreateGameRoom), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_CreateGameRoom : IPacket
{
    public string roomName;
	public int roomLimit;

    public ushort Protocol { get { return (ushort)PacketID.C_CreateGameRoom; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        ushort roomNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.roomName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, roomNameLen);
		count += roomNameLen;
		this.roomLimit = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_CreateGameRoom), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        ushort roomNameLen = (ushort)Encoding.Unicode.GetBytes(this.roomName, 0, this.roomName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(roomNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += roomNameLen;
		Array.Copy(BitConverter.GetBytes(this.roomLimit), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_GameRoomInfo : IPacket
{
    
	public struct Players
	{
	    public string playerName;
		public int sessionId;
		public bool isReady;
		public bool isMine;
		public bool isLeader;
	
	    public void Read(ArraySegment<byte> segment , ref ushort count)
	    {
	        ushort playerNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
			count += sizeof(ushort);
			this.playerName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerNameLen);
			count += playerNameLen;
			this.sessionId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
			this.isReady = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
			count += sizeof(bool);
			this.isMine = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
			count += sizeof(bool);
			this.isLeader = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
			count += sizeof(bool);
	    }
	
	    public bool Write(ArraySegment<byte> segment, ref ushort count)
	    {
	        bool success = true;
	        ushort playerNameLen = (ushort)Encoding.Unicode.GetBytes(this.playerName, 0, this.playerName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
			Array.Copy(BitConverter.GetBytes(playerNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
			count += sizeof(ushort);
			count += playerNameLen;
			Array.Copy(BitConverter.GetBytes(this.sessionId), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
			Array.Copy(BitConverter.GetBytes(this.isReady), 0, segment.Array, segment.Offset + count, sizeof(bool));
			count += sizeof(bool);
			Array.Copy(BitConverter.GetBytes(this.isMine), 0, segment.Array, segment.Offset + count, sizeof(bool));
			count += sizeof(bool);
			Array.Copy(BitConverter.GetBytes(this.isLeader), 0, segment.Array, segment.Offset + count, sizeof(bool));
			count += sizeof(bool);
	        return success;
	    }
	
	}
	
	public List<Players> playerss = new List<Players>();
	
	public string roomName;
	public int roomLimit;
	public int playerCount;

    public ushort Protocol { get { return (ushort)PacketID.S_GameRoomInfo; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.playerss.Clear();
		ushort playersLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		for (int i = 0; i < playersLen; i++)
		{
		    Players players = new Players();
		    players.Read(segment, ref count);
		    playerss.Add(players);
		}
		ushort roomNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.roomName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, roomNameLen);
		count += roomNameLen;
		this.roomLimit = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.playerCount = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_GameRoomInfo), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)this.playerss.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		foreach (Players players in this.playerss)
		{
		    players.Write(segment, ref count);
		}
		ushort roomNameLen = (ushort)Encoding.Unicode.GetBytes(this.roomName, 0, this.roomName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(roomNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += roomNameLen;
		Array.Copy(BitConverter.GetBytes(this.roomLimit), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.playerCount), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_GameRoomInfo : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_GameRoomInfo; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_GameRoomInfo), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_GameRoomConnectCall : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.S_GameRoomConnectCall; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_GameRoomConnectCall), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_GameRoomConnectCall : IPacket
{
    public int roomId;

    public ushort Protocol { get { return (ushort)PacketID.C_GameRoomConnectCall; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.roomId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_GameRoomConnectCall), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.roomId), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_GameRoomPlayerList : IPacket
{
    
	public struct Players
	{
	    public string playerName;
	
	    public void Read(ArraySegment<byte> segment , ref ushort count)
	    {
	        ushort playerNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
			count += sizeof(ushort);
			this.playerName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerNameLen);
			count += playerNameLen;
	    }
	
	    public bool Write(ArraySegment<byte> segment, ref ushort count)
	    {
	        bool success = true;
	        ushort playerNameLen = (ushort)Encoding.Unicode.GetBytes(this.playerName, 0, this.playerName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
			Array.Copy(BitConverter.GetBytes(playerNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
			count += sizeof(ushort);
			count += playerNameLen;
	        return success;
	    }
	
	}
	
	public List<Players> playerss = new List<Players>();
	
	public string roomName;
	public int roomLimit;
	public int playerCount;

    public ushort Protocol { get { return (ushort)PacketID.S_GameRoomPlayerList; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.playerss.Clear();
		ushort playersLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		for (int i = 0; i < playersLen; i++)
		{
		    Players players = new Players();
		    players.Read(segment, ref count);
		    playerss.Add(players);
		}
		ushort roomNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.roomName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, roomNameLen);
		count += roomNameLen;
		this.roomLimit = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.playerCount = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_GameRoomPlayerList), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)this.playerss.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		foreach (Players players in this.playerss)
		{
		    players.Write(segment, ref count);
		}
		ushort roomNameLen = (ushort)Encoding.Unicode.GetBytes(this.roomName, 0, this.roomName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(roomNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += roomNameLen;
		Array.Copy(BitConverter.GetBytes(this.roomLimit), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.playerCount), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_GameRoomPlayerList : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_GameRoomPlayerList; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_GameRoomPlayerList), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_GameRoomInfoRefresh : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.S_GameRoomInfoRefresh; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_GameRoomInfoRefresh), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_GameRoomInfoRefresh : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_GameRoomInfoRefresh; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_GameRoomInfoRefresh), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_InGameReady : IPacket
{
    public bool isReady;

    public ushort Protocol { get { return (ushort)PacketID.C_InGameReady; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.isReady = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
		count += sizeof(bool);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_InGameReady), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.isReady), 0, segment.Array, segment.Offset + count, sizeof(bool));
		count += sizeof(bool);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_GameStart : IPacket
{
    public bool isStart;

    public ushort Protocol { get { return (ushort)PacketID.S_GameStart; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.isStart = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
		count += sizeof(bool);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_GameStart), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.isStart), 0, segment.Array, segment.Offset + count, sizeof(bool));
		count += sizeof(bool);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_GameStart : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_GameStart; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_GameStart), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_DealHoleCards : IPacket
{
    
	public struct Hand
	{
	    public int rank;
		public int suit;
	
	    public void Read(ArraySegment<byte> segment , ref ushort count)
	    {
	        this.rank = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
			this.suit = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
	    }
	
	    public bool Write(ArraySegment<byte> segment, ref ushort count)
	    {
	        bool success = true;
	        Array.Copy(BitConverter.GetBytes(this.rank), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
			Array.Copy(BitConverter.GetBytes(this.suit), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
	        return success;
	    }
	
	}
	
	public List<Hand> hands = new List<Hand>();
	

    public ushort Protocol { get { return (ushort)PacketID.S_DealHoleCards; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.hands.Clear();
		ushort handLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		for (int i = 0; i < handLen; i++)
		{
		    Hand hand = new Hand();
		    hand.Read(segment, ref count);
		    hands.Add(hand);
		}
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_DealHoleCards), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)this.hands.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		foreach (Hand hand in this.hands)
		{
		    hand.Write(segment, ref count);
		}

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_DealHoleCards : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_DealHoleCards; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_DealHoleCards), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_ChipStackSync : IPacket
{
    
	public struct Player
	{
	    public int playerId;
		public int chip;
		public bool isMine;
	
	    public void Read(ArraySegment<byte> segment , ref ushort count)
	    {
	        this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
			this.chip = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
			this.isMine = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
			count += sizeof(bool);
	    }
	
	    public bool Write(ArraySegment<byte> segment, ref ushort count)
	    {
	        bool success = true;
	        Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
			Array.Copy(BitConverter.GetBytes(this.chip), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
			Array.Copy(BitConverter.GetBytes(this.isMine), 0, segment.Array, segment.Offset + count, sizeof(bool));
			count += sizeof(bool);
	        return success;
	    }
	
	}
	
	public List<Player> players = new List<Player>();
	
	public int pot;

    public ushort Protocol { get { return (ushort)PacketID.S_ChipStackSync; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.players.Clear();
		ushort playerLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		for (int i = 0; i < playerLen; i++)
		{
		    Player player = new Player();
		    player.Read(segment, ref count);
		    players.Add(player);
		}
		this.pot = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_ChipStackSync), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)this.players.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		foreach (Player player in this.players)
		{
		    player.Write(segment, ref count);
		}
		Array.Copy(BitConverter.GetBytes(this.pot), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_ChipStackSync : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_ChipStackSync; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_ChipStackSync), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_UserDataSync : IPacket
{
    
	public struct Player
	{
	    public int playerId;
		public string playerName;
		public bool isMine;
	
	    public void Read(ArraySegment<byte> segment , ref ushort count)
	    {
	        this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
			ushort playerNameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
			count += sizeof(ushort);
			this.playerName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, playerNameLen);
			count += playerNameLen;
			this.isMine = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
			count += sizeof(bool);
	    }
	
	    public bool Write(ArraySegment<byte> segment, ref ushort count)
	    {
	        bool success = true;
	        Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
			ushort playerNameLen = (ushort)Encoding.Unicode.GetBytes(this.playerName, 0, this.playerName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
			Array.Copy(BitConverter.GetBytes(playerNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
			count += sizeof(ushort);
			count += playerNameLen;
			Array.Copy(BitConverter.GetBytes(this.isMine), 0, segment.Array, segment.Offset + count, sizeof(bool));
			count += sizeof(bool);
	        return success;
	    }
	
	}
	
	public List<Player> players = new List<Player>();
	

    public ushort Protocol { get { return (ushort)PacketID.S_UserDataSync; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.players.Clear();
		ushort playerLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		for (int i = 0; i < playerLen; i++)
		{
		    Player player = new Player();
		    player.Read(segment, ref count);
		    players.Add(player);
		}
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_UserDataSync), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)this.players.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		foreach (Player player in this.players)
		{
		    player.Write(segment, ref count);
		}

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_UserDataSync : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_UserDataSync; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_UserDataSync), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_DealerButton : IPacket
{
    public int index;

    public ushort Protocol { get { return (ushort)PacketID.S_DealerButton; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.index = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_DealerButton), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.index), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_DealerButton : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_DealerButton; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_DealerButton), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_Bet : IPacket
{
    public int playerId;
	public bool canCheck;
	public bool canCall;
	public bool canBet;
	public bool canRaise;
	public bool canFold;
	public bool canAllIn;
	public int minAmount;

    public ushort Protocol { get { return (ushort)PacketID.S_Bet; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.canCheck = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
		count += sizeof(bool);
		this.canCall = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
		count += sizeof(bool);
		this.canBet = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
		count += sizeof(bool);
		this.canRaise = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
		count += sizeof(bool);
		this.canFold = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
		count += sizeof(bool);
		this.canAllIn = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
		count += sizeof(bool);
		this.minAmount = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_Bet), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.canCheck), 0, segment.Array, segment.Offset + count, sizeof(bool));
		count += sizeof(bool);
		Array.Copy(BitConverter.GetBytes(this.canCall), 0, segment.Array, segment.Offset + count, sizeof(bool));
		count += sizeof(bool);
		Array.Copy(BitConverter.GetBytes(this.canBet), 0, segment.Array, segment.Offset + count, sizeof(bool));
		count += sizeof(bool);
		Array.Copy(BitConverter.GetBytes(this.canRaise), 0, segment.Array, segment.Offset + count, sizeof(bool));
		count += sizeof(bool);
		Array.Copy(BitConverter.GetBytes(this.canFold), 0, segment.Array, segment.Offset + count, sizeof(bool));
		count += sizeof(bool);
		Array.Copy(BitConverter.GetBytes(this.canAllIn), 0, segment.Array, segment.Offset + count, sizeof(bool));
		count += sizeof(bool);
		Array.Copy(BitConverter.GetBytes(this.minAmount), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_Bet : IPacket
{
    public int playerId;
	public int chip;
	public string info;

    public ushort Protocol { get { return (ushort)PacketID.C_Bet; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.chip = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		ushort infoLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.info = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, infoLen);
		count += infoLen;
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_Bet), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.chip), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		ushort infoLen = (ushort)Encoding.Unicode.GetBytes(this.info, 0, this.info.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(infoLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += infoLen;

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_DealHoleCardsTrigger : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.S_DealHoleCardsTrigger; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_DealHoleCardsTrigger), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_DealHoleCardsTrigger : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_DealHoleCardsTrigger; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_DealHoleCardsTrigger), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_InGameLoadingComplete : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.S_InGameLoadingComplete; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_InGameLoadingComplete), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_InGameLoadingComplete : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_InGameLoadingComplete; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_InGameLoadingComplete), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_CommunityCards : IPacket
{
    
	public struct Card
	{
	    public int rank;
		public int suit;
	
	    public void Read(ArraySegment<byte> segment , ref ushort count)
	    {
	        this.rank = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
			this.suit = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
	    }
	
	    public bool Write(ArraySegment<byte> segment, ref ushort count)
	    {
	        bool success = true;
	        Array.Copy(BitConverter.GetBytes(this.rank), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
			Array.Copy(BitConverter.GetBytes(this.suit), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
	        return success;
	    }
	
	}
	
	public List<Card> cards = new List<Card>();
	

    public ushort Protocol { get { return (ushort)PacketID.S_CommunityCards; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.cards.Clear();
		ushort cardLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		for (int i = 0; i < cardLen; i++)
		{
		    Card card = new Card();
		    card.Read(segment, ref count);
		    cards.Add(card);
		}
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_CommunityCards), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)this.cards.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		foreach (Card card in this.cards)
		{
		    card.Write(segment, ref count);
		}

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_CommunityCards : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_CommunityCards; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_CommunityCards), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_ErrorCode : IPacket
{
    public int code;
	public string message;

    public ushort Protocol { get { return (ushort)PacketID.S_ErrorCode; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.code = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		ushort messageLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.message = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, messageLen);
		count += messageLen;
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_ErrorCode), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.code), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		ushort messageLen = (ushort)Encoding.Unicode.GetBytes(this.message, 0, this.message.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(messageLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += messageLen;

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_ErrorCode : IPacket
{
    public int code;
	public string message;

    public ushort Protocol { get { return (ushort)PacketID.C_ErrorCode; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.code = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		ushort messageLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.message = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, messageLen);
		count += messageLen;
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_ErrorCode), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.code), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		ushort messageLen = (ushort)Encoding.Unicode.GetBytes(this.message, 0, this.message.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(messageLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += messageLen;

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_EnterOk : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.S_EnterOk; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_EnterOk), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_EnterOk : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_EnterOk; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_EnterOk), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_Fold : IPacket
{
    public int playerId;

    public ushort Protocol { get { return (ushort)PacketID.S_Fold; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_Fold), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_Fold : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_Fold; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_Fold), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_ShowDown : IPacket
{
    
	public struct Hand1
	{
	    public int rank1;
		public int suit1;
	
	    public void Read(ArraySegment<byte> segment , ref ushort count)
	    {
	        this.rank1 = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
			this.suit1 = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
	    }
	
	    public bool Write(ArraySegment<byte> segment, ref ushort count)
	    {
	        bool success = true;
	        Array.Copy(BitConverter.GetBytes(this.rank1), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
			Array.Copy(BitConverter.GetBytes(this.suit1), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
	        return success;
	    }
	
	}
	
	public List<Hand1> hand1s = new List<Hand1>();
	
	
	public struct Hand2
	{
	    public int rank2;
		public int suit2;
	
	    public void Read(ArraySegment<byte> segment , ref ushort count)
	    {
	        this.rank2 = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
			this.suit2 = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
	    }
	
	    public bool Write(ArraySegment<byte> segment, ref ushort count)
	    {
	        bool success = true;
	        Array.Copy(BitConverter.GetBytes(this.rank2), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
			Array.Copy(BitConverter.GetBytes(this.suit2), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
	        return success;
	    }
	
	}
	
	public List<Hand2> hand2s = new List<Hand2>();
	

    public ushort Protocol { get { return (ushort)PacketID.S_ShowDown; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        this.hand1s.Clear();
		ushort hand1Len = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		for (int i = 0; i < hand1Len; i++)
		{
		    Hand1 hand1 = new Hand1();
		    hand1.Read(segment, ref count);
		    hand1s.Add(hand1);
		}
		this.hand2s.Clear();
		ushort hand2Len = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		for (int i = 0; i < hand2Len; i++)
		{
		    Hand2 hand2 = new Hand2();
		    hand2.Read(segment, ref count);
		    hand2s.Add(hand2);
		}
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_ShowDown), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)this.hand1s.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		foreach (Hand1 hand1 in this.hand1s)
		{
		    hand1.Write(segment, ref count);
		}
		Array.Copy(BitConverter.GetBytes((ushort)this.hand2s.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		foreach (Hand2 hand2 in this.hand2s)
		{
		    hand2.Write(segment, ref count);
		}

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_ShowDown : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_ShowDown; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_ShowDown), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_Winner : IPacket
{
    public string mainPot;
	public string sidePot;

    public ushort Protocol { get { return (ushort)PacketID.S_Winner; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        ushort mainPotLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.mainPot = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, mainPotLen);
		count += mainPotLen;
		ushort sidePotLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.sidePot = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, sidePotLen);
		count += sidePotLen;
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_Winner), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        ushort mainPotLen = (ushort)Encoding.Unicode.GetBytes(this.mainPot, 0, this.mainPot.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(mainPotLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += mainPotLen;
		ushort sidePotLen = (ushort)Encoding.Unicode.GetBytes(this.sidePot, 0, this.sidePot.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(sidePotLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += sidePotLen;

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_Winner : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_Winner; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_Winner), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_LeaveInGame : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.S_LeaveInGame; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_LeaveInGame), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_LeaveInGame : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_LeaveInGame; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_LeaveInGame), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

