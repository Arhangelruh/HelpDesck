using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Web.ViewModels
{
    /// <summary>
    /// Model from create user 
    /// </summary>
    public class CreateUserViewModel
    {
        /// <summary>
        /// Login. 
        /// </summary>
        [Required(ErrorMessage = "Имя пользователя не может быть пустым")]
        [Display(Name = "Имя пользователя")]
        public string Login { get; set; }

        /// <summary>
        /// First name.
        /// </summary>
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name.
        /// </summary>
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        /// <summary>
        /// Middle name.
        /// </summary>
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        /// <summary>
        /// User Phone.
        /// </summary>
        [Display(Name = "Номер телефона")]
        public string Phone { get; set; }

        /// <summary>
        /// User Email
        /// </summary>
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// User admin or no.
        /// </summary>
        [Required]
        public string IsAdmin { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 3)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        /// <summary>
        /// Confirm password
        /// </summary>
        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
    
}
