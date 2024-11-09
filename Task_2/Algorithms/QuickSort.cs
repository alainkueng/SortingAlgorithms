using System.Collections.Generic;

namespace Task_2.Algorithms
{
    internal class QuickSort : ISortingAlgorithm
    {
        private List<(int x, int y)> indices;

        public void Sort(List<int> list)
        {
            indices = new List<(int x, int y)>();
            Sort(list, 0, list.Count - 1);
        }

        public List<(int x, int y)> Indices
        {
            get { return indices; }
        }

        public string Name => "Quicksort";

        private void Sort(List<int> list, int left, int right)
        {
            if (left < right)
            {
                int pivotIndex = Partition(list, left, right);
                Sort(list, left, pivotIndex - 1);
                Sort(list, pivotIndex + 1, right);
            }
        }

        private int Partition(List<int> list, int left, int right)
        {
            int pivotValue = list[right];
            int pivotIndex = left;

            for (int i = left; i < right; i++)
            {
                if (list[i] < pivotValue)
                {
                    Swap(list, i, pivotIndex);

                    pivotIndex++;
                }
            }

            Swap(list, pivotIndex, right);


            return pivotIndex;
        }

        private void Swap(List<int> list, int i, int j)
        {
            indices.Add((i, j));
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

}