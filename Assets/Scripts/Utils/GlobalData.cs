using System;
using System.Collections.Generic;

namespace Utils
{
    public static class GlobalData
    {
        public static readonly Dictionary<string, object> Data = new();

        public static void Set(string key, object value)
        {
            if (Data.ContainsKey(key))
            {
                Data[key] = value;
                return;
            }
            Data.Add(key, value);
        }

        public static T Read<T>(string key)
        {
            return (T)Data[key];
        }

        public static void ComputeIfPresent<T>(string key, Action<T> action)
        {
            if (Data.ContainsKey(key))
            {
                action((T) Data[key]);
            }
        }

        public static bool HasKey(string key)
        {
            return Data.ContainsKey(key);
        }

        public static bool ExistAnd<T>(string key, Predicate<T> condition)
        {
            return HasKey(key) && condition(Read<T>(key));
        }

        public static void Remove(string key)
        {
            Data.Remove(key);
        }
    }
}