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
        private Statue _teamAStatue;
        private Statue _teamBStatue;

        public Statue GetStatue(ETeam team)
        {
            switch (team)
            {
                case ETeam.A: return _teamAStatue;
                case ETeam.B: return _teamBStatue;
                default: return null;
            }
        }

        public void SetStatue(Statue statue)
        {
            switch (statue.team)
            {
                case ETeam.A:
                    _teamAStatue = statue;
                    break;
                case ETeam.B:
                    _teamBStatue = statue;
                    break;
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

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
