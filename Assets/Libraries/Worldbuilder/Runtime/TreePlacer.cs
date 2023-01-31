using System.Collections.Generic;
using UnityEngine;

public class TreePlacer : ObjectPlacer
{
    [SerializeField] private Worldbuilder _worldbuilder;
    [SerializeField] private DynamicObjectData _treeData;
    [SerializeField] private int _minTreesPlaced = 50;
    [SerializeField] private int _maxTreesPlaced = 300;
    [SerializeField] private float _minTreeRadius = 5f;

    public bool ClampTreesToGrid = true;
    
    [SerializeField] private bool _clusterTrees = true;
    [SerializeField] private float _clusterTreesRadius = 3f;
    [SerializeField, Range(0, 1f)] private float _clusterTreeSpawnRate = .25f;
    [SerializeField, Range(0, 1f)] private float _clusterChance = .25f;
    [SerializeField] private int _minTreeClusterSize = 2;
    [SerializeField] private int _maxTreeClusterSize = 20;
    [SerializeField] private bool _allowExtraTreesForClusters = true;
    [SerializeField, Range(0, 1f)] private float _percentOverage = .25f;
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
            var currentPos = Vector3Int.zero;
            var directions = DirectionUtility.AllDirections;
            var remainingTrees = _maxTreesPlaced - treePositions.Count;
            var maxTrees = _maxTreesPlaced;
            if (_allowExtraTreesForClusters) maxTrees = Mathf.RoundToInt(maxTrees *(1 + _percentOverage));
            Debug.Log($"Max Tree Limit is {_maxTreesPlaced}. Remaining: {remainingTrees}. Clustering enabled. Allowing for {maxTrees} trees.");
            foreach (var startPos in startPositions)
            {
                if (treePositions.Count >= maxTrees) break;
                if (Random.value > _clusterChance) continue;
                
                currentPos = startPos;
                var clusterCount = Random.Range(_minTreeClusterSize, _maxTreeClusterSize);
                
                for (var i = 0; i < clusterCount; i++)
                {
                    if (treePositions.Count >= maxTrees) break;
                    if (Random.value > _clusterTreeSpawnRate) continue;
                    
                    var distance = Mathf.RoundToInt(Random.Range(1, _clusterTreesRadius));
                    var direction = directions[Random.Range(0, directions.Length)];
                    var finalPos = currentPos + (Vector3Int) direction * distance;
                    if (closedTiles.Contains(finalPos)) continue;
                    if (!Utils.IsInsideGrid(new Vector2(finalPos.x, finalPos.y), _worldSize)) continue;
                    treePositions.Add(finalPos);
                    closedTiles.Add(finalPos);
                    currentPos = finalPos;
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