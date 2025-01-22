using UnityEngine;

namespace CardGame.logic.Prepabs.Holdem
{
    public class EnemyPos : MonoBehaviour
    {
        public string enemyname;
        public int enemychip;
        public int enemyid;
        public EnemyHand _enemyHand;
        public GameObject foldcutton;
        
        public void setenemyname(string _enemyname)
        {
            enemyname = _enemyname;
            _enemyHand.enemyname.text = enemyname;
        }
        public void setenemychip(int chip)
        {
            string text;
            if (GetThousandCommaText(chip) == null)
            {
                int u = 0;
                text = "$ " + u;
            }
            else
            {
                text = "$ " + GetThousandCommaText(chip);
            }

            if (chip == 0)
            {
                text = "$ " + 0;
                Debug.Log("moneyzero" + chip);
            }
            Debug.Log("money" + chip);
            enemychip = chip;
            _enemyHand.enemymoney.text = text.ToString();
        }
        public void setenemyid(int _enemyid)
        {
            enemyid = _enemyid;
        }
        public string GetThousandCommaText(int data)
        {
            return string.Format("{0:#,###}", data);
        }
        public void setdelarButton(bool isb)
        {
            _enemyHand.DelarButton.SetActive(isb);
        }
    }
}