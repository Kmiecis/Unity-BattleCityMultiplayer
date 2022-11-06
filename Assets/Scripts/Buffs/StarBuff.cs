using Common.Injection;

namespace Tanks
{
    public class StarBuff : ATimedBuff
    {
        public float multiplier = 1.0f;

        [field: DI_Inject]
        public TanksController TanksController { get; private set; }

        public override void OnStart()
        {
            var tanks = TanksController.GetTanks(_team);
            foreach (var tank in tanks)
            {
                tank.BulletController.speed *= multiplier;
            }
        }

        public override void OnFinish()
        {
            var tanks = TanksController.GetTanks(_team);
            foreach (var tank in tanks)
            {
                tank.BulletController.speed /= multiplier;
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
