using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UniRx;

namespace CardGame.logic
{

    public class RoomInfoManager : MonoBehaviour
    {
        /// <summary>
        /// 모든 기본적인 Room에 대한 정보를 받아오도록 하기.
        /// </summary>
        // Start is called before the first frame update

        public string RoomName;

        public int RoomLimit;

        public int RoomId;

        private S_GameRoomInfo packet;
        public void initlize()
        {
            UpdateRoomData();
        }

        public void UpdateRoomData()
        {
            LobbyManager.Instance.OnRoomConnectPacketSubject.Subscribe(Roominfo => { SetRoomData(Roominfo); });
        }

        private void SetRoomData(IPacket roominfo)
        {
            packet = roominfo as S_GameRoomInfo;
            Debug.LogWarning("recv Room pkt");
            CallRoomGear();
        }

        public void CallRoomGear()
        {
            if(packet!=null)
                LogicManager.instance.ingameroomgear.InitRoomData(packet);
        }
    }
}