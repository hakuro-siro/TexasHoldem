using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using CardGame.logic;
using CardGame.view;

/// <summary>
/// :: Holdem_client_main ::
/// 
/// 게임의 시작부
/// Logic ,View 를 모두 실장해서 대기시킨다.
/// 
/// </summary>
namespace CardGame
{
    public class HoldemClientMain : MonoBehaviour
    {
        /// <param name="GameManager"> HoldemClientMain 을 소유하고있는 Object </param>
        /// <param name="SceneManager"> SceneManager 을 소유하고있는 Object </param>
        /// <param name="NetworkManager"> NetworkManager 을 소유하고있는 Object </param>
 
        public GameObject GameManager;
        public GameObject SceneManager;
        public NetworkManager NetworkManager;

        public static HoldemClientMain instance = null;

        void Awake(){
            if(instance == null){
                instance = this;
            }
        }

        void Start()
        {
            Debug.Log("Holdem_Client_main.......");

            // 실제 구동부의 Manager들을 등록
            GameManager = GameObject.Find("GameManager");
            SceneManager = GameObject.Find("SceneManager");
            NetworkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
            
            // View와 Logic을 등록. 
            View view = new View();
            Logic logic = new Logic();
            
            
            //게임 시작
            view.ViewInit();
            logic.LogicInit();

        }

    }
}
