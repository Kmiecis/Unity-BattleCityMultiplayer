using UnityEngine;

namespace Tanks
{
    [CreateAssetMenu(menuName = nameof(Tanks) + "/" + nameof(GameProperties), fileName = nameof(GameProperties))]
    public class GameProperties : ScriptableObject
    {
        public int minPlayers = 2;
        public int maxPlayers = 14;
        public byte defaultPlayers = 6;
        public int minTeamPlayers = 1;
        public int maxTeamPlayers = 7;
        public int maxRoomsListed = 7;

        public const int TEAM_A = -1;
        public const int TEAM_B = +1;
        public const int NO_TEAM = 0;

        public static int GetRandomTeam()
        {
            return Random.Range(0, 2) * 2 - 1;
        }
    }
}
