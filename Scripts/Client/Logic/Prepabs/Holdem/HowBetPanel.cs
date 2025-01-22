using System;
using CardGame.view.holdemview;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CardGame.logic.Prepabs.Holdem
{
    public class HowBetPanel : MonoBehaviour
    {
        public MyPos myp;
        public HoldemView hv;
        public TMP_Text panelname;
        public TMP_Text min;
        public TMP_Text max;
        public TMP_Text Betmoney;
        public TMP_InputField inputmoney;
        public BetPanel BetPanel;
        public int minmoney = 10;
        public int maxmoney = 100000;
        public int outmoney;

        public Slider MoneySlider;
        public const int MaxValue = 2147483647;
        
        public bool isbet = false;
        public bool israise = false;
        void Start()
        {
            SetFunction_UI();
            max.text = GetThousandCommaText(maxmoney).ToString();
            min.text = GetThousandCommaText(minmoney).ToString();
        }

        public void setIsbet()
        {
            isbet = true;
            israise = false;
        }        
        public void setIsraise()
        {
            isbet = false;
            israise = true;
        }
        
        private float tofloat;

        public void setpanelname(string v)
        {
            panelname.text = v;
        }
        private void Update()
        {
            if (float.TryParse(inputmoney.text, out tofloat))
            {
                if (tofloat >= minmoney)
                {
                    if ((tofloat <= maxmoney))
                    {
                        try
                        {
                            Betmoney.text = GetThousandCommaText(tofloat) + " $";
                            outmoney = int.Parse(inputmoney.text);

                        }
                        catch
                        {

                        }


                    }
                }
            }
        }

        public void Inituis()
        {
            min.text = minmoney.ToString();
            max.text = maxmoney.ToString();
            Betmoney.text = minmoney.ToString();
            inputmoney.text = minmoney.ToString();
            outmoney = minmoney;
            MoneySlider.value = 0;
            SetFunction_UI();
        }
        public void gobet()
        {

            Debug.Log("gobet");
            int number;
            
            if (int.TryParse(inputmoney.text, out number) == false)
            {
                LogicManager.instance.errorgear.ErrorCaller(8);
                BetPanel.ActivebyBool();
                return;
            }
            if (Mathf.Abs(outmoney) > maxmoney)
            {
                LogicManager.instance.errorgear.ErrorCaller(8);
                BetPanel.ActivebyBool();return;
            }

            if (Mathf.Abs(outmoney) < minmoney)
            {
                LogicManager.instance.errorgear.ErrorCaller(8);
                BetPanel.ActivebyBool();return;
            }
            
            int chk = LogicManager.instance.CheckText(inputmoney.text, true);
            if (chk == 0)
            {
                Debug.Log("outmoney"+Mathf.Abs(outmoney));
                if (isbet)
                { 
                    BetPanel.setactivefalse();
                    hv.Bet(Mathf.Abs(outmoney));
                }
                else
                { 
                    BetPanel.setactivefalse();
                    hv.Raise(Mathf.Abs(outmoney));
                }
            }
        }

    private void SetFunction_UI()
        {
            //Reset
            ResetFunction_UI();
            MoneySlider.onValueChanged.AddListener(Function_Slider);
        }
        
        private void Function_Slider(float _value)
        {
            outmoney = (int)_value;
            inputmoney.text = outmoney.ToString();
            Betmoney.text = GetThousandCommaText(_value)+" $";
        }
        public string GetThousandCommaText(float data)
        {
            return string.Format("{0:#,###}", data);
        }
        private void ResetFunction_UI()
        {
            MoneySlider.onValueChanged.RemoveAllListeners();
            MoneySlider.maxValue = maxmoney;
            MoneySlider.minValue = minmoney; 
            MoneySlider.wholeNumbers = true;
        }
    }
}