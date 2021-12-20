using HelpDesk.BLL.Interfaces;
using HelpDesk.BLL.Models;
using HelpDesk.Common.Interfaces;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.BLL.Services
{
    /// <inheritdoc cref="IFileService<T>"/>
    public class FileService : IFileService
    {
        private readonly IRepository<SavedFile> _repositoryFiles;

        public FileService(IRepository<SavedFile> repositoryFiles)
        {
            _repositoryFiles = repositoryFiles ?? throw new ArgumentNullException(nameof(repositoryFiles));
        }
        public async Task AddFileAsync(FileDto fileDto)
        {

            if (fileDto is null)
            {
                throw new ArgumentNullException(nameof(fileDto));
            }

            var newFile = new SavedFile
            {
                ProblemId = fileDto.ProblemId,               
                Name = fileDto.Name,
                ContentType = fileDto.ContentType,
                FileBody = fileDto.FileBody                
            };

            await _repositoryFiles.AddAsync(newFile);
            await _repositoryFiles.SaveChangesAsync();
        }

        public async Task DeleteFileAsync(int id)
        {
            var file = await _repositoryFiles.GetEntityWithoutTrackingAsync(file => file.Id == id);               

            if (file != null)
            {
                    _repositoryFiles.Delete(file);
                    await _repositoryFiles.SaveChangesAsync();                
            }
        }

        public async Task DeleteFilesAsync(int problemId)
        {
            var files = await _repositoryFiles
               .GetAll()
               .AsNoTracking()
               .Where(file => file.ProblemId == problemId)
               .ToListAsync();

            if (files.Any())
            {
                foreach (var file in files)
                {
                    _repositoryFiles.Delete(file);
                    await _repositoryFiles.SaveChangesAsync();
                }
            }
        }

        public async Task<FileDto> GetFileAsync(int id)
        {
            var file = await _repositoryFiles.GetEntityWithoutTrackingAsync(file => file.Id == id);

            if (file != null)
            {
                var fileDto = new FileDto
                {
                    Id = file.Id,
                    Name = file.Name,
                    ContentType = file.ContentType,
                    FileBody = file.FileBody
                };

                return fileDto;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<FileDto>> GetFilesAsync(int problemId)
        {
            var filesDto = new List<FileDto>();

            var files = await _repositoryFiles
               .GetAll()
               .AsNoTracking()
               .Where(file => file.ProblemId == problemId)
               .ToListAsync();

            if (files.Any())
            {
                foreach (var file in files)
                {
                    filesDto.Add(new FileDto{
                    Id = file.Id,
                    Name = file.Name,
                    ContentType = file.ContentType,
                    FileBody = file.FileBody
                    });
                }               
            }
            return filesDto;
        }

        public async Task<List<FileDto>> GetFilesNamesAsync(int problemId)
        {
            var filesDto = new List<FileDto>();

            var files = await _repositoryFiles
               .GetAll()
               .AsNoTracking()
               .Where(file => file.ProblemId == problemId)
               .ToListAsync();

            if (files.Any())
            {
                foreach (var file in files)
                {
                    filesDto.Add(new FileDto
                    {
                        Id = file.Id,
                        Name = file.Name,
                        ContentType = file.ContentType
                    });
                }
            }
            return filesDto;
        }
    }
}
