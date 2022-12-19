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

        public virtual bool IsEnabled
        {
            get => enabled;
            set => enabled = value;
        }

        public abstract Vector2Int Direction { get; }

        public abstract bool Shoot { get; }

        private void ApplyInput()
        {
            if (Direction != Vector2Int.zero)
            {
                MovementController.SetMovement(Direction);
            }
            else
            {
                MovementController.StopMovement();
            }

            if (Shoot)
            {
                BulletController.Fire();
            }
        }

        #region Unity methods
        protected virtual void Update()
        {
            ApplyInput();
        }

        private void OnDisable()
        {
            MovementController.StopMovement();
        }
        #endregion
    }
}
