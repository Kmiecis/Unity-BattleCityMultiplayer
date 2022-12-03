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

        private Dictionary<Vector2Int, int> _ids = new();
        private Dictionary<Vector2Int, GameObject> _objects = new();

        public void ChangeTile(Vector2Int coordinates)
        {
            var index = GetId(coordinates);
            var next = Mathx.NextIndex(index, Prefabs.Length);

            SetTile(coordinates, next);
        }

        private void SetTile(Vector2Int coordinates, int id)
        {
            ClearId(coordinates);
            ClearObject(coordinates);

            if (id != CLEAR_TILE)
            {
                _ids[coordinates] = id;
                _objects[coordinates] = Instantiate(Prefabs[id], (Vector2)coordinates, Quaternion.identity, transform);
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

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
