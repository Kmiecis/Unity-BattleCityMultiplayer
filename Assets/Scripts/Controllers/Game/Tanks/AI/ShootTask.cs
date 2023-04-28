using Common.BehaviourTrees;
using Common.Mathematics;
using UnityEngine;

namespace Tanks.AI
{
    public class ShootTask : BT_ATask<AIController>
    {
        public ShootTask(AIController context) :
            base(context)
        {
        }

        private bool IsLookingAtOwnStatue()
        {
            var team = _context.Tank.team;
            var position = Mathx.Round(_context.transform.position);
            var friendlyStatue = _context.StatuesController.Statues[team];
            var statuePosition = Mathx.Round(friendlyStatue.transform.position);
            var statueDirection = (statuePosition - position).normalized;
            var movementDirection = _context.InputController.MovementController.Direction.normalized;

            var directionsDot = Vector2.Dot(statueDirection, movementDirection);
            return Mathf.Approximately(directionsDot, 1.0f);
        }

        private bool CanShoot()
        {
            return (
                !IsLookingAtOwnStatue() &&
                _context.InputController.BulletController.CanShoot
            );
        }

        protected override BT_EStatus OnUpdate()
        {
            var shoot = CanShoot();
            _context.InputController.Shooting = shoot;
            return BT_EStatus.Success;
        }
    }
}
