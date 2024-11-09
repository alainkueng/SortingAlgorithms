using System.Collections.Generic;
using System.Linq;

namespace Task_2.Algorithms
{
    internal class MergeSort : ISortingAlgorithm
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
            Sort(list, 0, list.Count - 1);
        }

        public string Name => "Mergesort";
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
        private void Sort(List<int> list, int left, int right)
        {
            if (left < right)
            {
                int mid = (left + right) / 2;
                Sort(list, left, mid);
                Sort(list, mid + 1, right);
                Merge(list, left, mid, right);
            }
        }

        private void Merge(List<int> list, int left, int mid, int right)
        {
            int n1 = mid - left + 1;
            int n2 = right - mid;

            int[] leftArr = new int[n1];
            int[] rightArr = new int[n2];

            for (int i = 0; i < n1; i++)
            {
                leftArr[i] = list[left + i];
            }

            for (int j = 0; j < n2; j++)
            {
                rightArr[j] = list[mid + 1 + j];
            }

            int k = left;
            int x = 0;
            int y = 0;

            while (x < n1 && y < n2)
            {
                if (leftArr[x] <= rightArr[y])
                {
                    list[k] = leftArr[x];

                    int tempIndex = k;
                    int tempIndex2 = copiedList.FirstOrDefault(e => e.Value == leftArr[x]).Key;

                    indices.Add((tempIndex, tempIndex2));

                    int temp = copiedList[tempIndex];
                    copiedList[tempIndex] = copiedList[tempIndex2];
                    copiedList[tempIndex2] = temp;

                    x++;
                }
                else
                {
                    list[k] = rightArr[y];

                    int tempIndex = k;
                    int tempIndex2 = copiedList.FirstOrDefault(e => e.Value == rightArr[y]).Key;

                    indices.Add((tempIndex, tempIndex2));

                    int temp = copiedList[tempIndex];
                    copiedList[tempIndex] = copiedList[tempIndex2];
                    copiedList[tempIndex2] = temp;

                    y++;
                }

                k++;
            }

            while (x < n1)
            {
                list[k] = leftArr[x];

                int tempIndex = k;
                int tempIndex2 = copiedList.FirstOrDefault(e => e.Value == leftArr[x]).Key;

                indices.Add((tempIndex, tempIndex2));

                int temp = copiedList[tempIndex];
                copiedList[tempIndex] = copiedList[tempIndex2];
                copiedList[tempIndex2] = temp;

                x++;
                k++;
            }

            while (y < n2)
            {
                list[k] = rightArr[y];

                int tempIndex = k;
                int tempIndex2 = copiedList.FirstOrDefault(e => e.Value == rightArr[y]).Key;

                indices.Add((tempIndex, tempIndex2));

                int temp = copiedList[tempIndex];
                copiedList[tempIndex] = copiedList[tempIndex2];
                copiedList[tempIndex2] = temp;

                y++;
                k++;
            }
        }
    }
}