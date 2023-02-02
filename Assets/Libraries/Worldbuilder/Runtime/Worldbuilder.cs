using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class Worldbuilder : MonoBehaviour
{
    public Vector2Int WorldSize => new Vector2Int(_width, _height);
    [SerializeField, Range(10, 500)] private int _width = 100;
    [SerializeField, Range(10, 500)] private int _height = 100;
    [SerializeField, Range(1, 25f)] private float _scale = 15f;
    [SerializeField] private Vector2 _worldOffset = Vector2.zero;
    [SerializeField] private bool _generateLakes = true;
    [SerializeField] private bool _generateRivers = true;
    
    [SerializeField, Range(0, 1f)] private float _groundThreshold = .3f;
    [SerializeField, Range(0, 1f)] private float _lakeThreshhold = .2f;
    [SerializeField, Range(0, 1f)] private float _riverThreshhold = .4f;
    
    [SerializeField, Range(0, 10)] private int _grassIterations = 1;
    [SerializeField, Range(0, 250f)] private float _grassDistanceBetweenPlacements = 5f;

    [SerializeField, Range(0, 10)] private int _grassExpansions = 1;
    [SerializeField, Range(0, 250f)] private float _grassExpansionRadius = 3f;

    [field: SerializeField] public TilemapBuilder GroundTilemap { get; private set; }
    [field: SerializeField] public TilemapBuilder WaterTilemap { get; private set; }
    [field: SerializeField] public TilemapBuilder GroundDecorationTilemap { get; private set; }
    [field: SerializeField] public TilemapBuilder WaterDecorationTilemap { get; private set; }

    [SerializeField] private TreePlacer _treePlacer;
    [SerializeField] private PlantPlacer _plantPlacer;
    [SerializeField] private MineralPlacer _mineralPlacer;

    private float[,] _heightMap;
    
    public void Initialize(Vector2Int size, float scale, Vector2 offset)
    {
        _height = size.x;
        _width = size.y;
        _scale = scale;
        _worldOffset = offset;
    }

    [ContextMenu("Run Generation")]
    public void Run()
    {
        var startTime = Time.time;
        Debug.Log("Beginning generation.");
        Reset();
        Debug.Log("Generating terrain.");
        GenerateTerrain();
        Debug.Log("Generating grass.");
        GenerateGrass();
        Debug.Log("Generating plants.");
        _plantPlacer.GenerateObjects();
        Debug.Log("Generating trees.");
        _treePlacer.GenerateObjects();
        Debug.Log("Generating minerals.");
        _mineralPlacer.GenerateObjects();
        var endTime = Time.time;
        Debug.Log($"Completed generation. Elapsed time: {endTime}");
    }
    
    [ContextMenu("Reset World")]
    public void Reset()
    {
        _treePlacer.Reset();
        _plantPlacer.Reset();
        _mineralPlacer.Reset();
        
        GroundTilemap.ClearAllTiles();
        WaterTilemap.ClearAllTiles();
        GroundDecorationTilemap.ClearAllTiles();
    }

    public HashSet<Vector3Int> GetClosedPositions()
    {
        var closed = new HashSet<Vector3Int>();
        closed.UnionWith(WaterTilemap.GetAllTileLocations().ToHashSet());
        closed.UnionWith(_treePlacer.ClosedTiles);
        closed.UnionWith(_plantPlacer.ClosedTiles);
        closed.UnionWith(_mineralPlacer.ClosedTiles);
        return closed;
    }

    private void GenerateTerrain()
    {
        _heightMap = HeightMapGenerator.Generate(_width, _height, _worldOffset, _scale);
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                PlaceTile(_heightMap, x, y);
            }
        }
    }

    private void GenerateGrass()
    {
        var grassPositions = new HashSet<Vector3Int>();
        
        var candidatePositions = GroundTilemap.GetAllTileLocations().ToHashSet();
        foreach (var closedPosition in GetClosedPositions())
        {
            if (candidatePositions.Contains(closedPosition))
            {
                candidatePositions.Remove(closedPosition);
            }
        }
        
        for (int i = 0; i < _grassIterations; i++)
        {
            foreach (var position in PoissonDisc.GeneratePoints(_grassDistanceBetweenPlacements, WorldSize, 10))
            {
                var finalPosition = (Vector3Int)Utils.GetGridIndex(position);
                if (candidatePositions.Contains(finalPosition))
                    grassPositions.Add(finalPosition);
                
                for (int g = 0; g < _grassExpansions; g++)
                {
                    if(Random.value > .5) continue;
                    var point = Random.insideUnitCircle * _grassExpansionRadius;
                    var roundedPoint = new Vector3Int(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y));
                    var newPos = finalPosition + roundedPoint;
                    if (candidatePositions.Contains(newPos))
                        grassPositions.Add(newPos);
                }
            }
        }

        foreach (var pos in grassPositions)
        {
            GroundDecorationTilemap.SetTile(pos);
        }
    }

    private void PlaceTile(float[,] heightMap, int x, int y)
    {
        if (Utils.IsInsideGrid(new Vector2(x, y), heightMap))
        {
            if (heightMap[x, y] < _groundThreshold)
            {
                GroundTilemap.SetTile(x, y);
            }
            else if (_generateLakes && heightMap[x, y] < _lakeThreshhold)
            {
                WaterTilemap.SetTile(x, y);
            }
            else if (_generateRivers && heightMap[x, y] < _riverThreshhold)
            {
                WaterTilemap.SetTile(x, y);
            }
            else
            {
                GroundTilemap.SetTile(x, y);
            }  
        }
    }
}