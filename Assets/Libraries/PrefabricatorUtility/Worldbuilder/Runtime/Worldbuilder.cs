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


    [SerializeField] private bool _generateGrass = true;
    [SerializeField] private bool _generateTrees = true;
    [SerializeField] private bool _generateLakes = true;
    [SerializeField] private bool _generateRivers = true;
    
    [SerializeField, Range(0, 1f)] private float _groundThreshold = .3f;
    [SerializeField, Range(0, 1f)] private float _lakeThreshhold = .2f;
    [SerializeField, Range(0, 1f)] private float _riverThreshhold = .4f;

    [SerializeField, Range(0, 25)] private int _grassDistance = 1;
    [SerializeField, Range(0, 25)] private int _grassIterations = 1;
    [SerializeField, Range(0, 25)] private int _grassExpansionDistance = 1;
    [SerializeField, Range(0, 25)] private int _grassExpansionIterations = 1;

    [SerializeField] private TilemapBuilder _ground;
    [SerializeField] private TilemapBuilder _water;
    [SerializeField] private TilemapBuilder _grass;

    [SerializeField] private TreePlacer _treePlacer;
    
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
        if(_generateGrass) GenerateGrassTiles();
        if(_generateTrees) _treePlacer.GenerateTrees();
    }

    public HashSet<Vector3Int> GetClosedPositions()
    {
        var waterTiles = _water.GetAllTileLocations().ToHashSet();
        return waterTiles;
    }

    private void GenerateGrassTiles()
    {
        var validPositions = _ground.GetAllTileLocations();
        var grassPositions = SamplePositions(validPositions, WorldSize, _grassDistance, 5);

        foreach (var position in grassPositions)
        {
            var pos = Utils.GetGridIndex(position);
            PlaceGrassTile(pos.x, pos.y);
        }

        if (_grassIterations > 0)
        {
            for (int g = 0; g < _grassIterations; g++)
            {
                var moreGrass = ExpandPositions(_grass.GetAllTileLocations(), _grassExpansionDistance, _grassExpansionIterations);
                foreach (var position in moreGrass)
                {
                    PlaceGrassTile(position.x, position.y);
                }
            }
        }
    }


    public void Reset()
    {
        _ground.ClearAllTiles();
        _water.ClearAllTiles();
        _grass.ClearAllTiles();
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
        _water.SetTile(x, y);
    }

    private void PlaceGroundTile(int x, int y)
    {
        _ground.SetTile(x, y);
    }

    private void PlaceGrassTile(int x, int y)
    {
        if (_water.HasTile(x, y)) return;
        if (Utils.IsInsideGrid(new Vector2(x, y), WorldSize))
        {
            _grass.SetTile(x, y);
        }
    }


    private List<Vector3> SamplePositions(List<Vector3Int> candidatePositions, Vector2 regionSize, float minDistance, int samples = 30)
    {
        var validPositions = new List<Vector3>();

        foreach (var position in PoissonDisc.GeneratePoints(minDistance, regionSize, samples))
        {
            var index = (Vector3Int) Utils.GetGridIndex(position);
            if (candidatePositions.Contains(index)) validPositions.Add(position);
        }

        return validPositions;
    }

    private List<Vector3Int> ExpandPositions(List<Vector3Int> positions, int expansionSize, int iterations = 1)
    {
        var expandedPositions = new HashSet<Vector3Int>(positions);

        for (int it = 0; it < iterations; it++)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                for (int j = 0; j < expansionSize; j++)
                {
                    var direction = new Vector3Int(Random.Range(-1, 2), Random.Range(-1, 2), Random.Range(-1, 2));
                    var newPosition = positions[i] + direction;
                    expandedPositions.Add(newPosition);
                }
            }
        }

        return expandedPositions.ToList();
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