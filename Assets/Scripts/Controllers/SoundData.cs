using UnityEngine;

namespace Tanks
{
    [CreateAssetMenu(menuName = nameof(Tanks) + "/" + nameof(SoundData), fileName = nameof(SoundData))]
    public class SoundData : ScriptableObject
    {
        public AudioClip clip;
        public float volume = 1.0f;
    }
}
