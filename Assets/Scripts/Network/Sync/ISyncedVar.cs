namespace Network.Sync
{
    public interface ISyncedVar
    {
        void Set(object val);

        object Get();
    }
}