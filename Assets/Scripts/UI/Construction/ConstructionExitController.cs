using Common.Extensions;
using Common.Injection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tanks.UI
{
    public class ConstructionExitController : MonoBehaviour
    {
        [field: SerializeField]
        public GameScenes GameScenes { get; private set; }
        [field: SerializeField]
        public SelectionController SelectionController { get; private set; }
        [field: SerializeField]
        public SelectionEventHandler RestoreHandler { get; private set; }
        [field: SerializeField]
        public Graphic RestoreGraphic { get; private set; }
        [field: SerializeField]
        public SelectionEventHandler SaveHandler { get; private set; }
        [field: SerializeField]
        public Graphic SaveGraphic { get; private set; }

        [field: DI_Inject]
        public ConstructionController ConstructionController { get; private set; }

        private void CheckButtons()
        {
            CheckRestore();
            CheckSave();
            RefreshHighlight();
        }

        private void CheckRestore()
        {
            var isEnabled = CustomPlayerPrefs.HasMap();
            RestoreHandler.enabled = isEnabled;
            RestoreGraphic.color = isEnabled ? Color.white : Color.gray;
        }

        private void CheckSave()
        {
            var isEnabled = ConstructionController.IsDirty;
            SaveHandler.enabled = isEnabled;
            SaveGraphic.color = isEnabled ? Color.white : Color.gray;
        }

        private void RefreshHighlight()
        {
            if (SaveHandler.enabled)
            {
                SelectionController.ChangeTo(SaveHandler);
            }
            else
            {
                SelectionController.TryChangeIndex(0);
            }
        }

        #region External methods
        public void _OnClickRestore()
        {
            ConstructionController.RestoreDefaultMap();

            CheckButtons();
        }

        public void _OnClickSave()
        {
            ConstructionController.SaveCurrentMap();

            CheckButtons();
        }

        public void _OnClickExit()
        {
            SceneManager.LoadScene(GameScenes.LobbyScene);
        }
        #endregion

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void OnEnable()
        {
            CheckButtons();
        }
        #endregion
    }
}
