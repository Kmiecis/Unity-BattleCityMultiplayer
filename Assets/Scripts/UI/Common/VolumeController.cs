using Common.Injection;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks
{
    public class VolumeController : MonoBehaviour
    {
        private float VOLUME_STEP = 0.1f;

        [field: SerializeField]
        public Image VolumeFill { get; private set; }
        [field: SerializeField]
        public SoundData TestSound { get; private set; }

        [field: DI_Inject]
        public SoundsController SoundsController { get; private set; }

        private void RefreshVolume()
        {
            VolumeFill.fillAmount = SoundsController.Volume;
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
            SoundsController.PlaySound(TestSound);
        }

        public void _OnVolumeDown()
        {
            var volume = Mathf.Max(0.0f, SoundsController.Volume - VOLUME_STEP);
            SetVolume(volume);
            SoundsController.PlaySound(TestSound);
        }
        #endregion

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Start()
        {
            RefreshVolume();
        }

        private void OnDisable()
        {
            SoundsController.SaveVolume();
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
