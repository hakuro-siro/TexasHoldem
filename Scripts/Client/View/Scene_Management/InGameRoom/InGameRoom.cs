using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardGame.logic;
using CardGame.logic.Prepabs.InGameRoom;
using TMPro;
using UnityEngine.UI;
using Popup;

namespace CardGame.view.ingameroom {
    
    /// <summary>
    /// InGameRoom 의 UI view 드로잉을 담당합니다. 
    /// </summary>
    public class InGameRoom : MonoBehaviour
    {
        public Canvas mycanvas;
        // InGameRoomObjManager
        [SerializeField]
        private InGameRoomObjManager _inGameRoomObjManager;
        
        public GameObject PlayerOBJ;
        public List<GameObject> PlayerOBJPool;
      
        public GameObject Textline;
        public List<GameObject> TextlineOBJPool;

        public TMP_InputField inputfield;
        
        void Awake()
        {
            inputfield.ActivateInputField();
            Debug.Log("LobbyScene");
            LogicManager.instance.ingameroomgear.FindMe();
            LogicManager.instance._RoomInfoManager.CallRoomGear();
            
            C_EnterOk pkt = new C_EnterOk();
            HoldemClientMain.instance.NetworkManager.Send(pkt.Write());
        }

        private void Start()
        {
           PopupManager.instance.setCanvas(mycanvas);
        }

        private void Update()
        {
           
            if (Input.GetKeyDown(KeyCode.Return))
                if (inputfield.text !="")
                {
                    C_GameChat chatpkt = new C_GameChat();
                    chatpkt.playerName = LogicManager.instance.playergear.GetPlayername();
                    chatpkt.chat = inputfield.text;
                    LogicManager.instance.ingameroomgear.ServerCall(chatpkt);
                    inputfield.text = null;
                    inputfield.ActivateInputField();
                }
                else
                {
                    inputfield.ActivateInputField();
                }
        }

        public void DrawColorChat(string text, int color)
        {
            switch (color)
            {
                case 1:
                    GameObject _chattext = Instantiate(Textline);
                    _chattext.GetComponent<TMP_Text>().text = text;
                    _chattext.GetComponent<TMP_Text>().color = Color.red;
                    _chattext.transform.SetParent(_inGameRoomObjManager.ChatBox.transform,false);
                    TextlineOBJPool.Add(_chattext);
                    break;
            }
            LogicManager.instance.errorgear.ErrorCaller(7);
        }
        //TODO 텍스트 오브젝트 삭제하는 로직
        public void DrawChat(string text)
        {
            GameObject _chattext = Instantiate(Textline);
            _chattext.GetComponent<TMP_Text>().text = text;
            _chattext.transform.SetParent(_inGameRoomObjManager.ChatBox.transform,false);
            TextlineOBJPool.Add(_chattext);

        }
        public void DrawPlayers(List<S_GameRoomInfo.Players> playersList)
        {
            ClearObjPool(PlayerOBJPool);
            foreach (var player in  playersList)
            {
                GameObject _playerobj = Instantiate(PlayerOBJ);
                Debug.Log(" name : "+player.playerName+"mine : "+player.isMine + " isready : "+player.isReady+" isleader : "+player.isLeader);
                
                _playerobj = InitPlayerInfo(_playerobj,player);
                _playerobj.transform.SetParent(_inGameRoomObjManager.PlayerBox.transform,false);
                PlayerOBJPool.Add(_playerobj);
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
        
        public GameObject InitPlayerInfo(GameObject p, S_GameRoomInfo.Players player)
        {
            if (player.isMine)
                MyPlayerInfo(p,player);
            PlayerInfoPref pref = p.GetComponent<PlayerInfoPref>();

            if (player.isReady)
            { 
                pref.ReadyUI.SetActive(true);
                if ((player.isLeader == false) && player.isMine)
                {
                    _inGameRoomObjManager.unReadyButton.SetActive(true);
                    _inGameRoomObjManager.ReadyButton.SetActive(false);
                }
                if (player.isLeader&&player.isMine)
                {
                    _inGameRoomObjManager.unReadyButton.SetActive(false);
                    _inGameRoomObjManager.ReadyButton.SetActive(false);
                }
            }
            else 
            {
                pref.ReadyUI.SetActive(false);
                if ((player.isLeader == false)  && player.isMine)
                {
                    _inGameRoomObjManager.unReadyButton.SetActive(false);
                    _inGameRoomObjManager.ReadyButton.SetActive(true);
                }
                if (player.isLeader && player.isMine)
                {
                    _inGameRoomObjManager.ReadyButton.SetActive(false);
                    _inGameRoomObjManager.unReadyButton.SetActive(false);
                }
               
            }

            pref.playername.text = player.playerName;
           
            
            return p;
        }

        public void MyPlayerInfo(GameObject p,S_GameRoomInfo.Players player)
        {
           
            PlayerInfoPref pref = p.GetComponent<PlayerInfoPref>();
            pref.isMyUI.SetActive(true);
            if (player.isLeader)
            {
                LogicManager.instance.playergear.SetLeader(true);
                _inGameRoomObjManager.ReadyButton.SetActive(false);
                _inGameRoomObjManager.StartButton.SetActive(true);
            }else
            {
                LogicManager.instance.playergear.SetLeader(false);
                _inGameRoomObjManager.StartButton.SetActive(false);
                _inGameRoomObjManager.ReadyButton.SetActive(true);
            }
        }
        
        
        
        public void DrawRoomName(string roomname)
        {
            _inGameRoomObjManager.RoomName.text = roomname;
            _inGameRoomObjManager.PlayerName.text = LogicManager.instance.playergear.GetPlayername();
        }

        public void onGetOutButton()
        {
            LogicManager.instance.playergear.SetLeader(false);
            LogicManager.instance.ingameroomgear.OutRoom();
        }
        public void onGetReadyButton()
        {
            C_InGameReady pkt = new C_InGameReady();
            LogicManager.instance.ingameroomgear.ServerCall(pkt);
        }
        public void onGetunReadyButton()
        {
            C_InGameReady pkt = new C_InGameReady();
            LogicManager.instance.ingameroomgear.ServerCall(pkt);
        }

        public void onGetGameStartButton()
        {
            if (PlayerOBJPool.Count < 3)
            {
                LogicManager.instance.errorgear.ErrorCaller(4);
            }
            else
            {
                C_GameStart startpkt = new C_GameStart();
                LogicManager.instance.ingameroomgear.ServerCall(startpkt);
            }
        }
    }
}
