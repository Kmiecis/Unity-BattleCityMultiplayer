using UnityEngine;

namespace Tanks
{
    [CreateAssetMenu(menuName = nameof(Tanks) + "/" + nameof(GameScenes), fileName = nameof(GameScenes))]
    public class GameScenes : ScriptableObject
    {
        [SerializeField]
        private Object _lobbyScene;
        [SerializeField]
        private Object _gameScene;
        [SerializeField]
        private Object _constructionScene;

        public string LobbyScene
            => _lobbyScene.name;

        public string GameScene
            => _gameScene.name;

        public string ConstructionScene
            => _constructionScene.name;
    }
}
