using Hangfire.Storage;
using System.Threading.Tasks;

namespace HelpDesk.BLL.Interfaces
{
    /// <summary>
    /// Service from work whith schedulle jobs.
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Add job scheduller.
        /// </summary>
        /// <param name="cron">String in cron format</param>
        Task AddJobScheduller(string cron);

        /// <summary>
        /// Get job by id.
        /// </summary>
        Task<RecurringJobDto> GetJobScheduller(string id);

        /// <summary>
        /// Edit job.
        /// </summary>    
        Task EditJobScheduller(string id, string cron);

        /// <summary>
        /// Delete job.
        /// </summary>
        Task DeleteJobScheduller(string id);
    }
}
