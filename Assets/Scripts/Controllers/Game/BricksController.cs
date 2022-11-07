using Common.Extensions;
using Common.Injection;
using Photon.Pun;
using UnityEngine;

namespace Tanks
{
    [DI_Install]
    [RequireComponent(typeof(PhotonView))]
    public class BricksController : MonoBehaviourPun
    {
        private int _hitframe;

        private bool TryGetBrick(RaycastHit2D hit, out Brick brick)
        {
            return hit.collider.TryGetComponentInParent(out brick);
        }

        private RaycastHit2D[] MakeCleanHits(Vector2 position, Vector2 direction, float length)
        {
            while (true)
            {
                var check = true;

                var hits = Physics2D.RaycastAll(position, direction, length);
                foreach (var hit in hits)
                {
                    if (TryGetBrick(hit, out var brick) &&
                        !brick.IsFractured)
                    {
                        brick.Fracture();

                        check = false;
                    }
                }

                if (check)
                {
                    return hits;
                }
            }
        }

        public void StrikeBricks(Vector2 rayPosition, Vector2 rayVector)
        {
            var hits = MakeCleanHits(rayPosition, rayVector.normalized, rayVector.magnitude);
            foreach (var hit in hits)
            {
                if (TryGetBrick(hit, out var piece))
                {
                    Destroy(piece.gameObject);
                }
            }
        }

        public void RPCStrikeBricks(Vector2 rayPosition, Vector2 rayVector)
        {
            photonView.RPC(nameof(RPCStrikeBricks_Internal), RpcTarget.Others, rayPosition, rayVector);
        }

        [PunRPC]
        private void RPCStrikeBricks_Internal(Vector2 rayPosition, Vector2 rayVector)
        {
            StrikeBricks(rayPosition, rayVector);
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
