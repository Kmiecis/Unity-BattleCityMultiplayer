using UnityEngine;

namespace Tanks.UI
{
    public class ConstructionUIController : MonoBehaviour
    {
        [field: SerializeField]
        public GameObject ExitPanel { get; private set; }

        private GameObject _currentPanel;

        private void ChangeToPanel(GameObject panel)
        {
            _currentPanel?.SetActive(false);
            _currentPanel = panel;
            _currentPanel?.SetActive(true);
        }

        #region External methods
        public void _ToggleExitPanel()
        {
            ChangeToPanel(ExitPanel.activeSelf ? null : ExitPanel);
        }
        #endregion
    }
}
