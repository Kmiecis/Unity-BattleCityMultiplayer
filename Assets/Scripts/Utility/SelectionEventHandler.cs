using UnityEngine;
using UnityEngine.Events;

namespace Tanks
{
    public class SelectionEventHandler : MonoBehaviour
    {
        public UnityEvent<bool> CalledOnHighlight;
        public UnityEvent CalledOnSelect;

        public void SetHighlighted(bool value)
        {
            CalledOnHighlight?.Invoke(value);
        }

        public void Select()
        {
            CalledOnSelect?.Invoke();
        }
    }
}
