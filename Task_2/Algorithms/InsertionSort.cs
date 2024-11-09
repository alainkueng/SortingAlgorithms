using System.Collections.Generic;
using System.Linq;

namespace Task_2.Algorithms
{
    internal class InsertionSort : ISortingAlgorithm
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

        public string Name => "Insertionsort";
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
            for (int i = 1; i < list.Count; i++)
            {
                int key = list[i];
                int j = i - 1;

                while (j >= 0 && list[j] > key)
                {
                    list[j + 1] = list[j];

                    int tempIndex = j + 1;
                    int tempIndex2 = copiedList.FirstOrDefault(e => e.Value == list[j]).Key;

                    indices.Add((tempIndex, tempIndex2));

                    int temp = copiedList[tempIndex];
                    copiedList[tempIndex] = copiedList[tempIndex2];
                    copiedList[tempIndex2] = temp;

                    j--;
                }

                list[j + 1] = key;

                int tempIndex3 = j + 1;
                int tempIndex4 = copiedList.FirstOrDefault(e => e.Value == key).Key;

                indices.Add((tempIndex3, tempIndex4));

                int temp2 = copiedList[tempIndex3];
                copiedList[tempIndex3] = copiedList[tempIndex4];
                copiedList[tempIndex4] = temp2;
            }
        }
    }
}