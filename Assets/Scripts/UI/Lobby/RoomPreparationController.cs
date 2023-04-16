using Common.Extensions;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Tanks.Extensions;

namespace Tanks.UI
{
    public class RoomPreparationController : MonoBehaviourPunCallbacks
    {
        [field: SerializeField]
        public GameData GameProperties { get; private set; }
        [field: SerializeField]
        public ScenesData GameScenes { get; private set; }
        [field: SerializeField]
        public MapData DefaultMap { get; private set; }
        [field: SerializeField]
        public TextMeshProUGUI RoomNameText { get; private set; }
        [field: SerializeField]
        public RoomPlayerEntry EntryPrefab { get; private set; }
        [field: SerializeField]
        public Transform TeamAParent { get; private set; }
        [field: SerializeField]
        public Transform TeamBParent { get; private set; }
        [field: SerializeField]
        public GameObject StartObject { get; private set; }
        [field: SerializeField]
        public SelectionEventHandler StartSelectionEvent { get; private set; }
        [field: SerializeField]
        public SelectionController SelectionController { get; private set; }
        [field: SerializeField]
        public RoomPlayerEntry LocalEntry { get; private set; }

        private Dictionary<int, RoomPlayerEntry> _teamA = new Dictionary<int, RoomPlayerEntry>();
        private Dictionary<int, RoomPlayerEntry> _teamB = new Dictionary<int, RoomPlayerEntry>();

        private int GetTeamCount(ETeam team)
        {
            var count = team == LocalEntry.Team ? 1 : 0;
            switch (team)
            {
                case ETeam.A:
                    return count + _teamA.Count;
                case ETeam.B:
                    return count + _teamB.Count;
            }
            return 0;
        }

        private ETeam GetBestTeam()
        {
            if (GetTeamCount(ETeam.B) < GetTeamCount(ETeam.A))
                return ETeam.B;
            return ETeam.A;
        }

        private bool ArePlayersReady(Dictionary<int, RoomPlayerEntry> entries)
        {
            foreach (var entry in entries.Values)
            {
                if (!entry.IsReady)
                {
                    return false;
                }
            }
            return true;
        }

        private bool ArePlayersReady()
        {
            return (
                LocalEntry.IsReady &&
                ArePlayersReady(_teamA) &&
                ArePlayersReady(_teamB)
            );
        }

        private void DestroyEntry(int id, Dictionary<int, RoomPlayerEntry> entries)
        {
            if (entries.TryGetValue(id, out var entry))
            {
                entries.Remove(id);
                Destroy(entry.gameObject);
            }
        }

        private void DestroyEntry(int id)
        {
            DestroyEntry(id, _teamA);
            DestroyEntry(id, _teamB);
        }

        private void ClearEntries(Dictionary<int, RoomPlayerEntry> entries)
        {
            foreach (var entry in entries.Values)
            {
                Destroy(entry.gameObject);
            }
            entries.Clear();
        }

        private void ClearEntries()
        {
            ClearEntries(_teamA);
            ClearEntries(_teamB);
        }

        private Dictionary<int, RoomPlayerEntry> GetTeamEntries(ETeam team)
        {
            switch (team)
            {
                case ETeam.A: return _teamA;
                case ETeam.B: return _teamB;
                default: return null;
            }
        }

        private Transform GetTeamParent(ETeam team)
        {
            switch (team)
            {
                case ETeam.A: return TeamAParent;
                case ETeam.B: return TeamBParent;
                default: return null;
            }
        }

        private void AddEntry(RoomPlayerEntry entry, ETeam team)
        {
            var entries = GetTeamEntries(team);
            entries.Add(entry.Id, entry);
        }

        private void RemoveEntry(RoomPlayerEntry entry, ETeam team)
        {
            var entries = GetTeamEntries(team);
            entries.Remove(entry.Id);
        }

        private void SetEntryParent(RoomPlayerEntry entry, ETeam team)
        {
            var parent = GetTeamParent(team);
            entry.transform.SetParent(parent, false);
        }

        private void SwitchPlayerEntry(RoomPlayerEntry entry, ETeam oldTeam, ETeam newTeam)
        {
            RemoveEntry(entry, oldTeam);
            AddEntry(entry, newTeam);
            SetEntryParent(entry, newTeam);
        }

        private void CreateEntry(Player player)
        {
            var entry = Instantiate(EntryPrefab);

            var team = player.GetTeam(GetBestTeam());
            var isReady = player.GetIsReady();

            entry.Setup(player, isReady, team);

            AddEntry(entry, team);
            SetEntryParent(entry, team);
        }

        private void SetupLocalEntry()
        {
            ETeam team = GetBestTeam();
            bool isReady = false;

            LocalEntry.Setup(PhotonNetwork.LocalPlayer, isReady, team);

            SetEntryParent(LocalEntry, team);

            PhotonNetwork.LocalPlayer.SetTeam(team);
        }

        private bool TryGetEntry(int id, out RoomPlayerEntry entry)
        {
            return (
                _teamA.TryGetValue(id, out entry) ||
                _teamB.TryGetValue(id, out entry)
            );
        }

        private bool CanSwitchTeam(ETeam team)
        {
            var currentEntries = GetTeamEntries(team);
            var otherEntries = GetTeamEntries(team.Flip());
            return (
                currentEntries.Count >= GameProperties.minTeamPlayers &&
                otherEntries.Count < GameProperties.maxTeamPlayers
            );
        }

        private void SetupStart(bool isMaster)
        {
            StartObject.SetActive(isMaster);

            if (isMaster)
            {
                SetupMap();
            }
        }

        private void SetupMap()
        {
            if (!CustomPlayerPrefs.TryGetMap(out var map))
                map = DefaultMap.Value;
            PhotonNetwork.CurrentRoom.SetMap(map);
        }

        private void SetupStartSelection(bool active)
        {
            if (active && StartObject.activeSelf)
            {
                SelectionController.Events.AddUnique(StartSelectionEvent);
            }
            else
            {
                SelectionController.Refresh();
                SelectionController.Events.Remove(StartSelectionEvent);
            }
        }

        public void _OnStartGame()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(GameScenes.GameScene);
        }

        public void _OnSwitchReadiness()
        {
            var entry = LocalEntry;

            entry.IsReady = !entry.IsReady;

            PhotonNetwork.LocalPlayer.SetIsReady(entry.IsReady);

            SetupStartSelection(ArePlayersReady());
        }

        public void _OnSwitchTeam()
        {
            var entry = LocalEntry;

            if (
                CanSwitchTeam(entry.Team) &&
                !entry.IsReady
            )
            {
                entry.Team = entry.Team.Flip();

                SetEntryParent(entry, entry.Team);

                PhotonNetwork.LocalPlayer.SetTeam(entry.Team);
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            CreateEntry(newPlayer);

            SetupStartSelection(false);
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (TryGetEntry(targetPlayer.ActorNumber, out var entry))
            {
                if (changedProps.TryGetIsReady(out var isReady))
                {
                    entry.IsReady = isReady;

                    SetupStartSelection(ArePlayersReady());
                }

                if (changedProps.TryGetTeam(out var team))
                {
                    SwitchPlayerEntry(entry, entry.Team, team);

                    entry.Team = team;
                }
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            DestroyEntry(otherPlayer.ActorNumber);
        }

        public override void OnJoinedRoom()
        {
            RoomNameText.text = PhotonNetwork.CurrentRoom.Name;

            foreach (var player in PhotonNetwork.PlayerListOthers)
            {
                CreateEntry(player);
            }

            SetupLocalEntry();

            SetupStart(PhotonNetwork.IsMasterClient);
        }

        public override void OnLeftRoom()
        {
            ClearEntries();

            SetupStart(false);
            SetupStartSelection(false);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            var isMaster = PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber;
            SetupStart(isMaster);
            SetupStartSelection(ArePlayersReady());
        }

        private void Start()
        {
            StartObject.SetActive(false);
        }
    }
}
