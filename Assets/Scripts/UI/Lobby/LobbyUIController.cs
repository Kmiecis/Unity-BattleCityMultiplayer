using Common.MVVM;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tanks
{
    public class LobbyUIController : MonoBehaviourPunCallbacks
    {
        [field: SerializeField]
        public StringAsset ServerAppId { get; private set; }
        [field: SerializeField]
        public GameData GameProperties { get; private set; }
        [field: SerializeField]
        public ScenesData GameScenes { get; private set; }
        
        [field: SerializeField]
        public GameObject LoginPanel { get; private set; }
        [field: SerializeField]
        public GameObject MenuPanel { get; private set; }
        [field: SerializeField]
        public GameObject CreateRoomPanel { get; private set; }
        [field: SerializeField]
        public GameObject RoomListPanel { get; private set; }
        [field: SerializeField]
        public GameObject SettingsPanel { get; private set; }
        [field: SerializeField]
        public GameObject RoomPreparationPanel { get; private set; }
        [field: SerializeField]
        public GameObject LoginAttemptPanel { get; private set; }
        [field: SerializeField]
        public GameObject JoinRandomRoomAttemptPanel { get; private set; }
        [field: SerializeField]
        public GameObject LeaveRoomAttemptPanel { get; private set; }
        [field: SerializeField]
        public KeyCodeAsset CancelKeyCode { get; private set; }

        private GameObject _currentPanel;

        private void ChangeToPanel(GameObject panel)
        {
            _currentPanel?.SetActive(false);
            _currentPanel = panel;
            _currentPanel.SetActive(true);
        }

        private IEnumerable<GameObject> GetPanels()
        {
            yield return LoginPanel;
            yield return MenuPanel;
            yield return CreateRoomPanel;
            yield return RoomListPanel;
            yield return SettingsPanel;
            yield return RoomPreparationPanel;
            yield return LoginAttemptPanel;
            yield return JoinRandomRoomAttemptPanel;
            yield return LeaveRoomAttemptPanel;
        }

        #region External methods
        public void _OnLoginClicked()
        {
            var serverSettings = PhotonNetwork.PhotonServerSettings;
            var appSettings = serverSettings.AppSettings;
            appSettings.FixedRegion = "eu";
            appSettings.AppIdRealtime = ServerAppId;
            PhotonNetwork.ConnectUsingSettings(serverSettings.AppSettings, serverSettings.StartInOfflineMode);

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
            ChangeToPanel(RoomListPanel);
        }

        public void _OnSettingsClicked()
        {
            ChangeToPanel(SettingsPanel);
        }

        public void _OnConstructionClicked()
        {
            SceneManager.LoadScene(GameScenes.ConstructionScene);
        }

        public void _OnBackClicked()
        {
            ChangeToPanel(MenuPanel);
        }
        #endregion

        #region Photon methods
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            ChangeToPanel(MenuPanel);
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
            ChangeToPanel(MenuPanel);
        }

        public override void OnJoinedRoom()
        {
            ChangeToPanel(RoomPreparationPanel);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            ChangeToPanel(MenuPanel);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            var roomName = PhotonNetwork.LocalPlayer.NickName + " ROOM";
            var roomOptions = new RoomOptions { MaxPlayers = GameProperties.defaultPlayers };

            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
        }

        public override void OnLeftRoom()
        {
            ChangeToPanel(MenuPanel);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            ChangeToPanel(LoginPanel);
        }
        #endregion

        #region Unity methods
        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            foreach (var panel in GetPanels())
            {
                panel.SetActive(false);
            }
        }

        private void Start()
        {
            if (string.IsNullOrEmpty(PhotonNetwork.LocalPlayer.NickName))
            {
                ChangeToPanel(LoginPanel);
            }
            else
            {
                if (PhotonNetwork.IsConnected)
                {
                    ChangeToPanel(MenuPanel);
                }
                else
                {
                    _OnLoginClicked();
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(CancelKeyCode) && PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.InRoom)
                {
                    PhotonNetwork.LeaveRoom();

                    ChangeToPanel(LeaveRoomAttemptPanel);
                }
                else
                {
                    ChangeToPanel(MenuPanel);
                }
            }
        }
        #endregion
    }
}
