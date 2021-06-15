using Hangfire.Common;
using HelpDesk.Web.Attributs;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Web.ViewModels
{
    /// <summary>
    /// Model from Sheduller Job.
    /// </summary>
    public class ReccuringJobViewModel
    {
        /// <summary>
        /// Job ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Cron schedule.
        /// </summary>
        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [CronValidate(ErrorMessage = "Ошибка в Cron выражении!")]
        public string Cron { get; set; }

        /// <summary>
        /// Job.
        /// </summary>
        public Job Job { get; set; }
    }
}
