using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Tanks.Extensions
{
    public static class RoomStatueCracksExtensions
    {
        public const string STATUE_CRACKS_A_PROPERTY = "StatueCracksA";
        public const string STATUE_CRACKS_B_PROPERTY = "StatueCracksB";

        private static string GetStatueCracksProperty(int team)
        {
            if (team == GameProperties.TEAM_A)
                return STATUE_CRACKS_A_PROPERTY;
            if (team == GameProperties.TEAM_B)
                return STATUE_CRACKS_B_PROPERTY;
            return null;
        }

        public static bool TryGetStatueCracks(this Hashtable self, int team, out int cracks)
        {
            return self.TryGetValue(GetStatueCracksProperty(team), out cracks);
        }

        public static bool TryGetStatueCracks(this Room self, int team, out int cracks)
        {
            return self.CustomProperties.TryGetStatueCracks(team, out cracks);
        }

        public static int GetStatueCracks(this Room self, int team, int defaultValue = 0)
        {
            return self.CustomProperties.GetValueOrDefault(GetStatueCracksProperty(team), defaultValue);
        }

        public static void SetStatueCracks(this Room self, int team, int cracks)
        {
            var properties = new Hashtable { { GetStatueCracksProperty(team), cracks } };
            self.SetCustomProperties(properties);
        }

        public static void IncrStatueCracks(this Room self, int team)
        {
            self.SetStatueCracks(team, self.GetStatueCracks(team) + 1);
        }

        public static void DecrStatueCracks(this Room self, int team)
        {
            self.SetStatueCracks(team, self.GetStatueCracks(team) - 1);
        }
    }
}
