using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    public class Spawn : MonoBehaviour
    {
        [field: SerializeField]
        public Trigger2DController TriggerController { get; private set; }

        private List<GameObject> _triggers = new List<GameObject>();

        public bool IsValid
        {
            get => _triggers.Count == 0;
        }

        public void _OnTriggerEntered(Collider2D collision)
        {
            _triggers.Add(collision.gameObject);
        }

        public void _OnTriggerExited(Collider2D collision)
        {
            _triggers.Remove(collision.gameObject);
        }
    }
}
