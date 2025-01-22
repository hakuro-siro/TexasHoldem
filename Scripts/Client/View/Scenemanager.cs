using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System;

namespace CardGame.view
{
    /// <summary>
    /// Scnemanager 
    /// 씬 이동 등에 관련된 Subject 를 관리, 갱신해줍니다. 모든 Scene 파생은 Scenemanager를 구독합니다.
    /// </summary>
    public class Scenemanager : MonoBehaviour
    {
        // Init instance........
        public static Scenemanager instance = null;
        public int scenedata;
        void Awake(){
            if(instance == null){
                instance = this;
            }
        }

        // Init Subject........
        private Subject<int> SceneChangeSubject = new Subject<int>();

        public IObservable<int> OnSceneChangeSubject
        {
            get { return SceneChangeSubject; }
        } 
        
        
        /// <summary>
        /// Call Subject.......
        /// </summary>

        /// Scene 변경
        /// <param name="to"> 이동할 씬의 목적지. </param>
        public void ChangeScene(int to){
            SceneChangeSubject.OnNext(to);
        }
        public void ChangeSceneBack(){
            SceneChangeSubject.OnNext(scenedata-1);
        }
        
    }

}
