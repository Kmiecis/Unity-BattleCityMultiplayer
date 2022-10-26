using Common.MVB;
using Photon.Pun;
using Photon.Realtime;
using Tanks.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tanks.UI
{
    public class GameUIController : MonoBehaviourPunCallbacks
    {
        [field: SerializeField]
        public ScriptableKeyCode ExitKeyCode { get; private set; }
        [field: SerializeField]
        public ScriptableKeyCode ScoresKeyCode { get; private set; }

        [field: SerializeField]
        public GameObject ExitPanel { get; private set; }
        [field: SerializeField]
        public GameObject ScoresPanel { get; private set; }

        private GameObject _currentPanel;

        private void ChangeToPanel(GameObject panel)
        {
            _currentPanel?.SetActive(false);
            _currentPanel = panel;
            _currentPanel?.SetActive(true);
        }

        #region Photon methods
        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();

            PhotonNetwork.LocalPlayer.ResetProperties();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            SceneManager.LoadScene("Lobby");
        }
        #endregion

        #region Unity methods
        private void Update()
        {
            if (Input.GetKeyDown(ExitKeyCode))
            {
                ChangeToPanel(ExitPanel.activeSelf ? null : ExitPanel);
            }

            if (Input.GetKeyDown(ScoresKeyCode))
            {
                ChangeToPanel(ScoresPanel);
            }
            if (Input.GetKeyUp(ScoresKeyCode))
            {
                ChangeToPanel(null);
            }
        }
        #endregion
    }
}
