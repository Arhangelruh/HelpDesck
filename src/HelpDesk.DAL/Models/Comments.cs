using HelpDesk.Common.Interfaces;
using System;

namespace HelpDesk.DAL.Models
{
    /// <summary>
    /// Comments.
    /// </summary>
    public class Comments : IHasDbIdentity
    {
        /// <inheritdoc/>
        public int Id { get; set; }

        /// <inheritdoc/>
        public int ProblemId { get; set; }

        /// <summary>
        /// Navigation to Problem.
        /// </summary>
        public Problem Problem { get; set; }

        /// <inheritdoc/>
        public int ProfileId { get; set; }

        /// <summary>
        /// Navigation to Profile.
        /// </summary>
        public Profile Profile { get; set; }

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
