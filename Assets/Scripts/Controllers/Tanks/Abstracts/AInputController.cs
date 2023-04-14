using Common;
using Photon.Pun;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(PhotonView))]
    public abstract class AInputController : MonoBehaviourPun, IInputController
    {
        [field: SerializeField]
        public MovementController MovementController { get; private set; }
        [field: SerializeField]
        public BulletController BulletController { get; private set; }

        private Vector2Int _direction = UVector2Int.Max;

        public Vector2Int Direction { get; set; }

        public bool Shooting { get; set; }

        public virtual bool IsEnabled
        {
            get => enabled;
            set => enabled = value;
        }

        private void UpdateInput()
        {
            if (Direction != _direction)
            {
                OnMovementChange(Direction);
            }
            _direction = Direction;

            if (Direction != Vector2Int.zero)
            {
                MovementController.SetMovement(_direction);
            }
            else
            {
                MovementController.StopMovement();
            }

            if (Shooting)
            {
                BulletController.TryShoot();
            }
        }

        protected virtual void OnMovementChange(Vector2Int value)
        {
        }

        #region Unity methods
        protected virtual void Update()
        {
            UpdateInput();
        }

        private void OnDisable()
        {
            MovementController.StopMovement();
        }
        #endregion
    }
}
