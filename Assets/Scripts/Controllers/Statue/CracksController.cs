using Common.Materializer;
using UnityEngine;

namespace Tanks
{
    public class CracksController : MonoBehaviour
    {
        [field: SerializeField]
        public Texture[] Cracks { get; private set; }
        [field: SerializeField]
        public MaterialTexture MaterialTexture { get; private set; }

        private int _current;

        public int Current
        {
            get => _current;
        }

        public Texture CurrentCrack
        {
            get => Cracks[_current];
        }

        public bool HasNext
        {
            get => _current < Cracks.Length - 1;
        }

        public bool HasPrev
        {
            get => _current > 0;
        }

        public bool CanSetCrack(int crack)
        {
            return (
                crack > -1 &&
                crack < Cracks.Length
            );
        }

        public void SetCrack(int crack)
        {
            _current = crack;
            UpdateCurrent();
        }

        public void SetNext()
        {
            SetCrack(_current + 1);
        }

        public void SetPrev()
        {
            SetCrack(_current - 1);
        }

        public void SetDefault()
        {
            SetCrack(0);
        }

        private void UpdateCurrent()
        {
            MaterialTexture.Value = CurrentCrack;
        }

        #region Unity methods
        private void Start()
        {
            UpdateCurrent();
        }
        #endregion
    }
}
