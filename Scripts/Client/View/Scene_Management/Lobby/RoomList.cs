using UnityEngine;

namespace CardGame.view.lobby
{
    public class RoomList: MonoBehaviour
    {
        // RoomList 의 요소를 관리하는 Pannel
        [SerializeField]
        private GameObject RoomListGrid;

        public GameObject GetRoomListGrid()
        {
            return RoomListGrid;
        }
    }
}