using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Tanks.Extensions
{
    public static partial class RoomExtensions
    {
        public const string TEAM_WON_PROPERTY = "TeamWon";

        public static bool TryGetTeamWon(this Hashtable self, out ETeam team)
        {
            return self.TryGetValue(TEAM_WON_PROPERTY, out team);
        }

        public static bool TryGetTeamWon(this Room self, out ETeam team)
        {
            return self.CustomProperties.TryGetTeamWon(out team);
        }

        public static ETeam GetTeamWon(this Room self, ETeam defaultValue = ETeam.None)
        {
            return (ETeam)self.CustomProperties.GetValueOrDefault(TEAM_WON_PROPERTY, (int)defaultValue);
        }

        public static void SetTeamWon(this Room self, ETeam team)
        {
            var properties = new Hashtable { { TEAM_WON_PROPERTY, team } };
            self.SetCustomProperties(properties);
        }
    }
}
