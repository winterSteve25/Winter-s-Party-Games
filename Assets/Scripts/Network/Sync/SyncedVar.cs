using System;

namespace Network.Sync
{
    public class SyncedVar<T> : ISyncedVar
    {
        public Action OnChanged;
        
        protected readonly SyncManager SyncManager;
        protected readonly string ID;
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

        public SyncedVar(string id = "", bool uniqueID = false, Action onChanged = null) : this(default, id, uniqueID, onChanged)
        {
        }

        public SyncedVar(T value, string id = "", bool uniqueID = false, Action onChanged = null)
        {
            SyncManager = SyncManager.Instance;
            _Value = value;
            OnChanged = onChanged;
            ID = SyncManager.AddSynced(this, id, uniqueID);
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
        /// Called by SyncManager to internally set the new value. Does not notify change and will not cause the variable to sync
        /// </summary>
        /// <param name="val"></param>
        public virtual void SyncSet(object val)
        {
            _Value = (T)val;
            OnChanged?.Invoke();
        }

        public static implicit operator SyncedVar<T>(T value)
        {
            return new SyncedVar<T>(value);
        }
    }
}