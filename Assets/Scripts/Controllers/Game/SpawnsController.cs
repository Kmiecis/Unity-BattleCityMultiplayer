using Common.Injection;
using Common.Mathematics;
using Photon.Pun;
using System.Collections.Generic;
using Tanks.Extensions;
using UnityEngine;

namespace Tanks
{
    [DI_Install]
    public class SpawnsController : MonoBehaviour
    {
        private List<Spawn> _teamASpawns = new List<Spawn>();
        private List<Spawn> _teamBSpawns = new List<Spawn>();

        public List<Spawn> TeamASpawns
        {
            get => _teamASpawns;
        }

        public List<Spawn> TeamBSpawns
        {
            get => _teamBSpawns;
        }

        public List<Spawn> GetSpawns(ETeam team)
        {
            switch (team)
            {
                case ETeam.A: return TeamASpawns;
                case ETeam.B: return TeamBSpawns;
                default: return null;
            }
        }

        public Spawn GetBestSpawn(ETeam team)
        {
            var spawns = GetSpawns(team);
            var index = Random.Range(0, spawns.Count);
            for (int i = index; i != index - 1; index = Mathx.NextIndex(index, spawns.Count))
            {
                var spawn = spawns[i];
                if (spawn.IsValid)
                {
                    return spawn;
                }
            }
            return spawns[index];
        }

        public Spawn GetBestSpawn()
        {
            var team = PhotonNetwork.LocalPlayer.GetTeam();
            return GetBestSpawn(team);
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
