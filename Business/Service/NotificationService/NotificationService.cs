using Business.Repository.UserRepo;
using DataAccess.Entities;
using DataAccess.Repository.NotificationRepo;
using DataAccess.Repository.UserTypeRepo;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;
namespace Business.Service.NotificationService
{
    public class NotificationService : BaseService, INotificationService
    {

        private readonly INotificationRepository _notificationRepository;
        private string ClassName = typeof(Notification).Name;
        public NotificationService(IHttpContextAccessor httpContextAccessor
            , IUserRepository userRepository, IUserTypeRepository userTypeRepository
            , INotificationRepository notificationRepository) : base(httpContextAccessor, userRepository, userTypeRepository)
        {

            _notificationRepository = notificationRepository;
        }

        public async Task<ICollection<Notification>> GetAllNotifications()
        {
            int userId = GetCurrentUserId();
            return await _notificationRepository.GetAll(userId);
        }

        public async Task SendNotificationAfterInsertAsync(Notification notification)
        {
            notification.IsClick = false;
            await _notificationRepository.Insert(notification);

        }

        public async Task UpdateIsClickNotification(int id)
        {
            if ((await _notificationRepository.Get(x => x.Id == id)) == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            int userId = GetCurrentUserId();
            var notification = await _notificationRepository.Get(n => n.Id == id && n.UserId == userId && n.IsClick == false);
            if (notification == null)
            {
                throw new Exception(ClassName + " " + INVALID);
            }
            else
            {
                notification.IsClick = true;
                await _notificationRepository.Update(notification);
            }
            
        }
    }
}
