using System;
using UnityEngine;

[Serializable]
public abstract class DataObject : ScriptableObject
{
    [field: SerializeField] public string UniqueIdentifier { get; private set; }
    public abstract ObjectType DataType { get; }

    public enum ObjectType
    {
        Animal,
        Plant,
        Tree,
        Mineral,
        Building,
        Biome,
        Undefined
    }
}