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

        public SerializedSyncedList(Serializer serializer, Deserializer deserializer, Action<T> elementRemoved = null,
            Action<T> elementAdded = null, string id = "", bool uniqueID = false, Action onChanged = null) : base(
            elementRemoved, elementAdded, id, uniqueID, onChanged)
        {
            _serializer = serializer;
            _deserializer = deserializer;
        }

        public SerializedSyncedList(Serializer serializer, Deserializer deserializer, List<T> value,
            Action<T> elementRemoved = null, Action<T> elementAdded = null, string id = "", bool uniqueID = false,
            Action onChanged = null) : base(value, elementRemoved, elementAdded, id, uniqueID, onChanged)
        {
            _serializer = serializer;
            _deserializer = deserializer;
        }

        protected override void ValueAdded(T value)
        {
            SyncManager.AddedToList(ID, _serializer(value));
        }

        protected override void ValueRemoved(T value)
        {
            SyncManager.RemovedFromList(ID, _serializer(value));
        }

        protected override void ValueChanged()
        {
            var n = _Value.Select(element => _serializer(element)).ToArray();
            SyncManager.Changed(ID, n);
        }

        public override void SyncSet(object val)
        {
            _Value = ((IEnumerable<object>)val).Select(element => _deserializer(element)).ToList();
            OnChanged?.Invoke();
        }

        public override void SyncAdd(object value)
        {
            base.SyncAdd(_deserializer(value));
        }

        public override void SyncRemove(object value)
        {
            base.SyncRemove(_deserializer(value));
        }
    }
}