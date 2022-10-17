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
        public PlayerListEntry EntryPrefab { get; private set; }
        [field: SerializeField]
        public Transform Team0Parent { get; private set; }
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

        private Dictionary<int, PlayerListEntry> _teamA = new Dictionary<int, PlayerListEntry>();
        private Dictionary<int, PlayerListEntry> _teamB = new Dictionary<int, PlayerListEntry>();

        private int GetBestTeam()
        {
            if (_teamB.Count < _teamA.Count)
            {
                return GameProperties.TEAM_B;
            }
            return GameProperties.TEAM_A;
        }

        private bool ArePlayersReady(Dictionary<int, PlayerListEntry> entries)
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
                ArePlayersReady(_teamA) &&
                ArePlayersReady(_teamB)
            );
        }

        private void DestroyEntry(int id, Dictionary<int, PlayerListEntry> entries)
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

        private void ClearEntries(Dictionary<int, PlayerListEntry> entries)
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

        private Dictionary<int, PlayerListEntry> GetTeamEntries(int team)
        {
            if (team == GameProperties.TEAM_A)
                return _teamA;
            if (team == GameProperties.TEAM_B)
                return _teamB;
            return null;
        }

        private Transform GetTeamParent(int team)
        {
            if (team == GameProperties.NO_TEAM)
                return Team0Parent;
            if (team == GameProperties.TEAM_A)
                return TeamAParent;
            if (team == GameProperties.TEAM_B)
                return TeamBParent;
            return null;
        }

        private void AddEntry(PlayerListEntry entry, int team)
        {
            var entries = GetTeamEntries(team);
            entries.Add(entry.Id, entry);

            if (entry.IsLocal)
            {
                SelectionController.Events.Insert(0, entry.SelectionEvent);
            }
        }

        private void RemoveEntry(PlayerListEntry entry, int team)
        {
            var entries = GetTeamEntries(team);
            entries.Remove(entry.Id);

            if (entry.IsLocal)
            {
                SelectionController.Events.RemoveAt(0);
            }
        }

        private void SetEntryParent(PlayerListEntry entry, int team)
        {
            var parent = GetTeamParent(team);
            entry.transform.SetParent(parent, false);
        }

        private void SwitchPlayerEntry(PlayerListEntry entry, int oldTeam, int newTeam)
        {
            RemoveEntry(entry, oldTeam);
            AddEntry(entry, newTeam);
            SetEntryParent(entry, newTeam);
        }

        private void CreateEntry(Player player)
        {
            var team = GameProperties.GetTeam(player, GetBestTeam());
            var isReady = GameProperties.GetIsReady(player);

            var entry = Instantiate(EntryPrefab);
            entry.Setup(player, isReady, team);

            AddEntry(entry, team);
            SetEntryParent(entry, team);

            if (entry.IsLocal)
            {
                entry.SelectionEvent.OnSelect.AddListener(_OnSwitchReadiness);
            }
        }

        private bool TryGetEntry(int id, out PlayerListEntry entry)
        {
            return (
                _teamA.TryGetValue(id, out entry) ||
                _teamB.TryGetValue(id, out entry)
            );
        }

        private bool TryGetLocalEntry(out PlayerListEntry entry)
        {
            return TryGetEntry(PhotonNetwork.LocalPlayer.ActorNumber, out entry);
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
                SelectionController.Events.Remove(StartSelectionEvent);
                SelectionController.Refresh();
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
            if (TryGetLocalEntry(out var entry))
            {
                entry.IsReady = !entry.IsReady;

                GameProperties.SetIsReady(entry.IsReady);

                SetupStartSelection(ArePlayersReady());
            }
        }

        public void _OnSwitchTeam()
        {
            if (TryGetLocalEntry(out var entry))
            {
                SwitchPlayerEntry(entry, entry.Team, entry.Team * -1);

                entry.Team *= -1;

                GameProperties.SetTeam(entry.Team);
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            CreateEntry(newPlayer);
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (TryGetEntry(targetPlayer.ActorNumber, out var entry))
            {
                if (GameProperties.TryGetIsReady(changedProps, out var isReady))
                {
                    entry.IsReady = isReady;

                    SetupStartSelection(ArePlayersReady());
                }

                if (GameProperties.TryGetTeam(changedProps, out var team))
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
            foreach (var player in PhotonNetwork.PlayerListOthers)
            {
                CreateEntry(player);
            }

            GameProperties.SetInitialProperties(GetBestTeam());

            CreateEntry(PhotonNetwork.LocalPlayer);

            RoomNameText.text = PhotonNetwork.CurrentRoom.Name;

            SetupStart(PhotonNetwork.IsMasterClient);
        }

        public override void OnLeftRoom()
        {
            ClearEntries();

            SelectionController.Events.Clear();

            SetupStart(false);
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
