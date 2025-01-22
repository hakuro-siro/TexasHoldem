using System;
using UnityEngine;

namespace CardGame.logic.Prepabs.Holdem
{
    public class MyPos : MonoBehaviour
    {
        public string myname;
        public int mychip;
        public int myid;
        public MyHand myhand;

        public void setmyname(string _myname)
        {
            myname = _myname;
            myhand.myname.text = myname;
        }
        public void setmychip(int _mychip)
        {
            string text;
            mychip = _mychip;
            if(mychip == 0)
                text = "$ " + 0;
            else
            {
                if (GetThousandCommaText(_mychip) == null)
                {
                    int u = 0;
                    text = "$ " + u;
                }
                else
                {
                    text = "$ " + GetThousandCommaText(_mychip);
                }
            }

            myhand.mymoney.text = text;
        }

        public void setdelarButton(bool isb)
        {
            myhand.DelarButton.SetActive(isb);
        }
        public string GetThousandCommaText(int data)
        {
            return string.Format("{0:#,###}", data);
        }
    }
}