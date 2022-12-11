using UnityEngine;

namespace Tanks
{
    public class AIInput : AInputController
    {
        public override Vector2Int Direction
            => Vector2Int.zero;

        public override bool Shoot
            => true;
    }
}
