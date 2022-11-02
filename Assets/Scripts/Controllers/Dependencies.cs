using Common.Injection;
using UnityEngine;

namespace Tanks
{
    public class Dependencies : MonoBehaviour
    {
        [DI_Install, SerializeField]
        private TanksController _tanks;
        [DI_Install, SerializeField]
        private SpawnsController _spawns;

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
