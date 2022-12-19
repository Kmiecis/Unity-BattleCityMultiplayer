using Common;
using UnityEngine;

namespace Tanks
{
    [CreateAssetMenu(menuName = nameof(Tanks) + "/" + nameof(GameScenes), fileName = nameof(GameScenes))]
    public class GameScenes : ScriptableObject
    {
        [SerializeField]
        private ObjectReference _lobbyScene;
        [SerializeField]
        private ObjectReference _gameScene;
        [SerializeField]
        private ObjectReference _constructionScene;

        public string LobbyScene
            => _lobbyScene.AssetName;

        public string GameScene
            => _gameScene.AssetName;

        public string ConstructionScene
            => _constructionScene.AssetName;
    }
}
