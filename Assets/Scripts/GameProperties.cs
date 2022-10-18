using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Tanks
{
    public static class GameProperties
    {
        public const string IS_READY_PROPERTY = "IsPlayerReady";
        public const string TEAM_PROPERTY = "PlayerTeam";
        public const string IS_LOADED_PROPERTY = "PlayerLoadedLevel";

        public const int TEAM_A = -1;
        public const int TEAM_B = +1;
        public const int NO_TEAM = 0;

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

        #region IsReady
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
        #endregion

        #region Team
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
        #endregion

        #region IsLoaded
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
        #endregion

        public static void SetInitialProperties(Player player, int team)
        {
            var properties = new Hashtable
            {
                { IS_READY_PROPERTY, false },
                { TEAM_PROPERTY, team },
                { IS_LOADED_PROPERTY, false }
            };
            player.SetCustomProperties(properties);
        }

        public static void SetInitialProperties(int team)
        {
            SetInitialProperties(PhotonNetwork.LocalPlayer, team);
        }
    }
}
