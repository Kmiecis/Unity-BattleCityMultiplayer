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
        private bool _shoot;

        public override Vector2Int Direction
            => _direction;

        public override bool Shoot
            => _shoot;

        public void SetDirection(Vector2Int value)
        {
            _direction = value;
        }

        public bool HasDirection()
        {
            return _direction != Vector2Int.zero;
        }

        public void ResetDirection()
        {
            SetDirection(Vector2Int.zero);
        }

        public void SetShoot(bool value)
        {
            _shoot = value;
        }
    }
}
