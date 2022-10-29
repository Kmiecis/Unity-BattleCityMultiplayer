using Photon.Pun;
using Tanks.Extensions;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(PhotonView))]
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

        private TanksController _gameController;

        public int Team
        {
            get => photonView.Owner.GetTeam();
        }

        public void Setup(TanksController gameController)
        {
            _gameController = gameController;
        }

        private void SetVisiblity(bool value)
        {
            enabled = value && photonView.IsMine;
            ModelObject.SetActive(value);
        }

        public void Explode()
        {
            SetVisiblity(false);

            ExplosionController.Explode();
        }

        private void ExplodeMine()
        {
            SetVisiblity(false);

            ExplosionController.Explode();
            ExplosionController.SetCallback(OnExplode);

            photonView.Owner.IncrDeaths();
        }

        public void RPCExplode()
        {
            photonView.RPC(nameof(RPCTankExplode_Internal), RpcTarget.Others, transform.position);
        }

        [PunRPC]
        private void RPCTankExplode_Internal(Vector3 position)
        {
            transform.position = position;

            if (photonView.IsMine)
            {
                ExplodeMine();
            }
            else
            {
                Explode();
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

        private void OnDisable()
        {
            MovementController.ResetMovement();
        }
    }
}
