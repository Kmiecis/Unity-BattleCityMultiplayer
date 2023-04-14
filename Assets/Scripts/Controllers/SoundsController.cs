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

        private List<Sample> _sounds = new List<Sample>();
        private AudioSource _music;

        public float Volume
        {
            get => _volume;
            set { _volume = value; UpdateVolume(); }
        }

        public AudioSource PlaySound(SoundData data, Action onFinish = null)
        {
            if (!enabled)
                return null;

            var source = _sources.Borrow();

            source.clip = data.clip;
            source.volume = data.volume * _volume;
            source.loop = false;
            source.Play();

            var sample = new Sample { source = source, data = data, onFinish = onFinish };
            _sounds.Add(sample);

            return source;
        }

        public void StopSound(AudioSource source)
        {
            source.Stop();

            _sources.Return(source);
        }

        public void PlayMusic(SoundData data)
        {
            if (!enabled)
                return;

            StopMusic();

            var source = _sources.Borrow();

            source.clip = data.clip;
            source.volume = data.volume * _volume;
            source.loop = true;
            source.Play();

            _music = source;
        }

        public void StopMusic()
        {
            if (_music == null)
                return;

            _music.Stop();

            _sources.Return(_music);

            _music = null;
        }

        public bool HasMusic()
        {
            return _music != null;
        }

        private void UpdateSounds()
        {
            for (int i = 0; i < _sounds.Count; ++i)
            {
                var sample = _sounds[i];

                if (!sample.source.isPlaying)
                {
                    _sounds.RemoveAt(i);
                    i -= 1;

                    _sources.Return(sample.source);

                    if (sample.onFinish != null)
                    {
                        sample.onFinish();
                    }
                }
            }
        }
        
        private void UpdateVolume()
        {
            foreach (var sample in _sounds)
            {
                sample.source.volume = sample.data.volume * _volume;
            }
        }

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Update()
        {
            UpdateSounds();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateVolume();
        }
#endif

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
