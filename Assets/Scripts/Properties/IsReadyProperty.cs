using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Tanks
{
    public static partial class GameProperties
    {
        public const string IS_READY_PROPERTY = "IsPlayerReady";

        public static bool TryGetIsReady(Hashtable properties, out bool isReady)
        {
            return TryGetProperty(properties, IS_READY_PROPERTY, out isReady);
        }

        public static bool GetIsReady(Hashtable properties, bool defaultValue = false)
        {
            if (TryGetIsReady(properties, out var isReady))
            {
                return isReady;
            }
            return defaultValue;
        }

        public static bool GetIsReady(Player player, bool defaultValue = false)
        {
            return GetIsReady(player.CustomProperties, defaultValue);
        }

        public static void SetIsReady(Player player, bool isReady)
        {
            var properties = new Hashtable
            {
                { IS_READY_PROPERTY, isReady }
            };
            player.SetCustomProperties(properties);
        }

        public static void SetIsReady(bool isReady)
        {
            SetIsReady(PhotonNetwork.LocalPlayer, isReady);
        }
    }
}
