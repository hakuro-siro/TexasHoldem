using ServerCore;
using System;
using System.Collections.Generic;

public class PacketManager
{
    #region Singleton
    static PacketManager _instance = new PacketManager();
    public static PacketManager Instance { get { return _instance; } }
    #endregion

    PacketManager()
    {
        Register();
    }

    Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
    Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();

    public void Register()
    {

        _makeFunc.Add((ushort)PacketID.S_PlayerConnect, MakePacket<S_PlayerConnect>);
        _handler.Add((ushort)PacketID.S_PlayerConnect, PacketHandler.S_PlayerConnectHandler);


        _makeFunc.Add((ushort)PacketID.S_BroadcastEnterGame, MakePacket<S_BroadcastEnterGame>);
        _handler.Add((ushort)PacketID.S_BroadcastEnterGame, PacketHandler.S_BroadcastEnterGameHandler);


        _makeFunc.Add((ushort)PacketID.S_LeaveGame, MakePacket<S_LeaveGame>);
        _handler.Add((ushort)PacketID.S_LeaveGame, PacketHandler.S_LeaveGameHandler);


        _makeFunc.Add((ushort)PacketID.S_BroadcastLeaveGame, MakePacket<S_BroadcastLeaveGame>);
        _handler.Add((ushort)PacketID.S_BroadcastLeaveGame, PacketHandler.S_BroadcastLeaveGameHandler);


        _makeFunc.Add((ushort)PacketID.S_PlayerList, MakePacket<S_PlayerList>);
        _handler.Add((ushort)PacketID.S_PlayerList, PacketHandler.S_PlayerListHandler);


        _makeFunc.Add((ushort)PacketID.S_BroadcastMove, MakePacket<S_BroadcastMove>);
        _handler.Add((ushort)PacketID.S_BroadcastMove, PacketHandler.S_BroadcastMoveHandler);


        _makeFunc.Add((ushort)PacketID.S_GameRoomList, MakePacket<S_GameRoomList>);
        _handler.Add((ushort)PacketID.S_GameRoomList, PacketHandler.S_GameRoomListHandler);


        _makeFunc.Add((ushort)PacketID.S_GameChat, MakePacket<S_GameChat>);
        _handler.Add((ushort)PacketID.S_GameChat, PacketHandler.S_GameChatHandler);


        _makeFunc.Add((ushort)PacketID.S_CreateGameRoom, MakePacket<S_CreateGameRoom>);
        _handler.Add((ushort)PacketID.S_CreateGameRoom, PacketHandler.S_CreateGameRoomHandler);


        _makeFunc.Add((ushort)PacketID.S_GameRoomInfo, MakePacket<S_GameRoomInfo>);
        _handler.Add((ushort)PacketID.S_GameRoomInfo, PacketHandler.S_GameRoomInfoHandler);


        _makeFunc.Add((ushort)PacketID.S_GameRoomConnectCall, MakePacket<S_GameRoomConnectCall>);
        _handler.Add((ushort)PacketID.S_GameRoomConnectCall, PacketHandler.S_GameRoomConnectCallHandler);


        _makeFunc.Add((ushort)PacketID.S_GameRoomPlayerList, MakePacket<S_GameRoomPlayerList>);
        _handler.Add((ushort)PacketID.S_GameRoomPlayerList, PacketHandler.S_GameRoomPlayerListHandler);


        _makeFunc.Add((ushort)PacketID.S_GameRoomInfoRefresh, MakePacket<S_GameRoomInfoRefresh>);
        _handler.Add((ushort)PacketID.S_GameRoomInfoRefresh, PacketHandler.S_GameRoomInfoRefreshHandler);


        _makeFunc.Add((ushort)PacketID.S_GameStart, MakePacket<S_GameStart>);
        _handler.Add((ushort)PacketID.S_GameStart, PacketHandler.S_GameStartHandler);


        _makeFunc.Add((ushort)PacketID.S_DealHoleCards, MakePacket<S_DealHoleCards>);
        _handler.Add((ushort)PacketID.S_DealHoleCards, PacketHandler.S_DealHoleCardsHandler);


        _makeFunc.Add((ushort)PacketID.S_ChipStackSync, MakePacket<S_ChipStackSync>);
        _handler.Add((ushort)PacketID.S_ChipStackSync, PacketHandler.S_ChipStackSyncHandler);


        _makeFunc.Add((ushort)PacketID.S_UserDataSync, MakePacket<S_UserDataSync>);
        _handler.Add((ushort)PacketID.S_UserDataSync, PacketHandler.S_UserDataSyncHandler);


        _makeFunc.Add((ushort)PacketID.S_DealerButton, MakePacket<S_DealerButton>);
        _handler.Add((ushort)PacketID.S_DealerButton, PacketHandler.S_DealerButtonHandler);


        _makeFunc.Add((ushort)PacketID.S_Bet, MakePacket<S_Bet>);
        _handler.Add((ushort)PacketID.S_Bet, PacketHandler.S_BetHandler);


        _makeFunc.Add((ushort)PacketID.S_DealHoleCardsTrigger, MakePacket<S_DealHoleCardsTrigger>);
        _handler.Add((ushort)PacketID.S_DealHoleCardsTrigger, PacketHandler.S_DealHoleCardsTriggerHandler);


        _makeFunc.Add((ushort)PacketID.S_InGameLoadingComplete, MakePacket<S_InGameLoadingComplete>);
        _handler.Add((ushort)PacketID.S_InGameLoadingComplete, PacketHandler.S_InGameLoadingCompleteHandler);


        _makeFunc.Add((ushort)PacketID.S_CommunityCards, MakePacket<S_CommunityCards>);
        _handler.Add((ushort)PacketID.S_CommunityCards, PacketHandler.S_CommunityCardsHandler);


        _makeFunc.Add((ushort)PacketID.S_ErrorCode, MakePacket<S_ErrorCode>);
        _handler.Add((ushort)PacketID.S_ErrorCode, PacketHandler.S_ErrorCodeHandler);


        _makeFunc.Add((ushort)PacketID.S_EnterOk, MakePacket<S_EnterOk>);
        _handler.Add((ushort)PacketID.S_EnterOk, PacketHandler.S_EnterOkHandler);


        _makeFunc.Add((ushort)PacketID.S_Fold, MakePacket<S_Fold>);
        _handler.Add((ushort)PacketID.S_Fold, PacketHandler.S_FoldHandler);


        _makeFunc.Add((ushort)PacketID.S_ShowDown, MakePacket<S_ShowDown>);
        _handler.Add((ushort)PacketID.S_ShowDown, PacketHandler.S_ShowDownHandler);


        _makeFunc.Add((ushort)PacketID.S_Winner, MakePacket<S_Winner>);
        _handler.Add((ushort)PacketID.S_Winner, PacketHandler.S_WinnerHandler);


        _makeFunc.Add((ushort)PacketID.S_LeaveInGame, MakePacket<S_LeaveInGame>);
        _handler.Add((ushort)PacketID.S_LeaveInGame, PacketHandler.S_LeaveInGameHandler);


    }

    public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onRecvCallback = null)
    {
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
        if(_makeFunc.TryGetValue(id, out func))
        {
            IPacket packet = func.Invoke(session, buffer);
            if (onRecvCallback != null)
            {
                onRecvCallback.Invoke(session, packet);
            }
            else
            {
                HandlePacket(session, packet);
            }
        }
    }

    T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
    {
        T pkt = new T();
        pkt.Read(buffer);
        return pkt;
    }

    public void HandlePacket(PacketSession session, IPacket packet)
    {
        Action<PacketSession, IPacket> action = null;
        if (_handler.TryGetValue(packet.Protocol, out action))
        {
            action.Invoke(session, packet);
        }
    }
}
