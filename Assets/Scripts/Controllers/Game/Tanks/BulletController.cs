using Common;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    public class BulletController : MonoBehaviour
    {
        public int limit = 1;
        public float speed = 1.0f;
        public float delay = 1.0f;
        public AssetReference bulletPrefab;

        [field: SerializeField]
        public Transform SpawnPoint { get; private set; }
        [field: SerializeField]
        public Collider2D IgnoreCollider { get; private set; }

        private float _fired = 0.0f;
        private List<Bullet> _bullets = new List<Bullet>();

        public bool CanShoot
        {
            get => (
                _fired + delay < Time.time &&
                _bullets.Count < limit
            );
        }

        public void TryShoot()
        {
            if (CanShoot)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            _fired = Time.time;

            var bulletObject = PhotonNetwork.Instantiate(bulletPrefab.ResourcePath, SpawnPoint.position, SpawnPoint.rotation, data: new object[] { speed });
            var bullet = bulletObject.GetComponent<Bullet>();
            _bullets.Add(bullet);
        }

        private void CheckBullets()
        {
            for (int i = _bullets.Count - 1; i > -1; --i)
            {
                var bullet = _bullets[i];
                if (bullet == null)
                {
                    _bullets.RemoveAt(i);
                }
            }
        }

        #region Unity methods
        private void Update()
        {
            CheckBullets();
        }
        #endregion
    }
}
