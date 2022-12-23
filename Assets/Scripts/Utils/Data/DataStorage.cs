using System;
using System.Collections.Generic;

namespace Utils.Data
{
    public class DataStorage
    {
        private readonly Dictionary<string, object> _data;

        public DataStorage()
        {
            _data = new Dictionary<string, object>();
        }

        private void Set(string key, object value)
        {
            if (_data.ContainsKey(key))
            {
                _data[key] = value;
                return;
            }
            _data.Add(key, value);
        }
        
        public void Set<T>(DataSignature<T> signature, T value)
        {
            Set(signature.Key, value);
        }

        private T Read<T>(string key)
        {
            return (T)_data[key];
        }

        public T Read<T>(DataSignature<T> signature)
        {
            return Read<T>(signature.Key);
        }

        private void ComputeIfPresent<T>(string key, Action<T> action)
        {
            if (_data.ContainsKey(key))
            {
                action((T) _data[key]);
            }
        }
        
        public void ComputeIfPresent<T>(DataSignature<T> signature, Action<T> action)
        {
            ComputeIfPresent(signature.Key, action);
        }

        private bool HasKey(string key)
        {
            return _data.ContainsKey(key);
        }

        public bool HasKey<T>(DataSignature<T> signature)
        {
            return HasKey(signature.Key);
        }

        private bool ExistAnd<T>(string key, Predicate<T> condition)
        {
            return HasKey(key) && condition(Read<T>(key));
        }

        public bool ExistAnd<T>(DataSignature<T> key, Predicate<T> condition)
        {
            return ExistAnd(key.Key, condition);
        }

        private bool NotExistOr<T>(string key, Predicate<T> condition)
        {
            return !HasKey(key) || condition(Read<T>(key));
        }
        
        public bool NotExistOr<T>(DataSignature<T> key, Predicate<T> condition)
        {
            return NotExistOr(key.Key, condition);
        }
        
        private void Remove(string key)
        {
            _data.Remove(key);
        }

        public void Remove<T>(DataSignature<T> signature)
        {
            Remove(signature.Key);
        }

        private T GetOrDefault<T>(string key, T def)
        {
            if (HasKey(key))
            {
                return (T)_data[key];
            }

            return def;
        }
        
        public T GetOrDefault<T>(DataSignature<T> signature, T def)
        {
            return GetOrDefault(signature.Key, def);
        }
    }
}