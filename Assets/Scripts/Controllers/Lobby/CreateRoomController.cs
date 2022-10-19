using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Tanks.UI
{
    public class CreateRoomController : MonoBehaviour
    {
        private const int kPlayerTtl = 1000 * 10;

        [field: SerializeField]
        public TMP_InputField NameInput { get; private set; }
        [field: SerializeField]
        public TMP_InputField SizeInput { get; private set; }

        private string _defaultName;

        private string GetRoomName()
        {
            var roomName = NameInput.text;
            if (string.IsNullOrEmpty(roomName))
            {
                return _defaultName;
            }
            return roomName;
        }

        private byte GetRoomSize()
        {
            byte.TryParse(SizeInput.text, out var size);
            return (byte)Mathf.Clamp(size, GameProperties.MIN_PLAYERS, GameProperties.MAX_PLAYERS);
        }

        public void _OnNameInputFocused(bool focused)
        {
            if (focused)
            {
                NameInput.ActivateInputField();
            }
            else
            {
                NameInput.DeactivateInputField(true);
            }
        }

        public void _OnSizeInputFocused(bool focused)
        {
            if (focused)
            {
                SizeInput.ActivateInputField();
            }
            else
            {
                SizeInput.DeactivateInputField(true);
            }
        }

        public void _OnCreateRoomClicked()
        {
            var roomName = GetRoomName();
            var roomSize = GetRoomSize();
            var roomOptions = new RoomOptions
            {
                MaxPlayers = roomSize,
                PlayerTtl = kPlayerTtl
            };

            PhotonNetwork.CreateRoom(roomName, roomOptions, null);
        }

        private void Start()
        {
            _defaultName = PhotonNetwork.LocalPlayer.NickName + " ROOM";

            if (NameInput != null)
            {
                NameInput.text = _defaultName;
            }

            if (SizeInput != null)
            {
                SizeInput.text = GameProperties.MIN_PLAYERS.ToString();
            }
        }
    }
}
