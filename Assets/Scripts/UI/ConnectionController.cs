using Common.MVB;
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
        public GameObject LoginAttemptPanel { get; private set; }
        [field: SerializeField]
        public GameObject SelectionPanel { get; private set; }
        [field: SerializeField]
        public GameObject CreateRoomPanel { get; private set; }
        [field: SerializeField]
        public GameObject JoinRandomRoomAttemptPanel { get; private set; }
        [field: SerializeField]
        public GameObject RoomListPanel { get; private set; }
        [field: SerializeField]
        public GameObject RoomPreparationPanel { get; private set; }
        [field: SerializeField]
        public ScriptableKeyCode CancelKeyCode { get; private set; }

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

            ChangeToPanel(LoginAttemptPanel);
        }

        public void _OnCreateRoomClicked()
        {
            ChangeToPanel(CreateRoomPanel);
        }

        public void _OnJoinRandomRoomClicked()
        {
            ChangeToPanel(JoinRandomRoomAttemptPanel);

            PhotonNetwork.JoinRandomRoom();
        }

        public void _OnRoomListClicked()
        {
            /*if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }*/

            ChangeToPanel(RoomListPanel);
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            ChangeToPanel(SelectionPanel);
        }

        public override void OnLeftLobby()
        {
            // Nothing
        }

        public override void OnCreatedRoom()
        {
            // Nothing
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            ChangeToPanel(SelectionPanel);
        }

        public override void OnJoinedRoom()
        {
            ChangeToPanel(RoomPreparationPanel);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            ChangeToPanel(SelectionPanel);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            var roomName = PhotonNetwork.LocalPlayer.NickName + " ROOM";
            var roomOptions = new RoomOptions { MaxPlayers = GameProperties.MAX_PLAYERS };

            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
        }

        public override void OnLeftRoom()
        {
            ChangeToPanel(SelectionPanel);
        }

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            ChangeToPanel(LoginPanel);
        }

        private void Update()
        {
            if (PhotonNetwork.IsConnected && Input.GetKeyDown(CancelKeyCode))
            {
                /*if (PhotonNetwork.InLobby)
                {
                    PhotonNetwork.LeaveLobby();
                }*/
                if (PhotonNetwork.InRoom)
                {
                    PhotonNetwork.LeaveRoom();
                }

                ChangeToPanel(SelectionPanel);
            }
        }
    }
}
