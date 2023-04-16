using UnityEngine;

namespace Tanks
{
    public class CameraFitter : MonoBehaviour
    {
        private const float DEFAULT_ASPECT_RATIO = 16.0f / 9.0f;

        [field: SerializeField]
        public Camera Camera { get; private set; }

        [SerializeField]
        private float _targetAspectRatio = DEFAULT_ASPECT_RATIO;

        private float _aspectRatio = DEFAULT_ASPECT_RATIO;
        private float _orthographicSize;

        private void AspectRatioFitter()
        {
            var aspectRatio = Screen.width * 1.0f / Screen.height;
            if (!Mathf.Approximately(_aspectRatio, aspectRatio))
            {
                var factor = aspectRatio < _targetAspectRatio ? _targetAspectRatio / aspectRatio : 1.0f;
                Camera.orthographicSize = _orthographicSize * factor;

                _aspectRatio = aspectRatio;
            }
        }

        #region Unity methods
        private void Start()
        {
            _orthographicSize = Camera.orthographicSize;
        }

        private void Update()
        {
            AspectRatioFitter();
        }
        #endregion
    }
}
