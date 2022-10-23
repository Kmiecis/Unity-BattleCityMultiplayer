﻿using Photon.Pun;
using System;
using UnityEngine;

namespace Tanks
{
    public class ExplosionController : MonoBehaviourPun
    {
        private const string kExplosionEndEvent = "end";

        [field: SerializeField]
        public GameObject ExplosionObject { get; private set; }
        [field: SerializeField]
        public AnimatorEventHandler AnimatorEvent { get; private set; }

        private Action _onFinish;

        private void SetVisibility(bool value)
        {
            ExplosionObject.SetActive(value);
        }

        public void Explode(Action onFinish = null)
        {
            _onFinish = onFinish;

            SetVisibility(true);
        }

        public void RPCExplode()
        {
            photonView.RPC(nameof(RPCExplode_Internal), RpcTarget.Others);
        }

        [PunRPC]
        private void RPCExplode_Internal()
        {
            SetVisibility(true);
        }

        private void OnAnimatorEvent(string value)
        {
            if (value == kExplosionEndEvent)
            {
                SetVisibility(false);

                _onFinish?.Invoke();
                _onFinish = null;
            }
        }

        private void Awake()
        {
            SetVisibility(false);

            AnimatorEvent.OnAnimatorEvent.AddListener(OnAnimatorEvent);
        }
    }
}
