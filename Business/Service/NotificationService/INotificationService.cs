using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.NotificationService
{
    public interface INotificationService
    {
        Task SendNotificationAfterInsertAsync(Notification notification);
        Task UpdateIsClickNotification(int id);
        Task<ICollection<Notification>> GetAllNotifications();
    }
}
