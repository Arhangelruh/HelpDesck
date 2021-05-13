using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Web.ViewModels
{
    /// <summary>
    /// User model.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Id. 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Login. 
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// First name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Middle name.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// User Phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// User Email
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// User admin or no.
        /// </summary>
        public string IsAdmin { get; set; }

        /// <summary>
        /// Check account status
        /// </summary>
        public bool Locking { get; set; }
    }
}
