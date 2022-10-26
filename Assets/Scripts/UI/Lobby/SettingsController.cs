using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Tanks.UI
{
    public class SettingsController : MonoBehaviour
    {
        [field: SerializeField]
        public TMP_InputField NameInput { get; private set; }

        private bool _onEnabled;

        public void _OnAcceptClicked()
        {
            var playerName = NameInput.text;
            if (!string.IsNullOrEmpty(playerName))
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
            }
        }

        public void _OnNameInputFocused(bool focused)
        {
            if (focused)
            {
                NameInput.ActivateInputField();
            }
            else
            {
                NameInput.DeactivateInputField(true);
            }
        }

        #region Unity methods
        private void OnEnable()
        {
            _onEnabled = true;

            NameInput.text = PhotonNetwork.LocalPlayer.NickName;
        }

        private void Update()
        {
            if (_onEnabled)
            {
                NameInput.ActivateInputField();
                _onEnabled = false;
            }
        }
        #endregion
    }
}
