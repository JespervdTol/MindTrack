using System;

namespace MindTrack.Module
{
    public class Person
    {
        public int ID { get; set; }
        public int AccountID { get; set; }
        public required string Name { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public int? Score { get; set; }
        public DateTime? DatePlayed { get; set; }
    }
}