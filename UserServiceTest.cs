using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using StudentSatisfaction.Business.Surveys.Models;
using StudentSatisfaction.Business.Surveys.Models.Users;
using StudentSatisfaction.Business.Surveys.Services.Users;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Entities.Users;
using StudentSatisfaction.Persistence;
using StudentSatisfaction.Persistence.Repositories.Users;
using Xunit;

namespace StudentSatisfaction.Testing
{
    public class UserServiceTest : IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ISurveyRepository> _surveyRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly IUsersService _sut;
        private readonly User _user;
        private readonly Survey _survey;

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        public UserServiceTest()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _surveyRepositoryMock = _mockRepository.Create<ISurveyRepository>();
            _userRepositoryMock = _mockRepository.Create<IUserRepository>();
            _mapperMock = _mockRepository.Create<IMapper>();
            _sut = new UserService(_surveyRepositoryMock.Object, _userRepositoryMock.Object, _mapperMock.Object);
            _user = new User("User", "Username", "Password", "Name", "email@gmail.com",new DateTime(1999, 7, 13, 10, 15, 0), "AC");
            _survey = new Survey("IP - lecture", DateTime.Now, DateTime.Now.AddMonths(3));
        }

        [Fact]
        public void When_GetAllUsers_IsCalled_Expect_AllUsersToBeReturned()
        {
            //Arrange
            var user1 = new User("User1", "Username1", "Password1", "Name", "email@gmail.com",new DateTime(1999, 7, 13, 10, 15, 0), "AC");
            var user2 = new User("User2", "Username2", "Password2", "Name", "email@gmail.com",new DateTime(1999, 7, 13, 10, 15, 0), "AC");

            var model1 = new UserModel()
            {
                Type = user1.Type,
                Id = Guid.NewGuid(),
                Username = user1.Username,
                Password = user1.Password,
                Name = user1.Name,
                Email = user1.Email,
                BirthDate = user1.BirthDate,
                FacultyName = user1.FacultyName,
                
            };

            var model2 = new UserModel()
            {
                Type = user2.Type,
                Id = Guid.NewGuid(),
                Username = user2.Username,
                Password = user2.Password,
                Name = user2.Name,
                Email = user2.Email,
                BirthDate = user2.BirthDate,
                FacultyName = user2.FacultyName,

            };

            var usersList = new List<User> { user1, user2 };
            var userModels = new List<UserModel> { model1, model2 };
            _userRepositoryMock.Setup(m => m.GetAll()).Returns(usersList);
            _mapperMock.Setup(m => m.Map<IEnumerable<UserModel>>(usersList)).Returns(userModels);

            //Act
            var result = _sut.GetAllUsers();

            //Assert
            result.Should().BeEquivalentTo(userModels);
        }

        [Fact]
        public async void When_GetUserByIdIsCalled_Expect_UserToBeReturned()
        {
            //Arrange
            var expectedResult = new UserModel()
            {
                Id = Guid.NewGuid(),
                Type = _user.Type,
                Username = _user.Username,
                Password = _user.Password,
                Name = _user.Name,
                Email = _user.Email,
                BirthDate = _user.BirthDate,
                FacultyName = _user.FacultyName,

            };

            _userRepositoryMock.Setup(m => m.GetUserById(_user.Id)).ReturnsAsync(_user);
            _mapperMock.Setup(m => m.Map<UserModel>(_user)).Returns(expectedResult);

            //Act
            var result = await _sut.GetUserById(_user.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_CreateIsCalled_Expect_UserToBeCreated()
        {
            //Arrange
            var model = new CreateUserModel()
            {
                Type = "User",
                Username = "Username",
                Password = "Password",
                Name = "Name",
                Email = "email@mail.com",
                BirthDate = new DateTime(1999, 7, 13, 10, 15, 0),
                FacultyName = "AC",

            };

            var expectedResult = new UserModel()
            {
                Id = Guid.NewGuid(),
                Type = model.Type,
                Username = model.Username,
                Password = model.Password,
                Name = model.Name,
                Email = model.Email,
                BirthDate = model.BirthDate,
                FacultyName = model.FacultyName,
            };

            _mapperMock.Setup(m => m.Map<User>(model)).Returns(_user);
            _userRepositoryMock.Setup(m => m.Create(_user)).Returns(Task.CompletedTask);
            _userRepositoryMock.Setup(m => m.SaveChanges()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<UserModel>(_user)).Returns(expectedResult);

            //Act
            var result = await _sut.Create(model);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_DeleteIsCalled_Expect_UserToBeDeleted()
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.GetUserById(_user.Id)).ReturnsAsync(_user);
            _userRepositoryMock.Setup(m => m.Delete(_user));
            _userRepositoryMock.Setup(m => m.SaveChanges()).Returns(Task.CompletedTask);

            //Act
            await _sut.Delete(_user.Id);
        }

        [Fact]
        public async void When_DeleteCredentialsIsCalled_Expect_UsersCredentialsToBeDeleted()
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.DeleteCredentials(_user.Id)).Returns(Task.CompletedTask);
            _userRepositoryMock.Setup(m => m.SaveChanges()).Returns(Task.CompletedTask);

            //Act
            await _sut.DeleteCredentials(_user.Id);
        }

        [Fact]
        public async void When_UpdateIsCalled_Expect_UserToBeUpdated()
        {
            //Arrange
            var model = new UpdateUserModel()
            {
                Type = "User",
                Username = "Username",
                Password = "Password",
                Name = "Name",
                Email = "email@mail.com",
                BirthDate = new DateTime(1999, 7, 13, 10, 15, 0),
                FacultyName = "AC",
                
            };

            _userRepositoryMock.Setup(m => m.GetUserById(_user.Id)).ReturnsAsync(_user);
            _mapperMock.Setup(m => m.Map(model, _user)).Returns(_user);
            _userRepositoryMock.Setup(m => m.Update(_user));
            _userRepositoryMock.Setup(m => m.SaveChanges()).Returns(Task.CompletedTask);

            //Act
            await _sut.Update(_user.Id, model);
        }

        [Fact]
        public async void When_GetAnsweredSurveys_IsCalled_Expect_TheAnsweredSurveysOfThatUserToBeReturned()
        {
            //Arrange
            var expectedResult = new List<SurveyModel>();
            _userRepositoryMock.Setup(m => m.GetUserById(_user.Id)).ReturnsAsync(_user);
            var answeredSurveys = _user.Surveys;

            _mapperMock.Setup(m => m.Map<IEnumerable<SurveyModel>>(answeredSurveys)).Returns(expectedResult);

            //Act
            var result = await _sut.GetAnsweredSurveys(_user.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_GetNotAnsweredSurveys_IsCalled_Expect_TheNotAnsweredSurveysOfThatUserToBeReturned()
        {
            //Arrange
            var expectedResult = new List<SurveyModel>();
            var surveys = new List<Survey>();
            _userRepositoryMock.Setup(m => m.GetUserById(_user.Id)).ReturnsAsync(_user);
            _surveyRepositoryMock.Setup(m => m.GetAll()).Returns(surveys);
            _mapperMock.Setup(m => m.Map<IEnumerable<SurveyModel>>(surveys)).Returns(expectedResult);

            //Act
            var result = await _sut.GetNotAnsweredSurveys(_user.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_GetUserFromSurveyIsCalled_Expect_TheUserWithThatIdTHatCompletedThatSurvey_ToBeReturned()
        {
            //Arrange
            _survey.Users.Add(_user);

            var expectedResult = new UserModel()
            {
                Id = Guid.NewGuid(),
                Type = _user.Type,
                Username = _user.Username,
                Password = _user.Password,
                Name = _user.Name,
                Email = _user.Email,
                BirthDate = _user.BirthDate,
                FacultyName = _user.FacultyName,
            };

            _surveyRepositoryMock.Setup(m => m.GetSurveyById(_survey.Id)).ReturnsAsync(_survey);
            _mapperMock.Setup(m => m.Map<UserModel>(_user)).Returns(expectedResult);

            //Act
            var result = await _sut.GetUserFromSurvey(_survey.Id, _user.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_GetAllUsersFromSurveyIsCalled_Expect_AllUserThatCompletedThatSurvey_ToBeReturned()
        {
            //Arrange
            var user1 = new User("User1", "Username1", "Password1", "Name", "email@gmail.com",new DateTime(1999, 7, 13, 10, 15, 0), "AC");
            var user2 = new User("User2", "Username2", "Password2", "Name", "email@gmail.com",new DateTime(1999, 7, 13, 10, 15, 0), "AC");
            _survey.Users.Add(user1);
            _survey.Users.Add(user2);

            var model1 = new UserModel()
            {
                Id = Guid.NewGuid(),
                Type = user1.Type,
                Username = user1.Username,
                Password = user1.Password,
                Name = user1.Name,
                Email = user1.Email,
                BirthDate = user1.BirthDate,
                FacultyName = user1.FacultyName,
            };

            var model2 = new UserModel()
            {
                Id = Guid.NewGuid(),
                Type = user2.Type,
                Username = user2.Username,
                Password = user2.Password,
                Name = user2.Name,
                Email = user2.Email,
                BirthDate = user2.BirthDate,
                FacultyName = user2.FacultyName,
            };

            var expectedResult = new List<UserModel> { model1, model2 };
            _surveyRepositoryMock.Setup(m => m.GetSurveyById(_survey.Id)).ReturnsAsync(_survey);
            _mapperMock.Setup(m => m.Map<IEnumerable<UserModel>>(_survey.Users)).Returns(expectedResult);

            //Act
            var result = await _sut.GetAllUsersFromSurvey(_survey.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_AddUserToSurveyIsCalled_Expect_UserToBeAddedToTheSurvey()
        {
            //Arrange
            var model = new CreateUserModel()
            {
                Type = "User",
                Username = "Username",
                Password = "Password",
                Name = "Name",
                Email = "email@mail.com",
                BirthDate = new DateTime(1999, 7, 13, 10, 15, 0),
                FacultyName = "AC",

            };

            var expectedResult = new UserModel()
            {
                Id = Guid.NewGuid(),
                BirthDate = model.BirthDate,
                Email = model.Email,
                Name = model.Name,
                FacultyName = model.FacultyName,
                Password = model.Password,
                Type = model.Type,
                Username = model.Username
            };

            _surveyRepositoryMock.Setup(m => m.GetSurveyById(_survey.Id)).ReturnsAsync(_survey);
            _mapperMock.Setup(m => m.Map<User>(model)).Returns(_user);
            _surveyRepositoryMock.Setup(m => m.Update(_survey));
            _surveyRepositoryMock.Setup(m => m.SaveChanges()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<UserModel>(_user)).Returns(expectedResult);

            //Act
            var result = await _sut.AddUserToSurvey(_survey.Id, model);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_AddUserToSurveyIsCalled_Expect_ThatUserToBeAddedToTheSurvey()
        {
            //Arrange
            var expectedResult = new UserModel()
            {
                Id = Guid.NewGuid(),
                Type = _user.Type,
                Username = _user.Username,
                Password = _user.Password,
                Name = _user.Name,
                Email = _user.Email,
                BirthDate = _user.BirthDate,
                FacultyName = _user.FacultyName,
            };

            _surveyRepositoryMock.Setup(m => m.GetSurveyById(_survey.Id)).ReturnsAsync(_survey);
            _userRepositoryMock.Setup(m => m.GetUserById(_user.Id)).ReturnsAsync(_user);
            _surveyRepositoryMock.Setup(m => m.Update(_survey));
            _surveyRepositoryMock.Setup(m => m.SaveChanges()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<UserModel>(_user)).Returns(expectedResult);

            //Act
            var result = await _sut.AddUserToSurvey(_survey.Id, _user.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_DeleteFromSurvey_IsCalled_Expect_ThatUserToBeDeletedFromTheSurvey()
        {
            //Arrange
            _survey.Users.Add(_user);
            var usersListSize = _survey.Users.Count();
            _surveyRepositoryMock.Setup(m => m.GetSurveyById(_survey.Id)).ReturnsAsync(_survey);
            _surveyRepositoryMock.Setup(m => m.Update(_survey));
            _surveyRepositoryMock.Setup(m => m.SaveChanges()).Returns(Task.CompletedTask);

            //Act
            await _sut.DeleteUserFromSurvey(_survey.Id, _user.Id);

            //Assert
            _survey.Users.Count().Should().Be(usersListSize - 1);
        }

        [Fact]
        public void When_CreateIsCalled_ExpectThatUserToBeCreated()
        {
            var model = new CreateUserModel()
            {
                Type = "User",
                Username = "Username",
                Password = "Password",
                Name = "Name",
                Email = "email@mail.com",
                BirthDate = new DateTime(1999, 7, 13, 10, 15, 0),
                FacultyName = "AC",
            };

            var expectedResult = new UserModel()
            {
                Id = Guid.NewGuid(),
                Type = model.Type,
                Username = model.Username,
                Password = model.Password,
                Name = model.Name,
                Email = model.Email,
                BirthDate = model.BirthDate,
                FacultyName = model.FacultyName,
            };
        }
    }
}
