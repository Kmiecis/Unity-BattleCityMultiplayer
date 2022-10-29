using Photon.Pun;
using Tanks.Extensions;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(PhotonView))]
    public class StatuesController : MonoBehaviourPun
    {
        [field: SerializeField]
        public Statue TeamAStatue { get; private set; }
        [field: SerializeField]
        public Statue TeamBStatue { get; private set; }

        public Statue GetStatue(int team)
        {
            if (team == GameProperties.TEAM_A)
                return TeamAStatue;
            if (team == GameProperties.TEAM_B)
                return TeamBStatue;
            return null;
        }

        public void StatueDamage(int team, float lag = 0.0f)
        {
            var statue = GetStatue(team);
            statue.Damage(lag);
        }

        public void RPCStatueDamage(int team)
        {
            photonView.RPC(nameof(RPCStatueDamage_Internal), RpcTarget.Others, team);
        }

        [PunRPC]
        private void RPCStatueDamage_Internal(int team, PhotonMessageInfo info)
        {
            StatueDamage(team, info.GetLag());
        }

        public void StatueRepair(int team, float lag = 0.0f)
        {
            var statue = GetStatue(team);
            statue.Repair(lag);
        }

        public void RPCStatueRepair(int team)
        {
            photonView.RPC(nameof(RPCStatueRepair_Internal), RpcTarget.Others, team);
        }

        [PunRPC]
        private void RPCStatueRepair_Internal(int team, PhotonMessageInfo info)
        {
            StatueRepair(team, info.GetLag());
        }

        #region Unity methods
        private void Start()
        {
            TeamAStatue.Setup(this, GameProperties.TEAM_A);
            TeamBStatue.Setup(this, GameProperties.TEAM_B);
        }
        #endregion
    }
}
