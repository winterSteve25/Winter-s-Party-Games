using System;
using System.Collections.Generic;
using System.Linq;

namespace Network.Sync
{
    public class SerializedSyncedList<T> : SyncedList<T>
    {
        public delegate object Serializer(T value);
        public delegate T Deserializer(object value);
        
        private readonly Serializer _serializer;
        private readonly Deserializer _deserializer;

        public SerializedSyncedList(Serializer serializer, Deserializer deserializer, Action<T> elementRemoved = null, Action<T> elementAdded = null, Action onChanged = null) : base(elementRemoved, elementAdded, onChanged)
        {
            _serializer = serializer;
            _deserializer = deserializer;
        }

        public SerializedSyncedList(Serializer serializer, Deserializer deserializer, List<T> value, Action<T> elementRemoved = null, Action<T> elementAdded = null, Action onChanged = null) : base(value, elementRemoved, elementAdded, onChanged)
        {
            _serializer = serializer;
            _deserializer = deserializer;
        }

        protected override void ValueChanged()
        {
            var n = _Value.Select(element => _serializer(element)).ToList();
            SyncManager.Changed(ID, n);
        }

        public override void Set(object val)
        {
            _Value = ((List<object>) val).Select(element => _deserializer(element)).ToList();
            OnChanged?.Invoke();
        }
    }
}