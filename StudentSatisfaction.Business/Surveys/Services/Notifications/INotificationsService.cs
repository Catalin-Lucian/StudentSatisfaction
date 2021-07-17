using StudentSatisfaction.Business.Surveys.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services.Notifications
{
    public interface INotificationsService
    {
        Task<NotificationModel> GetById(Guid userId, Guid notificationId);
        Task<IEnumerable<NotificationModel>> GetAll(Guid userId);
        Task<NotificationModel> Add(Guid userId, CreateNotificationModel model);
        Task Delete(Guid userId, Guid notificationId);
        Task Update(Guid userId, Guid notificationId, UpdateNotificationModel model);
    }
}
