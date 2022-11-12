using Photon.Pun;
using Photon.Realtime;
using Tanks.Extensions;
using TMPro;
using UnityEngine;

namespace Tanks.UI
{
    public class CreateRoomController : MonoBehaviour
    {
        private const int kPlayerTtl = 1000 * 10;

        [field: SerializeField]
        public GameProperties GameProperties { get; private set; }
        [field: SerializeField]
        public TMP_InputField NameInput { get; private set; }
        [field: SerializeField]
        public TMP_InputField SizeInput { get; private set; }

        private string _defaultName;
        private byte _defaultSize;

        private string GetRoomName()
        {
            var roomName = NameInput.text;
            if (!string.IsNullOrEmpty(roomName))
                return roomName;
            return _defaultName;
        }

        private byte GetRoomSize()
        {
            var sizeText = SizeInput.text;
            if (byte.TryParse(sizeText, out var size))
                return (byte)Mathf.Clamp(size, GameProperties.minPlayers, GameProperties.maxPlayers);
            return _defaultSize;
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
            _defaultSize = GameProperties.minPlayers;

            NameInput.GetPlaceholderText().text = _defaultName;
            SizeInput.GetPlaceholderText().text = $"{GameProperties.minPlayers}-{GameProperties.maxPlayers}";
        }
    }
}
