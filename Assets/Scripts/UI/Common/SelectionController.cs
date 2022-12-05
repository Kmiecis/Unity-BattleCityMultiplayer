using Common.Extensions;
using Common.Mathematics;
using Common.MVB;
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
        public ScriptableKeyCode SelectKey { get; private set; }
        [field: SerializeField]
        public ScriptableKeyCode UpKey { get; private set; }
        [field: SerializeField]
        public ScriptableKeyCode DownKey { get; private set; }

        private bool _enabled;
        private int _index;

        public int Index
        {
            get => _index;
        }

        public SelectionEventHandler Current
        {
            get => Events[Index];
        }

        public void ChangeTo(SelectionEventHandler handler)
        {
            if (Events.TryIndexOf(handler, out int index))
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

        public void TryChangeIndex(int index)
        {
            var sanitized = (index + Events.Count) % Events.Count;

            if (_index != sanitized)
            {
                ChangeIndex(sanitized);
            }
        }

        public void IncreaseIndex()
        {
            var cindex = _index;
            var nindex = Mathx.NextIndex(cindex, Events.Count);

            while (!Events[nindex].enabled && nindex != cindex)
            {
                nindex = Mathx.NextIndex(nindex, Events.Count);
            }

            TryChangeIndex(nindex);
        }

        public void DecreaseIndex()
        {
            var cindex = _index;
            var nindex = Mathx.PrevIndex(cindex, Events.Count);

            while (!Events[nindex].enabled && nindex != cindex)
            {
                nindex = Mathx.PrevIndex(nindex, Events.Count);
            }

            TryChangeIndex(nindex);
        }

        public void SelectCurrent()
        {
            Current.Select();
        }

        public void Refresh()
        {
            var cindex = Mathx.PrevIndex(index, Events.Count);
            var nindex = index;
            while (!Events[nindex].enabled && nindex != cindex)
            {
                nindex = Mathx.NextIndex(nindex, Events.Count);
            }

            _index = nindex;
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

        private void OnEnable()
        {
            _enabled = true;
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
    }
}
