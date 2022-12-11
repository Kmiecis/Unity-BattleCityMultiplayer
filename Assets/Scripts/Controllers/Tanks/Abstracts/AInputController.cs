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

        #region Unity methods
        private void OnDisable()
        {
            MovementController.StopMovement();
        }
        #endregion
    }
}
