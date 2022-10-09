using UnityEngine;

namespace Tanks
{
    public abstract class AInputController : MonoBehaviour, IInputController
    {
        public abstract Vector2Int Direction { get; }
        public abstract bool Shoot { get; }
    }
}
