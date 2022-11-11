﻿using Common;
using Common.Extensions;
using Common.Injection;
using Photon.Pun;
using System.Collections;
using Tanks.Extensions;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(PhotonView))]
    public class Bullet : MonoBehaviourPun, IPunInstantiateMagicCallback
    {
        private const float kExplosionDelay = 5.0f;

        public ETeam team;
        public LayerMask hitMask;

        [field: SerializeField]
        public GameObject ModelObject { get; private set; }
        [field: SerializeField]
        public MovementController MovementController { get; private set; }
        [field: SerializeField]
        public Collision2DController CollisionController { get; private set; }
        [field: DI_Inject]
        public EffectsController EffectsController { get; private set; }

        public bool IsVisible
        {
            get => ModelObject.activeSelf;
        }

        private void Hit(Collider2D collider)
        {
            Explode();

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
                
                RPCExplode();
            }
        }

        private void HitTank(Tank tank)
        {
            if (tank.team != team)
            {
                if (tank.Hit())
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

        private void Fly()
        {
            if (photonView.IsMine)
            {
                new CoroutineWrapper()
                .WithTarget(this)
                .WithEnumerator(DestroyDelayed)
                .Start();
            }
        }

        private void Destroy()
        {
            PhotonNetwork.Destroy(gameObject);
        }

        public IEnumerator DestroyDelayed()
        {
            return UCoroutine.InvokeDelayed(Destroy, kExplosionDelay);
        }

        public void Explode()
        {
            ModelObject.SetActive(false);

            MovementController.ResetMovement();

            EffectsController.SpawnSmallExplosion(transform.position);
        }

        public void RPCExplode()
        {
            photonView.RPC(nameof(RPCBulletExplode_Internal), RpcTarget.Others, transform.position);
        }

        [PunRPC]
        private void RPCBulletExplode_Internal(Vector3 position)
        {
            transform.position = position;

            if (IsVisible)
            {
                Explode();
            }
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

            MovementController.SetMovement(direction, speed);

            if (UPhysics2D.Raycast(position, direction, out var hit, distance, hitMask))
            {
                transform.position = hit.point;

                Hit(hit.collider);
            }
            else
            {
                MovementController.ApplyMovement(lag);

                Fly();
            }
        }
        #endregion

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
