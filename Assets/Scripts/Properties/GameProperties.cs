using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Tanks
{
    public static partial class GameProperties
    {
        public const int MIN_PLAYERS = 2;
        public const int MAX_PLAYERS = 14;
        public const int MIN_TEAM_PLAYERS = 1;
        public const int MAX_TEAM_PLAYERS = 7;

        public static bool TryGetProperty<T>(Hashtable properties, object key, out T value)
        {
            if (properties.TryGetValue(key, out var obj))
            {
                value = (T)obj;
                return true;
            }
            value = default;
            return false;
        }

        public static void SetInitialProperties(Player player, int team)
        {
            var properties = new Hashtable
            {
                { TEAM_PROPERTY, team }
            };
            player.SetCustomProperties(properties);
        }

        public static void SetInitialProperties(int team)
        {
            SetInitialProperties(PhotonNetwork.LocalPlayer, team);
        }
    }
}
