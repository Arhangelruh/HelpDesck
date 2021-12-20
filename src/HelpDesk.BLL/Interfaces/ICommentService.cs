using HelpDesk.BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpDesk.BLL.Interfaces
{
    /// <summary>
    /// Class from works whith comments.
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// Delete comment.
        /// </summary>
        /// <param name="request">Dto model</param>
        Task DeleteCommentsAsync(int problemid);

        /// <summary>
        /// Get comments from request.
        /// </summary>
        /// <param name="problemid"></param>
        /// <returns>list comments</returns>
        Task<List<CommentDto>> GetCommentsByRequestAsync(int problemid);

        /// <summary>
        /// Add comments.
        /// </summary>
        /// <param name="commentDto">Dto model</param>
        Task AddCommentAsync(CommentDto commentDto);
    }
}
