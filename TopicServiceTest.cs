using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentSatisfaction.Business.Surveys.Services.Comments;
using StudentSatisfaction.Business.Surveys.Services.Topics;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Entities.Users;
using StudentSatisfaction.Persistence;
using StudentSatisfaction.Persistence.Repositories.Users;
using Xunit;
using System.Threading.Tasks;
using FluentAssertions;
using StudentSatisfaction.Business.Surveys.Models.Topics;


namespace StudentSatisfaction.Tests
{
    public class TopicServiceTest : IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<ISurveyRepository> _surveyRepositoryMock;
        private readonly Mock<ITopicRepository> _topicRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly ITopicsService _sut;

        private readonly Topic _topic;
        private readonly Survey _survey;

        public TopicServiceTest()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _surveyRepositoryMock = _mockRepository.Create<ISurveyRepository>();
            _topicRepositoryMock = _mockRepository.Create<ITopicRepository>();

            _mapperMock = _mockRepository.Create<IMapper>();
            _sut = new TopicsService(_surveyRepositoryMock.Object, _topicRepositoryMock.Object, _mapperMock.Object);


            _topic = new Topic("Lectures", "details about courses, lectures");
            _survey = new Survey("IP - lecture", DateTime.Now, DateTime.Now.AddMonths(3));
        }

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void When_GetAllIsCalled_Expect_AllTopicsToBeReturned()
        {
            //Arrange
            var topicModel = new List<TopicModel>();
            var topic = new List<Topic>();
            var topic1 = new Topic("Lectures", "topic details");
            var topic2 = new Topic("Courses", "topic details");
            topic.Add(topic1);
            topic.Add(topic2);
            var model1 = new TopicModel()
            {
                Id = topic1.Id,
                Title = topic1.Title,
                Details = topic1.Details
            };

            var model2 = new TopicModel()
            {
                Id = topic2.Id,
                Title = topic2.Title,
                Details = topic2.Details
            };
            topicModel.Add(model1);
            topicModel.Add(model2);
            _topicRepositoryMock.Setup(m => m.GetAll()).Returns(topic);
            _mapperMock.Setup(m => m.Map<IEnumerable<TopicModel>>(topic)).Returns(topicModel);

            //Act
            var result = _sut.GetAll();

            //Assert
            result.Should().BeEquivalentTo(topicModel);
        }

        [Fact]
        public async void When_GetById_IsCalled_Expect_ThatTopicToBeReturned()
        {
            //Arrange
            var expectedResult = new TopicModel()
            {
                Id = _topic.Id,
                Details = _topic.Details,
                Title = _topic.Title
            };

            _topicRepositoryMock.Setup(m => m.GetTopicById(_topic.Id)).ReturnsAsync(_topic);
            _mapperMock.Setup(m => m.Map<TopicModel>(_topic)).Returns(expectedResult);

            //Act
            var result = await _sut.GetById(_topic.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_CreateIsCalled_Expect_ThatTopicToBeCreated()
        {
            //Arrange
            var model = new CreateTopicModel()
            {
                Details = _topic.Details,
                Title = _topic.Title
            };

            var expectedResult = new TopicModel()
            {
                Details = model.Details,
                Title = model.Title
            };

            _mapperMock.Setup(m => m.Map<Topic>(model)).Returns(_topic);
            _topicRepositoryMock.Setup(m => m.Create(_topic)).Returns(Task.CompletedTask);
            _topicRepositoryMock.Setup(m => m.SaveChanges()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<TopicModel>(_topic)).Returns(expectedResult);

            //Act
            var result = await _sut.Create(model);

            //Assert
            model.Should().BeEquivalentTo(model);
        }


        [Fact]
        public async void When_Update_IsCalled_Expect_TheTopicWithTheGivenIdToBeUpdated()
        {
            //Arrange
            var model = new UpdateTopicModel()
            {
                Title = "update title",
                Details = "update details"
            };

            var updateTopic = new Topic(model.Title, model.Details);

            _topicRepositoryMock.Setup(m => m.GetTopicById(_topic.Id)).ReturnsAsync(_topic);
            _mapperMock.Setup(m => m.Map(model, _topic)).Returns(updateTopic);
            _topicRepositoryMock.Setup(m => m.Update(_topic));
            _topicRepositoryMock.Setup(m => m.SaveChanges()).Returns(Task.CompletedTask);

            //Act
            await _sut.Update(_topic.Id, model);
        }


        [Fact]
        public async void When_Delete_IsCalledWithATopicId_Expect_ThatTopicToBeDeleted()
        {
            //Arrange
            _topicRepositoryMock.Setup(m => m.GetTopicById(_topic.Id)).ReturnsAsync(_topic);
            _topicRepositoryMock.Setup(m => m.Delete(_topic));
            _topicRepositoryMock.Setup(m => m.SaveChanges()).Returns(Task.CompletedTask);

            //Act
            await _sut.Delete(_topic.Id);
        }
       

        [Fact]
        public async void When_AddTopicToSurvey_IsCalled_Expect_ThatTopicToBeAddedToTheSurvey()
        {
            //Arrange
            var model = new CreateTopicModel()
            {
                Title = "add title",
                Details = "add details"
            };

            var expectedResult = new TopicModel()
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Details = model.Details
            };

            _mapperMock.Setup(m => m.Map<Topic>(model)).Returns(_topic);
            _topicRepositoryMock.Setup(m => m.Create(_topic)).Returns(Task.CompletedTask);
            _topicRepositoryMock.Setup(m => m.SaveChanges()).Returns(Task.CompletedTask);
            _surveyRepositoryMock.Setup(m => m.GetSurveyById(_survey.Id)).ReturnsAsync(_survey);
            _surveyRepositoryMock.Setup(m => m.Update(_survey));
            _surveyRepositoryMock.Setup(m => m.SaveChanges()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<TopicModel>(_topic)).Returns(expectedResult);

            //Act
            var result = await _sut.AddTopicToSurvey(_survey.Id, model);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_AddTopicToSurvey_IsCalled_Expect_ThatTopicToBeAdded()
        {
            //Arrange
            var model = new CreateTopicModel()
            {
                Title = "add title",
                Details = "add details"
            };

            var expectedResult = new TopicModel()
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Details = model.Details
            };

            _surveyRepositoryMock.Setup(m => m.GetSurveyById(_survey.Id)).ReturnsAsync(_survey);
            _topicRepositoryMock.Setup(m => m.GetTopicById(_topic.Id)).ReturnsAsync(_topic);
            _surveyRepositoryMock.Setup(m => m.Update(_survey));
            _surveyRepositoryMock.Setup(m => m.SaveChanges()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<TopicModel>(_topic)).Returns(expectedResult);

            //Act
            var result = await _sut.AddTopicToSurvey(_survey.Id, _topic.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_DeleteTopicFromSurveyIsCalled_Expect_TopicToBeDeleted()
        {
            //Arrange
            _survey.Topics.Add(_topic);
            var topicSize = _survey.Topics.Count();

            _surveyRepositoryMock.Setup(m => m.GetSurveyById(_survey.Id)).ReturnsAsync(_survey);
            _surveyRepositoryMock.Setup(m => m.Update(_survey));
            _surveyRepositoryMock.Setup(m => m.SaveChanges()).Returns(Task.CompletedTask);

            //Act
            await _sut.DeleteTopicFromSurvey(_survey.Id, _topic.Id);

            //Assert
            _survey.Topics.Count().Should().Be(topicSize - 1);
        }

        [Fact]
        public async void When_GetAllTopicsFromSurveyISCalled_Expect_AllTopicsToBeReturned()
        {
            //Arrange
            var expectedResult = _survey.Topics.Select(m => new TopicModel()
            {
                Id = m.Id,
                Title = m.Title,
                Details = m.Details
            });

            _surveyRepositoryMock.Setup(m => m.GetSurveyById(_survey.Id)).ReturnsAsync(_survey);
            _mapperMock.Setup(m => m.Map<IEnumerable<TopicModel>>(_survey.Topics)).Returns(expectedResult);

            //Act
            var result = await _sut.GetAllTopicsFromSurvey(_survey.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
