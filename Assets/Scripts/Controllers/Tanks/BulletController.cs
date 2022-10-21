using Photon.Pun;
using System;
using System.Collections.Generic;
using Tanks.Extensions;
using UnityEngine;

namespace Tanks
{
    public class BulletController : MonoBehaviourPun
    {
        public int limit = 1;
        public float delay = 1.0f;
        public Bullet bulletPrefab;

        [field: SerializeField]
        public Transform SpawnPoint { get; private set; }
        [field: SerializeField]
        public Collider2D IgnoreCollider { get; private set; }

        private float _fired = 0.0f;
        private int _spawned = 0;

        private Action<Collider2D> _onBulletHit;

        public void Setup(Action<Collider2D> onBulletHit)
        {
            _onBulletHit = onBulletHit;
        }

        private bool CanFire()
        {
            return (
                _fired + delay < Time.time &&
                _spawned < limit
            );
        }

        public void Fire()
        {
            if (CanFire())
            {
                _fired = Time.time;
                _spawned += 1;

                var position = SpawnPoint.position;
                var direction = new Vector2(SpawnPoint.up.x, SpawnPoint.up.y);

                var bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
                bullet.Setup(direction, IgnoreCollider, OnBulletHit);

                photonView.RPC(nameof(RPCFire), RpcTarget.Others, position, direction);
            }
        }

        [PunRPC]
        public void RPCFire(Vector3 position, Vector2 direction, PhotonMessageInfo info)
        {
            var bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
            bullet.Setup(direction, IgnoreCollider, info.GetLag());
        }

        private void OnBulletHit(Collider2D collider)
        {
            _onBulletHit(collider);
            _spawned -= 1;
        }
    }
}
