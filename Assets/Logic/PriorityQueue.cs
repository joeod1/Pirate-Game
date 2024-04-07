using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Logic
{

    public class PriorityQueue<T>
    {
        public delegate bool EqualsComparison(T item1, T item2);

        public List<Tuple<T, float>> items = new List<Tuple<T, float>>();

        private int Parent(int index)
        {
            return (index - 1) / 2;
        }

        private int Left(int index)
        {
            return index * 2 + 1;
        }

        private int Right(int index)
        {
            return index * 2 + 2;
        }

        private void ShiftUp(int index)
        {
            while (index > 0 && items[Parent(index)].Item2 > items[index].Item2)
            {
                var tmp = items[index];
                items[index] = items[Parent(index)];
                items[Parent(index)] = tmp;

                index = Parent(index);
            }
        }

        private void ShiftDown(int index)
        {
            int max = index;

            int left = Left(index);
            int right = Right(index);

            if (left < items.Count && items[left].Item2 < items[max].Item2)
                max = left;

            if (right < items.Count && items[right].Item2 < items[max].Item2)
                max = right;

            if (index != max && (items[max].Item2 != items[index].Item2))
            {
                var tmp = items[index];
                items[index] = items[max];
                items[max] = tmp;

                ShiftDown(max);
            }
        }

        public void Enqueue(T obj, float priority)
        {
            items.Add(new Tuple<T, float>(obj, priority));
            ShiftUp(items.Count - 1);
        }

        public T Dequeue()
        {
            var res = items[0];

            items[0] = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);

            ShiftDown(0);
            // print(items.Count);
            return res.Item1;
        }

        public bool Contains(T search, EqualsComparison comparator)
        {
            foreach (Tuple<T, float> pair in items)
            {
                if (comparator(search, pair.Item1)) return true;
            }
            return false;
        }

        public T Find(T search, EqualsComparison comparator)
        {
            foreach (Tuple<T, float> pair in items)
            {
                if (comparator(search, pair.Item1)) return pair.Item1;
            }
            return default;
        }

        public string Print()
        {
            string output = "";
            
            /*for (int i = 0; i < Math.Log(items.Count); i++)
            {
                string line = "";
                for (int v = (int)Math.Pow(2, i); v < Math.Pow(2, i + 1); v++)
                {
                    line += " " + items[v].Item2;
                }
                output += line + "\n";
            }*/

            for (int i = 0; i < items.Count; i++)
            {
                output += items[i].Item2 + " ";
            }

            return output;
        }

        public int Count { get { return items.Count; } }

        public T this[int index] {
            get { return items[index].Item1; }
        }
    }
}
