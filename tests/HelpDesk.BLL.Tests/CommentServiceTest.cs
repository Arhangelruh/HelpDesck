using HelpDesk.BLL.Models;
using HelpDesk.BLL.Services;
using HelpDesk.BLL.Tests.Context;
using HelpDesk.Common.Interfaces;
using HelpDesk.DAL.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HelpDesk.BLL.Tests
{
    public class CommentServiceTest
    {
        private IQueryable<Comments> CommList()
        {
            var data = new List<Comments>
            {
                new Comments {
                Id = 1,
                ProblemId = 2,
                ProfileId = 1,
                Comment = "some text"
                },
                new Comments {
                Id = 1,
                ProblemId = 1,
                ProfileId = 1,
                Comment = "some text two"
                },
                new Comments {
                Id = 1,
                ProblemId = 2,
                ProfileId = 2,
                Comment = "some text three"
                },
            }.AsQueryable();
            return data;
        }

        [Fact]
        public async Task GetCommentsByRequestAsyncReturnWithListComments()
        {
            // Arrange
            int problemid = 1;
            var allCommentsInDb = CommList();

            var repositoryMock = new Mock<IRepository<Comments>>();
            repositoryMock.Setup(x => x.GetAll()).Returns(new TestAsyncEnumerable<Comments>(allCommentsInDb));

            var commentService = new CommentService(repositoryMock.Object);

            //Act
            var result = await commentService.GetCommentsByRequestAsync(problemid);

            //Assert
            Assert.NotEmpty(result);
            Assert.Single(result);
            Assert.Equal(problemid, result.First().ProblemId);
        }

        [Fact]
        public async Task DeleteCommentsAsyncNonReturns()
        {
            // Arrange
            int problemid = 2;
            var commentsDb = CommList();

            var repositoryMock = new Mock<IRepository<Comments>>();
            repositoryMock.Setup(x => x.GetAll()).Returns(new TestAsyncEnumerable<Comments>(commentsDb));

            var commentService = new CommentService(repositoryMock.Object);

            var comments = repositoryMock.Object
                .GetAll()
                .Where(comments => comments.ProblemId == problemid);

            //Act
            await commentService.DeleteCommentsAsync(problemid);

            ////Assert
            foreach (var comment in comments)
            repositoryMock.Verify(x => x.Delete(comment));
            repositoryMock.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task AddCommentsAsyncNonReturns()
        {
            // Arrange
            var commentDto = new CommentDto {
                ProblemId = 3,
                ProfileId = 1,
                Comment = "new comment text",                
            };

            var date = DateTime.Now;

            var newComment = new Comments
            {
                ProblemId = commentDto.ProblemId,
                ProfileId = commentDto.ProfileId,
                Comment = commentDto.Comment,
                CreateComment = date
            };

            var commentsDb = CommList();

            var repositoryMock = new Mock<IRepository<Comments>>();
            repositoryMock.Setup(x => x.GetAll()).Returns(new TestAsyncEnumerable<Comments>(commentsDb));

            var commentService = new CommentService(repositoryMock.Object);

            //Act
            await commentService.AddCommentAsync(commentDto);

            ////Assert
            repositoryMock.Verify(x => x.AddAsync(It.Is<Comments>(
            c =>
                c.ProblemId == 3 &&
                c.ProfileId == 1
             )));
            repositoryMock.Verify(x => x.SaveChangesAsync());
        }
    }
}
