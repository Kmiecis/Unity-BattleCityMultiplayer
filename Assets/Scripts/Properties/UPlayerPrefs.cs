using UnityEngine;

namespace Tanks
{
    public static class UPlayerPrefs
    {
        private const string NAME_KEY = "PlayerName";

        public static string GetName(string defaultValue = "")
        {
            return PlayerPrefs.GetString(NAME_KEY, defaultValue);
        }

        public static void SetName(string value)
        {
            PlayerPrefs.SetString(NAME_KEY, value);
        }
    }
}
