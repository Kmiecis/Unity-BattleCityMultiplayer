using Common.Injection;
using UnityEngine;

namespace Tanks
{
    public class IntroController : MonoBehaviour
    {
        [field: SerializeField]
        public SoundData IntroSound { get; private set; }

        [field: DI_Inject]
        public SoundsController SoundsController { get; private set; }

        private void StartIntro()
        {
            SoundsController.PlaySound(IntroSound, SoundsController.UnmuteIncoming);
            SoundsController.MuteIncoming();
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Start()
        {
            StartIntro();
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
