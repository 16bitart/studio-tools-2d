using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Libraries.Algorithms.Pathfinding
{
    public class Pathfinder : MonoBehaviour
    {
        private AstarGrid _astarGrid;
        [field: SerializeField] public Vector2Int GridSize { get; private set; }
        [field: SerializeField] public LayerMask Layers { get; private set; }

        private Collider2D[] _nonAllocArray = new Collider2D[1];
        private void Awake()
        {
            _astarGrid = new AstarGrid(GridSize.x, GridSize.y, transform.position);
        }

        private void Start()
        {
            _astarGrid.InitializeAstarNodeInfo(CheckIfOpen);
        }

        public void UpdateGrid()
        {
            _astarGrid.UpdateNodeInfoOpenStatus(CheckIfOpen);
        }

        public List<Vector2> CreatePath(Vector2Int start, Vector2Int end)
        {
            return _astarGrid.GeneratePath(start, end);
        }

        private bool CheckIfOpen(Vector2 point)
        {
            return Physics2D.OverlapCircleNonAlloc(point, .5f, _nonAllocArray, Layers.value) == 0;
        }
    }
    
    public class AstarGrid : Grid2D<AstarNode, AstarNodeInfo>
    {
        [field: SerializeField] public Vector2 WorldPosition { get; private set; }
        
        public AstarGrid(int width, int height, Vector2 worldPosition) : base(width, height)
        {
            WorldPosition = worldPosition;
        }

        public void InitializeAstarNodeInfo(Func<Vector2, bool> openCheckFunc)
        {
            foreach (var node in AllNodes())
            {
                var waypointX = (node.X + WorldPosition.x) + .5f;
                var waypointY = (node.Y + WorldPosition.y) + .5f;
                var waypoint = new Vector2(waypointX, waypointY);
                var isOpen = openCheckFunc(waypoint);
                node.Value.Waypoint = waypoint;
                node.Value.Open = isOpen;
            }
        }

        public void UpdateNodeInfoOpenStatus(Func<Vector2, bool> openCheckFunc)
        {
            foreach (var node in AllNodes())
            {
                node.Value.Open = openCheckFunc(node.Value.Waypoint);
            }
        }

        public List<Vector2> GeneratePath(Vector2Int start, Vector2Int end)
        {
            var operation = new AstarOperation(this);
            return operation.GeneratePath(start, end).ToList();
        }
    }

    public class AstarNode : Node2D<AstarNodeInfo>
    {
        public AstarNode(int xPos, int yPos, bool open, Vector2 waypoint)
            : this(xPos, yPos, new AstarNodeInfo(open, waypoint, new Vector2Int(xPos, yPos)))
        {
        }

        public AstarNode(int xPos, int yPos, AstarNodeInfo value = default)
            : base(xPos, yPos, value)
        {
        }

        public AstarNode()
        {
        }
    }

    public class AstarNodeInfo
    {
        public Vector2Int GridLocation { get; set; }
        public Vector2 Waypoint { get; set; }
        public bool Open { get; set; }

        public AstarNodeInfo()
        {
            Waypoint = Vector2.zero;
            Open = true;
        }

        public AstarNodeInfo(bool open, Vector2 waypoint, Vector2Int gridLocation)
        {
            GridLocation = gridLocation;
            Waypoint = waypoint;
            Open = open;
        }
    }
}