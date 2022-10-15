using Common.MVB;
using UnityEngine;

namespace Tanks.UI
{
    public class SelectionController : MonoBehaviour
    {
        public int current = 0;

        [field: SerializeField]
        public SelectionEventHandler[] Events { get; private set; }
        [field: Header("Input")]
        [field: SerializeField]
        public ScriptableKeyCode SelectKey { get; private set; }
        [field: SerializeField]
        public ScriptableKeyCode UpKey { get; private set; }
        [field: SerializeField]
        public ScriptableKeyCode DownKey { get; private set; }

        private int _current;

        private void ChangeCurrent(int current)
        {
            Events[_current].SetHighlighted(false);

            _current = current;

            Events[_current].SetHighlighted(true);
        }

        private void TryChangeCurrent(int current)
        {
            var sanitized = Mathf.Clamp(current, 0, Events.Length - 1);

            if (_current != sanitized)
            {
                ChangeCurrent(sanitized);
            }
        }

        private void OnEnable()
        {
            _current = current;

            for (int i = 0; i < Events.Length; ++i)
            {
                var selected = Events[i];
                selected.SetHighlighted(_current == i);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(UpKey))
            {
                TryChangeCurrent(_current - 1);
            }

            if (Input.GetKeyDown(DownKey))
            {
                TryChangeCurrent(_current + 1);
            }

            if (Input.GetKeyDown(SelectKey))
            {
                Events[_current].Select();
            }
        }
    }
}
