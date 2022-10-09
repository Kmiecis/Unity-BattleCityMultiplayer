using Photon.Pun;
using UnityEngine.SceneManagement;

namespace Tanks
{
    public class ConnectToServer : MonoBehaviourPunCallbacks
    {
        public ServerSettings settings;
        public string lobbySceneName = "Lobby";

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings(settings.AppSettings, settings.StartInOfflineMode);
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();

            SceneManager.LoadScene(lobbySceneName);
        }
    }
}
