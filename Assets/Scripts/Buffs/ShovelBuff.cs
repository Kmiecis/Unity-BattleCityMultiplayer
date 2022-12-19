using Common.Injection;

namespace Tanks
{
    public class ShovelBuff : AActionBuff
    {
        public float duration = 1.0f;

        [field: DI_Inject]
        public StatuesController StatuesController { get; private set; }

        public override void OnStart()
        {
            var statue = StatuesController.Statues[_team];
            statue.FortifyController.Fortify(duration - _lag);
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }
        #endregion
    }
}
