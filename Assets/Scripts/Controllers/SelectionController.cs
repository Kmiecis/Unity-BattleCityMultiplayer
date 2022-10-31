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

        private void ChangeIndex(int index)
        {
            Events[_index].SetHighlighted(false);

            _index = index;

            Events[_index].SetHighlighted(true);
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
            TryChangeIndex(_index + 1);
        }

        public void DecreaseIndex()
        {
            TryChangeIndex(_index - 1);
        }

        public void SelectCurrent()
        {
            Current.Select();
        }

        public void Refresh()
        {
            _index = index;

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
