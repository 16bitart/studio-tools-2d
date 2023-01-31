using UnityEngine;

namespace PrefabricatorUtility.Runtime
{
    public class PrefabricationRootObject : PrefabricatedObject
    {
        private bool _initialized;
        
        [field: SerializeField] public Vector3 CenterPoint { get; private set; }
        [field: SerializeField] public Vector3 BottomPivot { get; private set; }
        [field: SerializeField] public Vector3 RightLimit { get; private set; }
        [field: SerializeField] public Vector3 LeftLimit { get; private set; }
        [field: SerializeField] public Vector3 BottomLimit { get; private set; }

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
            CenterPoint = Bounds.center;
            BottomPivot = new Vector3(CenterPoint.x, CenterPoint.y - Bounds.extents.y, CenterPoint.z);
            RightLimit = new Vector3(CenterPoint.x + Bounds.extents.x, BottomPivot.y, CenterPoint.z);
            LeftLimit = new Vector3(CenterPoint.x - Bounds.extents.x, BottomPivot.y, CenterPoint.z);
            BottomLimit = new Vector3(BottomPivot.x, BottomPivot.y - 1, BottomPivot.z);
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            GizmoTool.DrawLineWithCubeMarkers(CenterPoint, BottomPivot, .2f, Color.cyan, Color.blue, Color.blue);
            GizmoTool.DrawLineWithCubeMarkers(BottomPivot, RightLimit, .1f, Color.cyan, Color.cyan, Color.red);
            GizmoTool.DrawLineWithCubeMarkers(BottomPivot, LeftLimit, .1f, Color.cyan, Color.cyan, Color.red);
            GizmoTool.DrawLineWithCubeMarkers(BottomPivot, BottomLimit, .1f, Color.cyan, Color.cyan, Color.red);
        }
        
        
    }

    public static class GizmoTool
    {
        public static void DrawLine(Vector3 from, Vector3 to, Color color = default)
        {
            if (color != default) Gizmos.color = color;
            else Gizmos.color = Color.cyan;
            Gizmos.DrawLine(from, to);
        }

        public static void DrawCubeMarker(Vector3 pos, float boxSize = .2f, Color color = default)
        {
            if (color != default) Gizmos.color = color;
            else Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(pos, Vector3.one * boxSize);
        }

        public static void DrawLineWithCubeMarkers(Vector3 from, Vector3 to, float markSizes, Color lineColor, Color fromColor, Color toColor)
        {
            GizmoTool.DrawCubeMarker(from, markSizes, fromColor);
            GizmoTool.DrawLine(from, to, lineColor);
            GizmoTool.DrawCubeMarker(to, markSizes, toColor);
        }
    }
}