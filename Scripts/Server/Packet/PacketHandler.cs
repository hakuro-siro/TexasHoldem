using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;
using CardGame;
using UnityEngine;
using CardGame.logic;


class PacketHandler
{
    public static void S_PlayerConnectHandler(PacketSession session, IPacket packet)
    {
        Debug.Log("서버연결완료!!!!");
        LogicManager.instance.Connected(true);
    }

    public static void S_BroadcastEnterGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastEnterGame pkt = packet as S_BroadcastEnterGame;
        ServerSession serverSession = session as ServerSession;

        C_GameRoomInfoRefresh pt = new C_GameRoomInfoRefresh();
        HoldemClientMain.instance.NetworkManager.Send(pt.Write());
    }

    public static void S_BroadcastLeaveGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastLeaveGame pkt = packet as S_BroadcastLeaveGame;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.LeaveGame(pkt);
    }

    public static void S_PlayerListHandler(PacketSession session, IPacket packet)
    {
        S_PlayerList pkt = packet as S_PlayerList;
        ServerSession serverSession = session as ServerSession;

        // PlayerManager.Instance.Add(pkt);
    }

    public static void S_BroadcastMoveHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastMove pkt = packet as S_BroadcastMove;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.Move(pkt);
    }

    public static void S_GameRoomListHandler(PacketSession session, IPacket packet)
    {
        S_GameRoomList pkt = packet as S_GameRoomList;
        ServerSession serverSession = session as ServerSession;

        LobbyManager.Instance.Refresh(pkt);
    }

    public static void S_GameChatHandler(PacketSession session, IPacket packet)
    {
        S_GameChat pkt = packet as S_GameChat;
        ServerSession serverSession = session as ServerSession;
        LogicManager.instance.ingameroomgear.InitChatData(pkt);
    }
    
    /// <summary>
    /// 현재 임시 미사용 Handler
    /// </summary>
    /// <param name="session"></param>
    /// <param name="packet"></param>
    public static void S_CreateGameRoomHandler(PacketSession session, IPacket packet)
    {
        // S_CreateGameRoom pkt = packet as S_CreateGameRoom;
        // ServerSession serverSession = session as ServerSession;
        //
        // LobbyManager.Instance.RoomConnected(true);
    }
    /// <summary>
    /// 현재 임시 미사용 Handler
    /// </summary>
    /// <param name="session"></param>
    /// <param name="packet"></param>
    public static void S_GameRoomConnectCallHandler(PacketSession session, IPacket packet)
    {
        // LobbyManager.Instance.RoomConnected(true);
    }

    public static void S_GameRoomInfoHandler(PacketSession session, IPacket packet)
    {
        S_GameRoomInfo pkt = packet as S_GameRoomInfo;

        // foreach (S_GameRoomConnectSucess.Players s in pkt.playerss)
        // {
        //     Debug.LogWarning(s.playerName);
        // }
        Debug.Log("S_GameRoomConnectSucessHandler");
        LobbyManager.Instance.RoomConnected(pkt);
    }
    
    //S_GameRoomInfoRefreshHandler
    public static void S_GameRoomInfoRefreshHandler(PacketSession session, IPacket packet)
    {

    }
    
    //S_GameStartHandler
    public static void S_GameStartHandler(PacketSession session, IPacket packet)
    {
        S_GameStart pkt = packet as S_GameStart;
        if (pkt.isStart)
        {
            LogicManager.instance.ingameroomgear.GotoInGame();
        }
        else
        {
            LogicManager.instance.ingameroomgear.InitChatDataWithColor("모두 레디하지 않았습니다",1);
        }
    }
    
    //S_GameRoomPlayerListHandler
    public static void S_GameRoomPlayerListHandler(PacketSession session, IPacket packet)
    {

    }
    
    //S_LeaveGameHandler
    public static void S_LeaveGameHandler(PacketSession session, IPacket packet)
    {
        LogicManager.instance.ingameroomgear.OutRoomIns();
    }
    
    //S_DealHoleCardsHandler
    public static void S_DealHoleCardsHandler(PacketSession session, IPacket packet)
    {
        S_DealHoleCards pkt = packet as S_DealHoleCards;
        ServerSession serverSession = session as ServerSession;
        LogicManager.instance.holdemgear.GetHand(pkt);
    }
    
    //S_ChipStackSyncHandler
    public static void S_ChipStackSyncHandler(PacketSession session, IPacket packet)
    {
        S_ChipStackSync pkt = packet as S_ChipStackSync;
        LogicManager.instance.holdemgear.SetMoneys(pkt);
    }
    
    //S_DealerButtonHandler
    public static void S_DealerButtonHandler(PacketSession session, IPacket packet)
    {
        Debug.Log("DelarButtonHandler");
        S_DealerButton pkt = packet as S_DealerButton;
        
        LogicManager.instance.holdemgear.SetDelarButton(pkt.index);
    }
    
    //S_BetHandler
    public static void S_BetHandler(PacketSession session, IPacket packet)
    {
        //todo베팅 핸들러
        //player id =>(배팅을 할) 플레이어 번호
        // 가능 여부들의 TF 값
        // IF( playerid -> 상대면 값만 띄워준다.
        S_Bet pkt = packet as S_Bet;
        LogicManager.instance.holdemgear.SetBetting(pkt);
    }
    
    //S_UserDataSyncHandler
    public static void S_UserDataSyncHandler(PacketSession session, IPacket packet)
    {
        S_UserDataSync pkt = packet as S_UserDataSync;
        LogicManager.instance.holdemgear.SetPlayers(pkt);
    }
    
    //S_DealHoleCardsTriggerHandler
    public static void S_DealHoleCardsTriggerHandler(PacketSession session, IPacket packet)
    {
        LogicManager.instance.holdemgear.SetDummyCards();
        C_DealHoleCardsTrigger pkt = new C_DealHoleCardsTrigger();
        HoldemClientMain.instance.NetworkManager.Send(pkt.Write());
    }
    
    //S_InGameLoadingCompleteHandler
    public static void S_InGameLoadingCompleteHandler(PacketSession session, IPacket packet)
    {
    }
    //S_CommunityCardsHandler
    
    public static void S_CommunityCardsHandler(PacketSession session, IPacket packet)
    {
        //todo 리스트 받은거 띄우기 
        S_CommunityCards pkt = packet as S_CommunityCards;
        LogicManager.instance.holdemgear.SetCommuCard(pkt);
    }
 
    //S_ErrorCodeHandler
    public static void S_ErrorCodeHandler(PacketSession session, IPacket packet)
    {
        S_ErrorCode pkt = packet as S_ErrorCode;
        LogicManager.instance.errorgear.ErrorOccured(pkt.code);
    }
    //S_EnterOkHandler
    public static void S_EnterOkHandler(PacketSession session, IPacket packet)
    {
        Debug.LogWarning("OKHANDLER");
        // 씬 로드
        LogicManager.instance.lobbygear.GotoGameRoom();
    }
    //S_FoldHandler
    public static void S_FoldHandler(PacketSession session, IPacket packet)
    {
        S_Fold pkt = packet as S_Fold;
        LogicManager.instance.holdemgear.SetFoldUser(pkt);
    }
    //S_ShowDownHandler
    public static void S_ShowDownHandler(PacketSession session, IPacket packet)
    {
        S_ShowDown pkt = packet as S_ShowDown;
        LogicManager.instance.holdemgear.ShowDownGame(pkt);
        // hads[0]
        
    }
    
    //S_WinnerHandler
    public static void S_WinnerHandler(PacketSession session, IPacket packet)
    {
        S_Winner pkt = packet as S_Winner;
        LogicManager.instance.holdemgear.SetWinner(pkt);
        // hads[0]
        
    }
    //S_LeaveInGameHandler
    public static void S_LeaveInGameHandler(PacketSession session, IPacket packet)
    {
        S_LeaveInGame pkt = packet as S_LeaveInGame;
        LogicManager.instance.holdemgear.Getout();
        // hads[0]
        
    }
}