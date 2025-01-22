using System;
using UniRx;
using System.Collections.Generic;
using UnityEngine;
using CardGame.view;
using CardGame.view.ingameroom;

namespace CardGame.logic
{
    public class InGameRoomGear
    {
        // Lobby.cs 의 클래스
        private InGameRoom _inGameRoom;
        private string ChatText;
        
        private List<S_GameRoomInfo.Players> PlayerList;
        /// <summary>
        /// Gear들은 모두 LogicManager를 가지고 있습니다.
        /// </summary>
        [SerializeField] 
        private LogicManager logicmanager;
        
        /// <summary>
        /// LogicManager의 Subject를 Subscribe
        /// </summary>
        
        // Subscribing............
        // nothing!
        public void SubscribeManager()
        {
            PlayerList = new List<S_GameRoomInfo.Players>();
            logicmanager = GameObject.Find("LogicManager").GetComponent<LogicManager>();
       }
        
        
        // InGameRoom.cs 클래스를 추적
        public void FindMe()
        {
            _inGameRoom = GameObject.Find("InGameRoomManager").GetComponent<InGameRoom>();
        }

        public void InitRoomData(S_GameRoomInfo roompacket)
        {
            //todo 여기서 룸 정보를 갈무리
            PlayerList.Clear();
            //players
            foreach (var playerinfo in roompacket.playerss)
            {
                PlayerList.Add(playerinfo);
            }
            _inGameRoom.DrawPlayers(PlayerList);
            //string    
            _inGameRoom.DrawRoomName(roompacket.roomName);
        }

        public void ServerCall(C_InGameReady v)
        { HoldemClientMain.instance.NetworkManager.Send(v.Write()); }
        public void ServerCall(C_GameChat v)
        { HoldemClientMain.instance.NetworkManager.Send(v.Write()); }
        
        public void ServerCall(C_LeaveGame v)
        { HoldemClientMain.instance.NetworkManager.Send(v.Write()); }
        public void ServerCall(C_GameStart v)
        { HoldemClientMain.instance.NetworkManager.Send(v.Write()); }

        /// <summary>
        /// outroom 패킷송신부
        /// </summary>
        public void OutRoom()
        { 
            Debug.Log("outroombtn");
            C_LeaveGame leavepkt = new C_LeaveGame();
            ServerCall(leavepkt);
        }
        /// <summary>
        /// receve packet 실제로 outroom 진행
        /// </summary>
        public void OutRoomIns()
        {
            Debug.LogWarning("outroom");
            Scenemanager.instance.ChangeScene(2);
        }
        /// <summary>
        /// 이하동일 gotoingame을 실제로 시작
        /// </summary>
        public void GotoInGame()
        {
            Debug.LogWarning("goto HOLDEM");
            Scenemanager.instance.ChangeScene(4);
        }
        /// <summary>
        /// 일반 채팅 
        /// </summary>
        /// <param name="pkt"></param>
        public void InitChatData(S_GameChat pkt)
        {
            ChatText = "";
            ChatText = ChatText + pkt.playerName;
            ChatText = ChatText +" : "+ pkt.chat;
            _inGameRoom.DrawChat(ChatText);
        }
        /// <summary>
        /// 색깔이 같이 있는 채팅
        /// </summary>
        /// <param name="pkt"></param>
        /// <param name="colorid"></param>
        public void InitChatDataWithColor(string pkt,int colorid)
        {
            //red
            switch (colorid)
            {
                case 1:
                    _inGameRoom.DrawColorChat(pkt,1);
                    break;
            }
        }
    }
}