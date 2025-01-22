using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using TMPro;
namespace Script.Popup
{
    public class TemplatePopup : Popup
    {
        [SerializeField]
        public TMP_Text titleText;

        [SerializeField]
        public Text bodyText;
        

        public void OnButtonPressed()
        {
           
        }


        public void OnCloseButtonPressed()
        {
            Close();
        }

    }
}
