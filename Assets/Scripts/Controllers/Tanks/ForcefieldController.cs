using Photon.Pun;
using UnityEngine;

namespace Tanks
{
    public class ForcefieldController : MonoBehaviourPun
    {
        public float duration = 1.0f;

        [field: SerializeField]
        public GameObject ForcefieldObject { get; private set; }

        private float _duration = 0.0f;

        public bool IsActive
        {
            get => _duration > 0.0f;
        }

        private void OnEnableChanged(bool value)
        {
            enabled = value;
            ForcefieldObject.SetActive(value);
        }

        public void Enable()
        {
            Enable(duration);
        }

        public void Enable(float duration)
        {
            _duration = duration;

            OnEnableChanged(IsActive);

            photonView.RPC(nameof(RPCEnable), RpcTarget.Others, duration);
        }

        [PunRPC]
        public void RPCEnable(float duration, PhotonMessageInfo info)
        {
            var lag = (float)(PhotonNetwork.Time - info.SentServerTime);
            _duration = duration - lag;

            OnEnableChanged(IsActive);
        }

        public void Disable()
        {
            OnEnableChanged(false);

            photonView.RPC(nameof(RPCDisable), RpcTarget.Others);
        }

        [PunRPC]
        public void RPCDisable()
        {
            OnEnableChanged(false);
        }

        private void Update()
        {
            _duration -= Time.deltaTime;
            if (!IsActive)
            {
                OnEnableChanged(false);
            }
        }
    }
}
