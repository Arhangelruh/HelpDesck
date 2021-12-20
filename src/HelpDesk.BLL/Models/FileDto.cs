namespace HelpDesk.BLL.Models
{
    public class FileDto
    {
        /// <inheritdoc/>
        public int Id { get; set; }

        /// <inheritdoc/>
        public int ProblemId { get; set; }

        /// <summary>
        /// File Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Body file.
        /// </summary>
        public byte[] FileBody { get; set; }
    }
}
