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
                { GetTeamWinsProperty(ETeam.A), null },
                { GetTeamWinsProperty(ETeam.B), null }
            };
            self.SetCustomProperties(properties);
        }
    }
}
