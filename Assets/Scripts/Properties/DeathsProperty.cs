using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Tanks
{
    public static partial class GameProperties
    {
        public const string DEATHS_PROPERTY = "PlayerDeaths";

        public static bool TryGetDeaths(Hashtable properties, out int deaths)
        {
            return TryGetProperty(properties, DEATHS_PROPERTY, out deaths);
        }

        public static int GetDeaths(Hashtable properties, int defaultValue = 0)
        {
            if (TryGetDeaths(properties, out var deaths))
            {
                return deaths;
            }
            return defaultValue;
        }

        public static int GetDeaths(Player player, int defaultValue = 0)
        {
            return GetDeaths(player.CustomProperties, defaultValue);
        }

        public static void SetDeaths(Player player, int deaths)
        {
            var properties = new Hashtable
            {
                { DEATHS_PROPERTY, deaths }
            };
            player.SetCustomProperties(properties);
        }

        public static void SetDeaths(int deaths)
        {
            SetDeaths(PhotonNetwork.LocalPlayer, deaths);
        }
    }
}
