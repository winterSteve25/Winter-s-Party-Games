using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Network.Sync
{
    public class SyncedList<T> : SyncedVar<List<T>>, ISyncedList, IList<T>
    {
        protected readonly Action<T> ElementAdded;
        protected readonly Action<T> ElementRemoved;
        
        public SyncedList(Action<T> elementRemoved = null, Action<T> elementAdded = null, string id = "", bool uniqueID = false, Action onChanged = null) : base(new List<T>(), id, uniqueID, onChanged)
        {
            ElementRemoved = elementRemoved;
            ElementAdded = elementAdded;
        }

        public SyncedList(List<T> value, Action<T> elementRemoved = null, Action<T> elementAdded = null, string id = "", bool uniqueID = false, Action onChanged = null) : base(value, id, uniqueID, onChanged)
        {
            ElementRemoved = elementRemoved;
            ElementAdded = elementAdded;
        }

        private void Added(T value)
        {
            OnChanged?.Invoke();
            ElementAdded?.Invoke(value);
            ValueAdded(value);
        }

        private void Removed(T value)
        {
            OnChanged?.Invoke();
            ElementRemoved?.Invoke(value);
            ValueRemoved(value);
        }

        private void Change()
        {
            OnChanged?.Invoke();
            ValueChanged();
        }

        protected virtual void ValueAdded(T value)
        {
            SyncManager.AddedToList(ID, value);
        }

        protected virtual void ValueRemoved(T value)
        {
            SyncManager.RemovedFromList(ID, value);
        }

        protected override void ValueChanged()
        {
            SyncManager.Changed(ID, _Value.ToArray());
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
            Added(item);
        }

        public void AddAll(IEnumerable<T> items)
        {
            _Value.AddRange(items);
            Change();
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
            Removed(item);
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
            Added(item);
        }

        public void RemoveAt(int index)
        {
            var value = _Value[index];
            _Value.RemoveAt(index);
            Removed(value);
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
        
        public override void SyncSet(object val)
        {
            _Value = ((IEnumerable<object>)val).Cast<T>().ToList();
            OnChanged?.Invoke();
        }

        public virtual void SyncAdd(object value)
        {
            var value1 = (T) value;
            _Value.Add(value1);
            OnChanged?.Invoke();
            ElementAdded?.Invoke(value1);
        }

        public virtual void SyncRemove(object value)
        {
            var value1 = (T) value;
            _Value.Remove(value1);
            OnChanged?.Invoke();
            ElementRemoved?.Invoke(value1);
        }
    }
}