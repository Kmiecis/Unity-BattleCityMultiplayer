using Photon.Pun;
using System;
using Tanks.Extensions;
using UnityEngine;

namespace Tanks
{
    public class RespawnController : MonoBehaviourPun
    {
        public float duration = 1.0f;

        [field: SerializeField]
        public Transform Target { get; private set; }
        [field: SerializeField]
        public GameObject BlinkObject { get; private set; }

        private Action _onRespawn;
        private float _duration;

        public bool IsRespawning
        {
            get => _duration > 0.0f;
        }

        public void Setup(Action onRespawn)
        {
            _onRespawn = onRespawn;
        }

        private void SetRespawning(bool value)
        {
            enabled = value;

            BlinkObject.SetActive(value);
        }

        public void Respawn(Vector3 position)
        {
            Respawn(position, duration);
        }

        public void Respawn(Vector3 position, float duration)
        {
            Target.position = position;

            _duration = duration;

            SetRespawning(true);

            RPCRespawn(position, duration);
        }

        public void RPCRespawn(Vector3 position)
        {
            RPCRespawn(position, duration);
        }

        public void RPCRespawn(Vector3 position, float duration)
        {
            photonView.RPC(nameof(RPCRespawn_Internal), RpcTarget.Others, position, duration);
        }
        
        [PunRPC]
        private void RPCRespawn_Internal(Vector3 position, float duration, PhotonMessageInfo info)
        {
            Target.position = position;

            _duration = duration - info.GetLag();

            SetRespawning(true);
        }

        private void Awake()
        {
            SetRespawning(false);
        }

        private void Update()
        {
            _duration -= Time.deltaTime;
            if (_duration <= 0.0f)
            {
                SetRespawning(false);

                _onRespawn.Invoke();
            }
        }
    }
}
