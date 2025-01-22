
/// <summary>
/// :: View ::
///    
/// View 는 씬의 이동, 씬 오브젝트 관리에 대한 모든 내용을 총괄합니다.
/// </summary>


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace CardGame.view
{
    public class View
    {
        /// <summary>
        /// <@param> SceneManager 
        /// 모든 기어를 총괄하는 SceneManager 가지고있는 Gameobj. 이후 확장성 고려해 obj로 가져옴
        /// </summary>
        public void ViewInit()
        {
            
            Debug.Log("View....");
            GameObject SceneManager = GameObject.Find("SceneManager");
            // SceneChanger 를 Scenemanager 에 구동.
            SceneManager.GetComponent<SceneChanger>().ChangeSceneEx();
            // Scenemanager의 초동을 구현.
            SceneManager.GetComponent<Scenemanager>().ChangeScene(1);
      
        }


    }
}