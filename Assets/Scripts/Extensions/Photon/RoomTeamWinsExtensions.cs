using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Tanks.Extensions
{
    public static partial class RoomExtensions
    {
        public const string TEAM_WINS_PROPERTY = "Team{0}Wins";

        public static string GetTeamWinsProperty(ETeam team)
        {
            return string.Format(TEAM_WINS_PROPERTY, team);
        }

        public static bool TryGetTeamWins(this Hashtable self, ETeam team, out int wins)
        {
            return self.TryGetValue(GetTeamWinsProperty(team), out wins);
        }

        public static bool TryGetTeamWins(this Room self, ETeam team, out int wins)
        {
            return self.CustomProperties.TryGetTeamWins(team, out wins);
        }

        public static int GetTeamWins(this Room self, ETeam team, int defaultValue = 0)
        {
            if (!self.TryGetTeamWins(team, out int wins))
                wins = defaultValue;
            return wins;
        }

        public static void IncrTeamWins(this Room self, ETeam team)
        {
            var properties = new Hashtable {
                { GetTeamWinsProperty(team), self.GetTeamWins(team) + 1 }
            };
            self.SetCustomProperties(properties);
        }
    }
}
