using Common.Injection;

namespace Tanks
{
    public class HelmetBuff : ABuff
    {
        public float duration = 1.0f;

        [DI_Inject]
        private TanksController _tanksController;

        private void OnStart()
        {
            var tanks = _tanksController.GetTanks(_team);
            foreach (var tank in tanks)
            {
                if (tank.IsVisible)
                {
                    tank.ForcefieldController.Enable(duration, 0.0f);
                }
            }

            Destroy(gameObject);
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Start()
        {
            OnStart();
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
