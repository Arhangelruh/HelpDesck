using Microsoft.AspNetCore.Http;

namespace HelpDesk.Web.ViewModels
{
    public class SaveFileViewModel
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
        /// Body file.
        /// </summary>
        public IFormFile FileBody { get; set; }
    }
}
