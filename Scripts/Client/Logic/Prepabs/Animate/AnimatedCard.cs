using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.logic.Prepabs.Animate
{
    public class AnimatedCard : MonoBehaviour
    {
        //for debug 
        //public GameObject posobj;
        public Transform startpos;
        public Vector3 despos;
        private float speed = 40f;
        private float startTime;
        private float movedDist;
        private float length;
        public bool inited =false;
        private void Awake()
        {
            //for debug 
            //despos = posobj.transform.position;
            startTime = Time.time;
        }

        private void Start()
        {
            if (startpos != null)
            {
                this.transform.position = startpos.position;
                length = Vector3.Distance(this.transform.position, despos);
                inited = true;
            }
        }

        public void Resign()
        {
            this.transform.position = startpos.position;
            length = Vector3.Distance(this.transform.position, despos);
            inited = true;
        }
        private void Update()
        {
            if (inited)
            {
                float distCover = (Time.time - startTime) * speed;
                float frac = distCover / length;
                this.transform.position = Vector3.Lerp(this.transform.position, despos, frac);
            }
        }
    }
}