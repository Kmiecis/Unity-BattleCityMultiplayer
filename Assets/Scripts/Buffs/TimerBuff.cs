﻿using Common.Injection;

namespace Tanks
{
    public class TimerBuff : ATimedBuff
    {
        [field: DI_Inject]
        public TanksController TanksController { get; private set; }

        public override void OnStart()
        {
            var tank = TanksController.GetMineTank();
            if (tank.team != _team)
            {
                tank.SetEnabled(false);
            }
        }

        public override void OnFinish()
        {
            var tank = TanksController.GetMineTank();
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
