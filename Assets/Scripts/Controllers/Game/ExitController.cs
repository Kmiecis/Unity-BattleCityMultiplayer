using Common.MVB;
using Photon.Pun;
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

        #region Photon methods
        public override void OnLeftRoom()
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
                ExitPanel.SetActive(!ExitPanel.activeSelf);
            }
        }
        #endregion
    }
}
