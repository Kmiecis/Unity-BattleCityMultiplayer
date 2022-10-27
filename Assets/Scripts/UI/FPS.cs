using TMPro;
using UnityEngine;

namespace Tanks.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class FPS : MonoBehaviour
    {
        private const float UPDATE_DELAY = 1.0f;

        [field: SerializeField]
        public TextMeshProUGUI Text { get; private set; }

        private int _frames;
        private float _accumulated;

        private void UpdateFPS()
        {
            var fps = Mathf.RoundToInt(_frames / _accumulated);

            Text.text = fps.ToString();
        }

        private void OnEnable()
        {
            _frames = 1;
            _accumulated = Time.smoothDeltaTime;

            UpdateFPS();
        }

        private void Start()
        {
            enabled = Text != null;
        }

        private void Update()
        {
            _frames += 1;
            _accumulated += Time.smoothDeltaTime;

            if (_accumulated > UPDATE_DELAY)
            {
                UpdateFPS();

                _frames = 0;
                _accumulated -= UPDATE_DELAY;
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
