using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using Tanks.Extensions;
using TMPro;
using UnityEngine;

namespace Tanks.UI
{
    public class ScoresController : MonoBehaviourPunCallbacks
    {
        public ETeam team = ETeam.None;

        [field: SerializeField]
        public ScoreEntry EntryPrefab { get; private set; }
        [field: SerializeField]
        public Transform EntryParent { get; private set; }
        [field: SerializeField]
        public TextMeshProUGUI ScoreText { get; private set; }

        private Dictionary<int, ScoreEntry> _entries = new Dictionary<int, ScoreEntry>();

        private ScoreEntry CreateEntry(Player player)
        {
            var entry = Instantiate(EntryPrefab);
            entry.Setup(player);
            entry.transform.SetParent(EntryParent, false);
            return entry;
        }

        private void DestroyEntry(int id)
        {
            if (_entries.TryGetValue(id, out var entry))
            {
                _entries.Remove(id);

                Destroy(entry.gameObject);
            }
        }

        private void SortEntries()
        {
            var entries = _entries.Values.ToList();

            int EntryComparer(ScoreEntry left, ScoreEntry right)
            {
                return left.Kills - right.Kills;
            }

            entries.Sort(EntryComparer);

            for (int i = 0; i < entries.Count; ++i)
            {
                entries[i].transform.SetSiblingIndex(i);
            }
        }

        private void RefreshEntries()
        {
            var playerSet = new HashSet<int>();

            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.GetTeam() == team)
                {
                    RefreshEntry(player);

                    playerSet.Add(player.ActorNumber);
                }
            }

            foreach (var id in _entries.Keys)
            {
                if (!playerSet.Contains(id))
                {
                    DestroyEntry(id);
                }
            }

            SortEntries();
        }

        private void RefreshEntry(Player player)
        {
            if (!_entries.TryGetValue(player.ActorNumber, out var entry))
            {
                entry = CreateEntry(player);
                _entries.Add(player.ActorNumber, entry);
            }

            entry.SetScore(player);
        }

        private void RefreshScore()
        {
            var wins = PhotonNetwork.CurrentRoom.GetTeamWins(team);
            UpdateScore(wins);
        }

        private void UpdateScore(int wins)
        {
            ScoreText.text = wins.ToString();
        }

        #region Photon methods
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.TryGetTeamWins(team, out var wins))
            {
                UpdateScore(wins);
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (targetPlayer.GetTeam() == team &&
                _entries.TryGetValue(targetPlayer.ActorNumber, out var entry))
            {
                if (changedProps.TryGetKills(out int kills))
                {
                    entry.SetKills(kills);

                    SortEntries();
                }

                if (changedProps.TryGetDeaths(out int deaths))
                {
                    entry.SetDeaths(deaths);
                }
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            DestroyEntry(otherPlayer.ActorNumber);
        }
        #endregion

        #region Unity methods
        public override void OnEnable()
        {
            base.OnEnable();

            RefreshEntries();
            RefreshScore();
        }
        #endregion
    }
}
