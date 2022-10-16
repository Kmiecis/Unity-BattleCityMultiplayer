using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Tanks.UI
{
    public class RoomListEntry : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI NameText { get; private set; }
        [field: SerializeField]
        public TextMeshProUGUI SizeText { get; private set; }
        [field: SerializeField]
        public SelectionEventHandler SelectionEvent { get; private set; }

        private RoomInfo _room;

        public void Setup(RoomInfo room)
        {
            _room = room;

            Setup(room.Name, room.PlayerCount, room.MaxPlayers);
        }

        private void Setup(string roomName, int playerCount, byte maxPlayers)
        {
            NameText.text = roomName;
            SizeText.text = $"{playerCount}/{maxPlayers}";
        }

        public void _OnJoinRoomClicked()
        {
            /*if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }*/

            PhotonNetwork.JoinRoom(_room.Name);
        }
    }
}
