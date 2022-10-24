using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Tanks.Extensions
{
    public static partial class PlayerExtensions
    {
        private const string KILLS_PROPERTY = "Kills";

        public static bool TryGetKills(this Hashtable self, out int kills)
        {
            return self.TryGetValue(KILLS_PROPERTY, out kills);
        }

        public static bool TryGetKills(this Player self, out int kills)
        {
            return self.CustomProperties.TryGetKills(out kills);
        }

        public static int GetKills(this Player self, int defaultValue = 0)
        {
            return self.CustomProperties.GetValueOrDefault(KILLS_PROPERTY, defaultValue);
        }

        public static void SetKills(this Player self, int kills)
        {
            var properties = new Hashtable { { KILLS_PROPERTY, kills } };
            self.SetCustomProperties(properties);
        }

        public static void IncrKills(this Player self)
        {
            self.SetKills(self.GetKills() + 1);
        }
    }
}
