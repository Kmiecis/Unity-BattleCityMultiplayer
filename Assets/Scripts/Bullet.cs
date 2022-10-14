using Common;
using System;
using System.Collections;
using UnityEngine;

namespace Tanks
{
    public class Bullet : MonoBehaviour
    {
        private const string kExplosionEndEvent = "end";
        private const float kExplosionDelay = 5.0f;

        public LayerMask hitMask;

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

        public event Action<Bullet> CalledOnDestroy;

        private CoroutineWrapper _explosionWrapper = new CoroutineWrapper();

        public void Setup(Vector2 direction, float lag, Collider2D ignoreCollider, Action<Bullet> onDestroy)
        {
            CalledOnDestroy += onDestroy;

            MovementController.SetMovement(direction);

            var position = (Vector2)transform.position;
            var distance = lag * MovementController.speed;
            var hit = Physics2D.Raycast(position, direction, distance, hitMask);
            if (hit)
            {
                var traveled = (hit.point - position).magnitude;
                lag = traveled / MovementController.speed;
                MovementController.ApplyMovement(lag);

                Explode();
            }
            else
            {
                Physics2D.IgnoreCollision(CollisionController.Collider, ignoreCollider);
                MovementController.ApplyMovement(lag);

                Prepare();
            }
        }

        public void Explode()
        {
            MovementController.ResetMovement();

            ModelObject.SetActive(false);
            ExplosionObject.SetActive(true);

            CalledOnDestroy?.Invoke(this);
            CalledOnDestroy = null;

            _explosionWrapper.Stop();
        }

        public void Prepare()
        {
            ModelObject.SetActive(true);
            ExplosionObject.SetActive(false);

            _explosionWrapper
                .WithTarget(this)
                .WithEnumerator(ExplodeDelayed)
                .Start();
        }

        public IEnumerator ExplodeDelayed()
        {
            return CoroutineUtility.InvokeDelayed(Explode, kExplosionDelay);
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
