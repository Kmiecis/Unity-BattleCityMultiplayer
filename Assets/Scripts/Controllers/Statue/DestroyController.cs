using UnityEngine;

namespace Tanks
{
    public class DestroyController : MonoBehaviour
    {
        [field: SerializeField]
        public Sprite DestroyedSprite { get; private set; }
        [field: SerializeField]
        public SpriteRenderer DestroyedRenderer { get; private set; }

        private bool _destroyed;

        public bool IsDestroyed
        {
            get => _destroyed;
        }

        public void SetDestroyed()
        {
            DestroyedRenderer.sprite = DestroyedSprite;
            _destroyed = true;
        }
    }
}
