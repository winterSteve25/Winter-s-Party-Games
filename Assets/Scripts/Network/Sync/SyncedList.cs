using System;
using System.Collections;
using System.Collections.Generic;

namespace Network.Sync
{
    public class SyncedList<T> : SyncedVar<List<T>>, IList<T>
    {
        protected readonly Action<T> ElementAdded;
        protected readonly Action<T> ElementRemoved;
        
        public SyncedList(Action<T> elementRemoved = null, Action<T> elementAdded = null, Action onChanged = null) : base(onChanged)
        {
            ElementRemoved = elementRemoved;
            ElementAdded = elementAdded;
        }

        public SyncedList(List<T> value, Action<T> elementRemoved = null, Action<T> elementAdded = null, Action onChanged = null) : base(value, onChanged)
        {
            ElementRemoved = elementRemoved;
            ElementAdded = elementAdded;
        }

        private void Change()
        {
            OnChanged?.Invoke();
            ValueChanged();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Value.GetEnumerator();
        }

        public void Add(T item)
        {
            _Value.Add(item);
            Change();
            ElementAdded?.Invoke(item);
        }

        public void Clear()
        {
            _Value.Clear();
            Change();
        }

        public bool Contains(T item)
        {
            return _Value.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _Value.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            var b = _Value.Remove(item);
            Change();
            ElementRemoved?.Invoke(item);
            return b;
        }

        public int Count => _Value.Count;

        public bool IsReadOnly => false;
        
        public int IndexOf(T item)
        {
            return _Value.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _Value.Insert(index, item);
            Change();
            ElementAdded?.Invoke(item);
        }

        public void RemoveAt(int index)
        {
            ElementRemoved?.Invoke(_Value[index]);
            Change();
            _Value.RemoveAt(index);
        }

        public T this[int index]
        {
            get => _Value[index];
            set
            {
                _Value[index] = value;
                Change();
            }
        }
    }
}