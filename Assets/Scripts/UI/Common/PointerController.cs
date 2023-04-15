using Common.Injection;
using Common.MVB;
using UnityEngine;

namespace Tanks
{
    public class PointerController : MonoBehaviour
    {
        [field: SerializeField]
        public SoundData ShowSound { get; private set; }
        [field: SerializeField]
        public KeyCodeAsset UpKey { get; private set; }
        [field: SerializeField]
        public KeyCodeAsset DownKey { get; private set; }

        [field: DI_Inject]
        public SoundsController SoundsController { get; private set; }

        private void Check()
        {
            if (Input.GetKeyDown(UpKey) || Input.GetKeyDown(DownKey))
            {
                SoundsController.PlaySound(ShowSound);
            }
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void OnEnable()
        {
            Check();
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
