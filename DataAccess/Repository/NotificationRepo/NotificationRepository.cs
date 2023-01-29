using Business.Repository.GenericRepo;
using DataAccess.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.NotificationRepo
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(SocialMediaContext context) : base(context)
        {
        }

        public async Task<ICollection<Notification>> GetAll(int userId)
        {
            return await context.Notifications.Where(x => x.UserId == userId).OrderByDescending(x => x.TimeStamp).ToListAsync();
        }
    }
}
