using JustForFun.NancyAndEventStore.Web.Models.Enums;

namespace CodeBlame.Models
{
    public class NotificationModel
    {
        public string Message { get; set; }
        public NotificationType Type { get; set; }
    }
}