using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using StudentSatisfaction.Business.Surveys.Models.Ratings;
using StudentSatisfaction.Business.Surveys.Services.Questions;
using StudentSatisfaction.Business.Surveys.Services.Ratings;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Entities.Users;
using StudentSatisfaction.Persistence.Repositories.Questions;
using StudentSatisfaction.Persistence.Repositories.Users;
using Xunit;

namespace StudentSatisfaction.Testing
{
    public class RatingServiceTest : IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IQuestionRepository> _questionRepositoryMock;
        private readonly IRatingService _sut;

        private readonly UserData _user;
        private readonly Question _question;

        public RatingServiceTest()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mapperMock = _mockRepository.Create<IMapper>();
            _userRepositoryMock = _mockRepository.Create<IUserRepository>();
            _questionRepositoryMock = _mockRepository.Create<IQuestionRepository>();

            _sut = new RatingService(_userRepositoryMock.Object, _questionRepositoryMock.Object, _mapperMock.Object);

            _user = new UserData("UserData", "Username", "password", "Random name", "something@gmail.com",
                new DateTime(1999, 1, 12, 9, 10, 0), "AC");
            
            _question = new Question(Guid.NewGuid(), "plain text", "question text");
        }

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async void When_GetAllFromUser_IsCalled_WithAnUserId_Expect_AllTheUsersNotificationsToBeReturned()
        {
            //Arrange
            var expectedResult = _user.Ratings.Select(m => new RatingModel()
            {
                Id = m.Id,
                UserId = m.UserId,
                QuestionId = m.QuestionId,
                Answear = m.Answear,
                Points = m.Points
            });

            _userRepositoryMock
                .Setup(m => m.GetUserById(_user.Id))
                .ReturnsAsync(_user);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<RatingModel>>(_user.Ratings))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetAllFromUser(_user.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_GetAllFromQuestion_IsCalled_WithAQuestionId_Expect_AllRatingsFromThatQuestionToBeReturned()
        {
            //Arrange
            _question.Ratings.Add(new Rating(Guid.NewGuid(), Guid.NewGuid(), 5, ""));
            _question.Ratings.Add(new Rating(Guid.NewGuid(), Guid.NewGuid(), 3, "great"));

            var expectedResult = _question.Ratings.Select(m => new RatingModel()
            {
                Id = m.Id,
                UserId = m.UserId,
                QuestionId = m.QuestionId,
                Answear = m.Answear,
                Points = m.Points
            });

            _questionRepositoryMock
                .Setup(m => m.GetById(_question.Id))
                .ReturnsAsync(_question);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<RatingModel>>(_question.Ratings))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetAllFromQuestion(_question.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_AddIsCalled_WithAQuestionIdAndAnUserIdAndACreateRatingModel_Expect_ThatRatingToBeAddedToTheUsersRatingList_AndToTheQuestionsRatingList()
        {
            //Arrange
            var model = new CreateRatingModel()
            {
                UserId = Guid.NewGuid(),
                QuestionId = Guid.NewGuid(),
                Answear = "nice",
                Points = 5
            };

            var rating = new Rating(model.QuestionId, model.UserId, model.Points, model.Answear);

            var expectedResult = new RatingModel()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                QuestionId = Guid.NewGuid(),
                Answear = "nice",
                Points = 5
            };

            _questionRepositoryMock
                .Setup(m => m.GetById(_question.Id))
                .ReturnsAsync(_question);

            _userRepositoryMock
                .Setup(m => m.GetUserById(_user.Id))
                .ReturnsAsync(_user);

            _mapperMock
                .Setup(m => m.Map<Rating>(model))
                .Returns(rating);

            _userRepositoryMock
                .Setup(m => m.Update(_user));
            _userRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            _questionRepositoryMock
                .Setup(m => m.Update(_question));
            _questionRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<RatingModel>(rating))
                .Returns(expectedResult);

            //Act
            var result = await _sut.Add(_question.Id, _user.Id, model);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_Update_IsCalledWithAQuestionIdAndARatingIdAndAnUpdateRatingModel_Expect_ThatRatingToBeUpdatedInTheQuestionRatingsList()
        {
            //Arrange
            var ratingToBeUpdated = new Rating(Guid.NewGuid(), Guid.NewGuid(), 3, "text");
            _question.Ratings.Add(ratingToBeUpdated);

            var model = new UpdateRatingModel()
            {
                QuestionId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Answear = "updated text",
                Points = 3
            };

            var expectedResult = new Rating(model.QuestionId, model.UserId, model.Points, model.Answear);

            _questionRepositoryMock
                .Setup(m => m.GetById(_question.Id))
                .ReturnsAsync(_question);

            _mapperMock
                .Setup(m => m.Map(model, ratingToBeUpdated))
                .Returns(expectedResult);

            _questionRepositoryMock
                .Setup(m => m.Update(_question));
            _questionRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.Update(_question.Id, ratingToBeUpdated.Id, model);
        }

        [Fact]
        public async void When_DeleteIsCalled_WithAQuestionIdAndARatingId_Expect_ThatRatingToBeErasedFromTheQuestion()
        {
            //Arrange
            var rating = new Rating(Guid.NewGuid(), Guid.NewGuid(), 5, "nice");
            _question.Ratings.Add(rating);

            var ratingListSize = _question.Ratings.Count();

            _questionRepositoryMock
                .Setup(m => m.GetById(_question.Id))
                .ReturnsAsync(_question);

            _questionRepositoryMock
                .Setup(m => m.Update(_question));
            _questionRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.Delete(_question.Id, rating.Id);

            //Assert
            _question.Ratings.Count().Should().Be(ratingListSize - 1);
        }
    }
}
