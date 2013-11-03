using System;
using CodeBlame.Models.Enums;

namespace CodeBlame.Models
{
    public class Blame
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public BlameLanguage Language { get; set; }
        public string Code { get; set; }
        public DateTime DateAdded { get; set; }
    }
}