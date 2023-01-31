using UnityEngine;


[CreateAssetMenu(menuName = "Settlements/New Building Classification", fileName = "BuildingClassification", order = 0)]
public class BuildingClassification : ScriptableObject
{
    [field: SerializeField] public BuildingClassificationValues Classification { get; private set; }
    [field: SerializeField] public GameObject[] Prefabs { get; private set; }
    [field: SerializeField] public Vector3 MaxPlotSize { get; private set; }
    [field: SerializeField] public Color ZoningColor { get; private set; }
}


