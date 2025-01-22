using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class LobbyManager
{
    public static LobbyManager Instance { get; } = new LobbyManager();

    /// <summary>
    ///  Subject 등록
    /// </summary>
    private Subject<IPacket> LobbyPacketSubject = new Subject<IPacket>();
    public IObservable<IPacket> OnLobbyPacketSubject
    {
        get { return LobbyPacketSubject; }
    }
    private Subject<IPacket> RoomConnectPacketSubject = new Subject<IPacket>();
    public IObservable<IPacket> OnRoomConnectPacketSubject
    {
        get { return RoomConnectPacketSubject; }
    } 
    public void RoomConnected(IPacket Roominfo)
    {
        RoomConnectPacketSubject.OnNext(Roominfo);
    }
    public void Refresh(S_GameRoomList list)
    {
        LobbyPacketSubject.OnNext(list);
    }

}
