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

        private List<Tank> _teamATanks = new List<Tank>();
        private List<Tank> _teamBTanks = new List<Tank>();

        public List<Tank> TeamATanks
            => _teamATanks;

        public List<Tank> TeamBTanks
            => _teamBTanks;

        public List<Tank> GetTanks(ETeam team)
        {
            switch (team)
            {
                case ETeam.A: return _teamATanks;
                case ETeam.B: return _teamBTanks;
            }
            return null;
        }

        public Tank GetMineTank()
        {
            var team = PhotonNetwork.LocalPlayer.GetTeam();
            var tanks = GetTanks(team);
            foreach (var tank in tanks)
            {
                if (tank.photonView.IsMine)
                {
                    return tank;
                }
            }
            return null;
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
            var spawn = SpawnsController.GetBestSpawn();
            var prefabPath = GetTankPrefabPath();

            PhotonNetwork.Instantiate(prefabPath, spawn.transform.position, Quaternion.identity);
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
