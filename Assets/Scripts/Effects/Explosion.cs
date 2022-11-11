using System;
using UnityEngine;

namespace Tanks
{
    public class Explosion : MonoBehaviour
    {
        private const string kExplosionEndEvent = "end";

        [field: SerializeField]
        public AnimatorEventHandler AnimatorEvent { get; private set; }

        private Action<Explosion> _callback;

        public void Setup(Action<Explosion> callback)
        {
            _callback = callback;
        }

        private void OnAnimatorEvent(string value)
        {
            if (value == kExplosionEndEvent)
            {
                if (_callback != null)
                    _callback(this);
                _callback = null;
            }
        }

        #region Unity methods
        private void Awake()
        {
            AnimatorEvent.OnAnimatorEvent.AddListener(OnAnimatorEvent);
        }
        #endregion
    }
}
