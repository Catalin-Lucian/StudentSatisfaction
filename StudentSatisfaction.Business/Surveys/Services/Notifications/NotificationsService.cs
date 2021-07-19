using AutoMapper;
using StudentSatisfaction.Business.Surveys.Models.Notifications;
using StudentSatisfaction.Entities.Users;
using StudentSatisfaction.Persistence.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services.Notifications
{
    public sealed class NotificationsService : INotificationsService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public NotificationsService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        //return all notifications from a user
        public async Task<IEnumerable<NotificationModel>> GetAll(Guid userId)
        {
            var user = await _userRepository.GetUserById(userId);

            return _mapper.Map<IEnumerable<NotificationModel>>(user.Notifications);
        }

        public async Task<NotificationModel> GetById(Guid userId, Guid notificationId)
        {
            var user = await _userRepository.GetUserById(userId);
            var notification = user.Notifications.FirstOrDefault(c => c.Id == notificationId);

            return _mapper.Map<NotificationModel>(notification);
        }

        public async Task<NotificationModel> Add(Guid userId, CreateNotificationModel model)
        {
            var user = await _userRepository.GetUserById(userId);

            var notification = _mapper.Map<Notification>(model);

            //daca user-ul respectiv nu are notificarea respectiva, o adaug
            //if(!user.Notifications.Contains(notification))
            //{
                user.Notifications.Add(notification);
            //}

            _userRepository.Update(user);
            await _userRepository.SaveChanges();

            return _mapper.Map<NotificationModel>(notification);
        }

        public async Task Delete(Guid userId, Guid notificationId)
        {
            var user = await _userRepository.GetUserById(userId);
            var notificationToDelete = user.Notifications.FirstOrDefault(c => c.Id == notificationId);

            if(notificationToDelete != null)
            {
                user.Notifications.Remove(notificationToDelete);
            }

            _userRepository.Update(user);
            await _userRepository.SaveChanges();
        }

        public async Task Update(Guid userId, Guid notificationId, UpdateNotificationModel model)
        {
            var user = await _userRepository.GetUserById(userId);
            var notification = user.Notifications.FirstOrDefault(c => c.Id == notificationId);

            _mapper.Map(model, notification);
            _userRepository.Update(user);

            await _userRepository.SaveChanges();
        }
    }
}
