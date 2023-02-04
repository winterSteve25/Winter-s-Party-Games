using System;

namespace Network.Sync
{
    public class SerializedSyncedVar<T> : SyncedVar<T>
    {
        public delegate object Serializer(T value);
        public delegate T Deserializer(object value);
        
        private readonly Serializer _serializer;
        private readonly Deserializer _deserializer;

        public SerializedSyncedVar(Serializer serializer, Deserializer deserializer, string id = "", bool uniqueID = false, Action onChanged = null) : base(id, uniqueID, onChanged)
        {
            _serializer = serializer;
            _deserializer = deserializer;
        }

        public SerializedSyncedVar(Serializer serializer, Deserializer deserializer, T value, string id = "", bool uniqueID = false, Action onChanged = null) : base(value, id, uniqueID, onChanged)
        {
            _serializer = serializer;
            _deserializer = deserializer;
        }

        protected override void ValueChanged()
        {
            SyncManager.Changed(ID, _serializer(_Value));
        }

        public override void SyncSet(object val)
        {
            _Value = _deserializer(val);
            OnChanged?.Invoke();
        }
    }
}