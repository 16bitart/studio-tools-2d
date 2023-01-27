using PrefabricatorUtility.Runtime;
using UnityEditor;

namespace PrefabricatorUtility.Editor
{
    [CustomEditor(typeof(Prefabricator))]
    public class PrefabricatorEditor : UnityEditor.Editor
    {
        private Prefabricator _prefabricator;
    
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _prefabricator = target as Prefabricator;
        }
    }
}