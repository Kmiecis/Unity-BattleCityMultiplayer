using Common.Extensions;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Tanks.UI
{
    public class NetworkClientState : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI Text { get; private set; }

        private ClientState _state = (ClientState)(-1);

        private void UpdateStatus()
        {
            var state = PhotonNetwork.NetworkClientState;
            if (_state != state)
            {
                _state = state;

                Text.text = state.ToString()
                    .SplitByCamelCase()
                    .Join(' ')
                    .ToUpper();
            }
        }

        private void Start()
        {
            enabled = Text != null;
        }

        private void Update()
        {
            UpdateStatus();
        }

#if UNITY_EDITOR
        private void Reset()
        {
            Text = GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}
