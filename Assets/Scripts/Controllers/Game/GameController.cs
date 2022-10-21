using Common.Mathematics;
using Photon.Pun;
using UnityEngine;

namespace Tanks
{
    public class GameController : MonoBehaviourPunCallbacks
    {
        public string tankAPrefabPath;
        public string tankBPrefabPath;

        [field: SerializeField]
        public Spawn[] TeamASpawns { get; private set; }
        [field: SerializeField]
        public Spawn[] TeamBSpawns { get; private set; }

        private GameObject _playerObject;

        private string GetTankPrefabPath(int team)
        {
            if (team == GameProperties.TEAM_A)
                return tankAPrefabPath;
            if (team == GameProperties.TEAM_B)
                return tankBPrefabPath;
            return null;
        }

        private Spawn[] GetSpawns(int team)
        {
            if (team == GameProperties.TEAM_A)
                return TeamASpawns;
            if (team == GameProperties.TEAM_B)
                return TeamBSpawns;
            return null;
        }

        private Spawn GetBestSpawn(int team)
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

        private void Start()
        {
            var team = GameProperties.GetTeam(PhotonNetwork.LocalPlayer);
            var spawn = GetBestSpawn(team);
            var prefabPath = GetTankPrefabPath(team);

            _playerObject = PhotonNetwork.Instantiate(prefabPath, spawn.transform.position, spawn.transform.rotation);
            if (_playerObject.TryGetComponent<Tank>(out var tank))
            {
                tank.Setup();
            }
        }
    }
}
