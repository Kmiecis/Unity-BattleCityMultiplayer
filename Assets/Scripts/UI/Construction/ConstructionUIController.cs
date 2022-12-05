using Common.Injection;
using UnityEngine;

namespace Tanks.UI
{
    public class ConstructionUIController : MonoBehaviour
    {
        [field: SerializeField]
        public GameObject ExitPanel { get; private set; }

        [field: DI_Inject]
        public ConstructionTank ConstructionTank { get; private set; }

        #region External methods
        public void _ToggleExitPanel()
        {
            var isActive = !ExitPanel.activeSelf;
            ExitPanel.SetActive(isActive);
            ConstructionTank.gameObject.SetActive(!isActive);
        }
        #endregion

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }
        #endregion
    }
}
