using UnityEngine;
using Script.Popup;

namespace Popup{
    public class PopupManager : PopupHandler
    {
        public static PopupManager instance = null;

        public void setCanvas(Canvas _canvas)
        {
            this.canvas = _canvas;
        }

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        /// <summary>
        /// 팝업 로딩용 OpenPopup()
        /// </summary>
        /// <param name="popupName"> 들고오는 팝업 이름 (주로 리소스 경로)</param> //TODO Resource -> Addressable로 변
        /// <param name="darkenBackground"> 팝업 뜨면 뜨는 검은 뒷배경 만들기용 </param>
        public void openTemplate(int ErrorCode,string ErrorMessage)
        {
            string erc = "Error : " + ErrorCode;
            OpenPopup<TemplatePopup>("Popup/TemplatePopup",true,erc,ErrorMessage);
        }
    }
}