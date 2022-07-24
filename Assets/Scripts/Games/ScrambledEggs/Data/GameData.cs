using System;
using System.Collections.Generic;
using UnityEngine;

namespace Games.ScrambledEggs.Data
{
    public class GameData
    {
        private readonly List<Submission<string>> _stage1Submissions;
        private readonly List<Submission<string>> _stage2Submissions;
        private readonly List<Submission<string>> _stage3Submissions;
        private readonly List<Submission<string>> _stage4Submissions;
        public readonly List<PaintingSubmission> Stage5Submissions;

        public GameData()
        {
            _stage1Submissions = new List<Submission<string>>();
            _stage2Submissions = new List<Submission<string>>();
            _stage3Submissions = new List<Submission<string>>();
            _stage4Submissions = new List<Submission<string>>();
            Stage5Submissions = new List<PaintingSubmission>();
        }

        public List<Submission<string>> GetSimpleTasks(int stage)
        {
            return stage switch
            {
                1 => _stage1Submissions,
                2 => _stage2Submissions,
                3 => _stage3Submissions,
                4 => _stage4Submissions,
                _ => throw new ArgumentOutOfRangeException(nameof(stage))
            };
        }
    }
}