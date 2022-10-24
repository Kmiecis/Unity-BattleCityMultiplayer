using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Tanks.Extensions
{
    public static partial class PlayerExtensions
    {
        private const string TEAM_PROPERTY = "Team";

        public static bool TryGetTeam(this Hashtable self, out int team)
        {
            return self.TryGetValue(TEAM_PROPERTY, out team);
        }

        public static bool TryGetTeam(this Player self, out int team)
        {
            return self.CustomProperties.TryGetTeam(out team);
        }

        public static int GetTeam(this Player self, int defaultValue = GameProperties.NO_TEAM)
        {
            return self.CustomProperties.GetValueOrDefault(TEAM_PROPERTY, defaultValue);
        }

        public static void SetTeam(this Player self, int team)
        {
            var properties = new Hashtable { { TEAM_PROPERTY, team } };
            self.SetCustomProperties(properties);
        }
    }
}
