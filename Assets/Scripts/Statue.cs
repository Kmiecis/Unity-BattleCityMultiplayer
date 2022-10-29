using UnityEngine;

namespace Tanks
{
    public class Statue : MonoBehaviour
    {
        public float repairDelay = 1.0f;

        [field: SerializeField]
        public CracksController CracksController { get; private set; }

        private StatuesController _controller;
        private int _team;
        private float _nextRepairTime = float.MaxValue;

        public int Team
        {
            get => _team;
        }

        public bool IsDestroyed
        {
            get => !CracksController.HasNext;
        }

        public bool IsRepaired
        {
            get => !CracksController.HasPrev;
        }

        public void Setup(StatuesController controller, int team)
        {
            _controller = controller;
            _team = team;
        }

        public void Damage(float lag = 0.0f)
        {
            if (CracksController.HasNext)
            {
                CracksController.SetNext();
            }

            var time = Time.time;
            _nextRepairTime = time + repairDelay * 2.0f - lag;
        }

        public void RPCDamage()
        {
            _controller.RPCStatueDamage(Team);
        }

        public void Repair(float lag = 0.0f)
        {
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
            _controller.RPCStatueRepair(Team);
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
