using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Text.RegularExpressions;
using CardGame.view;


namespace CardGame.logic
{

    public class LogicManager : MonoBehaviour
    {
        public static LogicManager instance = null;

        public RoomInfoManager _RoomInfoManager;
        
        /// <summary>
        ///  Subject 등록
        /// </summary>

        // PlayerName에 관련한 Subject
        // Subscribers
        // PlayerGear.cs
        private Subject<string> PlayerNameCallSubject = new Subject<string>();

        public IObservable<string> OnPlayerNameCallSubject
        {
            get { return PlayerNameCallSubject; }
        }

        // Server 연결에 관련한 Subject
        // Subscribers
        // LogicManager.cs
        private Subject<bool> ServerConnectedSubject = new Subject<bool>();

        public IObservable<bool> OnServerConnectedSubject
        {
            get { return ServerConnectedSubject; }
        }

        /// <summary>
        /// 인스턴스 활성화 
        /// </summary>
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void InitLogicManager()
        {
            SubscribeAllLogic();
        }

        /// <summary>
        ///  Gear들을 등록 
        /// </summary>

        public LobbyGear lobbygear;

        public PlayerGear playergear;
        public TitleGear titlegear;
        public InGameRoomGear ingameroomgear;
        public HoldemGear holdemgear;
        public ErrorGear errorgear;

        void SubscribeAllLogic()
        {
            // Gear들을 하위로 붙이기
            lobbygear = new LobbyGear();
            playergear = new PlayerGear();
            titlegear = new TitleGear();
            ingameroomgear = new InGameRoomGear();
            holdemgear = new HoldemGear();
            errorgear = new ErrorGear();
            // 구독을 실장.
            errorgear.SubscribeManager();
            holdemgear.SubscribeManager();
            titlegear.SubscribeManager();
            lobbygear.SubscribeManager();
            playergear.SubscribeManager();
            ingameroomgear.SubscribeManager();
        }


        /// <summary>
        ///  등록된 Gear를 호출
        /// </summary>

        // PlayerName을 설정
        public void SetPlayerName(string name)
        {
            PlayerNameCallSubject.OnNext(name);
        }

        // 서버 연결
        public void Connected(bool isconnect)
        {
            Debug.Log("Connected to Server");
            ServerConnectedSubject.OnNext(isconnect);
        }
        
        /// <summary>
        /// 텍스트 입력 처리
        /// </summary>
        /// <param name="t"></param>
        /// <param name="numberOption"></param>
        /// <returns> 0 = 정상 1 = 공백 2 = 숫자만 필요</returns>
        public int CheckText(string t,bool numberOption)
        {
            int number;
            if (t == "")
            {
                errorgear.ErrorCaller(1);
                return 1;
            }

            if (numberOption)
            {
                if (int.TryParse(t, out number)==false) 
                {
                    errorgear.ErrorCaller(2);
                    return 2;
                }
            }

            return 0;
        }
    }


}