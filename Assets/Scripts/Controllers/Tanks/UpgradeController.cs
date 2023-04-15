using Common.Injection;
using Photon.Pun;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(PhotonView))]
    public class UpgradeController : MonoBehaviourPun
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
        [field: SerializeField]
        public SoundData DowngradeSound { get; private set; }

        [field: DI_Inject]
        public SoundsController SoundsController { get; private set; }

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

        public bool TryUpgrade()
        {
            if (HasUpgrade)
            {
                Upgrade();
                return true;
            }
            return false;
        }

        public void Downgrade()
        {
            SetUpgrade(_current - 1);

            SoundsController.PlaySound(DowngradeSound);
        }

        public bool TryDowngrade()
        {
            if (HasDowngrade)
            {
                Downgrade();
                return true;
            }
            return false;
        }

        public void SetDefault()
        {
            SetUpgrade(_starting);
        }

        #region Photon methods
        public void RPCUpgrade()
        {
            photonView.RPC(nameof(RPCUpgrade_Internal), RpcTarget.Others);
        }

        [PunRPC]
        private void RPCUpgrade_Internal()
        {
            TryUpgrade();
        }

        public void RPCDowngrade()
        {
            photonView.RPC(nameof(RPCDowngrade_Internal), RpcTarget.Others);
        }

        [PunRPC]
        private void RPCDowngrade_Internal()
        {
            TryDowngrade();
        }
        #endregion

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Start()
        {
            SetDefault();
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
