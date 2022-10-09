using Common.Mathematics;
using UnityEngine;

namespace Tanks
{
    public class MovementController : MonoBehaviour
    {
        public float speed = 1.0f;

        [field: SerializeField]
        public Rigidbody2D Rigidbody { get; private set; }

        private static float ToAngle(Vector2Int direction)
        {
            if (direction.x < 0)
                return 90.0f;
            if (direction.y < 0)
                return 180.0f;
            if (direction.x > 0)
                return 270.0f;
            return 0.0f;
        }

        private static Vector2Int FromAngle(float angle)
        {
            if (45.0f < angle && angle <= 135.0f)
                return Vector2Int.left;
            if (135.0f < angle && angle <= 225.0f)
                return Vector2Int.down;
            if (225.0f < angle && angle <= 315.0f)
                return Vector2Int.right;
            return Vector2Int.up;
        }

        public void SetMovement(Vector2Int direction)
        {
            Rigidbody.rotation = ToAngle(direction);
            Rigidbody.velocity = Mathx.Mul(direction, speed);
        }

        public void ApplyMovement(float time)
        {
            Rigidbody.position += Rigidbody.velocity * time;
        }

        public void ResetMovement()
        {
            Rigidbody.velocity = Vector2.zero;
        }
    }
}
