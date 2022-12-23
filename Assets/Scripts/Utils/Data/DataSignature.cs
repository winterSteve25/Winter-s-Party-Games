namespace Utils.Data
{
    public readonly struct DataSignature<T>
    {
        public readonly string Key;

        public DataSignature(string key)
        {
            Key = key;
        }
    }
}