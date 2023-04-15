using UnityEngine;

namespace Tanks
{
    public static partial class CustomPlayerPrefs
    {
        private const string NAME_KEY = "PlayerName";

        public static bool HasName()
        {
            return PlayerPrefs.HasKey(NAME_KEY);
        }

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
    }
}
