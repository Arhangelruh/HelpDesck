using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Web.ViewModels
{
    /// <summary>
    /// Models for request in new password.
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Email.
        /// </summary>
        [Required(ErrorMessage = "Поле не может быть пустым")]
        [EmailAddress]
        public string Email { get; set; }
    }
}