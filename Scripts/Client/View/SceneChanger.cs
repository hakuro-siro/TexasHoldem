using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace CardGame.view
{
    using UnityEngine.SceneManagement;

    public class SceneChanger : MonoBehaviour
    {
        
        /// <summary>
        /// Scenemanager 를 씬에서 얻어서 고정함.
        /// </summary>
        
        [SerializeField] 
        private Scenemanager Scenemanager;

        public int Myscene = 0;
        /// <summary>
        /// Scenemanager 의 Subject 를 Subscribe
        /// </summary>
        public void ChangeSceneEx(){
            Scenemanager.OnSceneChangeSubject.Subscribe(to=>{
                StartCoroutine(ChangeSceneCoroutine(to));
            });
        }

        
        /// <summary>
        /// 비동기 씬 변경 루틴 시작.
        /// </summary>
        IEnumerator ChangeSceneCoroutine(int sceneNum){
            if (Myscene > 0)
            {
                SceneManager.UnloadSceneAsync(Myscene);
            }
            
            SceneManager.LoadSceneAsync(sceneNum, LoadSceneMode.Additive);
            Myscene = sceneNum;
            yield return new WaitForSeconds(1);
            Scenemanager.instance.scenedata = sceneNum;
        }


    }
}