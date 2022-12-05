using Common;
using UnityEngine;

namespace Tanks
{
    [CreateAssetMenu(menuName = nameof(Tanks) + "/" + nameof(GameScenes), fileName = nameof(GameScenes))]
    public class GameScenes : ScriptableObject
    {
        [SerializeField]
        private SceneReference _lobbyScene;
        [SerializeField]
        private SceneReference _gameScene;
        [SerializeField]
        private SceneReference _constructionScene;

        public string LobbyScene
            => _lobbyScene.SceneName;

        public string GameScene
            => _gameScene.SceneName;

        public string ConstructionScene
            => _constructionScene.SceneName;
    }
}
