using UnityEngine;
using UnityEngine.Events;

namespace Tanks
{
    public class AnimatorEventHandler : MonoBehaviour
    {
        public UnityEvent<string> OnAnimatorEvent;

        public void _OnAnimatorEvent(string value)
        {
            OnAnimatorEvent?.Invoke(value);
        }
    }
}
