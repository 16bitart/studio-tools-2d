using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscVisualizer : MonoBehaviour
{
    public int Size = 30;
    public bool[,] _grid;
    public float _minDistance;
    public int _numTries;

    public float _displaySize = 1f;

    private List<Vector2> _samplePositions;
    void Start()
    {
        CreateGrid();
    }

    [ContextMenu("Create Grid")]
    public void CreateGrid()
    {
        _grid = new bool[Size, Size];
        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                _grid[x, y] = true;
            }
        }
    }

    [ContextMenu("run")]
    public void Run()
    {
        CreateGrid();
        
        _samplePositions = PoissonDisc.GeneratePoints(_minDistance, new Vector2(Size, Size), _numTries);
        
        foreach (var position in _samplePositions)
        {
            if (Utils.IsInsideGrid(position, _grid))
            {
                var index = Utils.GetGridIndex(position);
                _grid[index.x, index.y] = false;
            }
        }
        
        Debug.Log("Ran.");
    }

    void OnDrawGizmos()
    {
        if(_grid == null) return;

        for (int y = 0; y < _grid.GetLength(1); y++)
        {
            for (int x = 0; x < _grid.GetLength(0); x++)
            {
                Gizmos.color = _grid[x, y] ? Color.green : Color.red;
                Gizmos.DrawCube(new Vector3(x + .5f, y + .5f, 0), Vector3.one);
            }
        }
    }
    
   
}
