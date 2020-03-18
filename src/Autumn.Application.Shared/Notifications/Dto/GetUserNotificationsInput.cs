using Abp.Notifications;
using Autumn.Dto;

namespace Autumn.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }
    }
}