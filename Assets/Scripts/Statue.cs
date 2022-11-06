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

        private StatuesController _controller;
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

        public void Setup(StatuesController controller)
        {
            _controller = controller;
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
            _controller.RPCStatueDamage(team);
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
            _controller.RPCStatueRepair(team);
        }

        private void UpdateRepairing()
        {
            var time = Time.time;
            if (_nextRepairTime <= time)
            {   // Currently, only local repairing. We'll see, if this causes problems...
                Repair();
            }
        }

        #region Unity methods
        private void Update()
        {
            UpdateRepairing();
        }
        #endregion
    }
}
