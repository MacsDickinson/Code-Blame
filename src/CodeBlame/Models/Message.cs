using System;

namespace CodeBlame.Models
{
    public class Message
    {
        public string Content { get; set; }
        public string To { get; set; }
        public DateTime Sent { get; set; }
    }
}