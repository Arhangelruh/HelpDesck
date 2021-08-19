using HelpDesk.BLL.Interfaces;
using HelpDesk.BLL.Models;
using HelpDesk.Common.Interfaces;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<List<CommentDto>> GetCommentsByRequestAsync(int problemid)
        {
            var comments = new List<CommentDto>();

            var getcomments = await _repositoryComments
                .GetAll()
                .AsNoTracking()
                .Where(comments => comments.ProblemId == problemid)
                .ToListAsync();
            
                foreach (var comment in getcomments)
                {
                    comments.Add(new CommentDto{
                        Id = comment.Id,
                        ProblemId = comment.ProblemId,
                        CreateComment = comment.CreateComment,
                        Comment =comment.Comment
                        });
                }

                return comments;
        }
    }
}
