using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using CardGame.logic;
using TMPro;
using Popup;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace CardGame.view
{
    using UnityEngine.SceneManagement;

    public class Title : MonoBehaviour
    {
        /// <summary>
        /// <param name="nickNameInput"> �г��� input �� </param>
        /// </summary>
        [SerializeField]
        private TMP_InputField nickNameInput;

        public Button btn;
        public Canvas mycanvas;
        private void Start()
        {
            PopupManager.instance.setCanvas(mycanvas);
            // LogicManager.instance.errorgear.ErrorCaller(0,"���߿���!");
            // LogicManager.instance.errorgear.ErrorCaller(0,"���߿���!2");
            // LogicManager.instance.errorgear.ErrorCaller(0,"���߿���!3");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SigninMyNickname();
                LogicManager.instance.titlegear.ConnectServer();
            }
        }
        /// <summary>
        /// titlegear�� ȣ���ؼ� connect
        /// </summary>
        public void startButton()
        {
            int cfrl = LogicManager.instance.CheckText(nickNameInput.text, false);
            if (cfrl == 0)
            {
                // TODO NetworkManager���� Connectȣ��
                LogicManager.instance.titlegear.ConnectServer();
            }
        }

        public void SigninMyNickname()
        {
            LogicManager.instance.SetPlayerName(nickNameInput.text);

        }
    }
}