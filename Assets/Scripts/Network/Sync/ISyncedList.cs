namespace Network.Sync
{
    public interface ISyncedList
    {
        void SyncAdd(object value);

        void SyncRemove(object value);
    }
}