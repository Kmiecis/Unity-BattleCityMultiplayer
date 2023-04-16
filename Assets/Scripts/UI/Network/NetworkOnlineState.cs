using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Tanks.UI
{
    public class NetworkOnlineState : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI Text { get; private set; }

        [SerializeField]
        private int _limit = 20;

        private int _online;

        private void CheckOnlineCount()
        {
            var online = PhotonNetwork.CountOfPlayers;
            if (_online != online)
            {
                Text.text = GetOnlineText(online);

                _online = online;
            }
        }

        private string GetOnlineText(int count)
        {
            if (count > 0)
            {
                return $"{count}/{_limit}";
            }
            return "_";
        }

        #region Unity methods
        private void Update()
        {
            CheckOnlineCount();
        }
        #endregion
    }
}
