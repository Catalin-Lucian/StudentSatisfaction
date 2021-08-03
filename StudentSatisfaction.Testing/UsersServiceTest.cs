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
    public class UsersServiceTest: IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ISurveyRepository> _surveyRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly IUsersService _sut;

        private readonly UserData _user;
        private readonly Survey _survey;

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        public UsersServiceTest()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _surveyRepositoryMock = _mockRepository.Create<ISurveyRepository>();
            _userRepositoryMock = _mockRepository.Create<IUserRepository>();

            _mapperMock = _mockRepository.Create<IMapper>();

            _sut = new UserService(_surveyRepositoryMock.Object, _userRepositoryMock.Object, _mapperMock.Object);
            _user = new UserData("UserData", "Username", "password", "Random name", "something@gmail.com",
                new DateTime(1999, 1, 12, 9, 10, 0), "AC");
            _survey = new Survey("IP - lecture", DateTime.Now, DateTime.Now.AddMonths(3));
        }

        [Fact]
        public void When_GetAllUsers_IsCalled_Expect_AllUsers_ToBeReturned()
        {
            //Arrange
            var user1 = new UserData("User1", "Username1", "password1", "Random name", "something@gmail.com",
                new DateTime(1999, 1, 12, 9, 10, 0), "AC");
            var user2 = new UserData("User2", "Username2", "password2", "Random name", "something@gmail.com",
                new DateTime(1999, 1, 12, 9, 10, 0), "AC");

            var model1 = new UserModel()
            {
                Id = Guid.NewGuid(),
                BirthDate = user1.BirthDate,
                Email = user1.Email,
                Name = user1.Name,
                FacultyName = user1.FacultyName,
                Password = user1.Password,
                Type = user1.Type,
                Username = user1.Username
            };

            var model2 = new UserModel()
            {
                Id = Guid.NewGuid(),
                BirthDate = user2.BirthDate,
                Email = user2.Email,
                Name = user2.Name,
                FacultyName = user2.FacultyName,
                Password = user2.Password,
                Type = user2.Type,
                Username = user2.Username
            };

            var usersList = new List<UserData> {user1, user2};
            var userModels = new List<UserModel> { model1, model2};

            _userRepositoryMock
                .Setup(m => m.GetAll())
                .Returns(usersList);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<UserModel>>(usersList))
                .Returns(userModels);

            //Act
            var result = _sut.GetAllUsers();

            //Assert
            result.Should().BeEquivalentTo(userModels);
        }

        [Fact]
        public async void When_GetUserByIdIsCalled_WithAnUserId_Expect_ThatUserToBeReturned()
        {
            //Arrange
            var expectedResult = new UserModel()
            {
                Id = Guid.NewGuid(),
                BirthDate = _user.BirthDate,
                Email = _user.Email,
                Name = _user.Name,
                FacultyName = _user.FacultyName,
                Password = _user.Password,
                Type = _user.Type,
                Username = _user.Username
            };

            _userRepositoryMock
                .Setup(m => m.GetUserById(_user.Id))
                .ReturnsAsync(_user);

            _mapperMock
                .Setup(m => m.Map<UserModel>(_user))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetUserById(_user.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_CreateIsCalled_WithACreateUserModel_Expect_ThatUserToBeCreated()
        {
            //Arrange
            var model = new CreateUserModel()
            {
                BirthDate = new DateTime(1999, 1, 12, 9, 10, 0),
                Email = "something@mail.com",
                Name = "name",
                FacultyName = "AC",
                Password = "pass",
                Type = "User",
                Username = "username"
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

            _mapperMock
                .Setup(m => m.Map<UserData>(model))
                .Returns(_user);

            _userRepositoryMock
                .Setup(m => m.Create(_user))
                .Returns(Task.CompletedTask);
            _userRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<UserModel>(_user))
                .Returns(expectedResult);

            //Act
            var result = await _sut.Create(model);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_DeleteIsCalled_WithAnUserId_Expect_ThatUserToBeDeleted()
        {
            //Arrange
            _userRepositoryMock
                .Setup(m => m.GetUserById(_user.Id))
                .ReturnsAsync(_user);

            _userRepositoryMock
                .Setup(m => m.Delete(_user));

            _userRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.Delete(_user.Id);
        }

        [Fact]
        public async void When_DeleteCredentialsIsCalled_WithAnUserId_Expect_ThatUsersCredentialsToBeDeleted()
        {
            //Arrange
            _userRepositoryMock
                .Setup(m => m.DeleteCredentials(_user.Id))
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.DeleteCredentials(_user.Id);
        }

        [Fact]
        public async void When_UpdateIsCalled_WithAnUserIdAndAnUpdateUserModel_Expect_ThatUserToBeUpdated()
        {
            //Arrange
            var model = new UpdateUserModel()
            {
                BirthDate = new DateTime(1999, 1, 12, 9, 10, 0),
                Email = "something@mail.com",
                Name = "name",
                FacultyName = "AC",
                Password = "pass",
                Type = "User",
                Username = "username"
            };

            _userRepositoryMock
                .Setup(m => m.GetUserById(_user.Id))
                .ReturnsAsync(_user);

            _mapperMock
                .Setup(m => m.Map(model, _user))
                .Returns(_user);

            _userRepositoryMock
                .Setup(m => m.Update(_user));

            _userRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.Update(_user.Id, model);
        }

        [Fact]
        public async void When_GetAnsweredSurveys_IsCalled_WithAnUserId_Expect_TheAnsweredSurveysOfThatUserToBeReturned()
        {
            //Arrange
            var expectedResult = new List<SurveyModel>();

            _userRepositoryMock
                .Setup(m => m.GetUserById(_user.Id))
                .ReturnsAsync(_user);

            var answeredSurveys = _user.Surveys;

            _mapperMock
                .Setup(m => m.Map<IEnumerable<SurveyModel>>(answeredSurveys))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetAnsweredSurveys(_user.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_GetNotAnsweredSurveys_IsCalled_WithAnUserId_Expect_TheNotAnsweredSurveysOfThatUserToBeReturned()
        {
            //Arrange
            var expectedResult = new List<SurveyModel>();
            var surveys = new List<Survey>();

            _userRepositoryMock
                .Setup(m => m.GetUserById(_user.Id))
                .ReturnsAsync(_user);

            _surveyRepositoryMock
                .Setup(m => m.GetAll())
                .Returns(surveys);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<SurveyModel>>(surveys))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetNotAnsweredSurveys(_user.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_GetUserFromSurveyIsCalled_WithASurveyIdAndAnUserId_Expect_TheUserWithThatIdTHatCompletedThatSurvey_ToBeReturned()
        {
            //Arrange
            _survey.Users.Add(_user);

            var expectedResult = new UserModel()
            {
                Id = Guid.NewGuid(),
                BirthDate = _user.BirthDate,
                Email = _user.Email,
                Name = _user.Name,
                FacultyName = _user.FacultyName,
                Password = _user.Password,
                Type = _user.Type,
                Username = _user.Username
            };

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .ReturnsAsync(_survey);

            _mapperMock
                .Setup(m => m.Map<UserModel>(_user))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetUserFromSurvey(_survey.Id, _user.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_GetAllUsersFromSurveyIsCalled_WithASurveyId_Expect_AllUserThatCompletedThatSurvey_ToBeReturned()
        {
            //Arrange
            var user1 = new UserData("User1", "Username1", "password1", "Random name", "something@gmail.com",
                new DateTime(1999, 1, 12, 9, 10, 0), "AC");
            var user2 = new UserData("User2", "Username2", "password2", "Random name", "something@gmail.com",
                new DateTime(1999, 1, 12, 9, 10, 0), "AC");

            _survey.Users.Add(user1);
            _survey.Users.Add(user2);

            var model1 = new UserModel()
            {
                Id = Guid.NewGuid(),
                BirthDate = user1.BirthDate,
                Email = user1.Email,
                Name = user1.Name,
                FacultyName = user1.FacultyName,
                Password = user1.Password,
                Type = user1.Type,
                Username = user1.Username
            };

            var model2 = new UserModel()
            {
                Id = Guid.NewGuid(),
                BirthDate = user2.BirthDate,
                Email = user2.Email,
                Name = user2.Name,
                FacultyName = user2.FacultyName,
                Password = user2.Password,
                Type = user2.Type,
                Username = user2.Username
            };

            var expectedResult = new List<UserModel> {model1, model2};

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .ReturnsAsync(_survey);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<UserModel>>(_survey.Users))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetAllUsersFromSurvey(_survey.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_AddUserToSurveyIsCalled_WithASurveyIdAndACreateUserMOdel_ExpectThatUserToBeAddedToTheSurvey()
        {
            //Arrange
            var model = new CreateUserModel()
            {
                BirthDate = new DateTime(1999, 1, 12, 9, 10, 0),
                Email = "something@mail.com",
                Name = "name",
                FacultyName = "AC",
                Password = "pass",
                Type = "User",
                Username = "username"
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

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .ReturnsAsync(_survey);

            _mapperMock
                .Setup(m => m.Map<UserData>(model))
                .Returns(_user);

            _surveyRepositoryMock
                .Setup(m => m.Update(_survey));

            _surveyRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<UserModel>(_user))
                .Returns(expectedResult);

            //Act
            var result = await _sut.AddUserToSurvey(_survey.Id, model);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_AddUserToSurveyIsCalled_WithASurveyIdAndnUserId_ExpectThatUserToBeAddedToTheSurvey()
        {
            //Arrange
            var expectedResult = new UserModel()
            {
                Id = Guid.NewGuid(),
                BirthDate = _user.BirthDate,
                Email = _user.Email,
                Name = _user.Name,
                FacultyName = _user.FacultyName,
                Password = _user.Password,
                Type = _user.Type,
                Username = _user.Username
            };

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .ReturnsAsync(_survey);

            _userRepositoryMock
                .Setup(m => m.GetUserById(_user.Id))
                .ReturnsAsync(_user);

            _surveyRepositoryMock
                .Setup(m => m.Update(_survey));
            _surveyRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<UserModel>(_user))
                .Returns(expectedResult);

            //Act
            var result = await _sut.AddUserToSurvey(_survey.Id, _user.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_DeleteFromSurvey_IsCalledWithASurveyIdAndAnUserId_Expect_ThatUserToBeDeletedFromTheSurvey()
        {
            //Arrange
            _survey.Users.Add(_user);
            var usersListSize = _survey.Users.Count();

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .ReturnsAsync(_survey);

            _surveyRepositoryMock
                .Setup(m => m.Update(_survey));
            _surveyRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.DeleteUserFromSurvey(_survey.Id, _user.Id);

            //Assert
            _survey.Users.Count().Should().Be(usersListSize - 1);
        }

        [Fact]
        public void When_CreateIsCalled_WithAnUserModel_ExpectThatUserToBeCreated()
        {
            var model = new CreateUserModel()
            {
                BirthDate = new DateTime(1999, 1, 12, 9, 10, 0),
                Email = "something@mail.com",
                Name = "name",
                FacultyName = "AC",
                Password = "pass",
                Type = "User",
                Username = "username"
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
        }
    }
}
