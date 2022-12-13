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
        private Dictionary<ETeam, List<Spawn>> _spawns = new Dictionary<ETeam, List<Spawn>>
        {
            { ETeam.A, new List<Spawn>() },
            { ETeam.B, new List<Spawn>() }
        };

        public Dictionary<ETeam, List<Spawn>> Spawns
            => _spawns;

        public List<Spawn> TeamASpawns
            => _spawns[ETeam.A];

        public List<Spawn> TeamBSpawns
            => _spawns[ETeam.B];

        public Spawn FindBestSpawn(ETeam team)
        {
            var spawns = _spawns[team];
            var from = Random.Range(0, spawns.Count);
            var to = Mathx.PrevIndex(from, spawns.Count);
            for (int i = from; i != to; i = Mathx.NextIndex(i, spawns.Count))
            {
                var spawn = spawns[i];
                if (spawn.IsValid)
                {
                    return spawn;
                }
            }
            return spawns[from];
        }

        public Spawn GetBestSpawn(ETeam team)
        {
            var spawn = FindBestSpawn(team);
            spawn.IsValid = false;
            return spawn;
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
