using Common.Extensions;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tanks.UI
{
    public class RoomPreparationController : MonoBehaviourPunCallbacks
    {
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

        private int GetBestTeam()
        {
            if (_teamB.Count < _teamA.Count)
            {
                return GameProperties.TEAM_B;
            }
            return GameProperties.TEAM_A;
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

        private Dictionary<int, RoomPlayerEntry> GetTeamEntries(int team)
        {
            if (team == GameProperties.TEAM_A)
                return _teamA;
            if (team == GameProperties.TEAM_B)
                return _teamB;
            return null;
        }

        private Transform GetTeamParent(int team)
        {
            if (team == GameProperties.TEAM_A)
                return TeamAParent;
            if (team == GameProperties.TEAM_B)
                return TeamBParent;
            return null;
        }

        private void AddEntry(RoomPlayerEntry entry, int team)
        {
            var entries = GetTeamEntries(team);
            entries.Add(entry.Id, entry);
        }

        private void RemoveEntry(RoomPlayerEntry entry, int team)
        {
            var entries = GetTeamEntries(team);
            entries.Remove(entry.Id);
        }

        private void SetEntryParent(RoomPlayerEntry entry, int team)
        {
            var parent = GetTeamParent(team);
            entry.transform.SetParent(parent, false);
        }

        private void SwitchPlayerEntry(RoomPlayerEntry entry, int oldTeam, int newTeam)
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
            int team = GetBestTeam();
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

        private bool CanSwitchTeam(int team)
        {
            var currentEntries = GetTeamEntries(team);
            var otherEntries = GetTeamEntries(team * -1);
            return (
                currentEntries.Count > GameProperties.MIN_TEAM_PLAYERS &&
                otherEntries.Count < GameProperties.MAX_TEAM_PLAYERS
            );
        }

        private void SetupStart(bool isMaster)
        {
            StartObject.SetActive(isMaster);
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
            PhotonNetwork.LoadLevel("Game");
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
                entry.Team *= -1;

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
