using System;
using UnityEngine;

namespace Libraries.Ecosystem
{
    
   
    
    public class Animal : MonoBehaviour
    {
        [SerializeField] private AnimalData _data;
        [SerializeField] private Hunger _hunger;
        [SerializeField] private Thirst _thirst;
        [SerializeField] private Fatigue _fatigue;
    }
}