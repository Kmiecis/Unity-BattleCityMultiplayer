using Common.Injection;
using UnityEngine;

namespace Tanks
{
    public class Statue : MonoBehaviour
    {
        public ETeam team;
        public float repairDelay = 1.0f;

        [field: SerializeField]
        public DestroyController DestroyController { get; private set; }
        [field: SerializeField]
        public CracksController CracksController { get; private set; }
        [field: SerializeField]
        public FortifyController FortifyController { get; private set; }
        [field: DI_Inject]
        public StatuesController StatuesController { get; private set; }

        private float _nextRepairTime = float.MaxValue;

        public bool IsDestroyed
        {
            get => DestroyController.IsDestroyed;
        }

        private void SetDestroyed()
        {
            CracksController.SetDefault();
            DestroyController.SetDestroyed();
        }

        public void Hit()
        {
            Damage();
            RPCDamage();
        }

        public void Damage(float lag = 0.0f)
        {
            if (IsDestroyed)
                return;

            if (CracksController.HasNext)
            {
                CracksController.SetNext();
            }

            if (CracksController.HasNext)
            {
                var time = Time.time;
                _nextRepairTime = time + repairDelay * 2.0f - lag;
            }
            else
            {
                _nextRepairTime = float.MaxValue;

                SetDestroyed();
            }
        }

        public void RPCDamage()
        {
            StatuesController.RPCStatueDamage(team);
        }

        public void Repair(float lag = 0.0f)
        {
            if (IsDestroyed)
                return;

            if (CracksController.HasPrev)
            {
                CracksController.SetPrev();
            }

            if (CracksController.HasPrev)
            {
                var time = Time.time;
                _nextRepairTime = time + repairDelay - lag;
            }
            else
            {
                _nextRepairTime = float.MaxValue;
            }
        }

        public void RPCRepair()
        {
            StatuesController.RPCStatueRepair(team);
        }

        private void UpdateRepairing()
        {
            var time = Time.time;
            if (_nextRepairTime <= time)
            {   // Currently, only local repairing. We'll see, if this causes problems...
                Repair();
            }
        }

        #region Injection methods
        private void OnStatuesControllerInject(StatuesController controller)
        {
            controller.SetStatue(this);
        }
        #endregion

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Update()
        {
            UpdateRepairing();
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
