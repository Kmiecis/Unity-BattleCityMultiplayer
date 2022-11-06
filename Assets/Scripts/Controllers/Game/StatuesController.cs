using Common.Injection;
using Photon.Pun;
using Tanks.Extensions;
using UnityEngine;

namespace Tanks
{
    [DI_Install]
    [RequireComponent(typeof(PhotonView))]
    public class StatuesController : MonoBehaviourPun
    {
        [field: SerializeField]
        public Statue TeamAStatue { get; private set; }
        [field: SerializeField]
        public Statue TeamBStatue { get; private set; }

        public Statue GetStatue(ETeam team)
        {
            switch (team)
            {
                case ETeam.A: return TeamAStatue;
                case ETeam.B: return TeamBStatue;
                default: return null;
            }
        }

        public void StatueDamage(ETeam team, float lag = 0.0f)
        {
            var statue = GetStatue(team);
            statue.Damage(lag);
        }

        public void RPCStatueDamage(ETeam team)
        {
            photonView.RPC(nameof(RPCStatueDamage_Internal), RpcTarget.Others, team);
        }

        [PunRPC]
        private void RPCStatueDamage_Internal(ETeam team, PhotonMessageInfo info)
        {
            StatueDamage(team, info.GetLag());
        }

        public void StatueRepair(ETeam team, float lag = 0.0f)
        {
            var statue = GetStatue(team);
            statue.Repair(lag);
        }

        public void RPCStatueRepair(ETeam team)
        {
            photonView.RPC(nameof(RPCStatueRepair_Internal), RpcTarget.Others, team);
        }

        [PunRPC]
        private void RPCStatueRepair_Internal(ETeam team, PhotonMessageInfo info)
        {
            StatueRepair(team, info.GetLag());
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Start()
        {
            TeamAStatue.Setup(this);
            TeamBStatue.Setup(this);
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
