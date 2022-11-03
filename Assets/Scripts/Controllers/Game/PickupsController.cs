using Common;
using Common.Injection;
using Common.Mathematics;
using Photon.Pun;
using Tanks.Extensions;
using UnityEngine;

namespace Tanks
{
    [DI_Install]
    public class PickupsController : MonoBehaviourPun
    {
        public Range2 spawnRange;
        public Range spawnDelayRange = Range.One;

        [field: SerializeField]
        public Pickup[] Pickups { get; private set; }

        private float _spawnTime;

        private void TrySpawnPickup(int index, Vector2 position)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                RPCSpawnPickup(index, position);
            }
        }

        private void UpdateSpawner()
        {
            var time = Time.time;
            if (_spawnTime < time)
            {
                var spawnDelay = Random.Range(spawnDelayRange.min, spawnDelayRange.max);
                _spawnTime = time + spawnDelay;

                var index = Random.Range(0, Pickups.Length);
                var position = URandom.Range(spawnRange.min, spawnRange.max);

                TrySpawnPickup(index, position);
            }
        }

        public void SpawnPickup(int index, Vector2 position, float lag = 0.0f)
        {
            var pickup = Pickups[index];
            var instance = Instantiate(pickup, position, Quaternion.identity);
            instance.Setup(lag);
        }

        public void RPCSpawnPickup(int index, Vector2 position)
        {
            photonView.RPC(nameof(RPCSpawnPickup_Internal), RpcTarget.All, index, position);
        }

        [PunRPC]
        private void RPCSpawnPickup_Internal(int index, Vector2 position, PhotonMessageInfo info)
        {
            SpawnPickup(index, position, info.GetLag());
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Start()
        {
            _spawnTime = Time.time + Random.Range(spawnDelayRange.min, spawnDelayRange.max);
        }

        private void Update()
        {
            UpdateSpawner();
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion

#if UNITY_EDITOR
        [Header("Gizmos"), SerializeField]
        private Color _color = Color.cyan;

        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            UGizmos.DrawWireRect(spawnRange.Center, spawnRange.Size);
        }
#endif
    }
}
