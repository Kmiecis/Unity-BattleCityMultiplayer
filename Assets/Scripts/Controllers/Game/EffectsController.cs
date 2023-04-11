using Common.Injection;
using Common.Pooling;
using Photon.Pun;
using UnityEngine;

namespace Tanks
{
    [DI_Install]
    [RequireComponent(typeof(PhotonView))]
    public class EffectsController : MonoBehaviourPun
    {
        [field: SerializeField]
        public ComponentPool<Explosion> SmallExplosionPool { get; private set; }
        [field: SerializeField]
        public ComponentPool<Explosion> BigExplosionPool { get; private set; }

        public void SpawnSmallExplosion(Vector2 position)
        {
            var explosion = SmallExplosionPool.Borrow();
            explosion.transform.position = position;
            explosion.Setup(SmallExplosionPool.Return);
        }

        public void RPCSpawnSmallExplosion(Vector2 position)
        {
            photonView.RPC(nameof(RPCSpawnSmallExplosion_Internal), RpcTarget.Others, position);
        }

        [PunRPC]
        private void RPCSpawnSmallExplosion_Internal(Vector2 position)
        {
            SpawnSmallExplosion(position);
        }

        public void SpawnBigExplosion(Vector2 position)
        {
            var explosion = BigExplosionPool.Borrow();
            explosion.transform.position = position;
            explosion.Setup(BigExplosionPool.Return);
        }

        public void RPCSpawnBigExplosion(Vector2 position)
        {
            photonView.RPC(nameof(RPCSpawnBigExplosion_Internal), RpcTarget.Others, position);
        }

        [PunRPC]
        private void RPCSpawnBigExplosion_Internal(Vector2 position)
        {
            SpawnBigExplosion(position);
        }

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
