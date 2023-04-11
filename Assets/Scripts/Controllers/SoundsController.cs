using Common.Pooling;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    public class SoundsController : MonoBehaviour
    {
        public ComponentPool<AudioSource> sources = new ComponentPool<AudioSource>();

        private List<AudioSource> _sounds;
        private AudioSource _music;

        public AudioSource PlaySound(SoundData sound)
        {
            var source = sources.Borrow();

            source.clip = sound.clip;
            source.volume = sound.volume;
            source.loop = false;
            source.Play();

            _sounds.Add(source);

            return source;
        }

        public void StopSound(AudioSource source)
        {
            source.Stop();

            sources.Return(source);
        }

        public void PlayMusic(SoundData sound)
        {
            var source = sources.Borrow();

            source.clip = sound.clip;
            source.volume = sound.volume;
            source.loop = true;
            source.Play();

            _music = source;
        }

        public void StopMusic()
        {
            if (_music == null)
                return;

            _music.Stop();

            sources.Return(_music);

            _music = null;
        }

        private void UpdateSounds()
        {
            for (int i = 0; i < _sounds.Count; ++i)
            {
                var source = _sounds[i];

                if (!source.isPlaying)
                {
                    _sounds.RemoveAt(i);
                    i -= 1;

                    sources.Return(source);
                }
            }
        }

        #region Unity methods
        private void Start()
        {
            _sounds = new List<AudioSource>();
        }

        private void Update()
        {
            UpdateSounds();
        }
        #endregion
    }
}
