using UnityEngine;
using UnityEngine.Events;

namespace Tanks
{
    public class AnimatorEventHandler : MonoBehaviour
    {
        public UnityEvent<string> CalledByAnimator;

        public void _OnAnimatorEvent(string value)
        {
            CalledByAnimator?.Invoke(value);
        }
    }
}
