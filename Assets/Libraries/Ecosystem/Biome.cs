using Libraries.Ecosystem;
using UnityEngine;


public class Biome
{
    [SerializeField] private BiomeData _data;
}

public class Plant
{
    [SerializeField] private PlantData _data;
}

public class PlantData : ScriptableObject
{
    [field: SerializeField] public BiomeTypes Biome { get; private set; }
    [field: SerializeField] public string PlantName { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    
    [field: SerializeField] public float GrowthRate { get; private set; } = 1f;
    [field: SerializeField] public float SeedRate { get; private set; } = 1f;
    [field: SerializeField] public float SeedDistance { get; private set; } = 1f;
    [field: SerializeField] public bool IsPoisonous { get; private set; }
}