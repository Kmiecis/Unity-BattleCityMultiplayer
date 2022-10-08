using UnityEngine;

namespace Tanks
{
    public class Player : MonoBehaviour
    {
        public KeyCode upKey = KeyCode.W;
        public KeyCode downKey = KeyCode.S;
        public KeyCode leftKey = KeyCode.A;
        public KeyCode rightKey = KeyCode.D;

        [field: SerializeField]
        public TankController Controller { get; private set; }

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

        private void Update()
        {
            var direction = ReadDirection();
            Controller.SetDirection(direction);
        }
    }
}
