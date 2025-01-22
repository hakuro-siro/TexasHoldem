using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.logic.Prepabs.Animate
{
    public class AnimatedPanel : MonoBehaviour
    {
        //for debug 
        //public GameObject posobj;
        public Transform startpos;
        public Transform despos;
        public Transform prestartpos;
        public Transform predespos;
        public Transform revdespos;
        public Transform revstartpos;
        public float speed = 10f;
        private float startTime;
        private float movedDist;
        private float length;
        private void Awake()
        {
            prestartpos = despos;
            predespos = startpos;
            revdespos = startpos;
            revstartpos = despos;
            //for debug 
            //despos = posobj.transform.position;
            this.transform.position = startpos.position;
            startTime = Time.time;
            change_despos();
        }
        public void change_uppos()
        {
            startTime = Time.time;
            Transform pos;
            pos = prestartpos;
            startpos = predespos;
            despos = pos;           
            length = Vector3.Distance(this.transform.position, despos.position); 

        }
        public void change_downpos()
        {
            startTime = Time.time;
            Transform pos;
            pos = revdespos;
            startpos = revstartpos;
            despos = pos;           
            length = Vector3.Distance(this.transform.position, despos.position); 

        }
        public void change_despos()
        {
            startTime = Time.time;
            Transform pos;
            pos = startpos;
            startpos = despos;
            despos = pos;           
            length = Vector3.Distance(this.transform.position, despos.position); 

        }
        private void Start()
        {
            //this.transform.position = startpos.position;
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