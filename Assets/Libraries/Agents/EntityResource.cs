using System;
using UnityEngine;

[Serializable] public class Health : EntityResource { }
[Serializable] public class Stamina : EntityResource { }
[Serializable] public class Hunger : EntityResource { }
[Serializable] public class Thirst : EntityResource { }
[Serializable] public class Fatigue : EntityResource { }

[Serializable]
public class EntityResource
{
    public event Action<float, float> OnChange;
    public event Action<float> OnSubtract;
    public event Action<float> OnAdd;
    public event Action OnEmpty;

    [SerializeField] private float _current;
        
    public float Current
    {
        get => _current;
        private set
        {
            _current = value;
            OnChange?.Invoke(_current, Maximum);
            if(value <= 0) OnEmpty?.Invoke();
        } 
    }
        
    [field: SerializeField] public float Maximum { get; private set; }

    protected virtual bool Subtract(float val, out float remainder)
    {
        remainder = val;
        Current -= val;
        OnSubtract?.Invoke(val);
        return true;
    }

    protected virtual bool Add(float val, out float remainder)
    {
        remainder = val;
        if (CheckIfEmpty()) return false;
            
        var result = Current + val;
        remainder = ClampToMaximum(ref result);
        Current = result;
        OnAdd?.Invoke(val);
        return true;
    }

    public bool CheckIfEmpty()
    {
        return Current <= 0;
    }

    private float ClampToMaximum(ref float amount)
    {
        var remainder = amount - Maximum;
        if (remainder <= 0)
        {
            return 0f;
        }
        amount = Maximum;
        return remainder;
    }
}