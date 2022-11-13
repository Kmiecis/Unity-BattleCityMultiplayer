using Common.Injection;

namespace Tanks
{
    public class PistolBuff : ATimedBuff
    {
        public int extras = 2;

        [field: DI_Inject]
        public TanksController TanksController { get; private set; }

        public override void OnStart()
        {
            var tank = TanksController.GetMineTank();
            if (tank.team == _team)
            {
                tank.BulletController.limit += extras;
            }
        }

        public override void OnFinish()
        {
            var tank = TanksController.GetMineTank();
            if (tank.team == _team)
            {
                tank.BulletController.limit -= extras;
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
