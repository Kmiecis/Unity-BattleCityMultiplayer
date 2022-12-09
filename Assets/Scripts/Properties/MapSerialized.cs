﻿using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    [CreateAssetMenu(menuName = nameof(Tanks) + "/" + nameof(MapSerialized), fileName = nameof(MapSerialized))]
    public class MapSerialized : ScriptableObject
    {
        [field: SerializeField]
        public string Value { get; private set; }

        public void Serialize(Dictionary<Vector2Int, int> map)
        {
            Value = MapSerializer.Serialize(map);
        }

        public Dictionary<Vector2Int, int> Deserialize()
        {
            return MapSerializer.Deserialize(Value);
        }
    }
}
