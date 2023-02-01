using UnityEngine;

namespace Libraries.Ecosystem
{
    [CreateAssetMenu(menuName = "Ecosystem/Biome Data")]

    public class BiomeData : DataObject
    {
        [field: SerializeField] public string BiomeName { get; private set; }
        [field: SerializeField] public BiomeTypes BiomeType { get; private set; }
        [field: SerializeField] public AnimalData[] Animals { get; private set; }
        [field: SerializeField] public float TreeRegrowthRate { get; private set; } = 1f;
        [field: SerializeField] public float PlantRegrowthRate { get; private set; } = 1f;
        [field: SerializeField] public override ObjectType DataType { get; } = ObjectType.Biome;
    }

    public enum BiomeTypes
    {
        Plains,
        Swamp,
        Wilderness,
        Desert
    }
}