using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardGame.logic;
using CardGame.logic.Prepabs.Lobby;
using TMPro;
using UnityEngine.EventSystems;
using Popup;


namespace CardGame.view.lobby {

    /// <summary>
    /// Lobby 의 UI view 드로잉을 담당합니다. 
    /// </summary>
    public class Lobby : MonoBehaviour
    {
        private List<S_GameRoomList.Room> Roomlist = new List<S_GameRoomList.Room>();

        public GameObject Room;
        public List<GameObject> RoomObjPool;

        /// <summary>
        /// Scene 내부의 UI item
        /// </summary>
        public Canvas mycanvas;

        // LobbyObjectManager
        [SerializeField] private LobbyObjManager _lobbyObjManager;

        // RoomList
        private GameObject RoomListGrid;

        // New Room
        private GameObject newRoomPannel;
        private TMP_InputField roomNameInput;
        private TMP_InputField roomLimitInput;

        /// <summary>
        /// LobbyGear에게 자신을 등록, View 드로잉에 필요한 패킷 요청과 실제 드로잉 Call을 요청.
        /// </summary>
        void Awake()
        {
            Debug.Log("LobbyScene");
            InitLobbyObjects();
            LogicManager.instance.lobbygear.CallRoomList();
            LogicManager.instance.lobbygear.FindMe();
        }

        private void Start()
        {
            PopupManager.instance.setCanvas(mycanvas);
        }

        void InitLobbyObjects()
        {
            RoomListGrid = _lobbyObjManager.GetRoomList().GetRoomListGrid();
            newRoomPannel = _lobbyObjManager.GetNewRoomPanel().GetNewRoomPannel();
            roomNameInput = _lobbyObjManager.GetNewRoomPanel().GetRoomNameInput();
            roomLimitInput = _lobbyObjManager.GetNewRoomPanel().GetRoomLimitInput();
        }

        /// <summary>
        /// GameRoom에 해당하는 Ui를 Draw
        /// <param name="roomlist"> 룸의 리스트의 string 값의 List </param>
        /// </summary>
        public void DrawGameRoom(List<S_GameRoomList.Room> roomlist)
        {
            ClearObjPool(RoomObjPool);
            UnityEngine.Debug.Log("DrawRoomList");
            Roomlist = roomlist;
            for (int i = 0; i < Roomlist.Count; i++)
            {
                S_GameRoomList.Room roominfo = Roomlist[i];
                GameObject room = Instantiate(Room);
                room = InitRoomBtnInfo(room, roominfo);
                RoomObjPool.Add(room);
            }

            DrawRoomListObjPool(RoomObjPool);
        }

        public void togNewRoomPanel()
        {
            newRoomPannel.SetActive(!newRoomPannel.activeSelf);
        }

        public void getNewRoomInputs()
        {
            NewRoomCall();
        }

        /// <summary>
        /// Gear에 Call 하는 함수의 모임
        /// </summary>
        public void RefreshRoomList()
        {
            LogicManager.instance.lobbygear.CallRoomList();

        }

        public void NewRoomCall()
        {
            int ckv = LogicManager.instance.CheckText(roomNameInput.text, false);
            ckv = LogicManager.instance.CheckText(roomLimitInput.text, true);

            if (ckv == 0){
                if(int.Parse(roomLimitInput.text)<=2)
                    LogicManager.instance.errorgear.ErrorCaller(3);
                else if (int.Parse(roomLimitInput.text) > 8)
                    roomLimitInput.text = "8";
                else
                    LogicManager.instance.lobbygear.CallNewRoom(roomNameInput.text, int.Parse(roomLimitInput.text));

            }
        }

    /// <summary>
        /// object에 개성을 더하는 함수의 모임
        /// </summary>
        private GameObject InitRoomBtnInfo(GameObject room,S_GameRoomList.Room roominfo)
        {
            room.GetComponent<RoomButton>().setRoomName(roominfo.roomName);
            room.GetComponent<RoomButton>().setRoomId(""+roominfo.roomId);
            room.GetComponent<RoomButton>().setIsIngame(roominfo.isStartGame);
            room.GetComponent<RoomButton>().setRoomLimit(roominfo.roomLimit+"/"+roominfo.roomPlayers);
            Debug.Log(room.GetComponent<RoomButton>().getRoomName());
            return room;
        }
            

        public void RoomBtnOnClick(string Roomname,string Roomid)
        {
            Debug.Log(Roomname);
            Debug.Log(Roomid);
            LogicManager.instance.lobbygear.CallGoToRoom(Roomname, Roomid);
        }
        /// <summary>
        /// objectPool의 총관리
        /// </summary>
        private void DrawRoomListObjPool(List<GameObject> objpool)
        {
            foreach (var obj in objpool)
            {
                obj.transform.SetParent(RoomListGrid.transform,false);
                //obj.GetComponent<Button>().onClick.AddListener(() => RoomBtnOnClick(obj.GetComponent<RoomButton>().getRoomName(),obj.GetComponent<RoomButton>().getRoomId()));
                obj.SetActive(true);
            }
        }
        private void ClearObjPool(List<GameObject> objpool)
        {
            foreach (GameObject obj in objpool)
            { 
                Destroy(obj);
            }
            objpool.Clear();
        }

    }
    
}