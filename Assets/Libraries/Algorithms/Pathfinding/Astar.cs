using System;
using System.Collections.Generic;
using UnityEngine;

namespace Libraries.Algorithms.Pathfinding
{
    public class AstarGrid : Grid2D<AstarNode, AstarNodeInfo>
    {
        [field: SerializeField] public Vector2 WorldPosition { get; private set; }

        private HashSet<AstarNode> _openNodes;
        private HashSet<AstarNode> _closedNodes;

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
            _closedNodes.Clear();
            foreach (var node in AllNodes())
            {
                var result = openCheckFunc(node.Value.Waypoint);

                if (result) _openNodes.Add(node);
                else _closedNodes.Add(node);

                node.Value.Open = result;
            }
        }
    }
    
    public class AstarNode : Node2D<AstarNodeInfo>
    {
        public AstarNode(int xPos, int yPos, bool open, Vector2 waypoint) 
            : this(xPos, yPos, new AstarNodeInfo(open, waypoint, new Vector2Int(xPos, yPos))) { }
        public AstarNode(int xPos, int yPos, AstarNodeInfo value = default) 
            : base(xPos, yPos, value) { }

        public AstarNode() { }
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

    public class AstarPathNode : IComparable<AstarPathNode>
    {
        public AstarPathNode Parent;
        public Vector2Int GridLocation { get; set; }
        public Vector2 Waypoint { get; set; }
        public bool Open { get; set; }

        public int F => G + H;
        public int G { get; set; }
        public int H { get; set; }

        public AstarPathNode(AstarNodeInfo info)
        {
            GridLocation = info.GridLocation;
            Waypoint = info.Waypoint;
            Open = info.Open;
        }

        public AstarPathNode(AstarNodeInfo info, AstarPathNode parent) : this(info)
        {
           Parent = parent;
        }

        //1 => Higher Priority
        public int CompareTo(AstarPathNode other)
        {
            if (other == null) return 1;
            var compare = F.CompareTo(other.F);
            if (compare == 0)
            {
                compare = H.CompareTo(other.H);
            }
            return -compare;
        }
    }
}