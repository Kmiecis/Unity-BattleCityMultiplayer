using UnityEngine;

namespace Tanks
{
    public class Brick : MonoBehaviour
    {
        public GameObject modelPrefab;

        [field: SerializeField]
        public Collision2DController CollisionController { get; private set; }

        private void _OnCollisionEntered(Collision2D collision)
        {
            var point = GetMeanPoint(collision);
            var normal = GetMeanNormal(collision);
            //TODO
        }

        private Vector2 GetMeanPoint(Collision2D collision)
        {
            var result = Vector2.zero;
            int count = collision.contactCount;
            for (int i = 0; i < count; ++i)
            {
                var contact = collision.GetContact(i);
                result += contact.point;
            }
            result /= count;
            return result;
        }

        private Vector2 GetMeanNormal(Collision2D collision)
        {
            var result = Vector2.zero;
            int count = collision.contactCount;
            for (int i = 0; i < count; ++i)
            {
                var contact = collision.GetContact(i);
                result += contact.normal;
            }
            result /= count;
            return result;
        }
    }
}
