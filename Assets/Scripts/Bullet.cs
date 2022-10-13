using Common;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tanks
{
    public class Bullet : MonoBehaviour
    {
        private const string kExplosionEndEvent = "end";
        private const float kExplosionDelay = 3.0f;

        [field: SerializeField]
        public GameObject ModelObject { get; private set; }
        [field: SerializeField]
        public GameObject ExplosionObject { get; private set; }
        [field: SerializeField]
        public MovementController MovementController { get; private set; }
        [field: SerializeField]
        public Collision2DController CollisionController { get; private set; }
        [field: SerializeField]
        public AnimatorEventHandler AnimatorEvent { get; private set; }

        public LayerMask hitMask;
        public event Action<Bullet> CalledOnDestroy;

        public void Setup(Vector2Int direction, float lag, Action<Bullet> onDestroy)
        {
            CalledOnDestroy += onDestroy;

            var hit = Physics2D.Raycast(transform.position, direction, lag, hitMask);
            if (hit)
            {
                Explode();
            }
            else
            {
                Prepare(direction, lag);
            }
        }

        private void Prepare(Vector2Int direction, float lag)
        {
            ModelObject.SetActive(true);
            ExplosionObject.SetActive(false);

            MovementController.SetMovement(direction);
            MovementController.ApplyMovement(lag);
        }

        private void Explode()
        {
            MovementController.ResetMovement();

            ModelObject.SetActive(false);
            ExplosionObject.SetActive(true);

            CalledOnDestroy?.Invoke(this);
            CalledOnDestroy = null;
        }

        public void _OnCollisionEntered(Collision2D collision)
        {
            Explode();
        }

        public void _OnAnimatorEvent(string value)
        {
            if (value == kExplosionEndEvent)
            {
                Destroy(gameObject);
            }
        }
    }
}
