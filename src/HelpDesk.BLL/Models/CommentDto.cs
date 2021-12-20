using System;

namespace HelpDesk.BLL.Models
{
    public class CommentDto
    {
        /// <summary>
        /// Comment id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Problem id.
        /// </summary>
        public int ProblemId { get; set; }

        /// <summary>
        /// Profile id.
        /// </summary>
        public int ProfileId { get; set; }

        /// <summary>
        /// Data and time create comment.
        /// </summary>
        public DateTime CreateComment { get; set; }

        /// <summary>
        /// Comment.
        /// </summary>
        public string Comment { get; set; }
    }
}
