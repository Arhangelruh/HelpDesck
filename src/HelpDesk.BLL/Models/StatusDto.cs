namespace HelpDesk.BLL.Models
{
    public class StatusDto
    {
        /// <summary>
        /// Status id.
        /// </summary>
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
        /// Name from buttons.
        /// </summary>
        public string StatusNameFromButton { get; set; }

        /// <summary>
        /// Access status from user.
        /// </summary>
        public bool Access { get; set; }
    }
}
