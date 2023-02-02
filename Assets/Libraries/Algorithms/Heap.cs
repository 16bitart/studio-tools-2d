using System;
using System.Collections.Generic;
using System.Linq;
using Libraries.Algorithms.Pathfinding;

namespace Libraries.Algorithms
{
    public class Heap<T> where T : IComparable<T>
    {
        private readonly List<T> _items;
        public int ItemCount => Count();

        public Heap()
        {
            _items = new List<T>();
        }

        public void Add(T item)
        {
            _items.Add(item);
            SortUp(_items.Count - 1);
        }

        public void Add(params T[] items)
        {
            foreach (var comparable in items)
            {
                Add(comparable);
            }
        }

        public T RemoveMin()
        {
            var minItem = _items[0];
            _items[0] = _items[^1];
            _items.RemoveAt(_items.Count - 1);
            SortDown(0);
            return minItem;
        }

        public int Count()
        {
            return _items.Count;
        }

        public T Peek()
        {
            if (_items.Count > 0)
                return _items[0];
            throw new NullReferenceException("There is nothing to peek.");
        }

        private void SortUp(int startIndex)
        {
            var currentIndex = startIndex;
            var parentIndex = (currentIndex - 1) / 2;
            while (currentIndex > 0 && _items[currentIndex].CompareTo(_items[parentIndex]) > 0)
            {
                Swap(currentIndex, parentIndex);
                currentIndex = parentIndex;
                parentIndex = (currentIndex - 1) / 2;
            }
        }

        private void SortDown(int startIndex)
        {
            var currentIndex = startIndex;
            while (true)
            {
                var leftIndex = 2 * currentIndex + 1;
                var rightIndex = 2 * currentIndex + 2;
                var swapIndex = currentIndex;

                if (leftIndex < _items.Count && _items[leftIndex].CompareTo(_items[swapIndex]) > 0)
                    swapIndex = leftIndex;

                if (rightIndex < _items.Count && _items[rightIndex].CompareTo(_items[swapIndex]) > 0)
                    swapIndex = rightIndex;

                if (swapIndex == currentIndex)
                    break;
                
                Swap(swapIndex, currentIndex);

                currentIndex = swapIndex;
            }
        }

        private void Swap(int indexA, int indexB)
        {
            (_items[indexA], _items[indexB]) = (_items[indexB], _items[indexA]);
        }

        public bool Contains(T item)
        {
            return _items.Any(x => x.Equals(item));
        }
    }
}