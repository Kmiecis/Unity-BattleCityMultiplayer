using Common.Injection;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.UI
{
    public class SettingsController : MonoBehaviour
    {
        private float VOLUME_STEP = 0.1f;

        [field: SerializeField]
        public TMP_InputField NameInput { get; private set; }
        [field: SerializeField]
        public Image VolumeFill { get; private set; }

        [field: DI_Inject]
        public SoundsController SoundsController { get; private set; }

        private void RefreshName()
        {
            NameInput.text = PhotonNetwork.LocalPlayer.NickName;
        }

        private void RefreshVolume()
        {
            var volume = CustomPlayerPrefs.GetVolume();
            SetVolume(volume);
        }

        private void SetVolume(float value)
        {
            SoundsController.Volume = value;
            VolumeFill.fillAmount = value;
        }

        #region External methods
        public void _OnVolumeUp()
        {
            var volume = Mathf.Min(1.0f, SoundsController.Volume + VOLUME_STEP);
            SetVolume(volume);
        }

        public void _OnVolumeDown()
        {
            var volume = Mathf.Max(0.0f, SoundsController.Volume - VOLUME_STEP);
            SetVolume(volume);
        }

        public void _OnAcceptClicked()
        {
            var playerName = NameInput.text;

            if (!string.IsNullOrEmpty(playerName))
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;

                CustomPlayerPrefs.SetName(playerName);
            }

            CustomPlayerPrefs.SetVolume(SoundsController.Volume);
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
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Start()
        {
            RefreshName();
            RefreshVolume();
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
