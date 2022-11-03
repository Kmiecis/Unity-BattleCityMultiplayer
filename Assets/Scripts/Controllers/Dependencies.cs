using Common.Injection;
using UnityEngine;

namespace Tanks
{
    public class Dependencies : MonoBehaviour
    {
        [SerializeField]
        private TanksController _tanks;
        [SerializeField]
        private SpawnsController _spawns;

        #region Unity methods
        private void Awake()
        {
            _tanks = Instantiate(_tanks);
            _spawns = Instantiate(_spawns);

            DI_Binder.Bind(_tanks);
            DI_Binder.Bind(_spawns);
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(_tanks);
            DI_Binder.Unbind(_spawns);
        }
        #endregion
    }
}
