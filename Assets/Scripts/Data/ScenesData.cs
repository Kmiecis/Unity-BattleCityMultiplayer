using Common;
using UnityEngine;

namespace Tanks
{
    [CreateAssetMenu(menuName = nameof(Tanks) + "/" + nameof(ScenesData), fileName = nameof(ScenesData))]
    public class ScenesData : ScriptableObject
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
