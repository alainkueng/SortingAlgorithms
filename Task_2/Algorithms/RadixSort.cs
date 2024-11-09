using System.Collections.Generic;
using System.Linq;

namespace Task_2.Algorithms
{
    internal class RadixSort : ISortingAlgorithm
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
        public string Name => "Radixsort";

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
            int maxVal = list.Max();
            int exp;
            for (exp = 1; maxVal / exp > 0; exp *= 10)
            {
                CountingSort(list, exp);
            }
        }

        private void CountingSort(List<int> list, int exp)
        {
            int n = list.Count;
            List<int> output = new List<int>(new int[n]);
            int[] count = new int[10];

            for (int i = 0; i < n; i++)
            {
                int index = (list[i] / exp) % 10;
                count[index]++;
            }

            for (int i = 1; i < 10; i++)
            {
                count[i] += count[i - 1];
            }

            for (int i = n - 1; i >= 0; i--)
            {
                int index = (list[i] / exp) % 10;
                output[count[index] - 1] = list[i];
                count[index]--;
            }

            for (int i = 0; i < n; i++)
            {
                int tempIndex = i;
                int tempIndex2 = copiedList.FirstOrDefault(e => e.Value == output[i]).Key;

                if (list[i] != output[i])
                {
                    indices.Add((tempIndex, tempIndex2));

                    int temp = copiedList[tempIndex];
                    copiedList[tempIndex] = copiedList[tempIndex2];
                    copiedList[tempIndex2] = temp;
                }

                list[i] = output[i];
            }
        }
    }
}