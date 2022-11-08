using Common.Injection;
using Common.MVB;
using UnityEngine;

namespace Tanks.UI
{
    [DI_Install]
    public class GameUIController : MonoBehaviour
    {
        [field: SerializeField]
        public GameProperties GameProperties { get; private set; }

        [field: SerializeField]
        public ScriptableKeyCode ExitKeyCode { get; private set; }
        [field: SerializeField]
        public ScriptableKeyCode ScoresKeyCode { get; private set; }

        [field: SerializeField]
        public GameObject ExitPanel { get; private set; }
        [field: SerializeField]
        public GameObject ScoresPanel { get; private set; }

        private GameObject _currentPanel;

        private void ChangeToPanel(GameObject panel)
        {
            _currentPanel?.SetActive(false);
            _currentPanel = panel;
            _currentPanel?.SetActive(true);
        }

        public void OnGameEnded()
        {
            ChangeToPanel(ScoresPanel);
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(ExitKeyCode))
            {
                ChangeToPanel(ExitPanel.activeSelf ? null : ExitPanel);
            }

            if (Input.GetKeyDown(ScoresKeyCode))
            {
                ChangeToPanel(ScoresPanel);
            }
            if (Input.GetKeyUp(ScoresKeyCode))
            {
                ChangeToPanel(null);
            }
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
