using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Worldbuilding/Dynamic Object Data")]
public class DynamicObjectData : ScriptableObject
{
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public DynamicObject DynamicObject { get; private set; }
    [field: SerializeField] public string PlacementName { get; private set; }
    [field: SerializeField] public Vector2 ObjectSize { get; private set; }

    private void OnValidate()
    {
        if (Prefab != null)
        {
            DynamicObject = Prefab.GetComponent<DynamicObject>();
            if (DynamicObject != null)
            {
                ObjectSize = DynamicObject.RenderSize;
            }
        }
    }
}