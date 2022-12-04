using Photon.Pun;
using Tanks.Extensions;
using TMPro;
using UnityEngine;

namespace Tanks.UI
{
    public class LoginController : MonoBehaviour
    {
        [field: SerializeField]
        public TMP_InputField NameInput { get; private set; }

        private string _defaultName;

        private string GetPlayerName()
        {
            var playerName = NameInput.text;
            if (!string.IsNullOrEmpty(playerName))
                return playerName;
            return _defaultName;
            
        }

        private void SetupNameInput()
        {
            _defaultName = CustomPlayerPrefs.GetName("P" + Random.Range(1000, 10000));

            NameInput.GetPlaceholderText().text = _defaultName;
        }

        public void _OnLoginClicked()
        {
            var playerName = GetPlayerName();

            PhotonNetwork.LocalPlayer.NickName = playerName;

            CustomPlayerPrefs.SetName(playerName);
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
        private void Start()
        {
            SetupNameInput();
        }
        #endregion
    }
}
