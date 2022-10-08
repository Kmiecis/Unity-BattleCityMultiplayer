using Common.Mathematics;
using UnityEngine;

namespace Tanks
{
    public class TankController : MonoBehaviour
    {
        public float speed = 1.0f;

        [field: SerializeField]
        public Rigidbody2D Rigidbody { get; private set; }

        private Vector2Int _direction;

        private float GetAngle(Vector2Int direction)
        {
            if (direction.y > 0)
                return 0.0f;
            if (direction.x < 0)
                return 90.0f;
            if (direction.y < 0)
                return 180.0f;
            if (direction.x > 0)
                return 270.0f;
            return 0.0f;
        }

        public void SetDirection(Vector2Int direction)
        {
            if (_direction != direction)
            {
                if (direction.sqrMagnitude > 0.0f)
                {
                    Rigidbody.rotation = GetAngle(direction);
                }

                _direction = direction;
            }
        }

        private void FixedUpdate()
        {
            Rigidbody.velocity = Mathx.Mul(_direction, speed);
        }
    }
}
