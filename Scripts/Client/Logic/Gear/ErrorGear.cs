using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Popup;
namespace CardGame.logic
{
    public class ErrorGear
    {
        /// <summary>
        /// Gear들은 모두 LogicManager를 가지고 있습니다.
        /// </summary>
        [SerializeField] 
        private LogicManager logicmanager;

        
        /// <summary>
        /// LogicManager의 Subject를 Subscribe
        /// </summary>
        
        // Subscribing............
        // OnPlayerNameCallSubject
        public void SubscribeManager(){
            logicmanager = GameObject.Find("LogicManager").GetComponent<LogicManager>();
        }

        public void ErrorOccured(int errcode)
        {
            ErrorCaller(errcode);            
        }
        
        /// <summary>
        /// Errorcode 0 = commonError
        /// Errorcode 1 = 문자를 입력해주세요
        /// Errorcode 2 = 숫자만 입력해주세요
        /// Errorcode 3 = 3명 이상 8명 이하만 가능합니다
        /// Errorcode 4 = 최소 3명 이상이 필요합니다.
        /// </summary>
        /// <param name="Errorcode"></param>
        /// <param name="ErrorMessage"></param>
        /// 
        Dictionary<int, string> Errors = new Dictionary<int, string>()
        {
            {0, "commonError."},
            {1, "문자를 입력해주세요."},
            {2, "숫자만 입력해주세요."},
            {3, "3명 이상 8명 이하만 가능합니다."},
            {4, "최소 3명 이상이 필요합니다."},
            {5, "문자를 입력해주세요."},
            {6, "문자를 입력해주세요."},
            {7, "모두 레디하지 않았습니다."},
            {8, "올바른 값을 입력해주세요"},
            
            {101, "이미 진행중인 게임입니다."},
            {102, "방 인원이 가득 찼습니다."},
            {103, "존재하지 않는 방입니다."},
            {104, "이미 진행중인 게임입니다"},
            {105, "이미 진행중인 게임입니다"},
        };

        public void ErrorCaller(int Errorcode)
        {
            PopupManager.instance.openTemplate(Errorcode,Errors[Errorcode]);   
        }
        
        
        
        
        

    }
}