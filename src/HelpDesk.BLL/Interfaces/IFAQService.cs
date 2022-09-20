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
