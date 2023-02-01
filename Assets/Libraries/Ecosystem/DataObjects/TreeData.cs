using UnityEngine;

public class TreeData : DataObject
{
    [field: SerializeField] public override ObjectType DataType { get; } = ObjectType.Tree;
}