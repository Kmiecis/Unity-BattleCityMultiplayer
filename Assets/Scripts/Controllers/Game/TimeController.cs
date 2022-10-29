using UnityEngine;

namespace Tanks
{
    public class TimeController : MonoBehaviour
    {
        private float _initialTimeScale;
        private float _initialFixedDeltaTime;
        private float _initialMaximumDeltaTime;
        private float _initialMaximumParticleDeltaTime;

        private void CacheTime()
        {
            _initialTimeScale = Time.timeScale;
            _initialFixedDeltaTime = Time.fixedDeltaTime;
            _initialMaximumDeltaTime = Time.maximumDeltaTime;
            _initialMaximumParticleDeltaTime = Time.maximumParticleDeltaTime;
        }

        public void StopTime()
        {
            Time.timeScale = 0.0f;
            Time.fixedDeltaTime = 0.0f;
            Time.maximumDeltaTime = 0.0f;
            Time.maximumParticleDeltaTime = 0.0f;
        }

        public void ResumeTime()
        {
            Time.timeScale = _initialTimeScale;
            Time.fixedDeltaTime = _initialFixedDeltaTime;
            Time.maximumDeltaTime = _initialMaximumDeltaTime;
            Time.maximumParticleDeltaTime = _initialMaximumParticleDeltaTime;
        }

        #region Unity methods
        private void Awake()
        {
            CacheTime();
        }

        private void OnDestroy()
        {
            ResumeTime();
        }
        #endregion
    }
}
