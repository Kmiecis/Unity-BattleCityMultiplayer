using Common.BehaviourTrees;
using Common.Injection;
using Common.Mathematics;
using UnityEngine;

namespace Tanks
{
    public class AIController : MonoBehaviour
    {
        public Range shootDelayRange;
        public Range driveDelayRange;
        public Range driveTimeRange;
        public Range2Int boundsRange;

        [field: SerializeField]
        public Tank Tank { get; private set; }
        [field: SerializeField]
        public AIInput InputController { get; private set; }
        [field: DI_Inject]
        public MapController MapController { get; private set; }
        [field: DI_Inject]
        public StatuesController StatuesController { get; private set; }

        private BT_ITask _shootingBehaviour;
        private BT_ITask _drivingBehaviour;

        private void CreateBehaviours()
        {
            _shootingBehaviour = new BT_TreeNode
            {
                Task = new BT_SequenceNode
                {
                    Tasks = new BT_ITask[]
                    {
                        new BT_Wait(shootDelayRange.Center, shootDelayRange.Length),
                        new AI.ShootTask(this)
                    }
                },
            };

            _drivingBehaviour = new BT_TreeNode
            {
                Task = new BT_SequenceNode
                {
                    Tasks = new BT_ITask[]
                    {
                        new BT_Wait(driveDelayRange.Center, driveDelayRange.Length),
                        new AI.DriveTask(this)
                        {
                            Conditionals = new BT_IConditional[] {
                                new BT_Limit(driveTimeRange.Center, driveTimeRange.Length),
                                new AI.IsNotStuckConditional(this)
                            }
                        }
                    }
                }
            };
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Start()
        {
            CreateBehaviours();
        }

        private void Update()
        {
            _shootingBehaviour.Update();
        }

        private void FixedUpdate()
        {
            _drivingBehaviour.Update();
        }
        #endregion
    }
}
