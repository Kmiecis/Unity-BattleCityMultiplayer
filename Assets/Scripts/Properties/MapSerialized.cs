using UnityEngine;

namespace Tanks
{
    [CreateAssetMenu(menuName = nameof(Tanks) + "/" + nameof(MapSerialized), fileName = nameof(MapSerialized))]
    public class MapSerialized : ScriptableObject
    {
        public string value;
    }
}
