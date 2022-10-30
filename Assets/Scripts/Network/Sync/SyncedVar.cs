using System;

namespace Network.Sync
{
    public class SyncedVar<T> : ISyncedVar
    {
        protected readonly SyncManager SyncManager;
        protected readonly int ID;
        protected readonly Action OnChanged;
        protected T _Value;

        public T Value
        {
            get => _Value;
            set
            {
                _Value = value;
                OnChanged?.Invoke();
                ValueChanged();
            }
        }

        public SyncedVar(Action onChanged = null) : this(default, onChanged)
        {
        }

        public SyncedVar(T value, Action onChanged = null)
        {
            SyncManager = SyncManager.Instance;
            _Value = value;
            ID = SyncManager.AddSynced(this);
            OnChanged = onChanged;
        }

        ~SyncedVar()
        {
            SyncManager.RemoveSynced(ID);
        }

        protected virtual void ValueChanged()
        {
            SyncManager.Changed(ID, _Value);
        }

        /// <summary>
        /// DO NOT call this. This is called by SyncManager to internally set the new value
        /// </summary>
        /// <param name="val"></param>
        public virtual void Set(object val)
        {
            _Value = (T)val;
            OnChanged?.Invoke();
        }
    }
}