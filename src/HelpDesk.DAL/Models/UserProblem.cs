using HelpDesk.Common.Interfaces;

namespace HelpDesk.DAL.Models
{
    /// <summary>
    /// User problems.
    /// </summary>
    public class UserProblem : IHasDbIdentity
    {
        /// <inheritdoc/>
        public int Id { get; set; }

        /// <summary>
        /// Profile identifier.
        /// </summary>
        public int ProfileId { get; set; }

        /// <summary>
        /// Navigation to Profile.
        /// </summary>
        public Profile Profile { get; set; }

        /// <summary>
        /// Groups identifier.
        /// </summary>
        public int ProblemId { get; set; }

        /// <summary>
        /// Navigation to Problem.
        /// </summary>
        public Problem Problem { get; set; }
    }
}
