using System.Collections.Generic;
using UnityEngine;

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