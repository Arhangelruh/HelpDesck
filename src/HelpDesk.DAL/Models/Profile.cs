using HelpDesk.Common.Interfaces;
using System.Collections.Generic;

namespace HelpDesk.DAL.Models
{
    /// <summary>
    /// Profile.
    /// </summary>
    public class Profile : IHasDbIdentity, IHasUserIdentity
    {
        /// <inheritdoc/>
        public int Id { get; set; }

        /// <inheritdoc/>
        public string UserId { get; set; }

        /// <summary>
        /// Navigation to User.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// First name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Last name.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Sid from users AD
        /// </summary>
        public string UserSid { get; set; }

        /// <summary>
        /// Navigation to User problem.
        /// </summary>
        public ICollection<UserProblem> UsersProblem { get; set; }

        /// <summary>
        /// Navigation to Comments.
        /// </summary>
        public ICollection<Comments> Comments { get; set; }
    }
}
