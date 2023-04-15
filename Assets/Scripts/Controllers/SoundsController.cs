using Common.Extensions;
using Common.Injection;
using Common.Pooling;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        }

        [SerializeField]
        private ComponentPool<AudioSource> _sources = new ComponentPool<AudioSource>();
        [Range(0.0f, 1.0f), SerializeField]
        private float _volume = 1.0f;

        private float _override = 1.0f;
        private List<Sample> _samples = new List<Sample>();

        public float Volume
        {
            get => _volume;
            set { _volume = value; RestoreVolume(); }
        }

        public AudioSource PlaySound(SoundData data, Action onFinish = null)
        {
            var source = _sources.Borrow();
            
            source.clip = data.clip;
            source.volume = data.volume * _volume * _override;
            source.loop = data.loop;
            source.Play();

            var sample = new Sample { source = source, data = data, onFinish = onFinish };
            _samples.Add(sample);

            return source;
        }

        public void StopSound(SoundData data)
        {
            if (_samples.TryFindIndex(s => Equals(s.data, data), out int i))
            {
                var source = _samples[i].source;
                
                if (source.isPlaying)
                {
                    source.Stop();

                    _samples.RemoveAt(i);

                    _sources.Return(source);
                }
            }
        }

        public void StopSound(AudioSource source)
        {
            if (source.isPlaying && _samples.TryFindIndex(s => Equals(s.source, source), out int i))
            {
                source.Stop();

                _samples.RemoveAt(i);

                _sources.Return(source);
            }
        }

        public void SetVolume(float value)
        {
            foreach (var sample in _samples)
            {
                sample.source.volume = sample.data.volume * _override * value;
            }
        }

        public void MuteVolume()
        {
            SetVolume(0.0f);
        }

        public void RestoreVolume()
        {
            SetVolume(_volume);
        }

        public void MuteIncoming()
        {
            _override = 0.0f;
        }

        public void UnmuteIncoming()
        {
            _override = 1.0f;

            RestoreVolume();
        }

        public void Clear()
        {
            foreach (var sample in _samples)
            {
                var source = sample.source;

                if (source != null)
                {
                    source.Stop();

                    _sources.Return(source);
                }
            }
            _samples.Clear();
        }

        private void UpdateSamples()
        {
            for (int i = 0; i < _samples.Count; ++i)
            {
                var sample = _samples[i];

                if (!sample.source.isPlaying)
                {
                    _samples.RemoveAt(i);
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
        }

        private void Update()
        {
            UpdateSamples();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            RestoreVolume();
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
