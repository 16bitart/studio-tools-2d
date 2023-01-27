using UnityEngine;

[CreateAssetMenu(menuName = "Worldbuilding/Dynamic Object Data")]
public class DynamicObjectData : ScriptableObject
{

    public enum ObjectType
    {
        Sprite,
        Prefab
    }

    [field: SerializeField] public ObjectType Type { get; private set; } = ObjectType.Sprite; 
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public Vector2Int ObjectSize { get; private set; }
    [field: SerializeField] public string PlacementName { get; private set; }

    private void OnValidate()
    {
        if (Type == ObjectType.Prefab)
        {
            ObjectSize = Prefab.GetComponent<DynamicObject>().ObjectSize;
        }

        else
        {
            ObjectSize = GetUnitsOfSpaceNeeded(Sprite);
        }

    }

    public Vector2Int GetUnitsOfSpaceNeeded(Sprite sprite)
    {
        if(sprite == null) return Vector2Int.zero;
        var size = sprite.bounds.extents * 2;
        return new Vector2Int(Mathf.CeilToInt(size.x), Mathf.CeilToInt(size.y));
    }
}