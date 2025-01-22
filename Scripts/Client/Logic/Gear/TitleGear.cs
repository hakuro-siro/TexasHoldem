using System;
using UniRx;
using UnityEngine;
using CardGame.view;
namespace CardGame.logic
{
    public class TitleGear
    {
        /// <summary>
        /// Gear들은 모두 LogicManager를 가지고 있습니다.
        /// </summary>
        ///
        [SerializeField] 
        private LogicManager logicmanager;
        // Subscribing............
        // OnServerConnectedSubject
        public void SubscribeManager(){
            logicmanager = GameObject.Find("LogicManager").GetComponent<LogicManager>();
            logicmanager.OnServerConnectedSubject.Subscribe(isconnected =>
            {
                if (isconnected)
                    changeScene(2);
            });
        }
        
        // 서버 연결
        public void ConnectServer()
        {
            HoldemClientMain.instance.NetworkManager.Connect();
        }
        // 씬 이동 
        private void changeScene(int to)
        {
            Scenemanager.instance.ChangeScene(2);
        }
    }
}