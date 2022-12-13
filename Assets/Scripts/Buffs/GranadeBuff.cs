using Common.Injection;

namespace Tanks
{
    public class GranadeBuff : AActionBuff
    {
        [field: DI_Inject]
        public TanksController TanksController { get; private set; }

        public override void OnStart()
        {
            var enemyTeam = _team.Flip();
            var tanks = TanksController.Tanks[enemyTeam];
            foreach (var tank in tanks)
            {
                if (tank.IsVisible)
                {
                    tank.Explode();
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
