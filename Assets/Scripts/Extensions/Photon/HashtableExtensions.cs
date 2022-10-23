using ExitGames.Client.Photon;

namespace Tanks.Extensions
{
    public static class HashtableExtensions
    {
        public static bool TryGetValue<T>(this Hashtable self, object key, out T value)
        {
            if (
                self.TryGetValue(key, out object obj) &&
                obj is T objValue
            )
            {
                value = objValue;
                return true;
            }

            value = default;
            return false;
        }

        public static T GetValueOrDefault<T>(this Hashtable self, object key, T defaultValue = default)
        {
            return self.TryGetValue(key, out T value) ? value : defaultValue;
        }
    }
}
