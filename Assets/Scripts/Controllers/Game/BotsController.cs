using Photon.Pun;
using Photon.Realtime;

namespace Tanks
{
    public class BotsController : MonoBehaviourPunCallbacks
    {
        public string tankABotPrefab;
        public string tankBBotPrefab;

        #region Photon methods
        public override void OnMasterClientSwitched(Player newMasterClient)
        {

        }
        #endregion
    }
}
