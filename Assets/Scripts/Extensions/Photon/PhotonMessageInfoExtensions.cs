using Photon.Pun;

namespace Tanks.Extensions
{
    public static class PhotonMessageInfoExtensions
    {
        public static float GetLag(this PhotonMessageInfo self)
        {
            return (float)(PhotonNetwork.Time - self.SentServerTime);
        }

        public static T GetDataAt<T>(this PhotonMessageInfo self, int index)
        {
            var data = self.photonView.InstantiationData;
            return (T)data[index];
        }
    }
}
