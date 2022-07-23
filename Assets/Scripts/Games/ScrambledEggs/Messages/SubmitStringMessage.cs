using ExitGames.Client.Photon;
using Games.ScrambledEggs.Data;
using Network;
using Utils;

namespace Games.ScrambledEggs.Messages
{
    public readonly struct SubmitStringMessage : IMessage
    {
        private readonly string _stringValue;
        private readonly int _submitter;
        private readonly int _stage;

        public SubmitStringMessage(Submission<string> submission, int stage)
        {
            _stringValue = submission.SubmissionContent;
            _submitter = submission.SubmitterActorID;
            _stage = stage;
        }

        public void Handle(EventData data)
        {
            var values = (string[])data.CustomData;
            GlobalData.Read<GameData>(GameConstants.GlobalData.ScrambledEggsGameData)
                .GetSimpleTasks(int.Parse(values[2]))
                .Add(new Submission<string>(int.Parse(values[1]), values[0]));
        }

        public object Send()
        {
            return new[]
            {
                _stringValue,
                _submitter.ToString(),
                _stage.ToString()
            };
        }
    }
}