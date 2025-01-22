/// <summary>
/// :: Logic ::
/// 
/// Logic은 모든 게임 내부의 기능을 담당합니다 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace CardGame.logic
{
    public class Logic
    {
        /// <summary>
        /// <@param> logicManager 
        /// 모든 기어를 총괄하는 logicmanager를 가지고있는 Gameobj. 이후 확장성 고려해 obj로 가져옴
        /// </summary>
        public void LogicInit()
        {
            
            // Init Logic Manager.........
            Debug.Log("Logic....");
            GameObject logicManager = GameObject.Find("LogicManager");
            logicManager.GetComponent<LogicManager>().InitLogicManager();

        }

    }
}