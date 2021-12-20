using HelpDesk.BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpDesk.BLL.Interfaces
{
    /// <summary>
    /// Class from works with files.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Add file.
        /// </summary>
        /// <param name="fileDto">Dto model</param>
        Task AddFileAsync(FileDto fileDto);

        /// <summary>
        /// Delete file.
        /// </summary>
        /// <param name="id">id file</param>
        Task DeleteFileAsync(int id);

        /// <summary>
        /// Delete files.
        /// </summary>
        /// <param name="id">id problem</param>
        Task DeleteFilesAsync(int problemId);

        /// <summary>
        /// Get file.
        /// </summary>
        /// <param name="id">id file</param>
        Task <FileDto> GetFileAsync(int id);

        /// <summary>
        /// Get files.
        /// </summary>
        /// <param name="id">id file</param>
        Task <List<FileDto>> GetFilesAsync(int problemId);

        /// <summary>
        /// Get files without body file.
        /// </summary>
        /// <param name="id">id file</param>
        Task<List<FileDto>> GetFilesNamesAsync(int problemId);
    }
}
