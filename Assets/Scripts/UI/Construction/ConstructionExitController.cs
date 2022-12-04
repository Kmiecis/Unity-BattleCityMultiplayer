using Common.Injection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tanks.UI
{
    public class ConstructionExitController : MonoBehaviour
    {
        [field: SerializeField]
        public GameScenes GameScenes;
        [field: DI_Inject]
        public ConstructionController ConstructionController { get; private set; }

        #region External methods
        public void _OnClickRestore()
        {
            CustomPlayerPrefs.DeleteMap();
            ConstructionController.Reload();
        }

        public void _OnClickSave()
        {
            var map = ConstructionController.GetMap();
            CustomPlayerPrefs.SetMap(map);
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
        #endregion
    }
}
