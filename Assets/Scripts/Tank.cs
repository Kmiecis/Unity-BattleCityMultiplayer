using Common.Extensions;
using Photon.Pun;
using UnityEngine;

namespace Tanks
{
    public class Tank : MonoBehaviourPun
    {
        [field: SerializeField]
        public GameObject ModelObject { get; private set; }
        [field: SerializeField]
        public AInputController InputController { get; private set; }
        [field: SerializeField]
        public MovementController MovementController { get; private set; }
        [field: SerializeField]
        public BulletController BulletController { get; private set; }
        [field: SerializeField]
        public ForcefieldController ForcefieldController { get; private set; }
        [field: SerializeField]
        public ExplosionController ExplosionController { get; private set; }

        public void Setup()
        {
            ModelObject.SetActive(true);
            ExplosionController.Setup();
            InputController.enabled = photonView.IsMine;
        }

        private void Explode()
        {
            ModelObject.SetActive(false);
            ExplosionController.Explode();
            InputController.enabled = false;
        }

        public void Destroy()
        {
            Explode();

            photonView.RPC(nameof(RPCDestroy), RpcTarget.Others, transform.position);
        }

        [PunRPC]
        private void RPCDestroy(Vector3 position, PhotonMessageInfo info)
        {
            transform.position = position;

            Explode();
        }

        private void OnBulletHit(Collider2D other)
        {
            if (
                other.TryGetComponentInParent<Tank>(out var tank) &&
                !tank.ForcefieldController.IsActive
            )
            {
                tank.Destroy();
            }
        }

        private void OnEnable()
        {
            ForcefieldController.Enable();
        }

        private void Awake()
        {
            BulletController.Setup(OnBulletHit);
        }

        private void Update()
        {
            var direction = InputController.Direction;
            if (direction != Vector2Int.zero)
            {
                MovementController.SetMovement(direction);
            }
            else
            {
                MovementController.ResetMovement();
            }

            if (InputController.Shoot)
            {
                BulletController.Fire();
            }
        }
    }
}
