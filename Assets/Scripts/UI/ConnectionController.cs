using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Tanks
{
    public class ConnectionController : MonoBehaviourPunCallbacks
    {
        [field: SerializeField]
        public ServerSettings ServerSettings { get; private set; }

        [field: SerializeField]
        public GameObject LoginPanel { get; private set; }
        [field: SerializeField]
        public GameObject SelectionPanel { get; private set; }
        [field: SerializeField]
        public GameObject CreateRoomPanel { get; private set; }
        [field: SerializeField]
        public GameObject JoinRandomRoomPanel { get; private set; }
        [field: SerializeField]
        public GameObject RoomListPanel { get; private set; }

        private GameObject _currentPanel;

        private void ChangeToPanel(GameObject panel)
        {
            _currentPanel?.SetActive(false);
            _currentPanel = panel;
            _currentPanel.SetActive(true);
        }

        public void _OnLoginClicked()
        {
            PhotonNetwork.ConnectUsingSettings(ServerSettings.AppSettings, ServerSettings.StartInOfflineMode);

            LoginPanel.SetActive(false);
        }

        public void _OnCreateRoomClicked()
        {
            ChangeToPanel(CreateRoomPanel);
        }

        public void _OnJoinRandomRoomClicked()
        {
            ChangeToPanel(JoinRandomRoomPanel);

            PhotonNetwork.JoinRandomRoom();
        }

        public void _OnRoomListClicked()
        {
            ChangeToPanel(RoomListPanel);
        }

        public void _OnCancelClicked()
        {
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }

            ChangeToPanel(SelectionPanel);
        }

        public override void OnConnectedToMaster()
        {
            ChangeToPanel(SelectionPanel);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            ChangeToPanel(SelectionPanel);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel("Game");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            ChangeToPanel(SelectionPanel);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            var roomName = PhotonNetwork.LocalPlayer.NickName + "'S ROOM";

            var roomOptions = new RoomOptions
            {
                MaxPlayers = 8
            };
            PhotonNetwork.CreateRoom(roomName, roomOptions, null);
        }

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
    }
}
