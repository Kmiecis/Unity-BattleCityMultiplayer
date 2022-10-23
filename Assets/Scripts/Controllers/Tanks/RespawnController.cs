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

        private Action _callback;
        private float _duration;

        public bool IsRespawning
        {
            get => _duration > 0.0f;
        }

        public void SetCallback(Action callback)
        {
            _callback = callback;
        }

        private void SetVisibility(bool value)
        {
            enabled = value;

            BlinkObject.SetActive(value);
        }

        private void SetRespawn(Vector3 position, float duration)
        {
            Target.position = position;

            _duration = duration;
        }

        public void Respawn(Vector3 position)
        {
            Respawn(position, duration);
        }

        public void Respawn(Vector3 position, float duration)
        {
            SetRespawn(position, duration);

            SetVisibility(true);

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
            SetRespawn(position, duration - info.GetLag());

            SetVisibility(true);
        }

        private void Awake()
        {
            SetVisibility(false);
        }

        private void Update()
        {
            _duration -= Time.deltaTime;
            if (_duration <= 0.0f)
            {
                SetVisibility(false);

                _callback.Invoke();
            }
        }
    }
}
