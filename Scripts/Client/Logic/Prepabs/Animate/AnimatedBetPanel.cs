using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.logic.Prepabs.Animate
{
    public class AnimatedBetPanel : MonoBehaviour
    {
        //for debug 
        //public GameObject posobj;
        public Transform startpos;
        public Transform despos;
        public Transform sdpos;
        public Transform dspos;
        public float speed = 20f;
        private float startTime;
        private float movedDist;
        private float length;
        private void Awake()
        {
            sdpos = startpos;
            dspos = despos;
            this.transform.position = startpos.position;
            startTime = Time.time;
            change_downpos();
        }
        public void change_uppos()
        {
            startpos = sdpos;
            despos = dspos;
            startTime = Time.time;
            length = Vector3.Distance(this.transform.position, despos.position); 

        }
        public void change_downpos()
        {
            startpos = dspos;
            despos = sdpos;
            startTime = Time.time;
            length = Vector3.Distance(this.transform.position, despos.position); 

        }
        private void Start()
        {
            length = Vector3.Distance(this.transform.position, despos.position); 
        }

        private void Update()
        {
            float distCover = (Time.time - startTime) * speed;
            float frac = distCover / length;
            if (this.transform.position != despos.position)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, despos.position, frac);
            }
            
        }
    }
}