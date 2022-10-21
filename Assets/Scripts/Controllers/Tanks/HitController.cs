using Common;
using Photon.Pun;
using System;
using UnityEngine;

namespace Tanks
{
    public class HitController : MonoBehaviourPun
    {
        public LayerMask hitLayer;
        [field: SerializeField]
        public Collision2DController CollisionController;

        public event Action OnHit;

        private void OnCollisionEntered(Collision2D collision)
        {
            if (hitLayer.Contains(collision.gameObject.layer))
            {
                Death();
            }
        }

        public void Death()
        {
            photonView.RPC(nameof(RPCDeath), RpcTarget.AllViaServer);
        }

        [PunRPC]
        public void RPCDeath()
        {

        }

        private void Start()
        {
            CollisionController.CalledOnEnter.AddListener(OnCollisionEntered);
        }
    }
}
