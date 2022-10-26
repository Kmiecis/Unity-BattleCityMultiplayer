using Photon.Pun;
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
        public GameObject HighlightObject { get; private set; }
        [field: SerializeField]
        public GameObject ReadyObject { get; private set; }
        [field: SerializeField]
        public SelectionEventHandler SelectionEvent { get; private set; }

        private int _id;
        private bool _isReady;
        private int _team;

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

        public int Team
        {
            get => _team;
            set
            {
                _team = value;
            }
        }

        public bool IsLocal
        {
            get => _id == PhotonNetwork.LocalPlayer.ActorNumber;
        }

        public void Setup(Player player, bool isReady, int team)
        {
            _id = player.ActorNumber;

            IsReady = isReady;
            Team = team;

            NameText.text = player.NickName;
            HighlightObject.SetActive(IsLocal);
        }
    }
}
