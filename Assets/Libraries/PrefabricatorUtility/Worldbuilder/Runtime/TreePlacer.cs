using System.Collections.Generic;
using UnityEngine;

public class TreePlacer : ObjectPlacer
{
    [SerializeField] private Worldbuilder _worldbuilder;
    [SerializeField] private DynamicObjectData _treeData;
    [SerializeField] private int _minTreesPlaced = 50;
    [SerializeField] private int _maxTreesPlaced = 300;
    [SerializeField] private float _minTreeRadius = 5f;

    
    [SerializeField] private bool _clusterTrees = true;
    [SerializeField] private float _clusterTreesRadius = 3f;
    [SerializeField] private int _minTreeClusterSize = 2;
    [SerializeField] private int _maxTreeClusterSize = 20;

    [SerializeField] private GameObject _treesParent;
    [SerializeField] private List<GameObject> _trees;
    
    
    public void GenerateTrees()
    {
        _worldSize = new Vector2Int((int)_worldbuilder.WorldSize.x, (int)_worldbuilder.WorldSize.y);
        Reset();

        var closedTiles = _worldbuilder.GetClosedPositions();
        
        var treePositions = DetermineObjectPositions(_treeData,
            ref closedTiles, 
            new Vector2Int((int)_worldbuilder.WorldSize.x, (int)_worldbuilder.WorldSize.y),
            _minTreeRadius,
            _maxTreesPlaced, 
            _minTreesPlaced);

        if (_clusterTrees)
        {
            var startPositions = new HashSet<Vector3Int>();
            startPositions.UnionWith(treePositions);
            
            foreach (var startPos in startPositions)
            {
                var currentPos = startPos;
                if (Random.value < 0.25f) continue;
                var clusterCount = 0;
                for (int i = 0; i < _maxTreeClusterSize; i++)
                {
                    var randomValue = Random.value;
                    foreach (var direction in DirectionUtility.AllDirections)
                    {
                        if (Random.value > randomValue)
                        {
                            randomValue = Random.value;
                            var distance = Random.Range(1, (int) _clusterTreesRadius);
                            var finalPos = currentPos + (Vector3Int) direction * distance;
                            
                            if (closedTiles.Contains(finalPos)) continue;
                            if (!Utils.IsInsideGrid(new Vector2(finalPos.x, finalPos.y), _worldSize)) continue;
                            
                            treePositions.Add(finalPos);
                            closedTiles.Add(finalPos);
                            currentPos = finalPos;
                        }
                    }
                }
            }
        }
        
        _treesParent = new GameObject("Tree Collection");
        _trees.AddRange(CreateObjectsAtPosition(_treeData, treePositions, _treesParent.transform));
    }

    public void Reset()
    {
        if (_treesParent != null) DestroyImmediate(_treesParent);
        if(_trees != null) _trees.Clear();
        if (_trees == null) _trees = new List<GameObject>();
    }
}