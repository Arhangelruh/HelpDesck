using System;

namespace HelpDesk.BLL.Models
{
    public class RequestDto
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

        /// <summary>
        /// Profile Creator.
        /// </summary>
        public int ProfileCreatorId { get; set; }

        /// <summary>
        /// Status.
        /// </summary>
        public int StatusId { get; set; }
    }
}
