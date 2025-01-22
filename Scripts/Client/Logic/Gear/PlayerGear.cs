using System;
using UniRx;
using UnityEngine;
namespace CardGame.logic
{
    public class PlayerGear
    {
        /// <summary>
        /// Gear들은 모두 LogicManager를 가지고 있습니다.
        /// </summary>
        [SerializeField] 
        private LogicManager logicmanager;
        private string Playername;
        private bool IsLeader;
        public void SetLeader(bool isleader)
        {
            Debug.LogWarning("LEADER SET"+isleader);
            IsLeader = isleader;
        }
        public bool GetLeader()
        {
            return IsLeader;
        }
        public string GetPlayername()
        {
            return Playername;
        }
        /// <summary>
        /// LogicManager의 Subject를 Subscribe
        /// </summary>
        
        // Subscribing............
        // OnPlayerNameCallSubject
        public void SubscribeManager(){
            logicmanager = GameObject.Find("LogicManager").GetComponent<LogicManager>();
            logicmanager.OnPlayerNameCallSubject.Subscribe(name => { Playername = name;});
        }


    }
}