using UnityEngine;

namespace Tanks
{
    public abstract class ATimedBuff : ABuff
    {
        public float duration = 1.0f;

        protected float _destroytime;

        public abstract void OnStart();

        public abstract void OnFinish();

        protected void ScheduleDestroy()
        {
            var time = Time.time;
            _destroytime = time + duration - _lag;
        }

        protected bool CheckDestroy()
        {
            var time = Time.time;
            if (_destroytime < time)
            {
                _destroytime = float.MaxValue;

                return true;
            }

            return false;
        }

        #region Unity methods
        private void Start()
        {
            ScheduleDestroy();

            OnStart();
        }

        private void Update()
        {
            if (CheckDestroy())
            {
                OnFinish();

                Destroy(gameObject);
            }
        }
        #endregion
    }
}
