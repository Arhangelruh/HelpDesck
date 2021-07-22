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
    /// <inheritdoc cref="IStatusService<T>"/>
    public class StatusService : IStatusService
    {
        private readonly IRepository<Status> _repositoryStatus;
        private readonly IRepository<Problem> _repositoryProblem;

        public StatusService(IRepository<Status> repositoryStatus, IRepository<Problem> repositoryProblem)
        {
            _repositoryStatus = repositoryStatus ?? throw new ArgumentNullException(nameof(repositoryStatus));
            _repositoryProblem = repositoryProblem ?? throw new ArgumentNullException(nameof(repositoryProblem));
        }
        public async Task AddStatusAsync(StatusDto statusDbo)
        {
            if (statusDbo is null)
            {
                throw new ArgumentNullException(nameof(statusDbo));
            }

            var newStatus = new Status
            {
                Id = statusDbo.Id,
                StatusName = statusDbo.StatusName,
                Queue = statusDbo.Queue,
                Access = statusDbo.Access
            };

            await _repositoryStatus.AddAsync(newStatus);
            await _repositoryStatus.SaveChangesAsync();
        }

        public async Task <List<StatusDto>> GetStatusesAsync()
        {
            var statusDtos = new List<StatusDto>();
            var statuses = await _repositoryStatus.GetAll().AsNoTracking().ToListAsync();


            foreach (var status in statuses)
            {
                statusDtos.Add(new StatusDto
                {
                    Id = status.Id,
                    StatusName = status.StatusName,
                    Queue = status.Queue,
                    Access = status.Access
                });
            }

            return statusDtos;
        }

        public async Task EditStatusAsync(StatusDto status)
        {
            if (status is null)
            {
                throw new ArgumentNullException(nameof(status));
            }

            var editStatus = await _repositoryStatus.GetEntityAsync(q => q.Id.Equals(status.Id));
            editStatus.StatusName = status.StatusName;
            editStatus.Queue = status.Queue;
            editStatus.Access = status.Access;
           
            _repositoryStatus.Update(editStatus);
            await _repositoryStatus.SaveChangesAsync();
        }

        public async Task<bool> DeleteStatusAsync(StatusDto statusDto)
        {
            if (statusDto is null)
            {
                throw new ArgumentNullException(nameof(statusDto));
            }
            var problems = await _repositoryProblem
                .GetAll()
                .AsNoTracking()
                .Where(problem => problem.StatusId == statusDto.Id)
                .ToListAsync();

            if (!problems.Any())
            {
                var status = await _repositoryStatus.GetEntityAsync(status => status.Id == statusDto.Id);
                _repositoryStatus.Delete(status);
                await _repositoryStatus.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<StatusDto> SearchStatusAsync(int queue)
        {
            var status = await _repositoryStatus.GetEntityWithoutTrackingAsync(status => status.Queue == queue);
            if (status is null)
            {
                return null;
            }

            var statusDto = new StatusDto
            {
                Id = status.Id,
                StatusName = status.StatusName,
                Queue = status.Queue,
                Access = status.Access
            };

            return statusDto;
        }

        public async Task<StatusDto> GetStatusByIdAsync(int id)
        {

            var status = await _repositoryStatus.GetEntityWithoutTrackingAsync(status => status.Id == id);
            if (status is null)
            {
                return new StatusDto();
            }

            var statusDto = new StatusDto
            {
                Id = status.Id,
                StatusName = status.StatusName,
                Queue = status.Queue,
                Access = status.Access
            };

            return statusDto;
        }
    }
}
