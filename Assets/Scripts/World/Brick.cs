using Common.Extensions;
using Common.Injection;
using Common.Mathematics;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    public class Brick : Block
    {
        [field: SerializeField]
        public GameObject BlockObject { get; private set; }
        [field: SerializeField]
        public GameObject PiecesObject { get; private set; }
        [field: SerializeField]
        public List<Brick> Pieces { get; private set; }
        [field: DI_Inject]
        public BricksController BricksController { get; private set; }

        private Brick _parent;
        private int _hitframe;

        public bool IsFractured
            => BlockObject == null;

        public void Fracture()
        {
            BlockObject.SetActive(false);
            BlockObject = null;

            PiecesObject.SetActive(true);

            _hitframe = Time.frameCount;
        }

        private void Remove(Brick piece)
        {
            Pieces.Remove(piece);

            if (Pieces.Count == 0)
            {
                Destroy(gameObject);
            }
        }

        private Brick FindClosestPiece(Vector2 position)
        {
            if (Pieces.Count == 0)
                return this;

            Brick closestPiece = null;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < Pieces.Count; ++i)
            {
                var piece = Pieces[i].FindClosestPiece(position);

                var distance = Vector2.Distance(position, piece.transform.position);
                if (closestPiece == null || distance < closestDistance)
                {
                    closestPiece = piece;
                    closestDistance = distance;
                }
            }

            return closestPiece;
        }

        
        public void Hit(Vector2 hitPosition, Vector2 hitDirection)
        {
            if (_hitframe == Time.frameCount)
                return;
            
            const float RAY_LENGTH = 0.8f;

            var piece = FindClosestPiece(hitPosition);

            var position = (Vector2)piece.transform.position;
            var parallel = Mathx.Abs(hitDirection.YX());
            var deltaPosition = position - hitPosition;
            var rayPosition = position - parallel * deltaPosition - parallel * RAY_LENGTH * 0.5f;
            var rayVector = parallel * RAY_LENGTH;

            BricksController.StrikeBricks(rayPosition, rayVector);
            BricksController.RPCStrikeBricks(rayPosition, rayVector);
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);

            foreach (var piece in Pieces)
            {
                piece._parent = this;
            }
        }

        private void OnDestroy()
        {
            if (_parent != null)
            {
                _parent.Remove(this);
            }
        }
        #endregion
    }
}
