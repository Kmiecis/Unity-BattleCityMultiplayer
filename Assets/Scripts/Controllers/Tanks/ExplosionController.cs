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

        public void Setup()
        {
            ExplosionObject.SetActive(false);
        }

        public void Explode(Action onFinish = null)
        {
            _onFinish = onFinish;

            ExplosionObject.SetActive(true);
        }

        private void OnAnimatorEvent(string value)
        {
            ExplosionObject.SetActive(false);

            if (value == kExplosionEndEvent)
            {
                _onFinish?.Invoke();
            }
        }

        private void Awake()
        {
            Setup();

            AnimatorEvent.OnAnimatorEvent.AddListener(OnAnimatorEvent);
        }
    }
}
