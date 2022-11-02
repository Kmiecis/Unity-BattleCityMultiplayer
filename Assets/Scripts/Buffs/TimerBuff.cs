using Common.Injection;

namespace Tanks
{
    public class TimerBuff : ATimedBuff
    {
        [DI_Inject]
        private TanksController _tanksController;

        public override void OnStart()
        {
            var tank = _tanksController.GetMineTank();
            if (tank.team != _team)
            {
                tank.SetEnabled(false);
            }
        }

        public override void OnFinish()
        {
            var tank = _tanksController.GetMineTank();
            if (tank.team != _team)
            {
                tank.SetEnabled(true);
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
