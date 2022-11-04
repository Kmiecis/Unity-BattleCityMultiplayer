using Common.Injection;

namespace Tanks
{
    public class StarBuff : ATimedBuff
    {
        public float speedMultiplier = 1.0f;

        [DI_Inject]
        private TanksController _tanksController;

        public override void OnStart()
        {
            var tanks = _tanksController.GetTanks(_team);
            foreach (var tank in tanks)
            {
                tank.BulletController.speed *= speedMultiplier;
            }
        }

        public override void OnFinish()
        {
            var tanks = _tanksController.GetTanks(_team);
            foreach (var tank in tanks)
            {
                tank.BulletController.speed /= speedMultiplier;
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
