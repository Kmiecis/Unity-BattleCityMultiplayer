using Photon.Pun;
using UnityEngine;

namespace Tanks.UI
{
    public class ExitController : MonoBehaviour
    {
        #region External methods
        public void _OnExitGame()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
        }
        #endregion
    }
}
