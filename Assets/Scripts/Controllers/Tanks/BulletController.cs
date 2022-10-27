using Photon.Pun;
using UnityEngine;

namespace Tanks
{
    public class BulletController : MonoBehaviourPun
    {
        public int limit = 1;
        public float delay = 1.0f;
        public string bulletPrefabPath;

        [field: SerializeField]
        public Transform SpawnPoint { get; private set; }
        [field: SerializeField]
        public Collider2D IgnoreCollider { get; private set; }

        private float _fired = 0.0f;
        private int _spawned = 0;

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

                var bulletObject = PhotonNetwork.Instantiate(bulletPrefabPath, SpawnPoint.position, SpawnPoint.rotation);
                if (bulletObject.TryGetComponent<Bullet>(out var bullet))
                {
                    bullet.SetCallback(OnBulletHit);
                }
            }
        }

        private void OnBulletHit()
        {
            _spawned -= 1;
        }
    }
}
