using System.Collections.Generic;
using UnityEngine;

namespace PrefabricatorUtility.Runtime
{
    public class PrefabricatedObject : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        [field: SerializeField] public Bounds Bounds { get; protected set; }
        public bool DrawBoundsGizmo = false;
        [field: SerializeField] public PrefabricatedObject Parent { get; protected set; }
        [field: SerializeField] public List<PrefabricatedObject> Children { get; protected set; } = new List<PrefabricatedObject>();

        protected SpriteRenderer spriteRenderer
        {
            get { return _spriteRenderer ??= GetComponent<SpriteRenderer>(); }
        }

        [ContextMenu("Evaluate Bounds")]
        protected void EvaluateBounds()
        {
            Bounds = gameObject.GetMaxBounds();
            foreach (var child in Children)
            {
                child.EvaluateBounds();
            }
        }



        public void SetParent(GameObject parent)
        {
            if (parent.TryGetComponent(out PrefabricatedObject prefabObj)) Parent = prefabObj;
            transform.SetParent(parent.transform);
        }

        public void SetChild(GameObject child)
        {
            if (child.TryGetComponent(out PrefabricatedObject prefabObj)) Children.Add(prefabObj);
            child.transform.SetParent(transform);
            EvaluateBounds();
        }

        public void SetupChildren(PrefabricatedObject parent, int parentIndex)
        {
            Parent = parent;
            SetupChildren(parentIndex);
            EvaluateBounds();
        }

        private void SetupChildren(int lastIndex)
        {
            Children ??= new List<PrefabricatedObject>();
            Children.Clear();
            foreach (var iteration in gameObject.GetChildren())
            {
                var child = iteration.child.gameObject;
                var childIndex = iteration.childIndex;
                child.name = $"{gameObject.name} [Layer {lastIndex}:{childIndex}]";
                var prefabObj = child.AssertComponent<PrefabricatedObject>();
                prefabObj.SetupChildren(childIndex);
                Children.Add(prefabObj);
            }
        }
    
 
        private void OnDrawGizmosSelected()
        {
            if (DrawBoundsGizmo)
            {
                EvaluateBounds();
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(Bounds.center, Bounds.size);
            }
        }
    }
}