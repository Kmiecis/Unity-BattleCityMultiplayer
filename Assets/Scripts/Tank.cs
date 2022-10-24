using Common.Extensions;
using Photon.Pun;
using Tanks.Extensions;
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
        [field: SerializeField]
        public RespawnController RespawnController { get; private set; }

        private GameController _gameController;

        public void Setup(GameController gameController)
        {
            _gameController = gameController;
        }

        private void SetVisiblity(bool value)
        {
            ModelObject.SetActive(value);
            InputController.enabled = value && photonView.IsMine;
        }

        private void Explode()
        {
            SetVisiblity(false);

            ExplosionController.Explode();
        }

        public void Destroy()
        {
            Explode();

            RPCDestroy();
        }

        public void RPCDestroy()
        {
            photonView.RPC(nameof(RPCDestroy_Internal), RpcTarget.Others, transform.position);
        }

        [PunRPC]
        private void RPCDestroy_Internal(Vector3 position)
        {
            transform.position = position;

            Explode();

            if (photonView.IsMine)
            {
                ExplosionController.SetCallback(OnExplode);

                PhotonNetwork.LocalPlayer.IncrDeaths();
            }
        }

        private void OnBulletHit(Collider2D other)
        {
            if (other.TryGetComponentInParent<Tank>(out var tank) &&
                !tank.ForcefieldController.IsActive)
            {
                tank.Destroy();

                PhotonNetwork.LocalPlayer.IncrKills();
            }
        }

        private void OnExplode()
        {
            var spawn = _gameController.GetBestSpawn();
            RespawnController.Respawn(spawn.transform.position);
        }

        private void OnRespawn()
        {
            SetVisiblity(true);

            ForcefieldController.Enable();
        }

        private void Awake()
        {
            BulletController.SetCallback(OnBulletHit);
            RespawnController.SetCallback(OnRespawn);
        }

        private void Start()
        {
            SetVisiblity(true);

            ForcefieldController.Enable();
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
