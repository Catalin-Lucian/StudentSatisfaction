using AutoMapper;
using Moq;
using StudentSatisfaction.Persistence.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using StudentSatisfaction.Business.Surveys.Models.SubmittedQuestions;
using StudentSatisfaction.Business.Surveys.Services.Ratings;
using StudentSatisfaction.Business.Surveys.Services.SubmittedQuestions;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Entities.Users;
using StudentSatisfaction.Persistence;
using StudentSatisfaction.Persistence.Repositories.SubmittedQuestions;
using Xunit;

namespace StudentSatisfaction.Testing
{
    public class SubmittedQuestionTest: IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<ISubmittedQuestionRepository> _submittedQuestionRepositoryMock;
        private readonly Mock<ISurveyRepository> _surveyRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ISubmittedQuestionsService _sut;

        private readonly UserData _user;
        private readonly Survey _survey;
        private readonly SubmittedQuestion _submittedQuestion;

        public SubmittedQuestionTest()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _submittedQuestionRepositoryMock = _mockRepository.Create<ISubmittedQuestionRepository>();
            _surveyRepositoryMock = _mockRepository.Create<ISurveyRepository>();
            _userRepositoryMock = _mockRepository.Create<IUserRepository>();
            _mapperMock = _mockRepository.Create<IMapper>();

            _sut = new SubmittedQuestionService(_surveyRepositoryMock.Object, _userRepositoryMock.Object,
                _submittedQuestionRepositoryMock.Object, _mapperMock.Object);

            _user = new UserData("UserData", "Username", "password", "Random name", "something@gmail.com",
                new DateTime(1999, 1, 12, 9, 10, 0), "AC");
            _survey = new Survey("IP - lecture", DateTime.Today, DateTime.Today.AddMonths(10));
            _submittedQuestion = new SubmittedQuestion(Guid.NewGuid(), Guid.NewGuid(), "text");
        }

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async void When_GetAllFromSurveyIsCalled_WithASurveyId_Expect_AllSubmittedQuestionsForThatSurveyToBeReturned()
        {
            //Arrange
            _survey.SubmittedQuestions.Add(new SubmittedQuestion(Guid.NewGuid(), Guid.NewGuid(), "question1 text"));
            _survey.SubmittedQuestions.Add(new SubmittedQuestion(Guid.NewGuid(), Guid.NewGuid(), "question2 text"));

            var expectedResult = _survey.SubmittedQuestions.Select(m => new SubmittedQuestionsModel()
            {
                Id = m.Id,
                QuestionText = m.QuestionText,
                SurveyId = m.SurveyId,
                UserId = m.UserId
            });

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .ReturnsAsync(_survey);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<SubmittedQuestionsModel>>(_survey.SubmittedQuestions))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetAllFromSurvey(_survey.Id);

            //Arrange
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_GetAllFromUser_IsCalledWithAnUserId_Expect_AllQuestionsSubmittedByThatUserToBeReturned()
        {
            //Arrange
            _user.SubmittedQuestions.Add(new SubmittedQuestion(Guid.NewGuid(), Guid.NewGuid(), "question1 text"));
            _user.SubmittedQuestions.Add(new SubmittedQuestion(Guid.NewGuid(), Guid.NewGuid(), "question2 text"));

            var expectedResult = _user.SubmittedQuestions.Select(m => new SubmittedQuestionsModel()
            {
                Id = m.Id,
                QuestionText = m.QuestionText,
                SurveyId = m.SurveyId,
                UserId = m.UserId
            });

            _userRepositoryMock
                .Setup(m => m.GetUserById(_survey.Id))
                .ReturnsAsync(_user);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<SubmittedQuestionsModel>>(_user.SubmittedQuestions))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetAllFromUser(_survey.Id);

            //Arrange
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_GetQuestionFromSurvey_IsCalledWithASubmittedQuestionIdAndASurveyId_ExpectThatSubmittedQuestionToBeReturnedFromASurvey()
        {
            //Arrange
            var sq1 = new SubmittedQuestion(Guid.NewGuid(), Guid.NewGuid(), "question1 text");
            var sq2 = new SubmittedQuestion(Guid.NewGuid(), Guid.NewGuid(), "question2 text");

            _survey.SubmittedQuestions.Add(sq1);
            _survey.SubmittedQuestions.Add(sq2);

            var expectedResult = new SubmittedQuestionsModel()
            {
                Id = sq1.Id,
                QuestionText = sq1.QuestionText,
                SurveyId = sq1.SurveyId,
                UserId = sq1.UserId
            };

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .ReturnsAsync(_survey);

            _mapperMock
                .Setup(m => m.Map<SubmittedQuestionsModel>(It.Is<SubmittedQuestion>(t => t.Id == sq1.Id)))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetQuestionFromSurvey(_survey.Id, sq1.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_GetQuestionFromUser_IsCalledWithASubmittedQuestionIdAndAnUserId_ExpectThatSubmittedQuestionToBeReturnedFromAUser()
        {
            //Arrange
            var sq1 = new SubmittedQuestion(Guid.NewGuid(), Guid.NewGuid(), "question1 text");
            var sq2 = new SubmittedQuestion(Guid.NewGuid(), Guid.NewGuid(), "question2 text");

            _user.SubmittedQuestions.Add(sq1);
            _user.SubmittedQuestions.Add(sq2);

            var expectedResult = new SubmittedQuestionsModel()
            {
                Id = sq1.Id,
                QuestionText = sq1.QuestionText,
                SurveyId = sq1.SurveyId,
                UserId = sq1.UserId
            };

            _userRepositoryMock
                .Setup(m => m.GetUserById(_user.Id))
                .ReturnsAsync(_user);

            _mapperMock
                .Setup(m => m.Map<SubmittedQuestionsModel>(It.Is<SubmittedQuestion>(t => t.Id == sq1.Id)))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetQuestionFromUser(_user.Id, sq1.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_AddIsCalled_WithASurveyIdAndACreateSubmittedQuestionsModel_ExpectThatSubmittedQuestionToBeAddedToTheSurvey()
        {
            //Arrange
            var model = new CreateSubmittedQuestionModel()
            {
                UserId = Guid.NewGuid(),
                SurveyId = Guid.NewGuid(),
                QuestionText = "text"
            };

            var submittedQuestion = new SubmittedQuestion(model.SurveyId, model.UserId, model.QuestionText);

            var expectedResult = new SubmittedQuestionsModel()
            {
                Id = submittedQuestion.Id,
                QuestionText = submittedQuestion.QuestionText,
                SurveyId = submittedQuestion.SurveyId,
                UserId = submittedQuestion.UserId
            };

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .ReturnsAsync(_survey);

            _userRepositoryMock
                .Setup(m => m.GetUserById(_user.Id))
                .ReturnsAsync(_user);

            _mapperMock
                .Setup(m => m.Map<SubmittedQuestion>(model))
                .Returns(submittedQuestion);

            _submittedQuestionRepositoryMock
                .Setup(m => m.Create(submittedQuestion))
                .Returns(Task.CompletedTask);

            _submittedQuestionRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            _surveyRepositoryMock
                .Setup(m => m.Update(_survey));
            _surveyRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(m => m.Update(_user));
            _userRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<SubmittedQuestionsModel>(submittedQuestion))
                .Returns(expectedResult);

            //Act
            var result = await _sut.Add(_survey.Id, _user.Id, model);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_UpdateIsCalled_WithASurveyIdAndASubmittedQUestionIdAndAnUpdateSubmittedQuestionModel_Expect_ThatQuestionToBeUpdated()
        {
            //Arrange
            var questionToBeUpdated = new SubmittedQuestion(Guid.NewGuid(), Guid.NewGuid(), "text");
            _survey.SubmittedQuestions.Add(questionToBeUpdated);

            var model = new UpdateSubmittedQuestionModel()
            {
                QuestionText = "updated text"
            };

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .ReturnsAsync(_survey);

            _mapperMock
                .Setup(m => m.Map(model, questionToBeUpdated))
                .Returns(questionToBeUpdated);
            _surveyRepositoryMock
                .Setup(m => m.Update(_survey));
            _surveyRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.Update(_survey.Id, questionToBeUpdated.Id, model);
        }

        [Fact]
        public async void When_DeleteFromSurveyIsCalled_WithASurveyIdAndASubmittedQuestionId_Expect_ThatSubmittedQuestionToBeDeletedFromThatSurvey()
        {
            //Arrange
            var submittedQuestion = new SubmittedQuestion(Guid.NewGuid(), Guid.NewGuid(), "text");

            _survey.SubmittedQuestions.Add(submittedQuestion);
            var submittedQuestionListSize = _survey.SubmittedQuestions.Count();

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .ReturnsAsync(_survey);

            _surveyRepositoryMock
                .Setup(m => m.Update(_survey));
            _surveyRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.DeleteFromSurvey(_survey.Id, submittedQuestion.Id);

            //Assert
            _survey.SubmittedQuestions.Count().Should().Be(submittedQuestionListSize - 1);
        }

        [Fact]
        public async void When_DeleteFromUserIsCalled_WithAnUserIdAndASubmittedQuestionId_Expect_ThatSubmittedQuestionToBeDeletedFromThatUser()
        {
            //Arrange
            var submittedQuestion = new SubmittedQuestion(Guid.NewGuid(), Guid.NewGuid(), "text");

            _user.SubmittedQuestions.Add(submittedQuestion);
            var submittedQuestionListSize = _user.SubmittedQuestions.Count();

            _userRepositoryMock
                .Setup(m => m.GetUserById(_user.Id))
                .ReturnsAsync(_user);

            _userRepositoryMock
                .Setup(m => m.Update(_user));
            _userRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.DeleteFromUser(_user.Id, submittedQuestion.Id);

            //Assert
            _user.SubmittedQuestions.Count().Should().Be(submittedQuestionListSize - 1);
        }
    }
}
