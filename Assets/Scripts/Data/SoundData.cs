using UnityEngine;

namespace Tanks
{
    [CreateAssetMenu(menuName = nameof(Tanks) + "/" + nameof(SoundData), fileName = nameof(SoundData))]
    public class SoundData : ScriptableObject
    {
        public AudioClip clip;
        public float volume = 1.0f;
        public float volumeOffset = 0.0f;
        public float pitch = 1.0f;
        public float pitchOffset = 0.0f;
        public bool loop;
    }
}
