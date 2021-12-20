using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Web.ViewModels
{
    /// <summary>
    /// Status model.
    /// </summary>
    public class StatusViewModel
    {
        /// <summary>
        /// Status id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Satatus name.
        /// </summary>
        [Required(ErrorMessage = "Наименование статуса не может быть пустым")]
        public string StatusName { get; set; }

        /// <summary>
        /// Name buttons.
        /// </summary>
        [Required(ErrorMessage = "Поле может быть пустым")]
        public string StatusNameFromButton { get; set; }

        /// <summary>
        /// Order number.
        /// </summary>
        [Required(ErrorMessage = "Номер в очереди не может быть пустым")]
        public int Queue { get; set; }

        /// <summary>
        /// Access status from user.
        /// </summary>
        [Required(ErrorMessage = "Поле доступность не может быть пустым")]
        public bool Access { get; set; }
    }
}
