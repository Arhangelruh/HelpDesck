using HelpDesk.Common.Interfaces;
using System.Collections.Generic;

namespace HelpDesk.DAL.Models
{
    public class FAQTopic : IHasDbIdentity
    {
        /// <inheritdoc/>
        public int Id { get; set; }

        /// <summary>
        /// Theme.
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// Navigation to FAQ.
        /// </summary>
        public ICollection<FAQ> FAQs { get; set; }
    }
}
