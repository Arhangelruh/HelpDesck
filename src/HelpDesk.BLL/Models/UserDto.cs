﻿namespace HelpDesk.BLL.Models
{
    /// <summary>
    /// Transport model from User profile
    /// </summary>
    class UserDto
    {
        /// <summary>
        /// Active Directory Name 
        /// </summary>
        public string ADName { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Display name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// EMail
        /// </summary>
        public string EMail { get; set; }

        /// <summary>
        ///  Phone
        /// </summary>
        public string NumberFull { get; set; }

        /// <summary>
        /// Mobile Phone
        /// </summary>
        public string MobileNumber { get; set; }
    }
}
