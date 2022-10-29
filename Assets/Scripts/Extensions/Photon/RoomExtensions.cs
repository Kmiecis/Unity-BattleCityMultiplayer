using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Tanks.Extensions
{
    public static partial class RoomExtensions
    {
        public static void ResetProperties(this Room self)
        {
            var properties = new Hashtable
            {
                { TEAM_WON_PROPERTY, null }
            };
            self.SetCustomProperties(properties);
        }
    }
}
