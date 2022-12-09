using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tanks
{
    [RequireComponent(typeof(Collider2D))]
    public class Collision2DController : MonoBehaviour
    {
        [field: SerializeField]
        public Collider2D Collider { get; private set; }

        public UnityEvent<Collision2D> CalledOnEnter;
        public UnityEvent<Collision2D> CalledOnStay;
        public UnityEvent<Collision2D> CalledOnExit;

        public List<Collider2D> Colliders { get; private set; } = new List<Collider2D>();

        private void OnCollisionEnter2D(Collision2D collision)
        {
            CalledOnEnter?.Invoke(collision);
            Colliders.Add(collision.collider);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            CalledOnStay?.Invoke(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            Colliders.Remove(collision.collider);
            CalledOnExit?.Invoke(collision);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            Collider = GetComponent<Collider2D>();
        }
#endif
    }
}
