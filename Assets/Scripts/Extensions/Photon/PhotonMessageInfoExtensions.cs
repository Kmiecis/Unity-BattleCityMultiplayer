using Photon.Pun;

namespace Tanks.Extensions
{
    public static class PhotonMessageInfoExtensions
    {
        public static float GetLag(this PhotonMessageInfo self)
        {
            return (float)(PhotonNetwork.Time - self.SentServerTime);
        }
    }
}
