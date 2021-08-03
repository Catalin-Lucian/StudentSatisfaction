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
        public async void When_GetAllFromQuestionIsCalled_WithAQuestionId_Expect_AllTheRatingsFromThatQuestionToBeReturned()
        {
            //Arrange
            var r1 = new Rating(Guid.NewGuid(), Guid.NewGuid(), 5, "rating text 1");
            var r2 = new Rating(Guid.NewGuid(), Guid.NewGuid(), 3, "rating text 2");

            _question.Ratings.Add(r1);
            _question.Ratings.Add(r2);

            var model1 = new RatingModel()
            {
                Id = r1.Id,
                Answear = r1.Answear,
                Points = r1.Points,
                UserId = r1.UserId
            };

            var model2 = new RatingModel()
            {
                Id = r2.Id,
                Answear = r2.Answear,
                Points = r2.Points,
                UserId = r2.UserId
            };

            var expectedResult = new List<RatingModel>{model1, model2};

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
        public async void When_GetQuestionRatingFromUser_IsCalledWithAnUserIdAndAQuestionId_Expect_TheRatingWithThoseFieldsToBeReturned()
        {
            //Arrange
            var rating = new Rating(_question.Id, Guid.NewGuid(), 5, "rating text 1");
            var expectedResult = new RatingModel()
            {
                Id = rating.Id,
                Answear = rating.Answear,
                Points = rating.Points,
                UserId = rating.UserId
            };

            _user.Ratings.Add(rating);
            _question.Ratings.Add(rating);

            _userRepositoryMock
                .Setup(m => m.GetUserById(_user.Id))
                .ReturnsAsync(_user);

            _mapperMock
                .Setup(m => m.Map<RatingModel>(It.Is<Rating>(r => r.Id == rating.Id)))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetQuestionRatingFromUser(_user.Id, _question.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void When_GetAllFromSurvey_IsCalled_WithASurveyId_Expect_AllRatingsFromThatSurveyToBeReturned()
        {
            //Arrange
            var survey = new Survey("IP - lecture", DateTime.Now, DateTime.Now.AddMonths(3));
            var q1 = new Question(Guid.NewGuid(), "plain text", "question text1");
            var q2 = new Question(Guid.NewGuid(), "plain text", "question text2");

            var questions = new List<Question>{q1, q2};
            var ratings = new List<Rating>();

            foreach (var question in questions)
            {
                ratings.AddRange(question.Ratings);
            }

            var expectedResult = ratings.Select(r => new RatingModel()
            {
                Id = r.Id,
                Answear = r.Answear,
                Points = r.Points,
                QuestionId = r.QuestionId,
                UserId = r.UserId
            });

            _questionRepositoryMock
                .Setup(m => m.GetAll())
                .Returns(questions);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<RatingModel>>(ratings))
                .Returns(expectedResult);

            //Act
            var result = _sut.GetAllFromSurvey(survey.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_GetRatingIsCalled_WithAQuestionIdAndARatingId_Expect_ThatRatingToBEReturned()
        {
            //Arrange
            var rating = new Rating(Guid.NewGuid(), Guid.NewGuid(), 5, "rating text 1");
            _question.Ratings.Add(rating);
            _user.Ratings.Add(rating);

            var expectedResult = new RatingModel()
            {
                Id = rating.Id,
                Answear = rating.Answear,
                Points = rating.Points,
                UserId = rating.UserId
            };

            _questionRepositoryMock
                .Setup(m => m.GetById(_question.Id))
                .ReturnsAsync(_question);

            _mapperMock
                .Setup(m => m.Map<RatingModel>(rating))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetRating(_question.Id, rating.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void When_GetUserRatingFromSurvey_IsCalled_WithASurveyIdAndAnUserId_Expect_AllRatingsFromThatUserAndSurveyToBEReturned()
        {
            //Arrange
            var survey = new Survey("IP - lecture", DateTime.Now, DateTime.Now.AddMonths(3));
            var q1 = new Question(Guid.NewGuid(), "plain text", "question text1");
            var q2 = new Question(Guid.NewGuid(), "plain text", "question text2");

            var questions = new List<Question> { q1, q2 };
            var ratings = new List<Rating>();

            foreach (var question in questions)
            {
                ratings.AddRange(question.Ratings);
            }

            var expectedResult = ratings.Select(r => new RatingModel()
            {
                Id = r.Id,
                Answear = r.Answear,
                Points = r.Points,
                QuestionId = r.QuestionId,
                UserId = r.UserId
            });

            _questionRepositoryMock
                .Setup(m => m.GetAll())
                .Returns(questions);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<RatingModel>>(ratings))
                .Returns(expectedResult);

            //Act
            var result = _sut.GetUserRatingFromSurvey(survey.Id, _user.Id);

            //Assert
            result.Should().BeEquivalentTo(ratings);
        }

        [Fact]
        public async void When_GetAllFromQuestion_IsCalledWithAQuestionId_Expect_AllRatingsFromThatQuestionToBeReturned()
        {
            //Arrange
            var r1 = new Rating(Guid.NewGuid(), Guid.NewGuid(), 5, "rating text 1");
            var r2 = new Rating(Guid.NewGuid(), Guid.NewGuid(), 3, "rating text 2");

            _user.Ratings.Add(r1);
            _user.Ratings.Add(r2);

            _question.Ratings.Add(r1);
            _question.Ratings.Add(r2);

            var expectedResult = _question.Ratings.Select(q => new RatingModel()
            {
                Id = q.Id,
                Answear = q.Answear,
                Points = q.Points,
                QuestionId = q.QuestionId,
                UserId = q.UserId
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
        public async void When_GetAllFromUser_IsCalled_WithAnUserId_Expect_AllTheUsersNotificationsToBeReturned()
        {
            //Arrange
            var r1 = new Rating(Guid.NewGuid(), Guid.NewGuid(), 5, "rating text 1");
            var r2 = new Rating(Guid.NewGuid(), Guid.NewGuid(), 3, "rating text 2");

            _user.Ratings.Add(r1);
            _user.Ratings.Add(r2);

            _question.Ratings.Add(r1);
            _question.Ratings.Add(r2);

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
