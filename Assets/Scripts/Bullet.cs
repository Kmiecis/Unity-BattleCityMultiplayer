using Common;
using Common.Extensions;
using Photon.Pun;
using System;
using System.Collections;
using Tanks.Extensions;
using UnityEngine;

namespace Tanks
{
    public class Bullet : MonoBehaviourPun, IPunInstantiateMagicCallback
    {
        private const float kExplosionDelay = 5.0f;

        public LayerMask hitMask;

        [field: SerializeField]
        public GameObject ModelObject { get; private set; }
        [field: SerializeField]
        public MovementController MovementController { get; private set; }
        [field: SerializeField]
        public Collision2DController CollisionController { get; private set; }
        [field: SerializeField]
        public ExplosionController ExplosionController { get; private set; }

        private Action _callback;

        public bool IsVisible
        {
            get => ModelObject.activeSelf;
        }

        public int Team
        {
            get => photonView.Owner.GetTeam();
        }

        public void SetCallback(Action callback)
        {
            _callback = callback;
        }
        
        private void Hit(Collider2D collider)
        {
            Explode();
        }

        private void HitMine(Collider2D collider)
        {
            if (collider.TryGetComponentInParent<Tank>(out var tank))
            {
                HitTank(tank);
            }

            _callback();

            Explode();
            RPCExplode();

            ExplosionController.SetCallback(Destroy);
        }

        private void HitTank(Tank tank)
        {
            if (!tank.ForcefieldController.IsActive &&
                tank.Team != Team)
            {
                tank.Explode();
                tank.RPCExplode();

                photonView.Owner.IncrKills();
            }
        }

        private void Fly()
        {
            new CoroutineWrapper()
                .WithTarget(this)
                .WithEnumerator(DestroyDelayed)
                .Start();
        }

        private void Destroy()
        {
            PhotonNetwork.Destroy(gameObject);
        }

        public IEnumerator DestroyDelayed()
        {
            return CoroutineUtility.InvokeDelayed(Destroy, kExplosionDelay);
        }

        public void Explode()
        {
            ModelObject.SetActive(false);

            MovementController.ResetMovement();

            ExplosionController.Explode();
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

        public void _OnCollisionEntered(Collision2D collision)
        {
            if (photonView.IsMine)
            {
                HitMine(collision.collider);
            }
            else
            {
                Hit(collision.collider);
            }
        }

        #region Photon methods
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            var lag = info.GetLag();

            var position = (Vector2)transform.position;
            var direction = (Vector2)(transform.rotation * Vector3.up);
            var distance = lag * MovementController.speed;

            MovementController.SetMovement(direction);

            if (UPhysics2D.Raycast(position, direction, out var hit, distance, hitMask))
            {
                var traveled = (hit.point - position).magnitude;
                lag = traveled / MovementController.speed;

                MovementController.ApplyMovement(lag);

                Hit(hit.collider);
            }
            else
            {
                MovementController.ApplyMovement(lag);

                Fly();
            }
        }
        #endregion
    }
}
