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
        public string tankABotPrefab;
        public string tankBBotPrefab;

        [field: DI_Inject]
        public SpawnsController SpawnsController { get; private set; }

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
                case ETeam.A: return tankABotPrefab;
                case ETeam.B: return tankBBotPrefab;
            }
            return null;
        }
    }
}
