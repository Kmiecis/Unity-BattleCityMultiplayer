﻿using Photon.Pun;
using System.Collections.Generic;
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

        private float _fired = 0.0f;
        private List<Bullet> _spawned = new List<Bullet>();

        private bool CanFire()
        {
            return (
                _fired + delay < Time.time &&
                _spawned.Count < limit
            );
        }

        public void Fire()
        {
            if (CanFire())
            {
                photonView.RPC("Fire", RpcTarget.AllViaServer, SpawnPoint.position, SpawnPoint.up);
                
                _fired = Time.time;
            }
        }

        [PunRPC]
        public void Fire(Vector3 position, Vector3 forward, PhotonMessageInfo info)
        {
            var direction = new Vector2Int(
                Mathf.RoundToInt(forward.x),
                Mathf.RoundToInt(forward.y)
            );
            var lag = (float)(PhotonNetwork.Time - info.SentServerTime);

            var bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
            bullet.MovementController.SetMovement(direction);
            bullet.MovementController.ApplyMovement(lag);
            bullet.CalledOnDestroy += OnBulletDestroy;

            _spawned.Add(bullet);
        }

        private void OnBulletDestroy(Bullet bullet)
        {
            _spawned.Remove(bullet);
        }
    }
}