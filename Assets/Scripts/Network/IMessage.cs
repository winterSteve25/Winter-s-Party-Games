using ExitGames.Client.Photon;

namespace Network
{
    public interface IMessage
    {
        void Handle(EventData data);

        object Send();
    }
}