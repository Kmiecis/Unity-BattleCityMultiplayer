using Common.Injection;
using Common.MVB;
using UnityEngine;

namespace Tanks
{
    public class PlayerInput : AInputController
    {
        [Header("Input keys")]
        public KeyCodeAsset upKey;
        public KeyCodeAsset downKey;
        public KeyCodeAsset leftKey;
        public KeyCodeAsset rightKey;
        public KeyCodeAsset fireKey;

        [field: SerializeField]
        public SoundData MovementSound { get; private set; }
        [field: SerializeField]
        public SoundData IdleSound { get; private set; }

        [field: DI_Inject]
        public SoundsController SoundsController { get; private set; }

        public override bool IsEnabled
        {
            set => base.IsEnabled = (
                value &&
                photonView.IsMine
            );
        }

        protected override void OnMovementChange(Vector2Int value)
        {
            SoundsController.StopSound(IdleSound);
            SoundsController.StopSound(MovementSound);

            var sound = value != Vector2Int.zero ? MovementSound : IdleSound;
            SoundsController.PlaySound(sound);
        }

        private void ReadInput()
        {
            Direction = ReadDirection();
            Shooting = ReadShoot();
        }

        private Vector2Int ReadDirection()
        {
            if (Input.GetKey(upKey))
                return Vector2Int.up;
            if (Input.GetKey(downKey))
                return Vector2Int.down;
            if (Input.GetKey(rightKey))
                return Vector2Int.right;
            if (Input.GetKey(leftKey))
                return Vector2Int.left;
            return Vector2Int.zero;
        }

        private bool ReadShoot()
        {
            return Input.GetKey(fireKey);
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        protected override void Update()
        {
            ReadInput();
            base.Update();
        }

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
