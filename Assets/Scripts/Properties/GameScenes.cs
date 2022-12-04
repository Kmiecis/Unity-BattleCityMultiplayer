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
            => _lobbyScene.Name;

        public string GameScene
            => _gameScene.Name;

        public string ConstructionScene
            => _constructionScene.Name;
    }
}
