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
        /// Navigation to ProblemStatus.
        /// </summary>
        public ICollection<Problem> Problems { get; set; }

    }
}
