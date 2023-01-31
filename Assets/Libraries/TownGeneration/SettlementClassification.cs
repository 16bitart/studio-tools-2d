using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Settlements/New Settlement Classification", fileName = "SettlementClassification", order = 0)]
public class SettlementClassification : ScriptableObject
{
    [field: SerializeField] public SettlementClassificationValues Classification { get; private set; }
    [field: SerializeField] public BuildingRequirements RequiredBuildings { get; private set; }
    
    
    [Serializable]
    public class BuildingRequirements
    {
        [field: SerializeField] public int MinimumResidential { get; private set; }
        [field: SerializeField] public int MinimumResidence {get; private set;}
        [field: SerializeField] public int MinimumCommercial {get; private set;}
        [field: SerializeField] public int MinimumPolitical {get; private set;}
        [field: SerializeField] public int MinimumMilitary {get; private set;}
        [field: SerializeField] public int MinimumMedical {get; private set;}
        [field: SerializeField] public int MinimumIndustrial {get; private set;}
        [field: SerializeField] public int MinimumAgricultural {get; private set;}
        [field: SerializeField] public int MinimumReligious {get; private set;}
    }
}

