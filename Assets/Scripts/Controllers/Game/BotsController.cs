using Common;
using Common.Injection;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Tanks.Extensions;
using UnityEngine;

namespace Tanks
{
    public class BotsController : MonoBehaviourPunCallbacks
    {
        public ObjectReference tankABotPrefab;
        public ObjectReference tankBBotPrefab;

        [field: DI_Inject]
        public SpawnsController SpawnsController { get; private set; }
        [field: DI_Inject]
        public TanksController TanksController { get; private set; }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Start()
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                CreateBots();
            }
        }
        #endregion

        #region Photon methods
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                ControlBots();
            }
        }
        #endregion

        private void CreateBots()
        {
            var room = PhotonNetwork.CurrentRoom;
            var players = room.Players;
            var roomCount = room.MaxPlayers;
            var teamACount = CountPlayersInTeam(players, ETeam.A);
            var teamBCount = CountPlayersInTeam(players, ETeam.B);

            for (int i = teamACount + teamBCount; i < roomCount; ++i)
            {
                if (teamACount < teamBCount)
                {
                    CreateBot(ETeam.A);
                    ++teamACount;
                }
                else
                {
                    CreateBot(ETeam.B);
                    ++teamBCount;
                }
            }
        }

        private void ControlBots()
        {
            foreach (var kv in TanksController.Tanks)
            {
                var tanks = kv.Value;
                foreach (var tank in tanks)
                {
                    if (tank.IsBot)
                    {
                        tank.InputController.IsEnabled = true;
                    }
                }
            }
        }

        private void CreateBot(ETeam team)
        {
            var prefabPath = GetBotPrefabPath(team);
            var spawn = SpawnsController.GetBestSpawn(team);

            PhotonNetwork.InstantiateRoomObject(prefabPath, spawn.transform.position, Quaternion.identity);
        }

        private int CountPlayersInTeam(Dictionary<int, Player> players, ETeam team)
        {
            int result = 0;
            foreach (var kv in players)
            {
                var player = kv.Value;
                if (player.GetTeam() == team)
                {
                    ++result;
                }
            }
            return result;
        }

        private string GetBotPrefabPath(ETeam team)
        {
            switch (team)
            {
                case ETeam.A: return tankABotPrefab.ResourcePath;
                case ETeam.B: return tankBBotPrefab.ResourcePath;
            }
            return null;
        }
    }
}
