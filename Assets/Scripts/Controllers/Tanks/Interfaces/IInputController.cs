using UnityEngine;

namespace Tanks
{
    public interface IInputController
    {
        Vector2Int Direction { get; }
        bool Shooting { get; }
    }
}
