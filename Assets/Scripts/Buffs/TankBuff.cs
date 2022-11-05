using Common.Injection;
using UnityEngine;

namespace Tanks
{
    public class TankBuff : AActionBuff
    {
        [SerializeField]
        private int _upgrades = 1;

        [DI_Inject]
        private TanksController _tanksController;

        public override void OnStart()
        {
            var tanks = _tanksController.GetTanks(_team);
            foreach (var tank in tanks)
            {
                tank.TryUpgrade();
            }
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
