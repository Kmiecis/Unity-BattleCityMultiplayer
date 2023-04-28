using Photon.Pun;

namespace Tanks
{
    public class AIInput : AInputController
    {
        public override bool IsPlayer
            => false;

        public override bool IsEnabled
        {
            set => base.IsEnabled = (
                value &&
                PhotonNetwork.LocalPlayer.IsMasterClient
            );
        }
    }
}
