using Photon.Pun;
using UnityEngine;
using Tanks.Extensions;
using System.Collections.Generic;
using Common.Injection;

namespace Tanks
{
    [DI_Install]
    public class TanksController : MonoBehaviour
    {
        public string tankAPrefabPath;
        public string tankBPrefabPath;

        [field: DI_Inject]
        public SpawnsController SpawnsController { get; private set; }

        private Dictionary<ETeam, List<Tank>> _tanks = new Dictionary<ETeam, List<Tank>>
        {
            { ETeam.A, new List<Tank>() },
            { ETeam.B, new List<Tank>() }
        };

        public Dictionary<ETeam, List<Tank>> Tanks
            => _tanks;

        public List<Tank> TeamATanks
            => _tanks[ETeam.A];

        public List<Tank> TeamBTanks
            => _tanks[ETeam.B];

        public Tank GetMineTank()
        {
            var team = PhotonNetwork.LocalPlayer.GetTeam();
            var tanks = _tanks[team];
            foreach (var tank in tanks)
            {
                if (tank.photonView.IsMine)
                {
                    return tank;
                }
            }
            return null;
        }

        private void SpawnMineTank()
        {
            var prefabPath = GetTankPrefabPath();
            var spawn = SpawnsController.GetBestSpawn();

            PhotonNetwork.Instantiate(prefabPath, spawn.transform.position, Quaternion.identity);
        }

        private string GetTankPrefabPath(ETeam team)
        {
            switch (team)
            {
                case ETeam.A: return tankAPrefabPath;
                case ETeam.B: return tankBPrefabPath;
                default: return null;
            }
        }

        public string GetTankPrefabPath()
        {
            var team = PhotonNetwork.LocalPlayer.GetTeam();
            return GetTankPrefabPath(team);
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Start()
        {
            SpawnMineTank();
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
