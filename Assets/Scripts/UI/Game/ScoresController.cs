using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Tanks.Extensions;
using TMPro;
using UnityEngine;

namespace Tanks.UI
{
    public class ScoresController : MonoBehaviourPunCallbacks
    {
        [field: SerializeField]
        public ScoreEntry EntryPrefab { get; private set; }
        [field: SerializeField]
        public TextMeshProUGUI TeamAScoreText { get; private set; }
        [field: SerializeField]
        public TextMeshProUGUI TeamBScoreText { get; private set; }
        [field: SerializeField]
        public Transform TeamAParent { get; private set; }
        [field: SerializeField]
        public Transform TeamBParent { get; private set; }

        private Dictionary<int, ScoreEntry> _entries = new Dictionary<int, ScoreEntry>();
        private int _teamAScore = 0;
        private int _teamBScore = 0;

        private TextMeshProUGUI GetScoreText(int team)
        {
            if (team == GameProperties.TEAM_A)
                return TeamAScoreText;
            if (team == GameProperties.TEAM_B)
                return TeamBScoreText;
            return null;
        }

        private Transform GetEntryParent(int team)
        {
            if (team == GameProperties.TEAM_A)
                return TeamAParent;
            if (team == GameProperties.TEAM_B)
                return TeamBParent;
            return null;
        }

        private int GetScore(int team)
        {
            if (team == GameProperties.TEAM_A)
                return _teamAScore;
            if (team == GameProperties.TEAM_B)
                return _teamBScore;
            return 0;
        }

        private void SetScore(int team, int value)
        {
            if (team == GameProperties.TEAM_A)
                _teamAScore = value;
            else if (team == GameProperties.TEAM_B)
                _teamBScore = value;
        }

        private ScoreEntry CreateEntry(Player player)
        {
            var entry = Instantiate(EntryPrefab);
            entry.Setup(player);

            int team = player.GetTeam();
            SetEntryParent(entry, team);

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
                    UpdateScore(entry.Player, delta);
                }

                Destroy(entry.gameObject);
            }
        }

        private void SetEntryParent(ScoreEntry entry, int team)
        {
            var parent = GetEntryParent(team);
            entry.transform.SetParent(parent, false);
        }

        private void RefreshEntries()
        {
            var playerSet = new HashSet<int>();

            foreach (var player in PhotonNetwork.PlayerList)
            {
                RefreshEntry(player);

                playerSet.Add(player.ActorNumber);
            }

            foreach (var id in _entries.Keys)
            {
                if (!playerSet.Contains(id))
                {
                    DestroyEntry(id);
                }
            }
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
                UpdateScore(player, delta);
            }
        }

        private void UpdateScore(Player player, int delta)
        {
            var team = player.GetTeam();
            UpdateScore(team, delta);
        }

        private void UpdateScore(int team, int delta)
        {
            var score = GetScore(team);
            score += delta;

            var scoreText = GetScoreText(team);
            scoreText.text = score.ToString();

            SetScore(team, score);
        }

        #region Photon methods
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (_entries.TryGetValue(targetPlayer.ActorNumber, out var entry))
            {
                if (changedProps.TryGetKills(out int kills))
                {
                    var delta = entry.SetKills(kills);

                    if (delta != 0)
                    {
                        UpdateScore(targetPlayer, delta);
                    }
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
            UpdateScore(GameProperties.TEAM_A, 0);
            UpdateScore(GameProperties.TEAM_B, 0);
        }

        public override void OnEnable()
        {
            base.OnEnable();

            RefreshEntries();
        }
        #endregion
    }
}
