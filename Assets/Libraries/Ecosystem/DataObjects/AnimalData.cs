using UnityEngine;

namespace Libraries.Ecosystem
{
    [CreateAssetMenu(menuName = "Ecosystem/Animal Data")]

    public class AnimalData : DataObject
    {
        [field: SerializeField] public BiomeTypes HabitatBiome { get; private set; }
        [field: SerializeField] public AnimalDietType Diet { get; private set; }
        [field: SerializeField] public GameObject SpritePrefab { get; private set; }
        [field: SerializeField] public string AnimalName { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public override ObjectType DataType { get; } = ObjectType.Animal;
    }
}