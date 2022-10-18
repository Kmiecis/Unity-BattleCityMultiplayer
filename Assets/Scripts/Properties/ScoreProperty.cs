using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Tanks
{
    public static partial class GameProperties
    {
        public const string SCORE_PROPERTY = "PlayerScore";

        public static bool TryGetScore(Hashtable properties, out int score)
        {
            return TryGetProperty(properties, SCORE_PROPERTY, out score);
        }

        public static int GetScore(Hashtable properties, int defaultValue = 0)
        {
            if (TryGetScore(properties, out var score))
            {
                return score;
            }
            return defaultValue;
        }

        public static int GetScore(Player player, int defaultValue = 0)
        {
            return GetScore(player.CustomProperties, defaultValue);
        }

        public static void SetScore(Player player, int score)
        {
            var properties = new Hashtable
            {
                { SCORE_PROPERTY, score }
            };
            player.SetCustomProperties(properties);
        }

        public static void SetScore(int score)
        {
            SetScore(PhotonNetwork.LocalPlayer, score);
        }
    }
}
