using UnityEngine;
using TMPro;

namespace CardGame.logic.Prepabs.Holdem
{
    public class BetPanel : MonoBehaviour
    {
        public GameObject checkbtn;
        public GameObject callbtn;
        public GameObject betbtn;
        public GameObject raisebtn;
        public GameObject foldbtn;
        public GameObject allinbutton;
        public GameObject mymoneytxt;

        public bool check;
        public bool call;
        public bool bet;
        public bool raise;
        public bool fold;
        public bool allin;

        public void ActivebyBool()
        {
            if(bet)
                betbtn.SetActive(bet);
            if(call)
                callbtn.SetActive(call);
            if(check)
                checkbtn.SetActive(check);
            if(fold)
                foldbtn.SetActive(fold);
            if(raise)
                raisebtn.SetActive(raise);
            if(allin)
                allinbutton.SetActive(allin);
        }
        public void setactive(bool t)
        {
            checkbtn.SetActive(t);
            callbtn.SetActive(t);
            betbtn.SetActive(t);
            raisebtn.SetActive(t);
            foldbtn.SetActive(t);
            allinbutton.SetActive(t);
        }
        public void setactivefalse(bool t = false)
        {
            checkbtn.SetActive(t);
            callbtn.SetActive(t);
            betbtn.SetActive(t);
            raisebtn.SetActive(t);
            foldbtn.SetActive(t);
            allinbutton.SetActive(t);
        }
    }
}