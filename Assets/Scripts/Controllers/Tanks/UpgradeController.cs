using UnityEngine;

namespace Tanks
{
    public class UpgradeController : MonoBehaviour
    {
        [SerializeField]
        private int _starting;

        [field: SerializeField]
        public Sprite[] UpgradeSprites { get; private set; }
        [field: SerializeField]
        public Sprite[] HighlightSprites { get; private set; }
        [field: SerializeField]
        public SpriteRenderer ModelRenderer { get; private set; }
        [field: SerializeField]
        public SpriteRenderer HighlightRenderer { get; private set; }

        private int _current;

        public int Current
            => _current;

        public int Limit
            => UpgradeSprites.Length;

        public bool HasUpgrade
            => Current < Limit - 1;

        public bool HasDowngrade
            => Current > 0;

        public Sprite CurrentUpgrade
            => UpgradeSprites[Current];

        public Sprite CurrentHighlight
            => HighlightSprites[Current];

        private void UpdateCurrent()
        {
            ModelRenderer.sprite = CurrentUpgrade;
            HighlightRenderer.sprite = CurrentHighlight;
        }

        public void SetUpgrade(int upgrade)
        {
            _current = upgrade;

            UpdateCurrent();
        }

        public void Upgrade()
        {
            SetUpgrade(_current + 1);
        }

        public void Downgrade()
        {
            SetUpgrade(_current - 1);
        }

        public void SetDefault()
        {
            SetUpgrade(_starting);
        }

        #region Unity methods
        private void Start()
        {
            SetDefault();
        }
        #endregion
    }
}
