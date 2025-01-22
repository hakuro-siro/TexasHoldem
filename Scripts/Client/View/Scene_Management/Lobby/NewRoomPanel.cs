using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.view.lobby
{
    public class NewRoomPanel: MonoBehaviour
    {
        // newRoom 의 요소를 관리하는 Pannel
        [SerializeField]
        private GameObject newRoomPannel;
        [SerializeField]
        private TMP_InputField RoomNameInput;
        [SerializeField]
        private TMP_InputField RoomLimitInput;
        public GameObject GetNewRoomPannel()
        {
            return newRoomPannel;
        }
        public TMP_InputField GetRoomNameInput()
        {
            return RoomNameInput;
        }
        public TMP_InputField GetRoomLimitInput()
        {
            return RoomLimitInput;
        }
    }
}