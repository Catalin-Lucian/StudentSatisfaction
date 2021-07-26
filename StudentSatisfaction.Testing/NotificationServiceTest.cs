using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using StudentSatisfaction.Business.Surveys.Models.Notifications;
using StudentSatisfaction.Business.Surveys.Services.Notifications;
using StudentSatisfaction.Business.Surveys.Services.Users;
using StudentSatisfaction.Entities.Users;
using StudentSatisfaction.Persistence;
using StudentSatisfaction.Persistence.Repositories.Users;
using Xunit;

namespace StudentSatisfaction.Testing
{
    public class NotificationServiceTest: IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly INotificationsService _sut;

        private readonly User _user;

        public NotificationServiceTest()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _userRepositoryMock = _mockRepository.Create<IUserRepository>();
            _mapperMock = _mockRepository.Create<IMapper>();

            _sut = new NotificationsService(_userRepositoryMock.Object, _mapperMock.Object);
            _user = new User("User", "Username", "password", "Random name", "something@gmail.com",
                new DateTime(1999, 1, 12, 9, 10, 0), "AC");
        }


        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async void When_GetAll_IsCalled_Expect_NotificationsFromUserToBeReturned()
        {
            //Arrange
            var expectedResult = _user.Notifications.Select(n => new NotificationModel()
            {
                Id = n.Id,
                UserId = n.UserId,
                Message = n.Message
            });

            _userRepositoryMock
                .Setup(m => m.GetUserById(_user.Id))
                .Returns(Task.FromResult(_user));

            _mapperMock
                .Setup(m => m.Map<IEnumerable<NotificationModel>>(_user.Notifications))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetAll(_user.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_GetById_IsCalledWithAnUserIdAndANotificationId_Expect_TheNotificationFromTheUserToBeReturned()
        {
            //Arrange
            var notification = new Notification(Guid.NewGuid(), "notification message");
            _user.Notifications.Add(notification);

            var expectedResult = new NotificationModel()
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Message = notification.Message
            };

            _userRepositoryMock
                .Setup(m => m.GetUserById(_user.Id))
                .Returns(Task.FromResult(_user));

            _mapperMock
                .Setup(m => m.Map<NotificationModel>(It.Is<Notification>(n => n.Id == expectedResult.Id)))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetById(_user.Id, expectedResult.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_AddIsCalled_WithAnUserIdAndACreateNotificationModel_ExpectThatNotification_ToBeAddedToTheUsersNotificationList()
        {
            //Arrange
            var model = new CreateNotificationModel()
            {
                UserId = Guid.NewGuid(),
                Message = "notification message"
            };

            var notification = new Notification(model.UserId, model.Message);

            var expectedResult = new NotificationModel()
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Message = notification.Message
            };

            var notificationListSize = _user.Notifications.Count();

            _userRepositoryMock
                .Setup(m => m.GetUserById(_user.Id))
                .Returns(Task.FromResult(_user));

            _mapperMock
                .Setup(m => m.Map<Notification>(model))
                .Returns(notification);

            _userRepositoryMock
                .Setup(m => m.Update(_user));

            _userRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<NotificationModel>(notification))
                .Returns(expectedResult);

            //Act
            var result = await _sut.Add(_user.Id, model);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
            _user.Notifications.Count().Should().Be(notificationListSize + 1);
        }

        [Fact]
        public async void When_DeleteIsCalled_WithAnUserIdAndANotificationId_Expect_ThatNotificationToBeRemovedFromTheUsersNotificationList()
        {
            //Arrange
            var notification = new Notification(Guid.NewGuid(), "notification message");
            _user.Notifications.Add(notification);

            var notificationListSize = _user.Notifications.Count();


            _userRepositoryMock
                .Setup(m => m.GetUserById(_user.Id))
                .Returns(Task.FromResult(_user));

            _userRepositoryMock
                .Setup(m => m.Update(_user));

            _userRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.Delete(_user.Id, notification.Id);

            //Assert
            _user.Notifications.Count().Should().Be(notificationListSize - 1);
        }

        [Fact]
        public async void When_UpdateIsCalled_WithAnUserIdAndANotificationIdAndAnUpdateNotificationModel_Expect_ThatNotificationToBe_UpdatedFromTheUsersNotificatuionList()
        {
            //Arrange
            var notification = new Notification(_user.Id, "notification to be updated");
            _user.Notifications.Add(notification);

            var model = new UpdateNotificationModel()
            {
                UserId = Guid.NewGuid(),
                Message = "updated notification message"
            };

            _userRepositoryMock
                .Setup(m => m.GetUserById(_user.Id))
                .Returns(Task.FromResult(_user));

            _mapperMock
                .Setup(m => m.Map(model, notification))
                .Returns(notification);

            _userRepositoryMock
                .Setup(m => m.Update(_user));

            _userRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.Update(_user.Id, notification.Id, model);
        }
    }
}
