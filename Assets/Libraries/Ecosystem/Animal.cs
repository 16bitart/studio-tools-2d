using System;
using UnityEngine;

namespace Libraries.Ecosystem
{
    
   
    
    public class Animal : MonoBehaviour
    {
        [SerializeField] private AnimalData _data;
        [SerializeField] private EntityNeeds _needs;
        [SerializeField] private Sex _sex = Sex.Female;
        
        private PlantNode _targetFood;

        private void OnTriggerEnter2D(Collider2D col)
        {
            var plant = col.gameObject.GetComponent<PlantNode>();
            if (plant != null)
            {
                _targetFood = plant;
                if(_needs.Hunger.IsHigh) return;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var plant = other.gameObject.GetComponent<PlantNode>();
            if (_targetFood != null && _targetFood.Equals(plant))
            {
                _targetFood = null;
            }
        }

        public enum Sex
        {
            Male,
            Female
        }
    }
}