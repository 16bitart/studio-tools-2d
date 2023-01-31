using System;
using UnityEngine;

public class BuildingData : ScriptableObject
{
    [SerializeField] private GameObject[] _prefabs;
}

[Serializable]
public abstract class DataObject : ScriptableObject
{
    [field: SerializeField] public string UniqueIdentifier { get; private set; }
    [field: SerializeField] public ObjectType DataType { get; private set; }

    public enum ObjectType
    {
        Animal,
        Plant,
        Building
    }
}