using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Tanks.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Ping : MonoBehaviour
    {
        private const float UPDATE_DELAY = 1.0f;

        [field: SerializeField]
        public TextMeshProUGUI Text { get; private set; }

        private float _time;

        private void UpdatePing()
        {
            var ping = PhotonNetwork.GetPing();

            Text.text = ping.ToString();
        }

        private void Start()
        {
            enabled = Text != null;
        }

        private void Update()
        {
            var time = Time.time;
            if (_time < time)
            {
                UpdatePing();

                _time = time + UPDATE_DELAY;
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            Text = GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}
