using Common.MVB;
using UnityEngine;
using UnityEngine.Events;

namespace Tanks
{
    public class KeyDownEventHandler : MonoBehaviour
    {
        [field: SerializeField]
        public KeyCodeAsset KeyCode { get; private set; }

        public UnityEvent OnKeyDown;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode))
            {
                OnKeyDown?.Invoke();
            }
        }
    }
}
