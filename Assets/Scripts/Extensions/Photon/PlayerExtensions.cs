using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Tanks.Extensions
{
    public static partial class PlayerExtensions
    {
        public static void ResetProperties(this Player self)
        {
            var properties = new Hashtable
            {
                { IS_READY_PROPERTY, null },
                { TEAM_PROPERTY, null },
                { KILLS_PROPERTY, null },
                { DEATHS_PROPERTY, null }
            };
            self.SetCustomProperties(properties);
        }
    }
}
