using HelpDesk.Common.Interfaces;
using System.Collections.Generic;

namespace HelpDesk.DAL.Models
{
    /// <summary>
    /// Status.
    /// </summary>
    public class Status : IHasDbIdentity
    {
        /// <inheritdoc/>
        public int Id { get; set; }

        /// <summary>
        /// Satatus name.
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// Order number.
        /// </summary>
        public int Queue { get; set; }

        /// <summary>
        /// Name buttom.
        /// </summary>
        public string StatusNameFromButton { get; set; }

        /// <summary>
        /// Access status from user.
        /// </summary>
        public bool Access { get; set; }

        /// <summary>
        /// Navigation to ProblemStatus.
        /// </summary>
        public ICollection<Problem> Problems { get; set; }

    }
}
