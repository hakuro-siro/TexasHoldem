using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.logic.Prepabs.Holdem;
using UnityEngine;

namespace CardGame.view.holdemview
{
    public class HoldemObjManager : MonoBehaviour
    {
        public MyPos mypos;

        [Header ("공개카드 위치")]
        
        public GameObject deckpos;
        public GameObject cardpos1;
        public GameObject cardpos2;
        public GameObject cardpos3;
        public GameObject cardpos4;
        public GameObject cardpos5;
        public List<GameObject> cardpos;

        public GameObject betPanelStartpos;
        public GameObject betPanelDespos;
        public GameObject betPanel;
        
        [Header("플레이어 위치")]
        
        public GameObject Betmoney;
        public GameObject Pandonmoney;
        public GameObject playerpos1;
        public GameObject playerpos2;
        public GameObject playerpos3;
        public GameObject playerpos4;
        public GameObject playerpos5;
        public GameObject playerpos6;
        public GameObject playerpos7;
        public Transform Center;
        public List<GameObject> poslist;

        [Header ("스포트라이트 위치")]
        
        public GameObject MySpot;
        public GameObject Spotpos1;
        public GameObject Spotpos2;
        public GameObject Spotpos3;
        public GameObject Spotpos4;
        public GameObject Spotpos5;
        public GameObject Spotpos6;
        public GameObject Spotpos7;
        public List<GameObject> Spotlist;

        private void Awake()
        {
            poslist.Add(playerpos1);
            poslist.Add(playerpos2);
            poslist.Add(playerpos3);
            poslist.Add(playerpos4);
            poslist.Add(playerpos5);
            poslist.Add(playerpos6);
            poslist.Add(playerpos7);
            cardpos.Add(cardpos1);
            cardpos.Add(cardpos2);
            cardpos.Add(cardpos3);
            cardpos.Add(cardpos4);
            cardpos.Add(cardpos5);
            Spotlist.Add(Spotpos1);
            Spotlist.Add(Spotpos2);
            Spotlist.Add(Spotpos3);
            Spotlist.Add(Spotpos4);
            Spotlist.Add(Spotpos5);
            Spotlist.Add(Spotpos6);
            Spotlist.Add(Spotpos7);
        }
        public GameObject getcardpos(int posnum)
        {
            Debug.Log("posnum"+posnum);
            return cardpos[posnum];
        }
        public GameObject getspotpos(int posnum)
        {
            return Spotlist[posnum-1];
        }
        public GameObject getplayerpos(int posnum)
        {
            return poslist[posnum - 1];
        }
    }
}