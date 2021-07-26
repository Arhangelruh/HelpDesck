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
    public class RequestsService : IRequestsService
    {
        private readonly IRepository<Problem> _repositoryProblem;

        public RequestsService(IRepository<Problem> repositoryProblem) {
            _repositoryProblem = repositoryProblem ?? throw new ArgumentNullException(nameof(repositoryProblem));
        }
        public async Task AddRequest(RequestDto request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var newRequest = new Problem
            {                
                Theme = request.Theme,
                Description = request.Description,
                Ip = request.Ip,
                IncomingDate = request.IncomingDate,
                ProfileCreatorId = request.ProfileCreatorId,
                StatusId = request.StatusId
            };

            await _repositoryProblem.AddAsync(newRequest);
            await _repositoryProblem.SaveChangesAsync();
        }

        public async Task ChangeStatus(RequestDto request, int statusId)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var editRequest = await _repositoryProblem.GetEntityAsync(q => q.Id.Equals(request.Id));
            editRequest.StatusId = statusId;

            _repositoryProblem.Update(editRequest);
            await _repositoryProblem.SaveChangesAsync();
        }

        public async Task DeleteRequest(RequestDto request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var requestSearch = await _repositoryProblem.GetEntityAsync(q => q.Id.Equals(request.Id));
            if (requestSearch is null)
            {
                return;
            }

            _repositoryProblem.Delete(requestSearch);
            await _repositoryProblem.SaveChangesAsync();
        }

        public async Task<List<RequestDto>> GetAllRequests()
        {
            var requestDtos = new List<RequestDto>();
            var requests = await _repositoryProblem.GetAll().AsNoTracking().ToListAsync();


            foreach (var request in requests)
            {
                requestDtos.Add(new RequestDto
                {
                    Id = request.Id,
                    Theme = request.Theme,
                    Description = request.Description,
                    Ip = request.Ip,
                    IncomingDate = request.IncomingDate,
                    ProfileCreatorId = request.ProfileCreatorId,
                    StatusId = request.StatusId
                });
            }

            return requestDtos;
        }

        public async Task<RequestDto> GetRequestById(int id)
        {
            var request = await _repositoryProblem.GetEntityWithoutTrackingAsync(request => request.Id == id);
            if (request is null)
            {
                return new RequestDto();
            }

            var requestDto = new RequestDto
            {
                Id = request.Id,
                Theme = request.Theme,
                Description = request.Description,
                Ip = request.Ip,
                IncomingDate = request.IncomingDate,
                ProfileCreatorId = request.ProfileCreatorId,
                StatusId = request.StatusId
            };

            return requestDto;
        }

        public async Task<List<RequestDto>> GetRequestsByUser(int profileId)
        {
            var requestDtos = new List<RequestDto>();

            var requests = await _repositoryProblem
             .GetAll()
             .AsNoTracking()
             .Where(request => request.ProfileCreatorId == profileId)
             .ToListAsync();

            if (!requests.Any())
            {
                return requestDtos;
            }

            foreach (var request in requests)
            {
                requestDtos.Add(new RequestDto
                {
                    Id = request.Id,
                    Theme = request.Theme,
                    Description = request.Description,
                    Ip = request.Ip,
                    IncomingDate = request.IncomingDate,
                    ProfileCreatorId = request.ProfileCreatorId,
                    StatusId = request.StatusId
                });
            }

            return requestDtos;
        }
    }
}
