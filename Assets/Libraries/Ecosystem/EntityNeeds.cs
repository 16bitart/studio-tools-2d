using System;
using UnityEngine;

namespace Libraries.Ecosystem
{
    [Serializable]
    public class EntityNeeds
    {
        [field: SerializeField] public Hunger Hunger {get; private set; } 
        [field: SerializeField] public Thirst Thirst {get; private set; } 
        [field: SerializeField] public Fatigue Fatigue {get; private set; } 
    }
}