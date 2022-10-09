using UnityEngine;

namespace Tanks
{
    public class PlayerInput : AInputController
    {
        public KeyCode upKey = KeyCode.W;
        public KeyCode downKey = KeyCode.S;
        public KeyCode leftKey = KeyCode.A;
        public KeyCode rightKey = KeyCode.D;
        public KeyCode fireKey = KeyCode.Space;

        private Vector2Int _direction;
        private bool _shoot;

        public override Vector2Int Direction => _direction;
        public override bool Shoot => _shoot;

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

        private void Update()
        {
            _direction = ReadDirection();
            _shoot = ReadShoot();
        }
    }
}
