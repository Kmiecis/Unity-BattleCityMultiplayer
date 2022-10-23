using UnityEngine;

namespace Tanks
{
    public static partial class GameProperties
    {
        public const int MIN_PLAYERS = 2;
        public const int MAX_PLAYERS = 14;
        public const int MIN_TEAM_PLAYERS = 1;
        public const int MAX_TEAM_PLAYERS = 7;

        public const int TEAM_A = -1;
        public const int TEAM_B = +1;
        public const int NO_TEAM = 0;

        public static int GetRandomTeam()
        {
            return Random.Range(0, 2) * 2 - 1;
        }
    }
}
