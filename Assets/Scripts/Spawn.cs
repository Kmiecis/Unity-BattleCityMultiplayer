using Common.Injection;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    public class Spawn : MonoBehaviour
    {
        public ETeam team;

        [field: SerializeField]
        public Trigger2DController TriggerController { get; private set; }
        [field: DI_Inject]
        public SpawnsController SpawnsController { get; private set; }

        private bool _valid = true;
        private List<GameObject> _triggers = new List<GameObject>();

        public bool IsValid
        {
            get => _valid;
            set => _valid = value;
        }

        public void _OnTriggerEntered(Collider2D collision)
        {
            _triggers.Add(collision.gameObject);
            OnTriggersChange();
        }

        public void _OnTriggerExited(Collider2D collision)
        {
            _triggers.Remove(collision.gameObject);
            OnTriggersChange();
        }

        private void OnTriggersChange()
        {
            var valid = _triggers.Count == 0;
        }

        #region Injection methods
        private void OnSpawnsControllerInject(SpawnsController controller)
        {
            controller.Spawns[team].Add(this);
        }
        #endregion

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }
        #endregion
    }
}
