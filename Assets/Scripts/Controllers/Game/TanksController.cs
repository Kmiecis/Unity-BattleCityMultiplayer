using Common.Mathematics;
using Photon.Pun;
using UnityEngine;
using Tanks.Extensions;

namespace Tanks
{
    public class TanksController : MonoBehaviour
    {
        public string tankAPrefabPath;
        public string tankBPrefabPath;

        [field: SerializeField]
        public Spawn[] TeamASpawns { get; private set; }
        [field: SerializeField]
        public Spawn[] TeamBSpawns { get; private set; }

        private string GetTankPrefabPath(ETeam team)
        {
            switch (team)
            {
                case ETeam.A: return tankAPrefabPath;
                case ETeam.B: return tankBPrefabPath;
                default: return null;
            }
        }

        private Spawn[] GetSpawns(ETeam team)
        {
            switch (team)
            {
                case ETeam.A: return TeamASpawns;
                case ETeam.B: return TeamBSpawns;
                default: return null;
            }
        }

        private Spawn GetBestSpawn(ETeam team)
        {
            var spawns = GetSpawns(team);
            var index = Random.Range(0, spawns.Length);
            for (int i = index; i != index -1; index = Mathx.NextIndex(index, spawns.Length))
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

        public string GetTankPrefabPath()
        {
            var team = PhotonNetwork.LocalPlayer.GetTeam();
            return GetTankPrefabPath(team);
        }

        #region Unity methods
        private void Start()
        {
            var spawn = GetBestSpawn();
            var prefabPath = GetTankPrefabPath();

            var tankObject = PhotonNetwork.Instantiate(prefabPath, spawn.transform.position, Quaternion.identity);
            if (tankObject.TryGetComponent<Tank>(out var tank))
            {
                tank.Setup(this);
            }
        }
        #endregion
    }
}
