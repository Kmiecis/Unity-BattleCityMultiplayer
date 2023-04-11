using Common;
using UnityEngine;

namespace Tanks
{
    [CreateAssetMenu(menuName = nameof(Tanks) + "/" + nameof(GameScenes), fileName = nameof(GameScenes))]
    public class GameScenes : ScriptableObject
    {
        [SerializeField]
        private AssetReference _lobbyScene;
        [SerializeField]
        private AssetReference _gameScene;
        [SerializeField]
        private AssetReference _constructionScene;

        public string LobbyScene
            => _lobbyScene.Name;

        public string GameScene
            => _gameScene.Name;

        public string ConstructionScene
            => _constructionScene.Name;
    }
}
