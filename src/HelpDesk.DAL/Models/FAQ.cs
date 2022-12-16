using HelpDesk.Common.Interfaces;

namespace HelpDesk.DAL.Models
{
    /// <summary>
    /// FAQs.
    /// </summary>
    public class FAQ : IHasDbIdentity
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

        /// <inheritdoc/>
        public int FAQTopicId { get; set; }

        /// <summary>
        /// Navigation to FAQTopic.
        /// </summary>
        public FAQTopic FAQTopic { get; set; }
    }
}
