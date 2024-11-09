using System.Collections.Generic;

namespace Task_2.Algorithms
{
    internal interface ISortingAlgorithm
    {
        void Sort(List<int> list);
        List<(int x, int y)> Indices { get; }
        string Name { get; }
    }
}