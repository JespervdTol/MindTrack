using System;

namespace MindTrack.Module
{
    public class Score
    {
        public int ID { get; set; }
        public required string Game { get; set; }
        public int ScoreValue { get; set; }
        public DateTime DatePlayed { get; set; }
        public int PersonID { get; set; }
    }
}