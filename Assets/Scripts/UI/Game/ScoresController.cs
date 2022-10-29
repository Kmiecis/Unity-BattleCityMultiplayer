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
        [field: SerializeField]
        public GameObject WonObject { get; private set; }
        [field: SerializeField]
        public GameObject LostObject { get; private set; }

        private Dictionary<int, ScoreEntry> _entries = new Dictionary<int, ScoreEntry>();
        private int _score = 0;

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

                var delta = -entry.Kills;
                if (delta != 0)
                {
                    UpdateScore(delta);
                }

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

            var kills = entry.Kills;
            entry.SetScore(player);
            var delta = entry.Kills - kills;

            if (delta != 0)
            {
                UpdateScore(delta);
            }
        }

        private void UpdateScore(int delta)
        {
            _score += delta;
            ScoreText.text = _score.ToString();
        }

        private void RefreshWon()
        {
            var teamWon = PhotonNetwork.CurrentRoom.GetTeamWon();
            RefreshWon(teamWon != ETeam.None, teamWon == team);
        }

        private void RefreshWon(bool anyWon, bool myWon)
        {
            WonObject.SetActive(anyWon && myWon);
            LostObject.SetActive(anyWon && !myWon);
        }

        #region Photon methods
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.TryGetTeamWon(out var teamWon))
            {
                RefreshWon(true, teamWon == team);
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (targetPlayer.GetTeam() == team &&
                _entries.TryGetValue(targetPlayer.ActorNumber, out var entry))
            {
                if (changedProps.TryGetKills(out int kills))
                {
                    var delta = entry.SetKills(kills);

                    if (delta != 0)
                    {
                        UpdateScore(delta);
                    }

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
        private void Awake()
        {
            UpdateScore(0);
        }

        public override void OnEnable()
        {
            base.OnEnable();

            RefreshEntries();
            RefreshWon();
        }
        #endregion
    }
}
