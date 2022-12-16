using HelpDesk.BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpDesk.BLL.Interfaces
{
    /// <summary>
    /// Work with FAQ
    /// </summary>
    public interface IFAQService
    {
        /// <summary>
        /// Add FAQ Topic.
        /// </summary>
        /// <param name="fAQTopicDto"></param>
        /// <returns></returns>
        Task AddFaqTopicAsync(FAQTopicDto fAQTopicDto);

        /// <summary>
        /// Edit FAQ Topic.
        /// </summary>
        /// <param name="fAQTopicDto"></param>
        /// <returns></returns>
        Task EditFAQTopicAsync(FAQTopicDto fAQTopicDto);

        /// <summary>
        /// Get FAQ Topic by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>FAQTopicDto</returns>
        Task<FAQTopicDto> GetFAQTopicByIdAsync(int id);

        /// <summary>
        /// Get all FAQ Topics.
        /// </summary>
        /// <returns>list FAQTopicDto</returns>
        Task<List<FAQTopicDto>> GetAllFAQTopicAsync();

        /// <summary>
        /// Delete FAQ Topic.
        /// </summary>
        /// <param name="fAQTopicDto"></param>
        /// <returns>result</returns>
        Task<bool> DeleteFAQTopicAsync(FAQTopicDto fAQTopicDto);

        /// <summary>
        /// Get FAQs by Topic.
        /// </summary>
        /// <param name="fAQTopicId"></param>
        /// <returns></returns>
        Task<List<FAQDto>> GetFAQByTopicAsync(int fAQTopicId);

        /// <summary>
        /// Add FAQ.
        /// </summary>
        /// <param name="fAQDto">Dto model</param>
        Task AddFAQAsync(FAQDto fAQDto);

        /// <summary>
        /// Delete FAQ.
        /// </summary>
        /// <param name="FAQid">FAQ id</param>
        Task DeleteFAQAsync(int FAQid);

        /// <summary>
        /// Get FAQs.
        /// </summary>
        /// <returns>list FAQ</returns>
        Task<List<FAQDto>> GetAllFAQAsync();

        /// <summary>
        /// Edit FAQ
        /// </summary>
        /// <param name="fAQDto"></param>
        /// <returns></returns>
        Task EditFAQAsync(FAQDto fAQDto);

        /// <summary>
        /// Get FAQ by id.
        /// </summary>
        /// <param name="id">FAQ id</param>
        /// <returns>FAQDto</returns>
        Task<FAQDto> GetFAQByIdAsync(int id);
    }
}
