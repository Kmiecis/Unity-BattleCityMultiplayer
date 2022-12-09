using UnityEngine;

namespace Tanks
{
    [CreateAssetMenu(menuName = nameof(Tanks) + "/" + nameof(ConstructionBlocks), fileName = nameof(ConstructionBlocks))]
    public class ConstructionBlocks : ScriptableObject
    {
        [field: SerializeField]
        public Block[] Blocks { get; private set; }

        public Block this[int index]
            => Blocks[index];

        public int Length
            => Blocks.Length;
    }
}
