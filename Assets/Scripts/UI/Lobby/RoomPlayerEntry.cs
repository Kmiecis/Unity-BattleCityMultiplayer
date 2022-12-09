using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Tanks.UI
{
    public class RoomPlayerEntry : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI NameText { get; private set; }
        [field: SerializeField]
        public GameObject ReadyObject { get; private set; }

        private int _id;
        private bool _isReady;
        private ETeam _team;

        public int Id
        {
            get => _id;
        }

        public bool IsReady
        {
            get => _isReady;
            set
            {
                _isReady = value;

                ReadyObject.SetActive(value);
            }
        }

        public ETeam Team
        {
            get => _team;
            set
            {
                _team = value;
            }
        }

        public void Setup(Player player, bool isReady, ETeam team)
        {
            _id = player.ActorNumber;

            IsReady = isReady;
            Team = team;

            NameText.text = player.NickName;
        }
    }
}
