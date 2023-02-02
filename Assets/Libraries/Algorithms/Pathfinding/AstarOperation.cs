using System;
using System.Collections.Generic;
using UnityEngine;

namespace Libraries.Algorithms.Pathfinding
{
    public class AstarOperation
    {
        private int Width;
        private int Height;

        private AstarPathNode _start;
        private AstarPathNode _end;
        private AstarPathNode[,] _pathGrid;
        
        public AstarOperation(AstarGrid astarGrid)
        {
            Width = astarGrid.Width;
            Height = astarGrid.Height;
            _pathGrid = new AstarPathNode[Width, Height];
            
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    _pathGrid[x, y] = new AstarPathNode(astarGrid.GetNodeValue(x, y));
                }
            }
        }
        
        public HashSet<Vector2> GeneratePath(Vector2Int start, Vector2Int end)
        {
            var path = new HashSet<Vector2>();
            _start = _pathGrid[start.x, start.y];
            _end =  _pathGrid[end.x, end.y];
            
            var explored = new List<AstarPathNode>();
            var pathHeap = new Heap<AstarPathNode>();
            
            _start.H = Utils.GetNodeDistance(_start.GridLocation, _end.GridLocation);
            _start.G = 0;
            
            pathHeap.Add(_start);
            
            while (pathHeap.ItemCount > 0)
            {
                var currentNode = pathHeap.RemoveMin();

                if (currentNode.GridLocation == _end.GridLocation) return ConstructPath(currentNode);
                
                foreach (var neighbor in GetNeighbors(currentNode))
                {
                    if (explored.Contains(neighbor) || pathHeap.Contains(neighbor) || !neighbor.Open)
                        continue;
                    
                    neighbor.Parent = currentNode;
                    neighbor.G = currentNode.G + Utils.GetNodeDistance(currentNode.GridLocation, neighbor.GridLocation);
                    neighbor.H = Utils.GetNodeDistance(neighbor.GridLocation, _end.GridLocation);
                    pathHeap.Add(neighbor);
                }
                
                explored.Add(currentNode);
            }

            return path;
        }

        private HashSet<Vector2> ConstructPath(AstarPathNode endNode)
        {
            var path = new HashSet<Vector2>();

            var currentNode = endNode;
            var currentGridLocation = endNode.GridLocation;
            var targetPos = _start.GridLocation;
            path.Add(currentNode.Waypoint);
            
            while (currentGridLocation != targetPos)
            {
                currentNode = currentNode.Parent;
                currentGridLocation = currentNode.GridLocation;
                path.Add(currentNode.Waypoint);
            }

            return path;
        }
        
        public List<AstarPathNode> GetNeighbors(AstarPathNode node)
        {
            var neighbors = new List<AstarPathNode>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    int checkX = node.GridLocation.x + x;
                    int checkY = node.GridLocation.y + y;
                    if (checkX >= 0 && checkX < Width && checkY >= 0 && checkY < Height)
                    {
                        neighbors.Add(_pathGrid[checkX, checkY]);
                    }
                }
            }
            return neighbors;
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

        //It should only match if the parents' location is the same.
        public override bool Equals(object obj)
        {
            if (obj is AstarPathNode other)
            {
                return CompareTo(other) == 0;
            }
            
            return false;
        }
    }
}