using TMPro;
using UnityEngine;

namespace Tanks.UI
{
    public class RangeInput : MonoBehaviour
    {
        public int min;
        public int max;

        [field: SerializeField]
        public TMP_InputField InputField { get; private set; }

        private void OnEndEdit(string value)
        {
            if (int.TryParse(value, out var number))
            {
                number = Mathf.Clamp(number, min, max);
                InputField.text = number.ToString();
            }
        }

        private void Start()
        {
            if (InputField != null)
            {
                InputField.onEndEdit.AddListener(OnEndEdit);
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            InputField = GetComponent<TMP_InputField>();
        }
#endif
    }
}
