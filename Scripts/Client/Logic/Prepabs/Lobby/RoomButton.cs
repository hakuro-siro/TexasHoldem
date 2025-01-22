using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace CardGame.logic.Prepabs.Lobby
{

    public class RoomButton : MonoBehaviour
    {
        public string RoomName;
        public string RoomId;
        public bool IsIngame;
        public string RoomLimit;
        public GameObject IsIngameOBJ;
        public TMP_Text RoomnameText;
        public TMP_Text RoomlimitText;
        
        
        public string getRoomName()
        {return RoomName; }
        public void setRoomName(string Roomname)
        { RoomName = Roomname; RoomnameText.text = Roomname;}
        public string getRoomId()
        {return RoomId; }
        public void setRoomId(string Roomid)
        { RoomId = Roomid; }
        public bool getIsIngame()
        {return IsIngame; }
        public void setIsIngame(bool isingame)
        { IsIngame = isingame; IsIngameOBJ.SetActive(isingame); }
        public string getRoomLimit()
        {return RoomLimit; }
        public void setRoomLimit(string roomlimit)
        { RoomLimit = roomlimit; RoomlimitText.text = roomlimit;}

        public void onPointerup()
        {
            GameObject manager = GameObject.Find("LobbySceneManager");
            manager.GetComponent<view.lobby.Lobby>().RoomBtnOnClick(RoomName,RoomId);
        }
    }

}