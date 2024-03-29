﻿using Common.Extensions;
using Common.Injection;
using Photon.Pun;
using Tanks.Extensions;
using UnityEngine;

namespace Tanks
{
    [DI_Install]
    [RequireComponent(typeof(PhotonView))]
    public class BuffsController : MonoBehaviourPun
    {
        [field: SerializeField]
        public ABuff[] Buffs { get; private set; }
        [field: SerializeField]
        public SoundData BuffPickupSound { get; private set; }

        [field: DI_Inject]
        public TanksController TanksController { get; private set; }
        [field: DI_Inject]
        public SoundsController SoundsController { get; private set; }

        private void CastBuff(int index, ETeam team, float lag = 0.0f)
        {
            if (Buffs.TryGetAt(index, out var buff))
            {
                var instance = Instantiate(buff);
                instance.Setup(team, lag);

                SoundsController.PlaySound(BuffPickupSound);
            }
        }

        public void CastBuff(EBuffType buffType, ETeam team)
        {
            var index = Buffs.FindIndex(b => b.buffType == buffType);
            CastBuff(index, team);
        }

        public void RPCCastBuff(EBuffType buffType, ETeam team)
        {
            var index = Buffs.FindIndex(b => b.buffType == buffType);
            photonView.RPC(nameof(RPCCastBuff_Internal), RpcTarget.Others, index, team);
        }

        [PunRPC]
        private void RPCCastBuff_Internal(int index, ETeam team, PhotonMessageInfo info)
        {
            CastBuff(index, team, info.GetLag());
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
