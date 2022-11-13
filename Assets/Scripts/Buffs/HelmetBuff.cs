using Common.Injection;

namespace Tanks
{
    public class HelmetBuff : AActionBuff
    {
        public float duration = 1.0f;

        [field: DI_Inject]
        public TanksController TanksController { get; private set; }

        public override void OnStart()
        {
            var tanks = TanksController.GetTanks(_team);
            foreach (var tank in tanks)
            {
                if (tank.IsVisible)
                {
                    tank.ForcefieldController.Enable(duration, _lag);
                }
            }
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }
        #endregion
    }
}
