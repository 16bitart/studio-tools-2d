using System.Collections.Generic;
using UnityEngine;

public class Settlement : MonoBehaviour
{
    [field: SerializeField] public SettlementClassification ClassificationInfo { get; private set; }
    [field: SerializeField] public Bounds Boundary { get; private set; }
    [field: SerializeField] public List<Building> Buildings { get; private set; }

    public void EnsureValidSettlement()
    {
        
    }
}

public class SettlementManager : MonoBehaviour
{
    [SerializeField] private Settlement[] _settlements;
}