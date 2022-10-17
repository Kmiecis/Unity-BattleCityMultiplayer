using UnityEngine;
using UnityEngine.Events;

namespace Tanks
{
    public class SelectionEventHandler : MonoBehaviour
    {
        public UnityEvent<bool> OnHighlight;
        public UnityEvent OnSelect;

        public void SetHighlighted(bool value)
        {
            OnHighlight?.Invoke(value);
        }

        public void Select()
        {
            OnSelect?.Invoke();
        }
    }
}
