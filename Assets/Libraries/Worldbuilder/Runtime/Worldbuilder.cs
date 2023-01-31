using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Worldbuilder : MonoBehaviour
{
    public Vector2 WorldSize => new Vector2(_worldWidth, _worldHeight);
    [SerializeField, Range(10, 500)] private int _worldWidth = 100;
    [SerializeField, Range(10, 500)] private int _worldHeight = 100;
    [SerializeField, Range(1, 100)] private int _worldScale = 15;
    [SerializeField] private Vector2 _worldOffset = Vector2.zero;


    [SerializeField] private bool _generateTrees = true;
    [SerializeField] private bool _generateLakes = true;
    [SerializeField] private bool _generateRivers = true;

    [SerializeField, Range(0, 1f)] private float _groundThreshold = .3f;
    [SerializeField, Range(0, 1f)] private float _lakeThreshhold = .2f;
    [SerializeField, Range(0, 1f)] private float _riverThreshhold = .4f;

    [field: SerializeField] public TilemapBuilder GroundTilemap { get; private set; }
    [field: SerializeField] public TilemapBuilder WaterTilemap { get; private set; }
    [field: SerializeField] public TilemapBuilder GrassTilemap { get; private set; }

    [SerializeField] private TreePlacer _treePlacer;
    [SerializeField] private GrassPlacement _grassPlacer;

    private float[,] _heightMap;

    public float[,] GenerateHeightMap()
    {
        return HeightMapGenerator.Generate(_worldWidth, _worldHeight, _worldOffset, _worldScale);
    }


    [ContextMenu("Run Generation")]
    public void Run()
    {
        Reset();
        GenerateBaseTiles();
        GenerateGrassTiles();
        if (_generateTrees) _treePlacer.GenerateTrees();
    }

    public HashSet<Vector3Int> GetClosedPositions()
    {
        var waterTiles = WaterTilemap.GetAllTileLocations().ToHashSet();
        return waterTiles;
    }

    private void GenerateGrassTiles()
    {
        _grassPlacer.GenerateGrassTiles();
    }

    public void Reset()
    {
        GroundTilemap.ClearAllTiles();
        WaterTilemap.ClearAllTiles();
        GrassTilemap.ClearAllTiles();
        _treePlacer.Reset();
    }

    private void GenerateBaseTiles()
    {
        _heightMap = GenerateHeightMap();
        for (int y = 0; y < _worldHeight; y++)
        {
            for (int x = 0; x < _worldWidth; x++)
            {
                PlaceTile(_heightMap, x, y);
            }
        }
    }

    private void PlaceTile(float[,] heightMap, int x, int y)
    {
        if (_generateLakes && heightMap[x, y] < _lakeThreshhold)
        {
            PlaceWaterTile(x, y);
        }
        else if (heightMap[x, y] < _groundThreshold)
        {
            PlaceGroundTile(x, y);
        }
        else if (_generateRivers && heightMap[x, y] < _riverThreshhold)
        {
            PlaceWaterTile(x, y);
        }
        else
        {
            PlaceGroundTile(x, y);
        }
    }

    private void PlaceWaterTile(int x, int y)
    {
        WaterTilemap.SetTile(x, y);
    }

    private void PlaceGroundTile(int x, int y)
    {
        GroundTilemap.SetTile(x, y);
    }
}


public static class DirectionUtility
{
    public static Vector2Int[] MajorDirections = new Vector2Int[]
    {
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right,
        Vector2Int.up
    };

    public static Vector2Int[] DiagonalDirections = new Vector2Int[]
    {
        Vector2Int.down + Vector2Int.left,
        Vector2Int.left + Vector2Int.up,
        Vector2Int.right + Vector2Int.down,
        Vector2Int.up + Vector2Int.right
    };

    public static Vector2Int[] AllDirections = new Vector2Int[]
    {
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right,
        Vector2Int.up,
        Vector2Int.down + Vector2Int.left,
        Vector2Int.left + Vector2Int.up,
        Vector2Int.right + Vector2Int.down,
        Vector2Int.up + Vector2Int.right
    };

    public static Dictionary<Vector2Int, float> CreateDirectionDictionaryWithWeights(
        float upWeight = 1f,
        float downWeight = 1f,
        float leftWeight = 1f,
        float rightWeight = 1f,
        float upRightWeight = 1f,
        float upLeftWeight = 1f,
        float downRightWeight = 1f,
        float downLeftWeight = 1f)
    {
        return new Dictionary<Vector2Int, float>()
        {
            {Vector2Int.down, downWeight},
            {Vector2Int.left, leftWeight},
            {Vector2Int.right, rightWeight},
            {Vector2Int.up, upWeight},
            {Vector2Int.down + Vector2Int.left, downLeftWeight},
            {Vector2Int.left + Vector2Int.up, upLeftWeight},
            {Vector2Int.right + Vector2Int.down, downRightWeight},
            {Vector2Int.up + Vector2Int.right, upRightWeight}
        };
    }
}