using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectPlacer : MonoBehaviour
{
    protected Vector2Int _worldSize;
    protected bool _clampToGrid = true;
    
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
        var closed = new Vector3Int[closedPositions.Count];
        closedPositions.CopyTo(closed);
        var closedList = closed.ToList();
        
        while (objectPositions.Count < targetPlacements)
        {
            var positions = PoissonDisc.GeneratePoints(minDistance, regionSize)
                .Select(position => new Vector3Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), 0))
                .Where(position => Utils.IsInsideGrid(new Vector2(position.x, position.y), _worldSize))
                .Where(objPos => !closedList.Contains(objPos));
            
            foreach (var objPos in positions)
            {
                objectPositions.Add(objPos);
                
                var yLength = objPos.y + objectData.ObjectSize.y;
                var xLength = objPos.x + objectData.ObjectSize.x;
                
                for (int y = objPos.y; y < yLength; y++)
                {
                    for (int x = objPos.x; x < xLength; x++)
                    {
                        closedList.Add(new Vector3Int(x, y, 0));
                    }
                }

                if (objectPositions.Count >= targetPlacements) break;
            }
        }
        
        closedPositions.UnionWith(closedList);
        
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


        if (data.Type == DynamicObjectData.ObjectType.Prefab)
        {
            if (parent != null)
            {
                var obj = Instantiate(data.Prefab, parent);
                obj.transform.localPosition = position;
                return obj;
            }
            
            return Instantiate(data.Prefab.gameObject, position, Quaternion.identity);
        }
        
        var gameObj = new GameObject(data.PlacementName);
        if (parent != null) gameObj.transform.SetParent(parent);
        var spriteRenderer = gameObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.Sprite;
        spriteRenderer.spriteSortPoint = SpriteSortPoint.Pivot;
        return gameObj;
    }
}
