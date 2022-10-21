using System;
using UnityEngine;

namespace Tanks
{
    public class ExplosionController : MonoBehaviour
    {
        private const string kExplosionEndEvent = "end";

        [field: SerializeField]
        public GameObject ExplosionObject { get; private set; }
        [field: SerializeField]
        public AnimatorEventHandler AnimatorEvent { get; private set; }

        private Action _onFinish;

        public void Explode(Action onFinish)
        {
            _onFinish = onFinish;

            ExplosionObject.SetActive(true);
        }

        private void OnAnimatorEvent(string value)
        {
            ExplosionObject.SetActive(false);

            if (value == kExplosionEndEvent)
            {
                _onFinish();
            }
        }

        private void Awake()
        {
            ExplosionObject.SetActive(false);

            AnimatorEvent.OnAnimatorEvent.AddListener(OnAnimatorEvent);
        }
    }
}
