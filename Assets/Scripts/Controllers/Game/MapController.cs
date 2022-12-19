using Common.Injection;
using Photon.Pun;
using System.Collections.Generic;
using Tanks.Extensions;
using UnityEngine;

namespace Tanks
{
    [DI_Install]
    public class MapController : MonoBehaviour
    {
        [field: SerializeField]
        public ConstructionBlocks Blocks { get; private set; }

        private Dictionary<Vector2Int, Block> _tiles = new Dictionary<Vector2Int, Block>();

        public Dictionary<Vector2Int, Block> Tiles
            => _tiles;

        private void Load()
        {
            var map = GetMapToLoad();
            LoadMap(map);
        }

        private void LoadMap(string map)
        {
            var deserialized = MapSerializer.Deserialize(map);
            foreach (var kv in deserialized)
            {
                var coordinates = kv.Key;
                var id = kv.Value;

                var block = CreateBlock(coordinates, id);
                _tiles.Add(coordinates, block);
            }
        }

        private Block CreateBlock(Vector2Int coordinates, int id)
        {
            var prefab = Blocks[id];
            var position = (Vector2)coordinates;
            var rotation = Quaternion.identity;
            return Instantiate(prefab, position, rotation, transform);
        }

        private string GetMapToLoad()
        {
            if (PhotonNetwork.InRoom)
                return PhotonNetwork.CurrentRoom.GetMap();
            return string.Empty;
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Start()
        {
            Load();
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
