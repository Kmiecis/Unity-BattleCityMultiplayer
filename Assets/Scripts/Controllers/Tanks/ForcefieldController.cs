using Photon.Pun;
using Tanks.Extensions;
using UnityEngine;

namespace Tanks
{
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

        public void Enable()
        {
            Enable(duration);
        }

        public void Enable(float duration)
        {
            _duration = duration;

            SetForcefield(true);

            RPCEnable(duration);
        }

        public void RPCEnable()
        {
            RPCEnable(duration);
        }

        public void RPCEnable(float duration)
        {
            photonView.RPC(nameof(RPCEnable_Internal), RpcTarget.Others, duration);
        }

        [PunRPC]
        private void RPCEnable_Internal(float duration, PhotonMessageInfo info)
        {
            _duration = duration - info.GetLag();

            SetForcefield(true);
        }

        private void Awake()
        {
            SetForcefield(false);
        }

        private void Update()
        {
            _duration -= Time.deltaTime;
            if (_duration <= 0.0f)
            {
                SetForcefield(false);
            }
        }
    }
}
