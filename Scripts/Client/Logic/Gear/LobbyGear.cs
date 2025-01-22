using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using CardGame.view;
using CardGame.view.lobby;
using Debug = UnityEngine.Debug;

namespace CardGame.logic{
    
    public class LobbyGear
    {
        /// <summary>
        /// Gear들은 모두 LogicManager를 가지고 있습니다.
        /// </summary>
        [SerializeField] 
        private LogicManager logicmanager;
        
        // Lobby.cs 의 클래스
        private Lobby lobby;
        
        /// <summary>
        /// Lobby Ui에서 필요한 모든 데이터의 모음
        /// </summary>
        
        /// <param name="Roomlist"> 룸들의 리스트 </param>
        private List<S_GameRoomList.Room> Roomlist = new List<S_GameRoomList.Room>();
            /// RoomList의 호출자 
        public List<S_GameRoomList.Room> GetRoomList()
        { return Roomlist; }
        
        
        /// <summary>
        /// LogicManager의 Subject를 Subscribe
        /// </summary>
        
        // Subscribing........
        // Nothing!
        public void SubscribeManager(){
            logicmanager = GameObject.Find("LogicManager").GetComponent<LogicManager>();
        }
        
        // Lobby.cs 클래스를 추적
        public void FindMe()
        {
            lobby = GameObject.Find("LobbySceneManager").GetComponent<Lobby>();
        }
        
        /// <summary>
        /// Server Calling Gear!!!!!
        /// </summary>
        // Room 정보에 대한 패킷을 Call
        public void CallRoomList(){
            
            LobbyManager.Instance.OnLobbyPacketSubject.Subscribe(rooms => { getRoomListPacket(rooms); });
            C_GameRoomList roomListPacket = new C_GameRoomList();
            HoldemClientMain.instance.NetworkManager.Send(roomListPacket.Write());
        }
        // Room 버튼을 눌렀을 때 작동하는 것들 (새로운 방 만들기)
        public void CallNewRoom(string roomname, int roomlimit)
        {
            C_CreateGameRoom pkt = new C_CreateGameRoom();
            pkt.roomLimit = roomlimit;
            pkt.roomName = roomname;
            HoldemClientMain.instance.NetworkManager.Send(pkt.Write());
            logicmanager._RoomInfoManager.initlize();
            //GotoGameRoom();
        }
        // (기존 방에 들어가기)
        public void CallGoToRoom(string roomname, string roomid)
        {
            C_GameRoomConnectCall pkt = new C_GameRoomConnectCall();
            pkt.roomId = int.Parse(roomid);
            HoldemClientMain.instance.NetworkManager.Send(pkt.Write());
            logicmanager._RoomInfoManager.initlize();
            //GotoGameRoom();
        }
        
        // Receve! 해당 RoomScene로 이동
        public void GotoGameRoom()
        {
            Debug.LogWarning("goto InGameRoom");
            Scenemanager.instance.ChangeScene(3);
        }
        // Receve! Room 정보에 대한 패킷을 저장 후 Draw
        private void getRoomListPacket(IPacket pkt){
            S_GameRoomList roomList = pkt as S_GameRoomList;
            Roomlist.Clear();
            foreach (S_GameRoomList.Room room in roomList.rooms)
            {
                Roomlist.Add(room);
            }

            lobby.DrawGameRoom(Roomlist);
            
        }

    }
    
}