using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Tanks.Extensions
{
    public static partial class PlayerExtensions
    {
        private const string IS_READY_PROPERTY = "IsReady";

        public static bool TryGetIsReady(this Hashtable self, out bool isReady)
        {
            return self.TryGetValue(IS_READY_PROPERTY, out isReady);
        }

        public static bool TryGetIsReady(this Player self, out bool isReady)
        {
            return self.CustomProperties.TryGetIsReady(out isReady);
        }

        public static bool GetIsReady(this Player self, bool defaultValue = false)
        {
            return self.CustomProperties.GetValueOrDefault(IS_READY_PROPERTY, defaultValue);
        }

        public static void SetIsReady(this Player self, bool isReady)
        {
            var properties = new Hashtable { { IS_READY_PROPERTY, isReady } };
            self.SetCustomProperties(properties);
        }
    }
}
