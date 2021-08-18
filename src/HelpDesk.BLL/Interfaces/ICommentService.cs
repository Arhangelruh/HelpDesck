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
    }
}
