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

        private void OnTriggerEntered(Collider2D collision)
        {
            _triggers.Add(collision.gameObject);
        }

        private void OnTriggerExited(Collider2D collision)
        {
            _triggers.Remove(collision.gameObject);
        }

        private void Awake()
        {
            TriggerController.CalledOnEnter += OnTriggerEntered;
            TriggerController.CalledOnExit += OnTriggerExited;
        }

        private void OnDestroy()
        {
            TriggerController.CalledOnEnter -= OnTriggerEntered;
            TriggerController.CalledOnExit -= OnTriggerExited;
        }
    }
}
