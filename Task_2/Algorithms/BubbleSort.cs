using System.Collections.Generic;
using System.Linq;

namespace Task_2.Algorithms
{
    internal class BubbleSort : ISortingAlgorithm
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
        public string Name => "Bubblesort";
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
            bool swapped;

            for (int i = 0; i < n - 1; i++)
            {
                swapped = false;
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (list[j] > list[j + 1])
                    {
                        Swap(list, j, j + 1);
                        SwapIndices(j, j + 1);
                        swapped = true;
                    }
                }

                if (!swapped) break;
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