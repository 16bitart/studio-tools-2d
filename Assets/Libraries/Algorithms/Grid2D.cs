using System;

namespace Libraries.Algorithms
{
    public class Grid2D<T, T2> 
        where T : Node2D<T2>, new()
        where T2 : new()
    {
        public int Width { get; set; } = 10;
        public int Height { get; set; } = 10;
        private T[,] _nodes;
        
        public T2 this[int x, int y]
        {
            get => GetNodeValue(x, y);
            set => SetNodeValue(x, y, value);
        }
    
        public Grid2D(int width, int height)
        {
            Width = width;
            Height = height;
            _nodes = GenerateArray();
        }
    
        private T[,] GenerateArray()
        {
            var nodes = new T[Width, Height];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    nodes[x, y] = new T
                    {
                        X = x,
                        Y = y,
                        Value = default
                    };
                }
            }
            return nodes;
        }

        public T2 GetNodeValue(int x, int y)
        {
            if (IsValidPos(x, y))
            {
                return _nodes[x, y].Value;
            }

            throw new IndexOutOfRangeException($"{x}, {y} is outside the bounds of the Grid.");
        }

        public void SetNodeValue(int x, int y, T2 value)
        {
            if (IsValidPos(x, y))
            {
                _nodes[x, y].Value = value;
            }
            
            throw new IndexOutOfRangeException($"{x}, {y} is outside the bounds of the Grid.");
        }
        
        public bool IsValidPos(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
    }
    
    
}