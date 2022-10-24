using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Tanks.Extensions
{
    public static partial class PlayerExtensions
    {
        private const string DEATHS_PROPERTY = "Deaths";

        public static bool TryGetDeaths(this Hashtable self, out int deaths)
        {
            return self.TryGetValue(DEATHS_PROPERTY, out deaths);
        }

        public static bool TryGetDeaths(this Player self, out int deaths)
        {
            return self.CustomProperties.TryGetDeaths(out deaths);
        }

        public static int GetDeaths(this Player self, int defaultValue = 0)
        {
            return self.CustomProperties.GetValueOrDefault(DEATHS_PROPERTY, defaultValue);
        }

        public static void SetDeaths(this Player self, int deaths)
        {
            var properties = new Hashtable { { DEATHS_PROPERTY, deaths } };
            self.SetCustomProperties(properties);
        }

        public static void IncrDeaths(this Player self)
        {
            self.SetDeaths(self.GetDeaths() + 1);
        }
    }
}
