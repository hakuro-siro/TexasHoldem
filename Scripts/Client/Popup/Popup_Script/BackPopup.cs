using CardGame.view;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using TMPro;
namespace Script.Popup
{
    public class BackPopup : Popup
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
            Scenemanager.instance.ChangeSceneBack();
        }

    }
}
