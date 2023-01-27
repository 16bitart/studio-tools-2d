using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace PrefabricatorUtility.Editor
{
    [CustomEditor(typeof(PrefabricatorBrush))]
    public class PrefabricatorBrushEditor : GridBrushEditor
    {
        private PrefabricatorBrush prefabBrush { get { return target as PrefabricatorBrush; } }

        public override void OnPaintSceneGUI(
            GridLayout grid, 
            GameObject brushTarget, 
            BoundsInt position,
            GridBrushBase.Tool tool,
            bool executing)
        {
            base.OnPaintSceneGUI(grid, brushTarget, position, tool, executing);
        
            var labelText = "Pos: " + position.position;
            if (position.size.x > 1 || position.size.y > 1) {
                labelText += " Size: " + position.size;
            }

            Handles.Label(grid.CellToWorld(position.position), labelText);
        
        }
    }
}