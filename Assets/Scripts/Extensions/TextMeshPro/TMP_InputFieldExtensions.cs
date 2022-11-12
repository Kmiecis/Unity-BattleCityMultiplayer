using TMPro;

namespace Tanks.Extensions
{
    public static class TMP_InputFieldExtensions
    {
        public static TextMeshProUGUI GetPlaceholderText(this TMP_InputField self)
        {
            return (TextMeshProUGUI)self.placeholder;
        }
    }
}
