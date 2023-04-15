using UnityEngine;

namespace Tanks
{
    public static partial class CustomPlayerPrefs
    {
        private const string VOLUME_KEY = "PlayerVolume";

        public static bool HasVolume()
        {
            return PlayerPrefs.HasKey(VOLUME_KEY);
        }

        public static float GetVolume(float defaultValue = 1.0f)
        {
            return PlayerPrefs.GetFloat(VOLUME_KEY, defaultValue);
        }

        public static void SetVolume(float value)
        {
            PlayerPrefs.SetFloat(VOLUME_KEY, value);
        }

        public static void DeleteVolume()
        {
            PlayerPrefs.DeleteKey(VOLUME_KEY);
        }
    }
}
