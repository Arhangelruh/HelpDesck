using HelpDesk.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Web.ViewModels
{
    public class FAQTopicViewModel
    {
        /// <summary>
        /// FAQ id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// FAQ bloc Topic.
        /// </summary>
        [Required(ErrorMessage = "Поле не может быть пустым")]
        [StringLength(ConfigurationContants.SqlMaxLengthMedium, MinimumLength = 3, ErrorMessage = "Это поле должно содержать от {2} до {1} символов")]
        public string Topic { get; set; }
    }
}
