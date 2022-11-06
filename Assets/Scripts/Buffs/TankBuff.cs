using Common.Injection;

namespace Tanks
{
    public class TankBuff : AActionBuff
    {
        public int upgrades = 1;

        [field: DI_Inject]
        public TanksController TanksController { get; private set; }

        public override void OnStart()
        {
            var tanks = TanksController.GetTanks(_team);
            foreach (var tank in tanks)
            {
                for (int i = 0; i < upgrades; ++i)
                {
                    tank.TryUpgrade();
                }
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
