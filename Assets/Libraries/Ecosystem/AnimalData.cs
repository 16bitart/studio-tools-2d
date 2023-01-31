using UnityEngine;

namespace Libraries.Ecosystem
{
    public class AnimalData : ScriptableObject
    {
        [field: SerializeField] public BiomeTypes HabitatBiome { get; private set; }
        [field: SerializeField] public AnimalDietType Diet { get; private set; }
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public string AnimalName { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
    }
}