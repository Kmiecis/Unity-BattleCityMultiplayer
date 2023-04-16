using Common.BehaviourTrees;
using UnityEngine;

namespace Tanks.AI
{
    public class IsNotStuckConditional : BT_AConditional<AIController>
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
