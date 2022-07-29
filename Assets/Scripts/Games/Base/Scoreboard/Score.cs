using System;

namespace Games.Base.Scoreboard
{
    [Serializable]
    public class Score
    {
        public int score;
        public int actorID;
        public ScoreboardItem item;

        public Score(int score, int actorID, ScoreboardItem item)
        {
            this.score = score;
            this.actorID = actorID;
            this.item = item;
        }
    }
}