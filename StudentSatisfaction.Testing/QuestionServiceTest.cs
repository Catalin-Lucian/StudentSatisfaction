using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using AutoMapper;
using FluentAssertions;
using Moq;
using StudentSatisfaction.Business.Surveys.Models.Questions;
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
    }
}
