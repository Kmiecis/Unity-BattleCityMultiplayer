using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tanks
{
    [RequireComponent(typeof(Collider2D))]
    public class Trigger2DController : MonoBehaviour
    {
        [field: SerializeField]
        public Collider2D Collider { get; private set; }

        public UnityEvent<Collider2D> CalledOnEnter;
        public UnityEvent<Collider2D> CalledOnStay;
        public UnityEvent<Collider2D> CalledOnExit;

        public List<Collider2D> Colliders { get; private set; } = new List<Collider2D>();

        private void OnTriggerEnter2D(Collider2D collider)
        {
            CalledOnEnter?.Invoke(collider);
            Colliders.Add(collider);
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            CalledOnStay?.Invoke(collider);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            Colliders.Remove(collider);
            CalledOnExit?.Invoke(collider);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            Collider = GetComponent<Collider2D>();
        }
#endif
    }
}
