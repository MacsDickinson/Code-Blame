using System;

namespace JustForFun.NancyAndEventStore.Web.Models
{
    public class Message
    {
        public string Content { get; set; }
        public string To { get; set; }
        public DateTime Sent { get; set; }
    }
}