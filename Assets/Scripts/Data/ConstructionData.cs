using UnityEngine;

namespace Tanks
{
    [CreateAssetMenu(menuName = nameof(Tanks) + "/" + nameof(ConstructionData), fileName = nameof(ConstructionData))]
    public class ConstructionData : ScriptableObject
    {
        [field: SerializeField]
        public Block[] Blocks { get; private set; }

        public Block this[int index]
            => Blocks[index];

        public int Length
            => Blocks.Length;
    }
}
