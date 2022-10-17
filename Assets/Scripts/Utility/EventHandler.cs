using UnityEngine;
using UnityEngine.Events;

namespace Tanks
{
    public class EventHandler : MonoBehaviour
    {
        public UnityEvent OnInvoke;

        public void Invoke()
        {
            OnInvoke?.Invoke();
        }
    }
}
