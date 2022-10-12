using UnityEngine;
using UnityEngine.Events;

namespace Tanks
{
    public class Trigger2DController : MonoBehaviour
    {
        public UnityEvent<Collider2D> CalledOnEnter;
        public UnityEvent<Collider2D> CalledOnStay;
        public UnityEvent<Collider2D> CalledOnExit;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            CalledOnEnter?.Invoke(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            CalledOnStay?.Invoke(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            CalledOnExit?.Invoke(collision);
        }
    }
}
