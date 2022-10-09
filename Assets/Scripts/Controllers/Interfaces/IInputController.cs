using UnityEngine;

namespace Tanks
{
    public interface IInputController
    {
        Vector2Int Direction { get; }
        bool Shoot { get; }
    }
}
