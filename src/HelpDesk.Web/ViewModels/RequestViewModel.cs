using System;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Web.ViewModels
{
    /// <summary>
    /// Request model.
    /// </summary>
    public class RequestViewModel
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Theme.
        /// </summary>
        [Required(ErrorMessage = "Введите тему.")]
        [StringLength(127, ErrorMessage = "Тема должна содержать от 3 до 127 символов", MinimumLength = 3)]
        public string Theme { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        [Required(ErrorMessage = "Необходимо описание проблемы.")]
        [StringLength(2000, ErrorMessage = "Описание должно содержать от 3 до 2000 символов", MinimumLength = 3)]
        public string Description { get; set; }

        /// <summary>
        /// Ip adress.
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// Creator.
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// Creator.
        /// </summary>
        public string Admin { get; set; }

        /// <summary>
        /// Status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Status queue
        /// </summary>
        public int StatusQueue { get; set; }

        /// <summary>
        /// Incoming date.
        /// </summary>
        public DateTime IncomingDate { get; set; }

    }
}
