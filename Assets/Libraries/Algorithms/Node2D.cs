using UnityEngine;

namespace Libraries.Algorithms
{
    public class Node2D<T> where T : new()
    {
        public int X { get; set; }
        public int Y { get; set; }
        public T Value { get; set; }

        public Node2D(int xPos, int yPos, T value = default)
        {
            X = xPos;
            Y = yPos;
            Value = value ?? new T();
        }

        public Node2D() : this(0, 0, default) { }
    }
}