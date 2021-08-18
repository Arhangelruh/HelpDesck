using HelpDesk.BLL.Interfaces;
using HelpDesk.Common.Interfaces;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.BLL.Services
{
    /// <inheritdoc cref="ICommentService<T>"/>
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comments> _repositoryComments;

        public CommentService(IRepository<Comments> repositoryComments)
        {
            _repositoryComments = repositoryComments ?? throw new ArgumentNullException(nameof(repositoryComments));
        }

        public async Task DeleteCommentsAsync(int problemid)
        {

            var comments = await _repositoryComments
                .GetAll()
                .AsNoTracking()
                .Where(comments => comments.ProblemId == problemid)
                .ToListAsync();

            if (comments.Any())
            {
                foreach(var comment in comments)
                {
                    _repositoryComments.Delete(comment);
                    await _repositoryComments.SaveChangesAsync();
                }               
            }
        }
    }
}
