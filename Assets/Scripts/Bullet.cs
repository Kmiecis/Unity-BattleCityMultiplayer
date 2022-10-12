using Common;
using System;
using System.Collections;
using UnityEngine;

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

        public event Action<Bullet> CalledOnDestroy;

        private CoroutineWrapper _explodeWrapper = new CoroutineWrapper();

        private void Explode()
        {
            _explodeWrapper.Stop();

            MovementController.ResetMovement();
            ModelObject.SetActive(false);
            ExplosionObject.SetActive(true);
        }

        private IEnumerator ExplodeDelayed()
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

        private void Start()
        {
            ModelObject.SetActive(true);
            ExplosionObject.SetActive(false);

            _explodeWrapper
                .With(this, ExplodeDelayed)
                .Start();
        }

        private void OnDestroy()
        {
            CalledOnDestroy?.Invoke(this);
            CalledOnDestroy = null;
        }
    }
}
