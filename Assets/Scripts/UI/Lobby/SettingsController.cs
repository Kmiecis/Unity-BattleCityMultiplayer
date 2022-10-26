using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Tanks.UI
{
    public class SettingsController : MonoBehaviour
    {
        [field: SerializeField]
        public TMP_InputField NameInput { get; private set; }

        private void OnEnable()
        {
            NameInput.text = PhotonNetwork.LocalPlayer.NickName;
            NameInput.ActivateInputField();
        }

        private void OnDisable()
        {
            var playerName = NameInput.text;
            if (!string.IsNullOrEmpty(playerName))
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
            }
        }
    }
}
