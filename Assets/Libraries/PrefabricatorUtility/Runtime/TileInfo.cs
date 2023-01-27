using UnityEngine;

namespace PrefabricatorUtility.Runtime
{
    public struct TileInfo
    {
        public Vector3 Position { get; set; }
        public Sprite TileSprite { get; set; }
        public int SortingOrder { get; set; }
    }
}