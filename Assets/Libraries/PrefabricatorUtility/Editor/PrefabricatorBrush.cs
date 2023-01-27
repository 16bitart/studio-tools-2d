using System.Collections.Generic;
using PrefabricatorUtility.Runtime;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PrefabricatorUtility.Editor
{
    [CustomGridBrush(true, false, false, "Prefabricator Brush")]
    public class PrefabricatorBrush : GridBrush
    {
        private Prefabricator _prefabricator => Prefabricator.Instance;
        private List<TileChangeData> _tileChanges;
    
        public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            base.Paint(gridLayout, brushTarget, position);
        
            var min = position - pivot;
            var bounds = new BoundsInt(min, size);
            var listSize = bounds.size.x * bounds.size.y * bounds.size.z;
            _tileChanges = new List<TileChangeData>(listSize);
            TileBase lastTile = null;
        
            foreach (Vector3Int location in bounds.allPositionsWithin)
            {
                var local = location - bounds.min;
                var cell = cells[GetCellIndexWrapAround(local.x, local.y, local.z)];
                if (cell.tile == null) continue;
                lastTile = cell.tile;
            }

            if (lastTile != null)
            {
                _prefabricator.SetLastTilePainted(lastTile);
            }
        
        }
    }
}