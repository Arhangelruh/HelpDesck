using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Web.ViewModels
{
    /// <summary>
    /// Model for change password.
    /// </summary>
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// New password.
        /// </summary>
        [Required(ErrorMessage = "Новый пароль не может быть пустым")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Old password.
        /// </summary>
        [Required(ErrorMessage = "Старый пароль не может быть пустым")]
        public string OldPassword { get; set; }
    }
}