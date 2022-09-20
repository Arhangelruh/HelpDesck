namespace HelpDesk.BLL.Models
{
    public class FAQDto
    {
        /// <summary>
        /// FAQ id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Theme.
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// Text.
        /// </summary>
        public string Description { get; set; }
    }
}
