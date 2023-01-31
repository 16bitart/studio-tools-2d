using UnityEngine;
public static class HeightMapGenerator
{
    public static float[,] Generate(int width, int height, Vector2 offset, float scale)
    {
        var heightMap = new float[width, height];
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var xCoord = (float)x / width * scale + offset.x;
                var yCoord = (float)y / height * scale + offset.y;
                heightMap[x, y] = Mathf.PerlinNoise(xCoord, yCoord);
            }
        }
        return heightMap;
    }
}