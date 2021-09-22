namespace HelpDesk.Web.ViewModels
{
    public class CommentViewModel
    {
        /// <summary>
        /// Comment id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Profile id.
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// Data and time create comment.
        /// </summary>
        public string CreateComment { get; set; }

        /// <summary>
        /// Comment.
        /// </summary>
        public string Comment { get; set; }
    }
}
