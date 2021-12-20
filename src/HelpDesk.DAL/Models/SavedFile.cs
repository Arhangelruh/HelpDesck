namespace HelpDesk.DAL.Models
{
    public class SavedFile
    {
        /// <inheritdoc/>
        public int Id { get; set; }

        /// <inheritdoc/>
        public int ProblemId { get; set; }

        /// <summary>
        /// Navigation to Problem.
        /// </summary>
        public Problem Problem { get; set; }

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
