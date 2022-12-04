using Common;
using UnityEngine;

namespace Tanks
{
    public static class CustomPlayerPrefs
    {
        private const string NAME_KEY = "PlayerName";
        private const string MAP_KEY = "PlayerMap";

        public static string GetName(string defaultValue = "")
        {
            return PlayerPrefs.GetString(NAME_KEY, defaultValue);
        }

        public static void SetName(string value)
        {
            PlayerPrefs.SetString(NAME_KEY, value);
        }

        public static void DeleteName()
        {
            PlayerPrefs.DeleteKey(NAME_KEY);
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
