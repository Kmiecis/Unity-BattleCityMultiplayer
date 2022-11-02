using UnityEngine;
using UnityEngine.Events;

namespace Tanks
{
    public class DebugEventHandler : MonoBehaviour
    {
        public UnityEvent events;

        public void OnRaise()
        {
            events.Invoke();
        }
    }
}
