using UnityEngine;

namespace Tanks
{
    [CreateAssetMenu(menuName = nameof(Tanks) + "/" + nameof(GameData), fileName = nameof(GameData))]
    public class GameData : ScriptableObject
    {
        [Header("Room generation")]
        public byte minPlayers = 2;
        public byte maxPlayers = 14;
        public byte defaultPlayers = 6;
        [Header("Room preparation")]
        public int minTeamPlayers = 1;
        public int maxTeamPlayers = 7;
        public int maxRoomsListed = 7;
        [Header("End game")]
        public float endGameDelay = 5.0f;

        public static ETeam GetRandomTeam()
        {
            return (ETeam)(Random.Range(0, 2) * 2 - 1);
        }
    }
}
