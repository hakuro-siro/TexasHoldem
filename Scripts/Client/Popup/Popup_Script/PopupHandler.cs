using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// ParkGyeongDam
/// 팝업이 실제로 올라오는 등을 관리해줍니다 
/// </summary>
namespace Script.Popup
{
    public class PopupHandler : MonoBehaviour
    {

        [SerializeField]

        protected Canvas canvas;

        protected Stack<GameObject> currentPopups = new Stack<GameObject>();
        protected Stack<GameObject> currentPanels = new Stack<GameObject>();

        /// <summary>
        /// 팝업 오픈용 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="popupName">팝업 이름</param>
        /// <param name="darkenBackground">뒷배경 선택, true 검은배경 false 블러</param>
        /// <param name="onOpened"></param>
        public void OpenPopup<T>(string popupName,bool darkenBackground = true ,string ErrorTitle = "0", string ErrorMessage = "Error",Action<T> onOpened = null) where T : Popup
        {
            StartCoroutine(OpenPopupAsync(popupName,onOpened,darkenBackground,ErrorTitle,ErrorMessage));
        }

        public void CloseCurrentPopup()
        {
            var currentPopup = currentPopups.Peek();
            if (currentPopup != null)
            {
                currentPopup.GetComponent<Popup>().Close();
            }
        }

        public void ClosePopup()
        {
            var topmostPopup = currentPopups.Pop();
            if (topmostPopup == null)
            {
                return;
            }

            var topmostPanel = currentPanels.Pop();
            if (topmostPanel != null)
            {
                Destroy(topmostPanel);
            }
        }

        /// <summary>
        /// 팝업 로딩용 
        /// </summary>
        /// <param name="popupName"> 들고오는 팝업 이름 (주로 리소스 경로)</param> //TODO Resource -> Addressable로 변
        /// <param name="onOpened"> 팝업이 떴는지 안떳는지 판별하기 위함 </param>
        /// <param name="darkenBackground"> 팝업 뜨면 뜨는 검은 뒷배경 만들기용 </param>
        
        protected IEnumerator OpenPopupAsync<T>(string popupName, Action<T> onOpened, bool darkenBackground,string ErrorTitle, string ErrorMessage) where T : Popup
        {
            GameObject panel;
            Debug.Log("AssetLoading...");
            // 블러 올리기
            //Debug.Log(currentPanels.Count);
            if (currentPanels.Count<=0)
            {
                //검은 패널 만들기
                panel = new GameObject("Panel");
                var panelImage = panel.AddComponent<Image>();
                var color = Color.black;
                color.a = 0;
                panelImage.color = color;
                var panelTransform = panel.GetComponent<RectTransform>();
                panelTransform.anchorMin = new Vector2(0, 0);
                panelTransform.anchorMax = new Vector2(1, 1);
                panelTransform.pivot = new Vector2(0.5f, 0.5f);
                panel.transform.SetParent(canvas.transform, false);
                currentPanels.Push(panel);
                if(darkenBackground)
                    StartCoroutine(FadeIn(panel.GetComponent<Image>(), 0.2f));
            }
            else
            {
                panel = new GameObject("Panel");
                currentPanels.Push(panel);
            }
            
            var request = Resources.LoadAsync<GameObject>(popupName);
            while (!request.isDone)
            {
                yield return null;
            }
            Debug.Log("Loaded!!!!");
            // 팝업 OBJ 가져오기 
            var popup = Instantiate(request.asset) as GameObject;
            Assert.IsNotNull((popup));
            popup.transform.SetParent(canvas.transform, false);
            popup.GetComponent<Popup>().parentScene = this;
            
            //template popup인 경우만 ( 경고 팝업이 필요한 경우에만) 
            try
            {
                popup.GetComponent<TemplatePopup>();
                popup.GetComponent<TemplatePopup>().titleText.text = ErrorTitle;
                popup.GetComponent<TemplatePopup>().bodyText.text = ErrorMessage;
            }
            catch (Exception e)
            {
                //noting to do
            }
          
            
            if (onOpened != null)
            {
                onOpened(popup.GetComponent<T>());
            }

            
            currentPopups.Push(popup);
        }

        protected IEnumerator FadeIn(Image image, float time)
        {
            var alpha = image.color.a;
            for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
            {
                var color = image.color;
                color.a = Mathf.Lerp(alpha, 220 / 256.0f, t);
                image.color = color;
                yield return null;
            }
        }

    }
}