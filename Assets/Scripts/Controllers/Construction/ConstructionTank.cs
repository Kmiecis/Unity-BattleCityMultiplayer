using Common;
using Common.Injection;
using Common.Mathematics;
using UnityEngine;

namespace Tanks
{
    public class ConstructionTank : MonoBehaviour
    {
        public Range2Int bounds;

        [field: DI_Inject]
        private ConstructionController ConstructionController;

        private Vector2Int _coordinates;

        public void _MoveLeft()
        {
            TryMoveTo(new Vector2Int(_coordinates.x - 1, _coordinates.y));
        }

        public void _MoveRight()
        {
            TryMoveTo(new Vector2Int(_coordinates.x + 1, _coordinates.y));
        }

        public void _MoveUp()
        {
            TryMoveTo(new Vector2Int(_coordinates.x, _coordinates.y + 1));
        }

        public void _MoveDown()
        {
            TryMoveTo(new Vector2Int(_coordinates.x, _coordinates.y - 1));
        }

        public void _ChangeTile()
        {
            ConstructionController.ChangeTile(_coordinates);
        }

        private void TryMoveTo(Vector2Int coordinates)
        {
            if (bounds.Contains(coordinates.x, coordinates.y))
            {
                _coordinates = coordinates;

                transform.position = (Vector2)coordinates;
            }
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }
        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UGizmos.DrawWireRect(bounds.Center, bounds.Size);
        }
#endif
    }
}
