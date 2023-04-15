using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Tanks.UI
{
    public class SettingsController : MonoBehaviour
    {
        [field: SerializeField]
        public TMP_InputField NameInput { get; private set; }

        private void RefreshName()
        {
            NameInput.text = PhotonNetwork.LocalPlayer.NickName;
        }

        private void SaveName()
        {
            var playerName = NameInput.text;

            if (!string.IsNullOrEmpty(playerName))
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;

                CustomPlayerPrefs.SetName(playerName);
            }
        }

        #region External methods
        public void _OnAcceptClicked()
        {
            SaveName();
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
        #endregion

        #region Unity methods
        private void Start()
        {
            RefreshName();
        }
        #endregion
    }
}
