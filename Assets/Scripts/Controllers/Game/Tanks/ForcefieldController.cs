﻿using Photon.Pun;
using Tanks.Extensions;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(PhotonView))]
    public class ForcefieldController : MonoBehaviourPun
    {
        public float duration = 1.0f;

        [field: SerializeField]
        public GameObject ForcefieldObject { get; private set; }

        private float _duration;

        public bool IsActive
        {
            get => _duration > 0.0f;
        }

        private void SetForcefield(bool value)
        {
            enabled = value;

            ForcefieldObject.SetActive(value);
        }

        public void Enable(float lag = 0.0f)
        {
            Enable(duration, lag);
        }

        public void Enable(float duration, float lag)
        {
            _duration = duration - lag;

            SetForcefield(true);
        }

        public void RPCEnable()
        {
            photonView.RPC(nameof(RPCEnable_Internal), RpcTarget.Others);
        }

        [PunRPC]
        private void RPCEnable_Internal(PhotonMessageInfo info)
        {
            Enable(info.GetLag());
        }

        private void CheckEnabled()
        {
            _duration -= Time.deltaTime;
            if (_duration <= 0.0f)
            {
                SetForcefield(false);
            }
        }

        #region Unity methods
        private void Awake()
        {
            SetForcefield(false);
        }

        private void Update()
        {
            CheckEnabled();
        }
        #endregion
    }
}
