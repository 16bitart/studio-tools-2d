using Libraries.Ecosystem;
using UnityEngine;

[CreateAssetMenu(menuName = "Ecosystem/Plant Data")]
public class PlantData : DataObject
{
    [field: SerializeField] public BiomeTypes Biome { get; private set; }
    [field: SerializeField] public string PlantName { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    
    [field: SerializeField] public float GrowthRate { get; private set; } = 1f;
    [field: SerializeField] public float SeedRate { get; private set; } = 1f;
    [field: SerializeField] public float SeedDistance { get; private set; } = 1f;
    [field: SerializeField] public float HungerValue { get; private set; } = 10f;
    [field: SerializeField] public bool IsPoisonous { get; private set; }
    public override ObjectType DataType { get; } = ObjectType.Plant;
    
    [field: SerializeField] public Sprite GrowingSprite { get; private set; }
    [field: SerializeField] public Sprite GrownSprite { get; private set; }
}