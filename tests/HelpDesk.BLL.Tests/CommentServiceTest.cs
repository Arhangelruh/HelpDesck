using HelpDesk.BLL.Repository;
using HelpDesk.BLL.Services;
using HelpDesk.BLL.Tests.Context;
using HelpDesk.Common.Interfaces;
using HelpDesk.DAL.Models;
using Moq;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HelpDesk.BLL.Tests
{
    public class CommentServiceTest
    {

        [Fact]
        public async Task GetCommentsByRequestAsyncReturnWithListComments()
        {
            // Arrange
            int problemid = 2;
            //var mock = new Mock<IRepository<Comments>>();
            //mock.Setup(_repositoryComments => _repositoryComments.GetAll()).Returns(CommList);
            //var _commentService = new CommentService(mock.Object);
            
            var mockSet = new Mock<Comments>();
            mockSet.As<IDbAsyncEnumerable<Comments>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Comments>(CommList().GetEnumerator()));

            mockSet.As<IQueryable<Comments>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Comments>(CommList().Provider));

            mockSet.As<IQueryable<Comments>>().Setup(m => m.Expression).Returns(CommList().Expression);
            mockSet.As<IQueryable<Comments>>().Setup(m => m.ElementType).Returns(CommList().ElementType);
            mockSet.As<IQueryable<Comments>>().Setup(m => m.GetEnumerator()).Returns(CommList().GetEnumerator());

            var mockContext = new Mock<Comments>();
            mockContext.Setup(_r => _r).Returns(mockSet.Object);
            IRepository<Comments> _repo =  new Repository<Comments>(mockContext);
      
            var _commentService = new CommentService(mockContext.Object);

            //Act
            //var result = mock.Object.GetAll().Where(id => id.ProblemId == problemid);
            var result = await _commentService.GetCommentsByRequestAsync(problemid);

            //Assert
            Assert.NotEmpty(result);
        }
        private IQueryable<Comments> CommList()
        {                       
            //var r = new List<Comments>();
            //r.Add(new Comments()
            //{
            //    Id = 1,
            //    ProblemId = 2,
            //    ProfileId = 1,
            //    Comment = "some text"
            //});
            //r.Add(new Comments()
            //{
            //    Id = 1,
            //    ProblemId = 1,
            //    ProfileId = 1,
            //    Comment = "some text two"
            //});
            //r.Add(new Comments()
            //{
            //    Id = 1,
            //    ProblemId = 2,
            //    ProfileId = 2,
            //    Comment = "some text three"
            //});
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
    }   
}
