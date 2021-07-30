using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using StudentSatisfaction.Business.Surveys.Models;
using StudentSatisfaction.Business.Surveys.Services;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Persistence;
using Xunit;

namespace StudentSatisfaction.Testing
{
    public class SurveyServiceTest: IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<ISurveyRepository> _surveyRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly ISurveyService _sut;
        private readonly Survey _survey;

        public SurveyServiceTest()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _surveyRepositoryMock = _mockRepository.Create<ISurveyRepository>();
            _mapperMock = _mockRepository.Create<IMapper>();
            _sut = new SurveyService(_surveyRepositoryMock.Object, _mapperMock.Object);

            _survey = new Survey("IP - lecture", DateTime.Now, DateTime.Now.AddMonths(3));
        }

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void When_GetAllIsCalled_ExpectAllSurveysToBeReturned()
        {
            //Arrange
            var s1 = new Survey("IP - lecture", DateTime.Now, DateTime.Now.AddMonths(3));
            var s2 = new Survey("PW - lecture", DateTime.Now, DateTime.Now.AddMonths(3));

            var surveys = new List<Survey>{ s1, s2 };

            var model1 = new SurveyModel()
            {
                Id = s1.Id,
                StartDate = s1.StartDate,
                EndDate = s1.EndDate,
                Name = s1.Name
            };

            var model2 = new SurveyModel()
            {
                Id = s2.Id,
                StartDate = s2.StartDate,
                EndDate = s2.EndDate,
                Name = s2.Name
            };

            var expectedResult = new List<SurveyModel>
            {
                model1, model2
            };

            _surveyRepositoryMock
                .Setup(m => m.GetAll())
                .Returns(surveys);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<SurveyModel>>(surveys))
                .Returns(expectedResult);

            //Act
            var result = _sut.GetAll();

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_GetById_IsCalledWithASurveyId_Expect_ThatSurveyToBeReturned()
        {
            //Arrange
            var expectedResult = new SurveyModel()
            {
                Id = _survey.Id,
                StartDate = _survey.StartDate,
                EndDate = _survey.EndDate,
                Name = _survey.Name
            };

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .ReturnsAsync(_survey);

            _mapperMock
                .Setup(m => m.Map<SurveyModel>(_survey))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetById(_survey.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_CreateIsCalled_WithACreateSurveyModel_ExpectThatSurveyToBeCreated()
        {
            //Arrange
            var model = new CreateSurveyModel()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Name = "Courses"
            };

            var expectedResult = new SurveyModel()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };

            _mapperMock
                .Setup(m => m.Map<Survey>(model))
                .Returns(_survey);

            _surveyRepositoryMock
                .Setup(m => m.Create(_survey))
                .Returns(Task.CompletedTask);

            _surveyRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<SurveyModel>(_survey))
                .Returns(expectedResult);

            //Act
            var result = await _sut.Create(model);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_Delete_IsCalledWithASurveyId_Expect_ThatSurveyToBeDeleted()
        {
            //Arrange
            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .ReturnsAsync(_survey);

            _surveyRepositoryMock
                .Setup(m => m.Delete(_survey));

            _surveyRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.Delete(_survey.Id);
        }

        [Fact]
        public async void When_UpdateIsCalled_WithASurveyIdAndAnUpdateSurveyModel_ExpectThatSurveyToBeUpdated()
        {
            //Arrange
            var model = new UpdateSurveyModel()
            {
                Name = "updated name",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(3)
            };

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .ReturnsAsync(_survey);

            _mapperMock
                .Setup(m => m.Map(model, _survey))
                .Returns(_survey);

            _surveyRepositoryMock
                .Setup(m => m.Update(_survey));

            _surveyRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.Update(_survey.Id, model);
        }
    }
}
