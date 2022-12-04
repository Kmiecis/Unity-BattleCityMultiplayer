using Common.Injection;
using Common.Mathematics;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    [DI_Install]
    public class ConstructionController : MonoBehaviour
    {
        private const int CLEAR_TILE = 0;

        [field: SerializeField]
        public GameObject[] Prefabs;
        [field: SerializeField]
        public MapSerialized DefaultMap;

        private Dictionary<Vector2Int, int> _ids = new();
        private Dictionary<Vector2Int, GameObject> _objects = new();
        private int _cached = CLEAR_TILE;

        public void ChangeTile(Vector2Int coordinates)
        {
            var index = GetId(coordinates);
            if (index != CLEAR_TILE || _cached == CLEAR_TILE)
            {
                _cached = Mathx.NextIndex(index, Prefabs.Length);
            }

            SetTile(coordinates, _cached);
        }

        private void SetTile(Vector2Int coordinates, int id)
        {
            ClearId(coordinates);
            ClearObject(coordinates);

            if (id != CLEAR_TILE)
            {
                var position = (Vector2)coordinates;
                var rotation = Quaternion.identity;
                var prefab = Prefabs[id];
                var instance = Instantiate(prefab, position, rotation, transform);

                _ids[coordinates] = id;
                _objects[coordinates] = instance;
            }
        }

        private int GetId(Vector2Int coordinates)
        {
            if (_ids.TryGetValue(coordinates, out var id))
                return id;
            return CLEAR_TILE;
        }

        private void ClearId(Vector2Int coordinates)
        {
            _ids.Remove(coordinates);
        }

        private void ClearObject(Vector2Int coordinates)
        {
            if (_objects.TryGetValue(coordinates, out var gameObject))
            {
                Destroy(gameObject);
                _objects.Remove(coordinates);
            }
        }

        private void Clear()
        {
            _ids.Clear();
            foreach (var gameObject in _objects.Values)
                Destroy(gameObject);
            _objects.Clear();
        }

        private string GetMapToLoad()
        {
            if (!CustomPlayerPrefs.TryGetMap(out var map))
                map = DefaultMap.value;
            return map;
        }

        public string GetMap()
        {
            return MapSerializer.Serialize(_ids);
        }

        public void SetMap(string serialized)
        {
            var map = MapSerializer.Deserialize(serialized);

            foreach (var kv in map)
            {
                SetTile(kv.Key, kv.Value);
            }
        }

        public void Reload()
        {
            Clear();
            var map = GetMapToLoad();
            SetMap(map);
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Start()
        {
            Reload();
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
