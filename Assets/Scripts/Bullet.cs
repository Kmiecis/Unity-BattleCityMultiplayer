﻿using Common;
using System;
using System.Collections;
using UnityEngine;

namespace Tanks
{
    public class Bullet : MonoBehaviour
    {
        private const float kExplosionDelay = 5.0f;

        public LayerMask hitMask;

        [field: SerializeField]
        public GameObject ModelObject { get; private set; }
        [field: SerializeField]
        public MovementController MovementController { get; private set; }
        [field: SerializeField]
        public Collision2DController CollisionController { get; private set; }
        [field: SerializeField]
        public ExplosionController ExplosionController { get; private set; }

        private Action _onExplode;
        private CoroutineWrapper _explosionWrapper = new CoroutineWrapper();
        
        public void Setup(Vector2 direction, Collider2D ignoreCollider, Action onExplode)
        {
            _onExplode = onExplode;

            Setup(direction, ignoreCollider);
        }

        public void Setup(Vector2 direction, Collider2D ignoreCollider, float lag = 0.0f)
        {
            var position = (Vector2)transform.position;
            var distance = lag * MovementController.speed;

            MovementController.SetMovement(direction);
            
            if (UPhysics2D.RaycastIgnoreCollision(position, direction, out var hit, ignoreCollider, distance, hitMask))
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

        private void Explode()
        {
            ModelObject.SetActive(false);

            MovementController.ResetMovement();
            ExplosionController.Explode(Destroy);

            _onExplode?.Invoke();
            _explosionWrapper.Stop();
        }

        private void Prepare()
        {
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

        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
