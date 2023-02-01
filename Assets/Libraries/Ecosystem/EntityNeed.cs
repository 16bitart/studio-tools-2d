using UnityEngine;

namespace Libraries.Ecosystem
{
    public class EntityNeed : EntityResource
    {
        [field: SerializeField] public Threshhold Thresholds { get; private set; }
        
        public Threshhold.Level CurrentNeedLevel {
            get
            {
                if (IsHigh) return Threshhold.Level.High;
                if (IsModerate) return Threshhold.Level.Moderate;
                if (IsLow) return Threshhold.Level.Low;
                return Threshhold.Level.Critical;
            }
        }
        
        public bool IsHigh => Current > Thresholds.High;
        public bool IsModerate => Current > Thresholds.Moderate;
        public bool IsLow => Current > Thresholds.Low;
        public bool IsCritical => Current > Thresholds.Critical;
        
        public class Threshhold
        {

            public enum Level
            {
                High,
                Moderate,
                Low,
                Critical
            }
            
            [field: SerializeField] public float High { get; private set; } = .8f;
            [field: SerializeField] public float Moderate { get; private set; } = .6f;
            [field: SerializeField] public float Low { get; private set; } = .4f;
            [field: SerializeField] public float Critical { get; private set; } = .2f;
        }
    }
}