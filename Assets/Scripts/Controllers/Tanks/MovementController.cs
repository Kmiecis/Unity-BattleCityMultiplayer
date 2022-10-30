using UnityEngine;

namespace Tanks
{
    public class MovementController : MonoBehaviour
    {
        public float speed = 1.0f;

        [field: SerializeField]
        public Rigidbody2D Rigidbody { get; private set; }

        public Vector2 Direction
        {
            get => FromAngle(Rigidbody.rotation);
        }

        private static float ToAngle(Vector2 direction)
        {
            var x = Mathf.RoundToInt(direction.x);
            var y = Mathf.RoundToInt(direction.y);
            if (x < 0)
                return 90.0f;
            if (y < 0)
                return 180.0f;
            if (x > 0)
                return 270.0f;
            return 0.0f;
        }

        private static Vector2 FromAngle(float angle)
        {
            if (45.0f < angle && angle <= 135.0f)
                return Vector2.left;
            if (135.0f < angle && angle <= 225.0f)
                return Vector2.down;
            if (225.0f < angle && angle <= 315.0f)
                return Vector2.right;
            return Vector2.up;
        }

        public void SetMovement(Vector2 direction)
        {
            Rigidbody.rotation = ToAngle(direction);
            Rigidbody.velocity = direction * speed;
        }

        public void ApplyMovement(float time)
        {
            Rigidbody.transform.position += (Vector3)(Rigidbody.velocity * time);
        }

        public void ResetMovement()
        {
            Rigidbody.velocity = Vector2.zero;
        }
    }
}
