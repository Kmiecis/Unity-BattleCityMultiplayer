using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.UI
{
    public class RoomEntry : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI NameText { get; private set; }
        [field: SerializeField]
        public TextMeshProUGUI SizeText { get; private set; }
        [field: SerializeField]
        public SelectionEventHandler SelectionEvent { get; private set; }
        [field: SerializeField]
        public Color IsAvailableColor { get; private set; }
        [field: SerializeField]
        public Color IsBlockedColor { get; private set; }
        [field: SerializeField]
        public Graphic[] ColoredGraphics { get; private set; }

        private RoomInfo _room;

        private bool IsAvailable
        {
            get => (
                _room.IsOpen &&
                _room.IsVisible &&
                !_room.RemovedFromList &&
                _room.PlayerCount < _room.MaxPlayers
            );
        }

        public void Setup(RoomInfo room)
        {
            _room = room;

            NameText.text = room.Name;
            SizeText.text = $"{room.PlayerCount}/{room.MaxPlayers}";

            var isAvailable = IsAvailable;
            foreach (var graphic in ColoredGraphics)
            {
                graphic.color = isAvailable ? IsAvailableColor : IsBlockedColor;
            }
        }

        public void _OnJoinRoomClicked()
        {
            /*if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }*/

            if (IsAvailable)
            {
                PhotonNetwork.JoinRoom(_room.Name);
            }
        }
    }
}
