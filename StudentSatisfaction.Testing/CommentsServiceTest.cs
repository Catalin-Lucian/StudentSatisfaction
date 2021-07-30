using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using StudentSatisfaction.Business.Surveys.Models.Comments;
using StudentSatisfaction.Business.Surveys.Services.Comments;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Entities.Users;
using StudentSatisfaction.Persistence;
using StudentSatisfaction.Persistence.Repositories.Comments;
using StudentSatisfaction.Persistence.Repositories.Users;
using Xunit;

namespace StudentSatisfaction.Testing
{
    public class CommentsServiceTest : IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IMapper> _mapperMock;

        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<ISurveyRepository> _surveyRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        private readonly ICommentsService _sut;

        private readonly UserData _userData;
        private readonly Survey _survey;
        private readonly Comment _comment;

        public CommentsServiceTest()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _commentRepositoryMock = _mockRepository.Create<ICommentRepository>();
            _surveyRepositoryMock = _mockRepository.Create<ISurveyRepository>();
            _userRepositoryMock = _mockRepository.Create<IUserRepository>();

            _mapperMock = _mockRepository.Create<IMapper>();

            _sut = new CommentsService(_surveyRepositoryMock.Object, _userRepositoryMock.Object,
                _commentRepositoryMock.Object, _mapperMock.Object);

            _userData = new UserData("UserData", "Username", "password", "Random name", "something@gmail.com",
                new DateTime(1999, 1, 12, 9, 10, 0), "AC");
            _survey = new Survey("IP - lecture", DateTime.Now, DateTime.Now.AddMonths(3));
            _comment = new Comment(_userData.Id, _survey.Id, "comment 1");
        }

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async void When_GetCommentByIdIsCalled_Expect_ThatCommentToBeReturned()
        {
            //Arrange
            var comment = new Comment(Guid.NewGuid(), Guid.NewGuid(), "some comment");

            var expectedResult = new CommentModel()
            {
                SurveyId = comment.SurveyId,
                UserId = comment.UserId,
                CommentText = comment.CommentText
            };

            _commentRepositoryMock
                .Setup(m => m.GetCommentById(comment.Id))
                .Returns(Task.FromResult(comment));

            _mapperMock
                .Setup(m => m.Map<CommentModel>(comment))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetCommentById(comment.Id);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_GetCommentsFromSurvey_IsCalledWithASurveyId_ExpectAllCommentsFromThatSurvey_ToBeReturned()
        {
            //Arrange
            var expectedResult = _survey.Comments.Select(c => new CommentModel()
            {
                SurveyId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                CommentText = "some text"
            });

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .Returns(Task.FromResult(_survey));

            _mapperMock
                .Setup(m => m.Map<IEnumerable<CommentModel>>(_survey.Comments))
                .Returns(expectedResult);

            //Act
            var result = await _sut.GetCommentsFromSurvey(_survey.Id);

            //Arrange
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_GetCommentsFromUser_IsCalled_WithAnUserId_ExpectAllItsCommentsToBeReturned()
        {
            //Arrange
            var expectedResult = _userData.Comments.Select(c => new CommentModel()
            {
                SurveyId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                CommentText = "comment 1"
            });

            _userRepositoryMock
                .Setup(m => m.GetUserById(_userData.Id))
                .Returns(Task.FromResult(_userData));

            _mapperMock
                .Setup(m => m.Map<IEnumerable<CommentModel>>(_userData.Comments))
                .Returns(expectedResult);

            //Assert
            var result = await _sut.GetCommentsFromUser(_userData.Id);

            //Act
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void When_AddIsCalled_WithASurveyIdAndAnUserIdAndACreateCommentModel_Expect_TheCommentToBeAddedToTheUsersCommentList()
        {
            //Arrange
            var model = new CreateCommentModel()
            {
                SurveyId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                CommentText = "comment text"
            };

            var comment = new Comment(model.UserId, model.SurveyId, model.CommentText);

            var expectedResult = new CommentModel()
            {
                SurveyId = model.SurveyId,
                UserId = model.UserId,
                CommentText = model.CommentText
            };

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .Returns(Task.FromResult(_survey));

            _userRepositoryMock
                .Setup(m => m.GetUserById(_userData.Id))
                .Returns(Task.FromResult(_userData));

            _mapperMock
                .Setup(m => m.Map<Comment>(model))
                .Returns(comment);

            _commentRepositoryMock
                .Setup(c => c.Create(comment))
                .Returns(Task.CompletedTask);

            _commentRepositoryMock
                .Setup(c => c.SaveChanges())
                .Returns(Task.CompletedTask);

            _surveyRepositoryMock
                .Setup(m => m.Update(_survey));
            _surveyRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(m => m.Update(_userData));
            _userRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<CommentModel>(comment))
                .Returns(expectedResult);

            //Act
            var result = await _sut.Add(_survey.Id, _userData.Id, model);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void When_UpdateIsCalled_WithACommentIdAndAnUpdateCommentModel_Expect_ThatCommentToBeAddedToTheCommentsRepository()
        {
            //Arrange
            var model = new UpdateCommentModel()
            {
                CommentText = "updated comment"
            };

            _commentRepositoryMock
                .Setup(m => m.GetCommentById(_comment.Id))
                .ReturnsAsync(_comment);

            _mapperMock
                .Setup(m => m.Map(model, _comment))
                .Returns(_comment);

            _commentRepositoryMock
                .Setup(m => m.Update(_comment));
            _commentRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            _sut.Update(_comment.Id, model);
        }

        [Fact]
        public async void When_DeleteCommentFromSurvey_IsCalled_WithASurveyIdAndACommentId_ExpectThatCommentToBeDeletedFromTheSurvey()
        {
            //Arrange
            var comment = new Comment(Guid.NewGuid(), Guid.NewGuid(), "comment 1");
            _survey.Comments.Add(comment);

            var commentListSize = _survey.Comments.Count();

            _surveyRepositoryMock
                .Setup(m => m.GetSurveyById(_survey.Id))
                .Returns(Task.FromResult(_survey));

            _surveyRepositoryMock
                .Setup(m => m.Update(_survey));
            _surveyRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.DeleteCommentFromSurvey(_survey.Id, comment.Id);

            //Assert
            _survey.Comments.Count().Should().Be(commentListSize - 1);
        }

        [Fact]
        public async void When_DeleteCommentFromUser_IsCalled_WithAUserIdAndACommentId_ExpectThatCommentToBeDeletedFromTheUsersListOfComments()
        {
            //Arrange
            var comment = new Comment(Guid.NewGuid(), Guid.NewGuid(), "comment 1");
            _userData.Comments.Add(comment);

            var commentListSize = _userData.Comments.Count();

            _userRepositoryMock
                .Setup(m => m.GetUserById(_userData.Id))
                .Returns(Task.FromResult(_userData));

            _userRepositoryMock
                .Setup(m => m.Update(_userData));
            _userRepositoryMock
                .Setup(m => m.SaveChanges())
                .Returns(Task.CompletedTask);

            //Act
            await _sut.DeleteCommentFromUser(_userData.Id, comment.Id);

            //Assert
            _userData.Comments.Count().Should().Be(commentListSize - 1);
        }
    }
}
