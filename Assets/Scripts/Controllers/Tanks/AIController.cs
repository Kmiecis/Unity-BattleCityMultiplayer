using Common;
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
        public AIInput InputController { get; private set; }
        [field: DI_Inject]
        public MapController MapController { get; private set; }
        [field: DI_Inject]
        public StatuesController StatuesController { get; private set; }

        private BT_ITask _shootingBehaviour;
        private BT_ITask _drivingBehaviour;

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Start()
        {
            CreateBehaviour();
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

        private void CreateBehaviour()
        {
            // Shoot randomly every x-y seconds
            // Pick a direction and follow it
            // Direction should be picked with % chance of 'good' direction and rest % change of 'bad' direction
            // If there is more enemies than allies behind middle of map, then good direction is coming back
            // Otherwise the good direction is pushing forward
            // If seeing a goal, then should focus on it

            _shootingBehaviour = new BT_TreeNode
            {
                Task = new BT_SequenceNode
                {
                    Tasks = new BT_ITask[]
                    {
                        new BT_Wait(shootDelayRange.Center, shootDelayRange.Length),
                        new ShootTask(this)
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
                        new DriveTask(this)
                        {
                            Conditionals = new BT_IConditional[] {
                                new BT_Limit(driveTimeRange.Center, driveTimeRange.Length),
                                new IsNotStuckConditional(this)
                            }
                        }
                    }
                }
            };
        }
        // TODO: Add not shooting at your own Statue
        private class ShootTask : BT_ATask<AIController>
        {
            public ShootTask(AIController context) :
                base(context)
            {
            }

            protected override BT_EStatus OnUpdate()
            {
                var shoot = _context.InputController.BulletController.CanFire;
                _context.InputController.SetShoot(shoot);
                return shoot ? BT_EStatus.Running : BT_EStatus.Success;
            }
        }
        // TODO: Add driving to enemy statue
        private class DriveTask : BT_ATask<AIController>
        {
            public DriveTask(AIController context) :
                base(context)
            {
            }

            protected override void OnStart()
            {
                SetDirection();
            }

            protected override BT_EStatus OnUpdate()
            {
                return BT_EStatus.Running;
            }

            protected override void OnFinish()
            {
                _context.InputController.ResetDirection();
            }

            private void SetDirection()
            {
                var dx = URandom.Sign() * URandom.Unit();
                var dy = dx == 0 ? URandom.Sign() : 0;

                _context.InputController.SetDirection(new Vector2Int(dx, dy));
            }
        }

        private class IsNotStuckConditional : BT_AConditional<AIController>
        {
            const float DISTANCE_COEFFICIENT = 0.5f;
            const float CHECK_TIME_DELTA = 0.1f;

            private float _time;
            private float _distance;
            private Vector2 _position;

            public IsNotStuckConditional(AIController context) :
                base(context)
            {
            }

            protected override void OnStart()
            {
                _time = CHECK_TIME_DELTA;
                _distance = _context.InputController.MovementController.speed * DISTANCE_COEFFICIENT * _time;
                _position = _context.transform.position;
            }

            public override bool CanExecute()
            {
                var dt = Time.deltaTime;
                _time -= dt;

                if (_time < 0.0f)
                {
                    var position = (Vector2)_context.transform.position;
                    var distance = (position - _position).magnitude;
                    if (distance < _distance)
                    {
                        return false;
                    }

                    _position = position;
                    _time = CHECK_TIME_DELTA;
                }
                return true;
            }
        }
    }
}
