using PrefabricatorUtility.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PrefabricatorUtility.Editor
{
    public class PrefabricatorWindow : EditorWindow
    {
        private StyleSheet _stylesheet;
        private VisualElement _root;
        private Prefabricator _prefabricator;
    
        [MenuItem("Window/Prefabricator/Add Prefabricator To Scene")]
        public static void ShowExample()
        {

        }


        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;
            _root = root;
        
            // VisualElements objects can contain other VisualElement following a tree hierarchy.
            //VisualElement label = new Label("Hello World! From C#");
            //root.Add(label);

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/PrefabricatorUtility/Editor/PrefabricatorWindow.uxml");
            VisualElement uxml = visualTree.Instantiate();
            root.Add(uxml);

        
            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            _stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/PrefabricatorUtility/Editor/PrefabricatorWindow.uss");
        }
    }
}