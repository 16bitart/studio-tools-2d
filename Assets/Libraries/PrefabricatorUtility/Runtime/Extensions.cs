using System.Collections.Generic;
using UnityEngine;

namespace PrefabricatorUtility.Runtime
{
    public static class Extensions
    {
        public static bool TryGetPrefabObject(this GameObject gameObject, out PrefabricatedObject prefabObject)
        {
            return gameObject.TryGetComponent(out prefabObject);
        }
    
        public static bool TryGetComponent<T>(this GameObject gameObject, out T obj) where T : Component
        {
            obj = gameObject.GetComponent<T>();
            return obj != null;
        }

        public static T AssertComponent<T>(this GameObject gameObject) where T : Component
        {
            var target = gameObject.GetComponent<T>();
            if (target == null)
            {
                target = gameObject.AddComponent<T>();
            }
            return target;
        }

        public static IEnumerable<(int childIndex, Transform child)> GetChildren(this GameObject gameObject)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                yield return (i, gameObject.transform.GetChild(i));
            }
        }
    
        public static Bounds GetMaxBounds(this GameObject g)
        {
            var renderers = g.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) return new Bounds(g.transform.position, Vector3.zero);
            var b = renderers[0].bounds;
            foreach (var r in renderers) b.Encapsulate(r.bounds);
            return b;
        }
    }
}