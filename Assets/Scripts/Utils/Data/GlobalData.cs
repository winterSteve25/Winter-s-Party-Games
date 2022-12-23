using System;

namespace Utils.Data
{
    public static class GlobalData
    {
        private static readonly DataStorage Instance = new();

        public static void Set<T>(DataSignature<T> signature, T value)
        {
            Instance.Set(signature, value);
        }
        
        public static T Read<T>(DataSignature<T> signature)
        {
            return Instance.Read(signature);
        }

        public static void ComputeIfPresent<T>(DataSignature<T> signature, Action<T> action)
        {
            Instance.ComputeIfPresent(signature, action);
        }

        public static bool HasKey<T>(DataSignature<T> signature)
        {
            return Instance.HasKey(signature);
        }

        public static bool ExistAnd<T>(DataSignature<T> key, Predicate<T> condition)
        {
            return Instance.ExistAnd(key, condition);
        }

        public static bool NotExistOr<T>(DataSignature<T> key, Predicate<T> condition)
        {
            return Instance.NotExistOr(key, condition);
        }
        
        public static void Remove<T>(DataSignature<T> signature)
        {
            Instance.Remove(signature);
        }

        public static T GetOrDefault<T>(DataSignature<T> signature, T def)
        {
            return Instance.GetOrDefault(signature, def);
        }
    }
}