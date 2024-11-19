using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindTrack.Module
{
    public class Account
    {
        public int ID { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
