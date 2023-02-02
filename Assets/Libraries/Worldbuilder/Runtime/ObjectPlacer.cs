using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public abstract class ObjectPlacer : MonoBehaviour
{
    protected Vector2Int _worldSize => _worldbuilder != null ? _worldbuilder.WorldSize : Vector2Int.zero;
    
    [SerializeField] protected Worldbuilder _worldbuilder;
    [SerializeField] protected DynamicObjectData _data;
    [SerializeField] protected bool _generate;
    
    
    [SerializeField] protected int _minPlacements = 50;
    [SerializeField] protected int _maxPlacements = 300;
    [SerializeField] protected float _minDistanceBetween = 5f;

    [SerializeField] protected bool _allowClustering = false;
    [SerializeField] protected bool _allowClusteringToExceedMaxPlacements = false;
    [SerializeField, Range(0, 1f)] protected float _clusterChance = .25f;
    [SerializeField, Range(0, 1f)] protected float _clusterSpawnChance = .5f;
    [SerializeField, Range(0, 25f)] protected float _clusterRadius = 3f;
    [SerializeField] protected int _minClusterSize = 2;
    [SerializeField] protected int _maxClusterSize = 6;
    [SerializeField] protected int _maxClusters = 15;
    public abstract string CollectionGameObjectName { get; }
    [SerializeField] protected GameObject _parentGameObject;
    [SerializeField] protected List<GameObject> _placements;

    protected HashSet<Vector3Int> _closedTiles;

    public HashSet<Vector3Int> ClosedTiles
    {
        get
        {
            if (_closedTiles == null) _closedTiles = new HashSet<Vector3Int>();
            return _closedTiles;
        }
    }

    private void OnValidate()
    {
        _worldbuilder = GetComponent<Worldbuilder>();
    }

    public void GenerateObjects()
    {
        Reset();
        
        if(!_generate) return;
        
        var closedTiles = _worldbuilder.GetClosedPositions();
        
        var objectPositions = DetermineObjectPositions(_data,
            ref closedTiles, 
            _worldSize,
            _minDistanceBetween,
            _maxPlacements, 
            _minPlacements);
        
        Debug.Log($"Determined {objectPositions.Count} positions for [{_data.PlacementName}] placement.");
        
        if (_allowClustering)
        {
            var startPositions = new HashSet<Vector3Int>();
            startPositions.UnionWith(objectPositions);
            var currentPos = Vector3Int.zero;
            var directions = DirectionUtility.AllDirections;
            
            var maximumForClusters = _allowClusteringToExceedMaxPlacements 
                ? Mathf.RoundToInt(_maxPlacements * 1.2f) 
                :  _maxPlacements;


            var currentTime = Time.time;
            var maxTime = Time.time + 30f;
            Debug.Log($"Clustering enabled. Starting generation. Current time is {currentTime}.");

            var clustersGenerated = 0;
            
            foreach (var startPos in startPositions)
            {
                if (Time.time > maxTime)
                {
                    Debug.Log($"Exceeding max generation time of {maxTime}. Breaking.");
                    break;
                }
                if (objectPositions.Count >= maximumForClusters) break;
                if (Random.value > _clusterChance) continue;
                clustersGenerated++;
                currentPos = startPos;
                var clusterCount = Random.Range(_minClusterSize, _maxClusterSize);
                
                for (var i = 0; i < clusterCount; i++)
                {
                    if (objectPositions.Count >= maximumForClusters) break;
                    if (Random.value > _clusterSpawnChance) continue;
                    
                    var distance = Mathf.RoundToInt(Random.Range(1, _clusterRadius));
                    var direction = directions[Random.Range(0, directions.Length)];
                    var finalPos = currentPos + (Vector3Int) direction * distance;
                    if (closedTiles.Contains(finalPos)) continue;
                    if (!Utils.IsInsideGrid(new Vector2(finalPos.x, finalPos.y), _worldSize)) continue;
                    objectPositions.Add(finalPos);
                    closedTiles.Add(finalPos);
                    currentPos = finalPos;
                }
            }
            Debug.Log($"Generated {clustersGenerated} clusters.");
        }

        _parentGameObject = new GameObject(CollectionGameObjectName);
        _parentGameObject.transform.SetParent(transform);
        _placements.AddRange(CreateObjectsAtPosition(_data, objectPositions, _parentGameObject.transform));
        _closedTiles = objectPositions;

        foreach (var plant in _placements.Select(x => x.GetComponent<PlantNode>()))
        {
            if(plant != null) plant.Initialize();
        }
    }
    
    public void Reset()
    {
        if (_placements != null)
        {
            foreach (var placement in _placements)
            {
                DestroyImmediate(placement);
            }
            _placements.Clear();
        }
        else
            _placements = new List<GameObject>();
        
        if (_parentGameObject != null)
            DestroyImmediate(_parentGameObject);

        _closedTiles = new HashSet<Vector3Int>();
    }
    
    public HashSet<Vector3Int> DetermineObjectPositions(
        DynamicObjectData objectData,
        ref HashSet<Vector3Int> closedPositions,
        Vector2Int regionSize,
        float minDistance,
        int maxPlacements = 1000,
        int minPlacements = 1)
    {
        var objectPositions = new HashSet<Vector3Int>();
        var targetPlacements = Mathf.FloorToInt(Random.Range(minPlacements, maxPlacements));

        var maxTime = Time.time + 100f;
        while (objectPositions.Count < targetPlacements)
        {
            if (Time.time > maxTime)
            {
                Debug.Log($"Exceeding max placement position determination time of {maxTime}. Breaking.");
                break;
            }
            var positions = PoissonDisc.GeneratePoints(minDistance, regionSize)
                .Where(position => Utils.IsInsideGrid(new Vector2(position.x, position.y), _worldSize))
                .Select(position => new Vector3Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), 0));

            foreach (var objPos in positions)
            {
                if (closedPositions.Contains(objPos) || objectPositions.Contains(objPos)) continue;

                objectPositions.Add(objPos);
                
                if (objectData.ObjectSize != Vector2.zero)
                {
                    var yLength = objPos.y + objectData.ObjectSize.y;
                    var xLength = objPos.x + objectData.ObjectSize.x;
                
                    for (int y = objPos.y; y < yLength; y++)
                    {
                        for (int x = objPos.x; x < xLength; x++)
                        {
                            closedPositions.Add(new Vector3Int(x, y, 0));
                        }
                    }
                }
                
                if (objectPositions.Count >= targetPlacements) break;
            }
        }
        
        Debug.Log($"Determined {objectPositions.Count} positions for object placement. Target count was {targetPlacements}");
        return objectPositions;
    }
    
    protected List<GameObject> CreateObjectsAtPosition(DynamicObjectData objectData, HashSet<Vector3Int> objectPositions, Transform parent = null)
    {
        if (parent != null)
        {
            return objectPositions
                .Select(position => CreateObject(objectData, new Vector3(position.x + .5f, position.y + .5f, 0), parent))
                .ToList();
        }
        
        return objectPositions
            .Select(position => CreateObject(objectData, new Vector3(position.x + .5f, position.y + .5f, 0)))
            .ToList();
    }

    protected GameObject CreateObject(DynamicObjectData data, Vector3 position, Transform parent = null)
    {
        if (parent != null)
        {
            var obj = Instantiate(data.Prefab, parent);
            obj.transform.localPosition = position;
            return obj;
        }
        
        return Instantiate(data.Prefab, position, Quaternion.identity);
    }
}
