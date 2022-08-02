namespace Network.Sync
{
    public class SyncedVar<T> : ISyncedVar
    {
        private int _id;
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                ValueChanged();
            }
        }

        public SyncedVar() : this(default)
        {
        }

        public SyncedVar(T value)
        {
            _value = value;
        }

        private void ValueChanged()
        {
        }

        public void Set(object val)
        {
            Value = (T)val;
        }

        public object Get()
        {
            return Value;
        }
    }
}