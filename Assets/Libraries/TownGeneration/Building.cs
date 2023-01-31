using System;
using System.Collections;
using UnityEngine;

public class Building : MonoBehaviour
{
    [field: SerializeField] public BuildingClassification Classification { get; private set; } 
    [field: SerializeField] public Bounds BuildingBounds { get; private set; }
    [field: SerializeField] public Bounds PlotBounds { get; private set; }
    public Vector3 PlotSize => PlotBounds.size;
}