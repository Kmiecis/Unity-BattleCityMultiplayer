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

        private List<Tank> _teamATanks = new List<Tank>();
        private List<Tank> _teamBTanks = new List<Tank>();

        [DI_Inject]
        private SpawnsController _spawnsController;

        public List<Tank> TeamATanks
        {
            get => _teamATanks;
        }

        public List<Tank> TeamBTanks
        {
            get => _teamBTanks;
        }

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
            var spawn = _spawnsController.GetBestSpawn();
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
