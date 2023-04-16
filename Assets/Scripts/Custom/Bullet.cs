using Common;
using Common.Coroutines;
using Common.Extensions;
using Common.Injection;
using Photon.Pun;
using Tanks.Extensions;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(PhotonView))]
    public class Bullet : MonoBehaviourPun, IPunInstantiateMagicCallback
    {
        private const float kLivetime = 5.0f;

        public ETeam team;
        public LayerMask hitMask;

        [field: SerializeField]
        public GameObject ModelObject { get; private set; }
        [field: SerializeField]
        public MovementController MovementController { get; private set; }
        [field: SerializeField]
        public Collision2DController CollisionController { get; private set; }
        [field: SerializeField]
        public SoundData ShootSound { get; private set; }
        [field: DI_Inject]
        public EffectsController EffectsController { get; private set; }
        [field: DI_Inject]
        public SoundsController SoundsController { get; private set; }

        public bool IsExploded
        {
            get => !ModelObject.activeSelf;
            set => ModelObject.SetActive(!value);
        }

        private void Hit(Collider2D collider)
        {
            if (IsExploded)
                return;

            if (photonView.IsMine)
            {
                if (collider.TryGetComponentInParent<Brick>(out var brick))
                {
                    HitBrick(brick);
                }
                else if (collider.TryGetComponentInParent<Tank>(out var tank))
                {
                    HitTank(tank);
                }
                else if(collider.TryGetComponentInParent<Statue>(out var statue))
                {
                    HitStatue(statue);
                }
            }

            Explode();
        }

        private void HitTank(Tank tank)
        {
            if (tank.team != team)
            {
                tank.Hit();

                if (!tank.IsVisible)
                {
                    photonView.Owner.IncrKills();
                }
            }
        }

        private void HitStatue(Statue statue)
        {
            if (statue.team != team)
            {
                statue.Hit();
            }
        }

        private void HitBrick(Brick brick)
        {
            var hitPosition = transform.position;
            var hitDirection = MovementController.Direction;
            brick.Hit(hitPosition, hitDirection);
        }

        public void Explode()
        {
            IsExploded = true;

            EffectsController.SpawnSmallExplosion(transform.position);

            if (photonView.IsMine)
            {
                RPCExplode(transform.position);

                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                MovementController.StopMovement();
            }
        }

        public void RPCExplode(Vector3 position)
        {
            photonView.RPC(nameof(RPCBulletExplode_Internal), RpcTarget.Others, position);
        }

        [PunRPC]
        private void RPCBulletExplode_Internal(Vector3 position)
        {
            transform.position = position;

            if (IsExploded)
                return;

            Explode();
        }

        #region External methods
        public void _OnCollisionEntered(Collision2D collision)
        {
            Hit(collision.collider);
        }
        #endregion

        #region Photon methods
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            var lag = info.GetLag();
            var speed = info.GetDataAt<float>(0);

            var position = (Vector2)transform.position;
            var direction = (Vector2)(transform.rotation * Vector3.up);
            var distance = lag * speed;

            SoundsController.PlaySound(ShootSound);

            MovementController.SetMovement(direction, speed);

            if (UPhysics2D.Raycast(position, direction, out var hit, distance, hitMask))
            {
                transform.position = hit.point;

                Hit(hit.collider);
            }
            else
            {
                MovementController.ApplyMovement(lag);
            }
        }
        #endregion

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Start()
        {
            UCoroutine.YieldTime(kLivetime)
                .Then(Explode)
                .Start(this);
        }
        #endregion
    }
}
