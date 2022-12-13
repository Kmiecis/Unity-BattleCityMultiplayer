using Common;
using Photon.Pun;
using UnityEngine;

namespace Tanks
{
    public class AIInput : AInputController
    {
        public override bool IsEnabled
        {
            set => base.IsEnabled = (
                value &&
                PhotonNetwork.LocalPlayer.IsMasterClient
            );
        }

        private Vector2Int _direction;

        public override Vector2Int Direction
            => _direction;

        public override bool Shoot
            => true;

        #region Unity methods
        private void Start()
        {
            _direction = new Vector2Int(URandom.Sign(), URandom.Sign());
        }
        #endregion
    }
}
