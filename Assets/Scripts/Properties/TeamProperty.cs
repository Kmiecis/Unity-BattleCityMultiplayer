using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Tanks
{
    public static partial class GameProperties
    {
        public const string TEAM_PROPERTY = "PlayerTeam";

        public const int TEAM_A = -1;
        public const int TEAM_B = +1;
        public const int NO_TEAM = 0;

        public static bool TryGetTeam(Hashtable properties, out int team)
        {
            return TryGetProperty(properties, TEAM_PROPERTY, out team);
        }

        public static int GetTeam(Hashtable properties, int defaultValue = NO_TEAM)
        {
            if (TryGetTeam(properties, out var team))
            {
                return team;
            }
            return defaultValue;
        }

        public static int GetTeam(Player player, int defaultValue = NO_TEAM)
        {
            return GetTeam(player.CustomProperties, defaultValue);
        }

        public static void SetTeam(Player player, int team)
        {
            var properties = new Hashtable
            {
                { TEAM_PROPERTY, team }
            };
            player.SetCustomProperties(properties);
        }

        public static void SetTeam(int team)
        {
            SetTeam(PhotonNetwork.LocalPlayer, team);
        }

        public static int GetRandomTeam()
        {
            return Random.Range(0, 2) * 2 - 1;
        }
    }
}
