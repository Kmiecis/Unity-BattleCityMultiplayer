using Common.Extensions;
using Common.Injection;
using Common.Pooling;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tanks
{
    [DI_Install]
    public class SoundsController : MonoBehaviour
    {
        private class Sample
        {
            public AudioSource source;
            public SoundData data;
            public Action onFinish;
            public float muter;
        }

        [SerializeField]
        private ComponentPool<AudioSource> _sources = new ComponentPool<AudioSource>();
        [Range(0.0f, 1.0f), SerializeField]
        private float _volume = 1.0f;

        private List<Sample> _playing = new List<Sample>();
        private bool _muted = false;

        public float Volume
        {
            get => _volume;
            set { _volume = value; ApplyVolume(); }
        }

        public AudioSource PlaySound(SoundData data, Action onFinish = null)
        {
            var source = _sources.Borrow();
            
            source.clip = data.clip;
            source.volume = (data.volume + Random.Range(-data.volumeOffset, +data.volumeOffset)) * _volume;
            source.pitch = (data.pitch + Random.Range(-data.pitchOffset, +data.pitchOffset));
            source.loop = data.loop;
            source.mute = _muted;
            source.Play();

            var sample = new Sample { source = source, data = data, onFinish = onFinish };
            _playing.Add(sample);

            return source;
        }

        public void StopSound(SoundData data)
        {
            if (_playing.TryFindIndex(s => Equals(s.data, data), out int i))
            {
                var source = _playing[i].source;
                
                if (source.isPlaying)
                {
                    source.Stop();

                    _playing.RemoveAt(i);

                    _sources.Return(source);
                }
            }
        }

        public void StopSound(AudioSource source)
        {
            if (source.isPlaying && _playing.TryFindIndex(s => Equals(s.source, source), out int i))
            {
                source.Stop();

                _playing.RemoveAt(i);

                _sources.Return(source);
            }
        }

        public void SetVolume(float value)
        {
            foreach (var sample in _playing)
            {
                sample.source.volume = sample.data.volume * value;
            }
        }

        public void SetMuted(bool muted)
        {
            foreach (var sample in _playing)
            {
                sample.source.mute = muted;
            }
        }

        public void MuteIncoming()
        {
            _muted = true;
        }

        public void UnmuteIncoming()
        {
            _muted = false;

            SetMuted(_muted);
        }

        public void Clear()
        {
            foreach (var sample in _playing)
            {
                var source = sample.source;

                if (source != null)
                {
                    source.Stop();

                    _sources.Return(source);
                }
            }
            _playing.Clear();
        }

        public void SaveVolume()
        {
            CustomPlayerPrefs.SetVolume(_volume);
        }

        public void LoadVolume()
        {
            _volume = CustomPlayerPrefs.GetVolume();
        }

        private void ApplyVolume()
        {
            SetVolume(_volume);
        }

        private void UpdateSamples()
        {
            for (int i = 0; i < _playing.Count; ++i)
            {
                var sample = _playing[i];

                if (!sample.source.isPlaying)
                {
                    _playing.RemoveAt(i);
                    i -= 1;

                    _sources.Return(sample.source);

                    if (sample.onFinish != null)
                    {
                        sample.onFinish();
                    }
                }
            }
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);

            LoadVolume();
            ApplyVolume();
        }

        private void Update()
        {
            UpdateSamples();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            ApplyVolume();
        }
#endif

        private void OnDestroy()
        {
            Clear();

            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
