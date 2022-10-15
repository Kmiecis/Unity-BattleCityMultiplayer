using TMPro;
using UnityEngine;

namespace Tanks.UI
{
    public class UppercaseInput : MonoBehaviour
    {
        [field: SerializeField]
        public TMP_InputField InputField { get; private set; }

        private void OnInputChanged(string value)
        {
            InputField.text = value.ToUpper();
        }

        private void Start()
        {
            if (InputField != null)
            {
                InputField.onValueChanged.AddListener(OnInputChanged);
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
