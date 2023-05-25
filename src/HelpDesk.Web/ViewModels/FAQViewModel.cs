using HelpDesk.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Web.ViewModels
{
    public class FAQViewModel
    {

        /// <summary>
        /// FAQ id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// FAQ theme.
        /// </summary>
        [Required(ErrorMessage = "Поле не может быть пустым")]
        [StringLength(ConfigurationContants.SqlMaxLengthMedium, MinimumLength = 3, ErrorMessage = "Это поле должно содержать от {2} до {1} символов")]
        public string FAQTheme { get; set; }

        /// <summary>
        /// FAQ answer.
        /// </summary>
        [Required(ErrorMessage = "Поле не может быть пустым")]
        [StringLength(ConfigurationContants.SqlMaxLengthLongForDescription, MinimumLength = 3, ErrorMessage = "Это поле должно содержать от {2} до {1} символов")]
        public string FAQAnswer { get; set; }

        /// <summary>
        /// FAQ Topic Id.
        /// </summary>
        public int FAQTopic { get; set; }
    }
}
