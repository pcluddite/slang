/** +++====+++
*  
*  Copyright (c) Timothy Baxendale
*
*  +++====+++
**/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tbasic.Components
{
    internal class ArrayList<T> : IList<T>, ICollection<T>, IEnumerable<T>
    {
        public T this[int index]
        {
            get {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException();
                return InnerArray[index];
            }
            set {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException();
                InnerArray[index] = value;
            }
        }

        public T[] InnerArray { get; private set; }
        public int Capacity => InnerArray.Length;
        public int Count { get; private set; } = 0;

        bool ICollection<T>.IsReadOnly => false;

        public void Add(T item)
        {
            if (Capacity < ++Count) {
                ExpandArray();
            }
            InnerArray[Count - 1] = item;
        }

        private void ExpandArray()
        {
            T[] newArr = new T[InnerArray.Length * 2];
            InnerArray.CopyTo(newArr, 0);
            InnerArray = newArr;
        }

        public void Clear()
        {
            Count = 0;
        }

        public bool Contains(T item)
        {
            return InnerArray.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            InnerArray.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int idx = 0; idx < Capacity; ++idx)
                yield return InnerArray[idx];
        }

        public int IndexOf(T item)
        {
            return Array.IndexOf(InnerArray, item);
        }

        public void Insert(int index, T item)
        {
            if (index == Count) {
                Add(item);
            }
            else if (Capacity < ++Count) {
                T[] newArr = new T[Capacity * 2];
                Array.Copy(InnerArray, 0, newArr, 0, index);
                newArr[index] = item;
                Array.Copy(InnerArray, index, newArr, index + 1, Count - index);
            }
            else {
                Array.Copy(InnerArray, index, InnerArray, index + 1, Count - index);
                InnerArray[index] = item;
            }
        }

        public bool Remove(T item)
        {
            int idx = IndexOf(item);
            if (idx > -1) {
                RemoveAt(idx);
                return true;
            }
            else {
                return false;
            }
        }

        public void RemoveAt(int index)
        {
            Array.Copy(InnerArray, index + 1, InnerArray, index, Count - index);
            --Count;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return InnerArray.GetEnumerator();
        }
    }
}
