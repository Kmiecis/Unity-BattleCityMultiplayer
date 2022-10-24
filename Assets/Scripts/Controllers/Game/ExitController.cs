using Common.MVB;
using Photon.Pun;
using Photon.Realtime;
using Tanks.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tanks.UI
{
    public class ExitController : MonoBehaviourPunCallbacks
    {
        [field: SerializeField]
        public ScriptableKeyCode ExitKeyCode { get; private set; }
        [field: SerializeField]
        public GameObject ExitPanel { get; private set; }
        [field: SerializeField]
        public TextMeshProUGUI KDText { get; private set; }

        public bool IsVisible
        {
            get => ExitPanel.activeSelf;
        }
        
        private void SetVisibility(bool value)
        {
            ExitPanel.SetActive(value);

            if (value)
            {
                var kills = PhotonNetwork.LocalPlayer.GetKills();
                var deaths = PhotonNetwork.LocalPlayer.GetDeaths();
                KDText.text = $"{kills}/{deaths}";
            }
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

        #region External methods
        public void _OnExitGame()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
        }
        #endregion

        #region Unity methods
        private void Update()
        {
            if (Input.GetKeyDown(ExitKeyCode))
            {
                SetVisibility(!IsVisible);
            }
        }
        #endregion
    }
}
