using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Tanks
{
    public static class MapSerializer
    {
        private const char TILE_DELIMITER = '|';
        private const char VALUE_DELIMITER = ';';

        public static string Serialize(Dictionary<Vector2Int, int> map)
        {
            var builder = new StringBuilder();

            builder.Append(map.Count);
            foreach (var kv in map)
            {
                var coordinates = kv.Key;
                var id = kv.Value;

                builder.Append(TILE_DELIMITER)
                    .Append(coordinates.x)
                    .Append(VALUE_DELIMITER)
                    .Append(coordinates.y)
                    .Append(VALUE_DELIMITER)
                    .Append(id);
            }

            return builder.ToString();
        }

        public static Dictionary<Vector2Int, int> Deserialize(string value)
        {
            var result = new Dictionary<Vector2Int, int>();

            var values = value.Split(TILE_DELIMITER);
            for (int i = 1; i < values.Length; ++i)
            {
                var tile = values[i].Split(VALUE_DELIMITER);
                var cx = int.Parse(tile[0]);
                var cy = int.Parse(tile[1]);
                var id = int.Parse(tile[2]);

                result.Add(new Vector2Int(cx, cy), id);
            }

            return result;
        }
    }
}
