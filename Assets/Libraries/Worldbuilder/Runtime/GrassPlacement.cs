using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrassPlacement : MonoBehaviour
{

    [SerializeField] private Worldbuilder _worldbuilder;
    [SerializeField] private bool _generateGrass = true;
    [SerializeField, Range(0, 25)] private int _grassDistance = 1;
    [SerializeField, Range(0, 25)] private int _grassIterations = 1;
    [SerializeField, Range(0, 25)] private int _grassExpansionDistance = 1;
    [SerializeField, Range(0, 25)] private int _grassExpansionIterations = 1;
    [SerializeField, Range(0, 1f)] private float _grassExpansionChance = .4f;
    private Vector2 _worldSize => _worldbuilder.WorldSize;
    private TilemapBuilder _ground => _worldbuilder.GroundTilemap;
    private TilemapBuilder _grass => _worldbuilder.GrassTilemap;
    private TilemapBuilder _water => _worldbuilder.WaterTilemap;

    public void GenerateGrassTiles()
    {
        if(!_generateGrass) return;
        for (int i = 0; i < _grassIterations; i++)
        {
            var validPositions = _ground.GetAllTileLocations().ToHashSet();
            var closedPositions = _grass.GetAllTileLocations().ToHashSet();
            validPositions.RemoveWhere(x => closedPositions.Contains(x));

            var grassPositions = SamplePositions(validPositions, _worldSize, _grassDistance, 5).ToHashSet();

            if (_grassExpansionDistance > 0)
            {
                var min = (1 + _grassExpansionDistance) * -1;
                var max = 2 + _grassExpansionDistance;

                var expandedPositions = new HashSet<Vector3>();

                foreach (var position in grassPositions)
                {
                    for (int j = 0; j < _grassExpansionIterations; j++)
                    {
                        if (Random.value > _grassExpansionChance)
                        {
                            var direction = new Vector3Int(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
                            var newPosition = position + direction;
                            expandedPositions.Add(newPosition);
                        }
                    }
                }

                grassPositions.UnionWith(expandedPositions);
            }

            foreach (var pos in grassPositions.Select(position => Utils.GetGridIndex(position)))
            {
                PlaceGrassTile(pos.x, pos.y);
            }
        }
    }
    
    private void PlaceGrassTile(int x, int y)
    {
        if (_water.HasTile(x, y)) return;
        if (Utils.IsInsideGrid(new Vector2(x, y), _worldSize))
        {
            _grass.SetTile(x, y);
        }
    }

    private HashSet<Vector3> SamplePositions(HashSet<Vector3Int> candidatePositions, Vector2 regionSize, float minDistance, int samples = 30)
    {
        var validPositions = new HashSet<Vector3>();

        foreach (var position in PoissonDisc.GeneratePoints(minDistance, regionSize, samples))
        {
            var index = (Vector3Int) Utils.GetGridIndex(position);
            if (candidatePositions.Contains(index)) validPositions.Add(position);
        }

        return validPositions;
    }
}