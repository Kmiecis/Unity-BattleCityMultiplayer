using System;
using UnityEngine;

namespace Tanks
{
    public class Bullet : MonoBehaviour
    {
        [field: SerializeField]
        public MovementController MovementController { get; private set; }
        [field: SerializeField]
        public Collision2DController CollisionController { get; private set; }

        public event Action<Bullet> CalledOnDestroy;

        private void OnCollisionEntered(Collision2D collision)
        {
            Destroy(gameObject);
        }

        private void Awake()
        {
            CollisionController.CalledOnEnter += OnCollisionEntered;
        }

        private void OnDestroy()
        {
            CalledOnDestroy?.Invoke(this);
        }
    }
}
