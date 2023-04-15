using Common;
using UnityEngine;

namespace Tanks
{
    public static partial class CustomPlayerPrefs
    {
        private const string MAP_KEY = "PlayerMap";

        public static bool HasMap()
        {
            return PlayerPrefs.HasKey(MAP_KEY);
        }

        public static bool TryGetMap(out string value)
        {
            return UPlayerPrefs.TryGetString(MAP_KEY, out value);
        }

        public static void SetMap(string value)
        {
            PlayerPrefs.SetString(MAP_KEY, value);
        }

        public static void DeleteMap()
        {
            PlayerPrefs.DeleteKey(MAP_KEY);
        }
    }
}
