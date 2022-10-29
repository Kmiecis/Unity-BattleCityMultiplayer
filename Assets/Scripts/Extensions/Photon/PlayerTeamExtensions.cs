using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Tanks.Extensions
{
    public static partial class PlayerExtensions
    {
        private const string TEAM_PROPERTY = "Team";

        public static bool TryGetTeam(this Hashtable self, out ETeam team)
        {
            return self.TryGetValue(TEAM_PROPERTY, out team);
        }

        public static bool TryGetTeam(this Player self, out ETeam team)
        {
            return self.CustomProperties.TryGetTeam(out team);
        }

        public static ETeam GetTeam(this Player self, ETeam defaultValue = ETeam.None)
        {
            return (ETeam)self.CustomProperties.GetValueOrDefault(TEAM_PROPERTY, (int)defaultValue);
        }

        public static void SetTeam(this Player self, ETeam team)
        {
            var properties = new Hashtable { { TEAM_PROPERTY, team } };
            self.SetCustomProperties(properties);
        }
    }
}
