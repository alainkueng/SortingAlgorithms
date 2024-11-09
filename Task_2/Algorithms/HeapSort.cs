using System.Collections.Generic;
using System.Linq;

namespace Task_2.Algorithms
{
    internal class HeapSort : ISortingAlgorithm
    {
        Dictionary<int, int> copiedList;
        List<(int x, int y)> indices;

        public void Sort(List<int> list)
        {
            copiedList = new Dictionary<int, int>();
            for (int i = 0; i < list.Count; i++)
            {
                copiedList[i] = list[i];
            }
            indices = new List<(int x, int y)>();
            Sorting(list);
        }

        public string Name => "Heapsort";

        public Dictionary<int, int> CopiedList
        {
            get { return copiedList; }
            private set { copiedList = value; }
        }

        public List<(int x, int y)> Indices
        {
            get { return indices; }
            set { indices = value; }
        }

        private void Sorting(List<int> list)
        {
            int n = list.Count;

            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(list, n, i);

            for (int i = n - 1; i >= 0; i--)
            {
                Swap(list, 0, i);
                SwapIndices(0, i);

                Heapify(list, i, 0);
            }
        }

        private void Heapify(List<int> list, int n, int i)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n && list[left] > list[largest])
                largest = left;

            if (right < n && list[right] > list[largest])
                largest = right;

            if (largest != i)
            {
                Swap(list, i, largest);
                SwapIndices(i, largest);

                Heapify(list, n, largest);
            }
        }

        private void Swap(List<int> list, int i, int j)
        {
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        private void SwapIndices(int i, int j)
        {
            int tempIndex1 = copiedList.FirstOrDefault(e => e.Value == copiedList[i]).Key;
            int tempIndex2 = copiedList.FirstOrDefault(e => e.Value == copiedList[j]).Key;

            indices.Add((tempIndex1, tempIndex2));

            int temp = copiedList[tempIndex1];
            copiedList[tempIndex1] = copiedList[tempIndex2];
            copiedList[tempIndex2] = temp;
        }
    }
}