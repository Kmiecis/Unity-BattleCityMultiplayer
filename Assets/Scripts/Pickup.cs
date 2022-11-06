using Common.Injection;
using UnityEngine;

namespace Tanks
{
    public class Pickup : MonoBehaviour
    {
        public EBuffType buffType;
        public float duration = 1.0f;

        [field: DI_Inject]
        public BuffsController BuffsController { get; private set; }

        private float _destroytime;

        public void Setup(float lag = 0.0f)
        {
            var time = Time.time;
            _destroytime = time + duration - lag;
        }

        public void Picked()
        {
            Destroy(gameObject);
        }

        public void PickedFor(ETeam team)
        {
            BuffsController.CastBuff(buffType, team);
            BuffsController.RPCCastBuff(buffType, team);
        }

        private void UpdateLifetime()
        {
            var time = Time.time;
            if (_destroytime < time)
            {
                Destroy(gameObject);
            }
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Update()
        {
            UpdateLifetime();
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
