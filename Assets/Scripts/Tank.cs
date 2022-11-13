using Common.Extensions;
using Common.Injection;
using Photon.Pun;
using Tanks.Extensions;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(PhotonView))]
    public class Tank : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {
        public ETeam team;

        [field: SerializeField]
        public GameObject ModelObject { get; private set; }
        [field: SerializeField]
        public GameObject HighlightedObject { get; private set; }
        [field: SerializeField]
        public AInputController InputController { get; private set; }
        [field: SerializeField]
        public MovementController MovementController { get; private set; }
        [field: SerializeField]
        public BulletController BulletController { get; private set; }
        [field: SerializeField]
        public ForcefieldController ForcefieldController { get; private set; }
        [field: SerializeField]
        public RespawnController RespawnController { get; private set; }
        [field: SerializeField]
        public UpgradeController UpgradeController { get; private set; }
        [field: DI_Inject(nameof(OnTanksControllerInject))]
        public TanksController TanksController { get; private set; }
        [field: DI_Inject]
        public SpawnsController SpawnsController { get; private set; }
        [field: DI_Inject]
        public EffectsController EffectsController { get; private set; }

        public bool IsVisible
        {
            get => ModelObject.activeSelf;
        }

        public bool IsEnabled
        {
            get => enabled;
            set => enabled = value && photonView.IsMine;
        }    

        public void SetVisiblity(bool value)
        {
            IsEnabled = value;
            ModelObject.SetActive(value);
            HighlightedObject.SetActive(value && photonView.IsMine);
        }

        public void Hit()
        {
            if (!ForcefieldController.IsActive)
            {
                if (UpgradeController.TryDowngrade())
                {
                    UpgradeController.RPCDowngrade();
                }
                else
                {
                    Explode();
                    RPCExplode();
                }
            }
        }

        public void Explode()
        {
            SetVisiblity(false);

            EffectsController.SpawnBigExplosion(transform.position);
            UpgradeController.SetDefault();

            if (photonView.IsMine)
            {
                OnExplode();

                photonView.Owner.IncrDeaths();
            }
        }

        public void RPCExplode()
        {
            photonView.RPC(nameof(RPCTankExplode_Internal), RpcTarget.Others, transform.position);
        }

        [PunRPC]
        private void RPCTankExplode_Internal(Vector3 position)
        {
            transform.position = position;

            Explode();
        }

        private void OnExplode()
        {
            var spawn = SpawnsController.GetBestSpawn();
            RespawnController.Respawn(spawn.transform.position);
            RespawnController.RPCRespawn(spawn.transform.position);
        }

        private void OnRespawn()
        {
            SetVisiblity(true);

            ForcefieldController.Enable(0.0f);
            ForcefieldController.RPCEnable();
        }

        private void Touched(Collider2D other)
        {
            if (other.TryGetComponentInParent<Pickup>(out var pickup))
            {
                Touched(pickup);
            }
        }

        private void Touched(Pickup pickup)
        {
            if (photonView.IsMine)
            {
                pickup.PickedFor(team);
            }
            pickup.Picked();
        }

        #region External methods
        public void _OnTriggerEntered(Collider2D other)
        {
            Touched(other);
        }
        #endregion

        #region Photon methods
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            SetVisiblity(true);

            ForcefieldController.Enable(info.GetLag());
        }
        #endregion

        #region Injection methods
        private void OnTanksControllerInject(TanksController controller)
        {
            controller.GetTanks(team).Add(this);
        }
        #endregion

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);

            RespawnController.SetCallback(OnRespawn);
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
                MovementController.StopMovement();
            }

            if (InputController.Shoot)
            {
                BulletController.Fire();
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();

            MovementController.StopMovement();
        }
        #endregion
    }
}
