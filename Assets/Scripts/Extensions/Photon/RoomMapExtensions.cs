using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Tanks.Extensions
{
    public static partial class RoomExtensions
    {
        public const string MAP_PROPERTY = "Map";

        public static bool TryGetMap(this Hashtable self, out string map)
        {
            return self.TryGetValue(MAP_PROPERTY, out map);
        }

        public static bool TryGetMap(this Room self, out string map)
        {
            return self.CustomProperties.TryGetMap(out map);
        }

        public static string GetMap(this Room self, string defaultValue = "")
        {
            if (!self.TryGetMap(out var map))
                map = defaultValue;
            return map;
        }

        public static void SetMap(this Room self, string map)
        {
            var properties = new Hashtable {
                { MAP_PROPERTY, map }
            };
            self.SetCustomProperties(properties);
        }

        public static void DeleteMap(this Room self)
        {
            var properties = new Hashtable {
                { MAP_PROPERTY, null }
            };
            self.SetCustomProperties(properties);
        }
    }
}
