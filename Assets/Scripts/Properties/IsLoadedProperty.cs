using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Tanks
{
    public static partial class GameProperties
    {
        public const string IS_LOADED_PROPERTY = "PlayerLoadedLevel";

        public static bool TryGetIsLoaded(Hashtable properties, out bool isLoaded)
        {
            return TryGetProperty(properties, IS_LOADED_PROPERTY, out isLoaded);
        }

        public static bool GetIsLoaded(Hashtable properties, bool defaultValue = false)
        {
            if (TryGetIsLoaded(properties, out var isLoaded))
            {
                return isLoaded;
            }
            return defaultValue;
        }

        public static bool GetIsLoaded(Player player, bool defaultValue = false)
        {
            return GetIsLoaded(player.CustomProperties, defaultValue);
        }

        public static void SetIsLoaded(Player player, bool isLoaded)
        {
            var properties = new Hashtable
            {
                { IS_LOADED_PROPERTY, isLoaded }
            };
            player.SetCustomProperties(properties);
        }

        public static void SetIsLoaded(bool isLoaded)
        {
            SetIsLoaded(PhotonNetwork.LocalPlayer, isLoaded);
        }
    }
}
