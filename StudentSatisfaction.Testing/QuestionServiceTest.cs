using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using AutoMapper;
using FluentAssertions;
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
            var questions = await _sut.Get(survey.Id);

            //Assert
            questions.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_GetById_IsCalled_Expect_QuestionWithSpecifiedId_ToBeReturned()
        {
            //Arrange
            var survey = new Survey("Title", DateTime.Now, DateTime.Now.AddMonths(1));

            var q1 = new Question(Guid.NewGuid(), "plain text", "Question1");
            var q2 = new Question(Guid.NewGuid(), "plain text", "Question2");

            var searchedId = q1.Id;

            //!!!! cum caut o Question cu un ANUMIT id??
            //q1.Id = searchedId;

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

            //?????
            _mapperMock
                .Setup(m => m.Map<QuestionModel>(survey.Questions.FirstOrDefault(q => q.Id == q1.Id)))
                .Returns(expectedResult);

            //Act
            var question = await _sut.GetById(survey.Id, q1.Id);

            //Assert
            question.Should().BeEquivalentTo(expectedResult);
        }
    }
}
