using System.Collections.Generic;

namespace JustForFun.NancyAndEventStore.Web.Models
{
    public class PageModel
    {
        public string TitleSuffix { get; set; }
        public string Title { get; set; }
        public bool IsAuthenticated { get; set; }
        public List<NotificationModel> Notifications { get; set; }
    }
}