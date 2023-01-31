using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PrefabricatorUtility.Runtime
{
    public class PrefabricatorLayer : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TilemapRenderer _tilemapRenderer;
        [field: SerializeField] public int LayerIndex { get; private set; }

        public GameObject BakeTiles(string prefabName)
        {
            _tilemap.CompressBounds();
            _tilemap.RefreshAllTiles();
            var gameObj = new GameObject(prefabName);
            var tiles = GetAllTileInfo();
            if (tiles.Count == 0) return null;
        
            foreach (var tileInfo in tiles)
            {
                var tileObject = new GameObject
                {
                    transform =
                    {
                        localPosition = tileInfo.Position
                    }
                };
        
                var spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = tileInfo.TileSprite;
                spriteRenderer.sortingOrder = tileInfo.SortingOrder;
            
                tileObject.transform.SetParent(gameObj.transform);
            }

            return gameObj;
        }

        private List<TileInfo> GetAllTileInfo()
        {
            var tiles = new List<TileInfo>();
            for (int x = _tilemap.cellBounds.xMin; x < _tilemap.cellBounds.xMax; x++)
            {
                for (int y = _tilemap.cellBounds.yMin; y < _tilemap.cellBounds.yMax; y++)
                {
                    var cellPosition = new Vector3Int(x, y, 0);
                    var tile = _tilemap.GetTile(cellPosition) as Tile;
                    if (tile != null)
                    {
                        tiles.Add(new TileInfo
                        {
                            Position = _tilemap.CellToWorld(cellPosition),
                            TileSprite = tile.sprite,
                            SortingOrder = LayerIndex
                        });
                    }
                }
            }

            return tiles;
        }

        public void ClearLayer()
        {
            _tilemap.ClearAllTiles();
            _tilemap.CompressBounds();
        }

        public void Initialize(int index)
        {
            LayerIndex = index;
            _tilemap = gameObject.AddComponent<Tilemap>();
            _tilemapRenderer = _tilemap.gameObject.AddComponent<TilemapRenderer>();
            _tilemapRenderer.sortingOrder = LayerIndex;
        }
    }
}