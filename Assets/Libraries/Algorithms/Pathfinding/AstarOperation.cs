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
            
            var explored = new HashSet<AstarPathNode>();
            var frontier = new Queue<AstarPathNode>();
            _start.H = Utils.GetNodeDistance(_start.GridLocation, _end.GridLocation);
            _start.G = 0;
            
            frontier.Enqueue(_start);

            while (frontier.Count > 0)
            {
                var currentNode = frontier.Dequeue();
                
                if (currentNode == _end)
                {
                    path = ConstructPath(_end, currentNode);
                    break;
                }

                foreach (var neighbor in GetNeighbors(currentNode))
                {
                    if (explored.Contains(neighbor) || frontier.Contains(neighbor) || !neighbor.Open)
                        continue;
                    
                    neighbor.Parent = currentNode;
                    neighbor.G = currentNode.G + Utils.GetNodeDistance(currentNode.GridLocation, neighbor.GridLocation);
                    neighbor.H = Utils.GetNodeDistance(neighbor.GridLocation, _end.GridLocation);
                    frontier.Enqueue(neighbor);
                }
                
                explored.Add(currentNode);
            }

            return path;
        }
        
        private HashSet<Vector2> ConstructPath(AstarPathNode endNode, AstarPathNode startNode)
        {
            var path = new HashSet<Vector2>();
            var currentNode = endNode;
            while (currentNode != startNode)
            {
                path.Add(currentNode.Waypoint);
                currentNode = currentNode.Parent;
            }
            path.Add(startNode.Waypoint);
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
}