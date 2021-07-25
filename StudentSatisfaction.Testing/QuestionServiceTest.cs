using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FluentAssertions.Common;
using Moq;
using StudentSatisfaction.Business.Surveys.Models.Questions;
using StudentSatisfaction.Business.Surveys.Models.Users;
using StudentSatisfaction.Business.Surveys.Services.Questions;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Persistence;
using Xunit;

namespace StudentSatisfaction.Testing
{
    public class QuestionServiceTest: IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ISurveyRepository> _surveyRepositoryMock;
        private readonly QuestionService _sut;

        public QuestionServiceTest()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _surveyRepositoryMock = _mockRepository.Create<ISurveyRepository>();
            _mapperMock = _mockRepository.Create<IMapper>();

            _sut = new QuestionService(_surveyRepositoryMock.Object, _mapperMock.Object);
        }

        //se apeleaza dupa fiecare test in parte
        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async void When_Get_IsCalled_Expect_QuestionsToBeReturned()
        {
            //Arrange
            var survey = new Survey("Title", DateTime.Now, DateTime.Now.AddMonths(1));
            survey.Questions.Add(new Question(Guid.NewGuid(), "plain text", "Question1"));
            survey.Questions.Add(new Question(Guid.NewGuid(), "plain text", "Question2"));

            var expectedResult = survey.Questions.Select(q => new QuestionModel()
            {
                Type = q.Type,
                QuestionText = q.QuestionText,
                Id = q.Id,
                SurveyId = q.SurveyId
            });

            _surveyRepositoryMock
                .Setup(s => s.GetSurveyById(survey.Id))
                .ReturnsAsync(survey);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<QuestionModel>>(survey.Questions))
                .Returns(expectedResult);

            //Act
            var result = await _sut.Get(survey.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_GetById_IsCalled_Expect_QuestionWithSpecifiedId_ToBeReturned()
        {
            //Arrange
            var survey = new Survey("Title", DateTime.Now, DateTime.Now.AddMonths(1));

            var q1 = new Question(Guid.NewGuid(), "plain text", "Question1");
            var q2 = new Question(Guid.NewGuid(), "plain text", "Question2");

            var searchedId = q1.Id;

            survey.Questions.Add(q1);
            survey.Questions.Add(q2);

            var expectedResult = new QuestionModel()
            {
                Id = q1.Id,
                SurveyId = q1.SurveyId,
                QuestionText = q1.QuestionText,
                Type = q1.Type
            };

            _surveyRepositoryMock
                .Setup(s => s.GetSurveyById(survey.Id))
                .ReturnsAsync(survey);

            _mapperMock
                .Setup(m => m.Map<QuestionModel>(It.Is<Question>(q => q.Id == searchedId)))
                .Returns(expectedResult);

            //Act
            var question = await _sut.GetById(survey.Id, q1.Id);

            //Assert
            question.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_AddIsCalled_WithASpecificSurveyIdAndACreateQuestionModel_AQuestion_ShouldBeAddedToTheSurvey()
        {
            //Arrange
            var survey = new Survey("Title", DateTime.Now, DateTime.Now.AddMonths(1));
            var surveyQuestionSize = survey.Questions.Count();

            var model = new CreateQuestionModel()
            {
                SurveyId = survey.Id,
                QuestionText = "Random Question",
                Type = "plain text"
            };

            var question = new Question(model.SurveyId, model.Type, model.QuestionText);
            var expectedResult = new QuestionModel()
            {
                Id = question.Id,
                SurveyId = question.SurveyId,
                QuestionText = question.QuestionText,
                Type = question.Type
            };

            _surveyRepositoryMock
                .Setup(s => s.GetSurveyById(survey.Id))
                .ReturnsAsync(survey);

            _mapperMock
                .Setup(m => m.Map<Question>(model))
                .Returns(question);


            _surveyRepositoryMock
                .Setup(m => m.Update(survey));

            _surveyRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<QuestionModel>(question))
                .Returns(expectedResult);
            
            //Act
            var result = await _sut.Add(survey.Id, model);
            
            //Assert
            result.Should().BeEquivalentTo(expectedResult);
            survey.Questions.Count().Should().Be(surveyQuestionSize+1);
        }

        [Fact]
        public async void When_DeleteIsCalled_WithASurveyIdAndAQuestionId_TheQuestionWithThatId_ShouldBeRemovedFromTheSurveyWithTheSpecifiedId()
        {
            //Arrange
            var survey = new Survey("Title", DateTime.Now, DateTime.Now.AddMonths(1));

            var question = new Question(survey.Id, "plain text", "question 1");
            survey.Questions.Add(question);

            var surveyQuestionSize = survey.Questions.Count();

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(survey.Id))
                .ReturnsAsync(survey);

            _surveyRepositoryMock
                .Setup(m => m.Update(survey));

            _surveyRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.Delete(survey.Id, question.Id);

            //Assert
            survey.Questions.Count().Should().Be(surveyQuestionSize - 1);
        }

        //??????
        [Fact]
        public async void When_UpdateIsCalled_WithASurveyIdAndAQuestionIdAndAUpdateQuestionModel_TheQuestionListFromThatSurvey_ShouldBeUpdated_WithTheSpecifiedModel()
        {
            //Arrange
            var survey = new Survey("Title", DateTime.Now, DateTime.Now.AddMonths(1));

            var question = new Question(survey.Id, "plain text", "random question");
            survey.Questions.Add(question);

            var model = new UpdateQuestionModel()
            {
                QuestionText = "updated question text",
                Type = "plain text"
            };

            var expectedResult = question;

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(survey.Id))
                .ReturnsAsync(survey);

            _mapperMock
                .Setup(m => m.Map(model, question))
                .Returns(expectedResult);

            _surveyRepositoryMock
                .Setup(m => m.Update(survey));

            _surveyRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.Update(survey.Id, question.Id, model);

            //Assert
        }
    }
}
