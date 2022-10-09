using Photon.Pun;
using UnityEngine;

namespace Tanks
{
    public class EnableIfLocal : MonoBehaviourPun
    {
        public MonoBehaviour[] targets;

        private void Awake()
        {
            foreach (var target in targets)
            {
                target.enabled = photonView.IsMine;
            }
        }
    }
}
