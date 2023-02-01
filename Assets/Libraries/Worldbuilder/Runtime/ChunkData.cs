using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkData : MonoBehaviour
{
    [field: SerializeField] public TileBase GroundTile { get; private set; }
    [field: SerializeField] public TileBase WaterTile { get; private set; }
    [field: SerializeField] public TileBase GrassTile { get; private set; }
    [field: SerializeField] public TileBase GroundDecorationTile { get; private set; }
    [field: SerializeField] public DynamicObjectData TreeData { get; private set; }
    [field: SerializeField] public DynamicObjectData PlantData { get; private set; }
    [field: SerializeField] public DynamicObjectData MineralData { get; private set; }
}
