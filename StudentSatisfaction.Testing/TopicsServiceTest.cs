using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using StudentSatisfaction.Business.Surveys.Models.Topics;
using StudentSatisfaction.Business.Surveys.Services.Comments;
using StudentSatisfaction.Business.Surveys.Services.Topics;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Entities.Users;
using StudentSatisfaction.Persistence;
using StudentSatisfaction.Persistence.Repositories.Users;
using Xunit;

namespace StudentSatisfaction.Testing
{
    public class TopicsServiceTest: IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<ISurveyRepository> _surveyRepositoryMock;
        private readonly Mock<ITopicRepository> _topicRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly ITopicsService _sut;

        private readonly Topic _topic;
        private readonly Survey _survey;

        public TopicsServiceTest()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _surveyRepositoryMock = _mockRepository.Create<ISurveyRepository>();
            _topicRepositoryMock = _mockRepository.Create<ITopicRepository>();

            _mapperMock = _mockRepository.Create<IMapper>();

            _sut = new TopicsService(_surveyRepositoryMock.Object, _topicRepositoryMock.Object, _mapperMock.Object);
            _topic = new Topic("Lectures", "lectures, courses");
            _survey = new Survey("IP - lecture", DateTime.Now, DateTime.Now.AddMonths(3));
        }

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async void When_GetAllIsCalled_Expect_AllTopicsToBeReturned()
        {
            //Arrange
            var topicModels = new List<TopicModel>();
            var topics = new List<Topic>();

            var topic1 = new Topic("Lectures", "topic details");
            var topic2 = new Topic("Courses", "topic details");

            topics.Add(topic1);
            topics.Add(topic2);

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

            topicModels.Add(model1);
            topicModels.Add(model2);

            _topicRepositoryMock
                .Setup(m => m.GetAll())
                .Returns(topics);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<TopicModel>>(topics))
                .Returns(topicModels);

            //Act
            var result = _sut.GetAll();

            //Assert
            result.Should().BeEquivalentTo(topicModels);
        }

        [Fact]
        public async void When_GetById_IsCalledWithATopicId_Expect_ThatTopicToBeReturned()
        {
            //Arrange
            var expectedResult = new TopicModel()
            {
                Id = _topic.Id,
                Details = _topic.Details,
                Title = _topic.Title
            };

            _topicRepositoryMock
                .Setup(m => m.GetTopicById(_topic.Id))
                .ReturnsAsync(_topic);

            _mapperMock
                .Setup(m => m.Map<TopicModel>(_topic))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetById(_topic.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_CreateIsCalled_WithACreateTopicModel_Expect_ThatTopicToBeCreated()
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

            _mapperMock
                .Setup(m => m.Map<Topic>(model))
                .Returns(_topic);

            _topicRepositoryMock
                .Setup(m => m.Create(_topic))
                .Returns(Task.CompletedTask);

            _topicRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<TopicModel>(_topic))
                .Returns(expectedResult);

            //Act
            var result = await _sut.Create(model);

            //Assert
            model.Should().BeEquivalentTo(model);
        }

        [Fact]
        public async void When_Delete_IsCalledWithATopicId_Expect_ThatTopicToBeDeleted()
        {
            //Arrange
            _topicRepositoryMock
                .Setup(m => m.GetTopicById(_topic.Id))
                .ReturnsAsync(_topic);

            _topicRepositoryMock
                .Setup(m => m.Delete(_topic));

            _topicRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.Delete(_topic.Id);
        }

        [Fact]
        public async void When_Update_IsCalledWithATopicIdAndAnUpdateTopicModel_ExpectTheTopicWithTheGivenIdToBeUpdated()
        {
            //Arrange
            var model = new UpdateTopicModel()
            {
                Title = "updated title",
                Details = "updated details"
            };

            var updatedTopic = new Topic(model.Title, model.Details);

            _topicRepositoryMock
                .Setup(m => m.GetTopicById(_topic.Id))
                .ReturnsAsync(_topic);

            _mapperMock
                .Setup(m => m.Map(model, _topic))
                .Returns(updatedTopic);

            _topicRepositoryMock
                .Setup(m => m.Update(_topic));

            _topicRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.Update(_topic.Id, model);
        }

        [Fact]
        public async void When_AddTopicToSurvey_IsCalled_WithASurveyIdAndACreateTopicMOdel_Expect_ThatTopicToBeAddedToTheSurvey()
        {
            //Arrange
            var model = new CreateTopicModel()
            {
                Title = "topic title",
                Details = "topic details"
            };

            var expectedResult = new TopicModel()
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Details = model.Details
            };

            _mapperMock
                .Setup(m => m.Map<Topic>(model))
                .Returns(_topic);

            _topicRepositoryMock
                .Setup(m => m.Create(_topic))
                .Returns(Task.CompletedTask);

            _topicRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .ReturnsAsync(_survey);

            _surveyRepositoryMock
                .Setup(m => m.Update(_survey));
            _surveyRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<TopicModel>(_topic))
                .Returns(expectedResult);

            //Act
            var result = await _sut.AddTopicToSurvey(_survey.Id, model);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_AddTopicToSurvey_IsCalled_WithASurveyIdAndATopicId_Expect_ThatTopicToBeAddedToTheSurvey()
        {
            //Arrange
            var model = new CreateTopicModel()
            {
                Title = "topic title",
                Details = "topic details"
            };

            var expectedResult = new TopicModel()
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Details = model.Details
            };

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .ReturnsAsync(_survey);

            _topicRepositoryMock
                .Setup(m => m.GetTopicById(_topic.Id))
                .ReturnsAsync(_topic);

            _surveyRepositoryMock
                .Setup(m => m.Update(_survey));
            _surveyRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<TopicModel>(_topic))
                .Returns(expectedResult);

            //Act
            var result = await _sut.AddTopicToSurvey(_survey.Id, _topic.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_DeleteTopicFromSurveyIsCalled_With_ASurveyIdAndATopicId_ExpectThatTopicToBeDeletedFromTheSurvey()
        {
            //Arrange
            _survey.Topics.Add(_topic);
            var topicListSize = _survey.Topics.Count();

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .ReturnsAsync(_survey);

            _surveyRepositoryMock
                .Setup(m => m.Update(_survey));
            _surveyRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.DeleteTopicFromSurvey(_survey.Id, _topic.Id);

            //Assert
            _survey.Topics.Count().Should().Be(topicListSize - 1);
        }

        [Fact]
        public async void When_GetAllTopicsFromSurveyISCalled_Expect_AllTopicsFromThatSurveyToBeReturned()
        {
            //Arrange
            var expectedResult = _survey.Topics.Select(m => new TopicModel()
            {
                Id = m.Id,
                Title = m.Title,
                Details = m.Details
            });

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .ReturnsAsync(_survey);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<TopicModel>>(_survey.Topics))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetAllTopicsFromSurvey(_survey.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
