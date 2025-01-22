using UnityEngine;

namespace CardGame.view.lobby
{
    public class LobbyObjManager : MonoBehaviour
    {
        // RoomList 의 최고관리자
        [SerializeField]
        private RoomList _roomList;
        // NewRoom을 만드는 Pannel 의 최고관리자
        [SerializeField]
        private NewRoomPanel _newRoomPanel;
        public RoomList GetRoomList()
        {
            return _roomList;
        }
        public NewRoomPanel GetNewRoomPanel()
        {
            return _newRoomPanel;
        }
    }
}