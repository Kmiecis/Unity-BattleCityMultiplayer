using UnityEngine;
using UnityEngine.Events;

namespace Tanks
{
    public class EventHandler : MonoBehaviour
    {
        public UnityEvent CalledExternally;

        public void Invoke()
        {
            CalledExternally?.Invoke();
        }
    }
}
