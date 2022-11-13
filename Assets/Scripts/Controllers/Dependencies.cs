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
        }
        #endregion
    }
}
