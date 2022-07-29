using System;
using System.Collections.Generic;
using Games.Base;
using Games.Base.Submissions;

namespace Games.ScrambledEggs.Data
{
    public class ScrambledEggsGameData : IGameData
    {
        private readonly List<Submission<string>>[] _wordTasks;
        private readonly List<ScrambledEggsPaintingSubmission>[] _drawingTasks;
        private int _roundOn;
        
        public ScrambledEggsGameData(int wordStagesCount, int drawingStages)
        {
            _wordTasks = new List<Submission<string>>[wordStagesCount];
            _drawingTasks = new List<ScrambledEggsPaintingSubmission>[drawingStages];
            _roundOn = 0;
        }

        public List<Submission<string>> GetWordTask(int index)
        {
            if (index >= _wordTasks.Length || index < 0) throw new ArgumentOutOfRangeException(nameof(index)); 
            _wordTasks[index] ??= new List<Submission<string>>();
            return _wordTasks[index];
        }

        public List<ScrambledEggsPaintingSubmission> GetDrawingTask(int index)
        {
            if (index >= _drawingTasks.Length || index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            _drawingTasks[index] ??= new List<ScrambledEggsPaintingSubmission>();
            return _drawingTasks[index];
        }

        public void NextRound()
        {
            _roundOn++;
        }

        public int GetRoundOn()
        {
            return _roundOn;
        }

        public int GetTotalRounds()
        {
            return 3;
        }
    }
}