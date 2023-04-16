using Common.BehaviourTrees;
using Common.Extensions;
using Common.Mathematics;
using Common;
using UnityEngine;

namespace Tanks.AI
{
    public class DriveTask : BT_ATask<AIController>
    {
        public DriveTask(AIController context) :
            base(context)
        {
        }

        private Vector2Int GetRandomDirection()
        {
            var dx = URandom.Sign() * URandom.Unit();
            var dy = dx == 0 ? URandom.Sign() : 0;

            return new Vector2Int(dx, dy);
        }

        private Vector2Int GetStatueDirection()
        {
            var position = _context.transform.position.XY();
            var enemyTeam = _context.Tank.team.Flip();
            var enemyStatue = _context.StatuesController.Statues[enemyTeam];
            var statuePosition = enemyStatue.transform.position.XY();
            var statueDirection = statuePosition - position;
            var isStatueDirectionHorizontal = Mathf.Abs(statueDirection.x) > Mathf.Abs(statueDirection.y);

            var movementDirection = Mathx.RoundToInt(_context.InputController.MovementController.Direction);
            var isMovementDirectionHorizontal = Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y);

            if (isStatueDirectionHorizontal == isMovementDirectionHorizontal)
                isStatueDirectionHorizontal = !isStatueDirectionHorizontal;

            var dx = isStatueDirectionHorizontal ? Mathf.RoundToInt(Mathf.Sign(statueDirection.x)) : 0;
            var dy = !isStatueDirectionHorizontal ? Mathf.RoundToInt(Mathf.Sign(statueDirection.y)) : 0;

            return new Vector2Int(dx, dy);
        }

        private void SetDirection()
        {
            var direction = URandom.Bool() ? GetRandomDirection() : GetStatueDirection();
            _context.InputController.Direction = direction;
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
            _context.InputController.Direction = Vector2Int.zero;
        }
    }
}
