using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapBuilder : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    
    [SerializeField] private TileBase _baseTile;

    private HashSet<Vector3Int> _tileLocations = new HashSet<Vector3Int>();

    private void OnValidate()
    {
        if (_tilemap == null) _tilemap = GetComponent<Tilemap>();
    }

    public List<Vector3Int> GetAllTileLocations()
    {
        return _tileLocations.ToList();
    }

    public bool HasTile(int x, int y)
    {
        return _tileLocations.Contains(new Vector3Int(x, y, 0));
    }

    public void SetTile(int x, int y)
    {
        var pos = new Vector3Int(x, y, 0);
        
        _tilemap.SetTile(pos, _baseTile);

        _tileLocations.Add(pos);
    }

    public void ClearTile(int x, int y)
    {
        var pos = new Vector3Int(x, y, 0);
        _tilemap.DeleteCells(pos, Vector3Int.one);
        _tileLocations.Remove(pos);
    }

    public void ClearAllTiles()
    {
        _tileLocations.Clear();
        _tilemap.ClearAllTiles();
        _tilemap.CompressBounds();
    }
}