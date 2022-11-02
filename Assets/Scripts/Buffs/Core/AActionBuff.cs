namespace Tanks
{
    public abstract class AActionBuff : ABuff
    {
        public abstract void OnStart();

        #region Unity methods
        private void Start()
        {
            OnStart();

            Destroy(gameObject);
        }
        #endregion
    }
}
