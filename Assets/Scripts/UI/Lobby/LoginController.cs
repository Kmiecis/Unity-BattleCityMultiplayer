using Photon.Pun;
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
            if (string.IsNullOrEmpty(playerName))
            {
                return _defaultName;
            }
            return playerName;
        }

        public void _OnLoginClicked()
        {
            PhotonNetwork.LocalPlayer.NickName = GetPlayerName();
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
            _defaultName = "P" + Random.Range(1000, 10000);

            NameInput.text = _defaultName;

            NameInput.ActivateInputField();
        }
        #endregion
    }
}
