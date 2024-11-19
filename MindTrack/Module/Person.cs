using System;

namespace MindTrack.Module
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        public int? Score { get; set; }
        public DateTime? DatePlayed { get; set; }
        public string ScoreFormatted { get; set; }
    }
}