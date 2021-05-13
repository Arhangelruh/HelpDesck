using HelpDesk.Common.Interfaces;
using System;
using System.Collections.Generic;

namespace HelpDesk.DAL.Models
{
    /// <summary>
    /// Problem.
    /// </summary>
    public class Problem : IHasDbIdentity
    {
        /// <inheritdoc/>
        public int Id { get; set; }

        /// <summary>
        /// Theme.
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ip adress.
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// Incoming date.
        /// </summary>
        public DateTime IncomingDate { get; set; }

        /// <summary>
        /// Close date.
        /// </summary>
        public DateTime? CloseDate { get; set; }

        /// <inheritdoc/>
        public int ProfileCreatorId { get; set; }

        /// <summary>
        /// Navigation to Profile.
        /// </summary>
        public Profile Profile { get; set; }

        /// <inheritdoc/>
        public int StatusId { get; set; }

        /// <summary>
        /// Navigation to Status.
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Navigation to User problem.
        /// </summary>
        public ICollection<UserProblem> UsersProblem { get; set; }
    }
}
