using UnityEngine;

namespace PrefabricatorUtility.Runtime
{
    public class PrefabricationRootObject : PrefabricatedObject
    {
        private bool _initialized;
    
    
        private void Awake()
        {
            Initialize();
        }
    
        [ContextMenu("Initialize")]
        public void Initialize()
        {
            if (!_initialized)
            {
                SetupChildren();
                _initialized = true;
            }
        }

        [ContextMenu("Setup Children")]
        public void SetupChildren()
        {
            Children.Clear();
            foreach (var iteration in gameObject.GetChildren())
            {
                var child = iteration.child.gameObject;
                var index = iteration.childIndex;
                var prefabObj = child.AssertComponent<PrefabricatedObject>();
                child.name = $"{gameObject.name} [Layer {index}]";
                prefabObj.SetupChildren(this, index);
                Children.Add(prefabObj);
            }

            EvaluateBounds();
        }

        [ContextMenu("Validate Max Bounds")]
        public void ValidateBounds()
        {
            EvaluateBounds();
        }


    }
}