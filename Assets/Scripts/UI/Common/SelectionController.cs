using Common.Extensions;
using Common.Mathematics;
using Common.MVVM;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks.UI
{
    public class SelectionController : MonoBehaviour
    {
        public int index = 0;

        [field: SerializeField]
        public List<SelectionEventHandler> Events { get; private set; }
        [field: Header("Input")]
        [field: SerializeField]
        public KeyCodeAsset SelectKey { get; private set; }
        [field: SerializeField]
        public KeyCodeAsset UpKey { get; private set; }
        [field: SerializeField]
        public KeyCodeAsset DownKey { get; private set; }

        private bool _enabled;
        private int _index;

        public int Index
            => _index;

        public SelectionEventHandler Current
            => Events[Index];

        public void ChangeTo(SelectionEventHandler handler)
        {
            if (Events.TryIndexOf(handler, out int index))
            {
                ChangeIndex(index);
            }
        }

        public void IncreaseIndex()
        {
            var nindex = FindNextIndex(_index);
            ChangeTo(nindex);
        }

        public void DecreaseIndex()
        {
            var pindex = FindPrevIndex(_index);
            ChangeTo(pindex);
        }

        private int FindNextIndex(int index)
        {
            for (int t = 0; t < Events.Count; ++t)
            {
                int i = (index + t + 1) % Events.Count;
                if (Events[i].enabled)
                {
                    return i;
                }
            }
            return index;
        }

        private int FindPrevIndex(int index)
        {
            for (int t = 0; t < Events.Count; ++t)
            {
                int i = (index - t - 1 + Events.Count) % Events.Count;
                if (Events[i].enabled)
                {
                    return i;
                }
            }
            return index;
        }

        private void ChangeTo(int index)
        {
            if (_index != index)
            {
                ChangeIndex(index);
            }
        }

        private void ChangeIndex(int index)
        {
            Current.SetHighlighted(false);

            _index = index;

            Current.SetHighlighted(true);
        }

        public void SelectCurrent()
        {
            Current.Select();
        }

        public void Refresh()
        {
            _index = FindNextIndex(_index - 1);
            for (int i = 0; i < Events.Count; ++i)
            {
                Events[i].SetHighlighted(_index == i);
            }
        }

        private void LateOnEnable()
        {
            Refresh();
        }

        private void CheckLateOnEnabled()
        {
            if (_enabled)
            {
                LateOnEnable();
                _enabled = false;
            }
        }

        #region Unity methods
        private void OnEnable()
        {
            _enabled = true;
        }

        private void Start()
        {
            Refresh();
        }

        private void Update()
        {
            CheckLateOnEnabled();

            if (Input.GetKeyDown(UpKey))
            {
                DecreaseIndex();
            }

            if (Input.GetKeyDown(DownKey))
            {
                IncreaseIndex();
            }

            if (Input.GetKeyDown(SelectKey))
            {
                SelectCurrent();
            }
        }
        #endregion
    }
}
