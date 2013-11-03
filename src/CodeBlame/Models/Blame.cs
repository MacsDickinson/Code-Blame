using System;
using System.ComponentModel.DataAnnotations;
using CodeBlame.Models.Enums;

namespace CodeBlame.Models
{
    public class Blame
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public BlameLanguage Language { get; set; }

        public string LanguageName
        {
            get
            {
                var type = typeof(BlameLanguage);
                var member = type.GetMember(Language.ToString());
                var attributes = member[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                return ((DisplayAttribute)attributes[0]).Name;
            }
        }

        public string Code { get; set; }
        public DateTime DateAdded { get; set; }
    }
}