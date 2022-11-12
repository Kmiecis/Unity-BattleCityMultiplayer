using Photon.Pun;
using Photon.Realtime;
using Tanks.Extensions;
using TMPro;
using UnityEngine;

namespace Tanks.UI
{
    public class ScoreEntry : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI NameText { get; private set; }
        [field: SerializeField]
        public TextMeshProUGUI ScoreText { get; private set; }
        [field: SerializeField]
        public GameObject HighlightedObject { get; private set; }

        private Player _player;
        private int _kills;
        private int _deaths;

        public Player Player
            => _player;

        public int Kills
            => _kills;

        public int Deaths
            => _deaths;

        public void Setup(Player player)
        {
            _player = player;

            NameText.text = player.NickName;
            HighlightedObject.SetActive(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);
        }

        public void SetScore(Player player)
        {
            _kills = _player.GetKills();
            _deaths = _player.GetDeaths();
            SetScore(_kills, _deaths);
        }

        private void SetScore(int kills, int deaths)
        {
            ScoreText.text = $"{kills}/{deaths}";
        }

        public int SetKills(int value)
        {
            SetScore(value, _deaths);

            var delta = value - _kills;
            _kills = value;
            return delta;
        }

        public int SetDeaths(int value)
        {
            SetScore(_kills, value);

            var delta = value - _deaths;
            _deaths = value;
            return delta;
        }
    }
}
