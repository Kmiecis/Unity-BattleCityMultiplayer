using Common.MVB;
using UnityEngine;

namespace Tanks
{
    public class PlayerInput : AInputController
    {
        [Header("Input keys")]
        public ScriptableKeyCode upKey;
        public ScriptableKeyCode downKey;
        public ScriptableKeyCode leftKey;
        public ScriptableKeyCode rightKey;
        public ScriptableKeyCode fireKey;

        private Vector2Int _direction;
        private bool _shoot;

        public override bool IsEnabled
        {
            set => base.IsEnabled = value && photonView.IsMine;
        }

        public override Vector2Int Direction
            => _direction;

        public override bool Shoot
            => _shoot;

        private void ReadInput()
        {
            _direction = ReadDirection();
            _shoot = ReadShoot();
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
        protected override void Update()
        {
            ReadInput();
            base.Update();
        }
        #endregion
    }
}
