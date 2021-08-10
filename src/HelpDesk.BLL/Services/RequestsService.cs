using HelpDesk.BLL.Interfaces;
using HelpDesk.BLL.Models;
using HelpDesk.Common.Constants;
using HelpDesk.Common.Interfaces;
using HelpDesk.DAL.Models;
using Microsoft.AspNetCore.Identity;
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
        private readonly IRepository<UserProblem> _repositoryUserProblem;
        private readonly IRepository<Profile> _repositoryProfile;        
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Status> _repositoryStatus;

        public RequestsService(IRepository<Problem> repositoryProblem,
            IRepository<UserProblem> repositoryUserProblem,
            IRepository<Profile> repositoryProfile,
            UserManager<User> userManager,
            IRepository<Status> repositoryStatus) {
            _repositoryProblem = repositoryProblem ?? throw new ArgumentNullException(nameof(repositoryProblem));
            _repositoryUserProblem = repositoryUserProblem ?? throw new ArgumentNullException(nameof(repositoryUserProblem));
            _repositoryProfile = repositoryProfile ?? throw new ArgumentNullException(nameof(repositoryProfile));            
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _repositoryStatus = repositoryStatus ?? throw new ArgumentNullException(nameof(repositoryStatus));
        }
        public async Task AddRequestAsync(RequestDto request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var status = await _repositoryStatus.GetEntityAsync(status => status.Queue == 1);
            var dateRequest = DateTime.Now;

            var newRequest = new Problem
            {                
                Theme = request.Theme,
                Description = request.Description,
                Ip = request.Ip,
                IncomingDate = dateRequest,                
                StatusId = status.Id,                
            };

            await _repositoryProblem.AddAsync(newRequest);
            await _repositoryProblem.SaveChangesAsync();

            var newConnectionUserProblem = new UserProblem
            {
                ProblemId = newRequest.Id,
                ProfileId = request.ProfileCreatorId
            };

            await _repositoryUserProblem.AddAsync(newConnectionUserProblem);
            await _repositoryUserProblem.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(RequestDto request, int statusId)
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

        public async Task DeleteRequestAsync(RequestDto request)
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

        public async Task<List<RequestDto>> GetAllRequestsAsync()
        {
            var requestDtos = new List<RequestDto>();
            var requests = await _repositoryProblem.GetAll().AsNoTracking().ToListAsync();

            

            foreach (var request in requests)
            {
                             
                var dtoModel = new RequestDto
                {
                    Id = request.Id,
                    Theme = request.Theme,
                    Description = request.Description,
                    Ip = request.Ip,
                    IncomingDate = request.IncomingDate,
                    StatusId = request.StatusId
                };

                var firstUserProblem = await _repositoryUserProblem.GetEntityAsync(problem => problem.ProblemId == request.Id);

                if (firstUserProblem != null)
                {
                    var getProfile = await _repositoryProfile.GetEntityAsync(profile => profile.Id.Equals(firstUserProblem.ProfileId));
                    if (getProfile != null)
                    {
                        var searchUser = await _userManager.FindByIdAsync(getProfile.UserId);
                        if (searchUser != null)
                        {

                            var checkRole = await _userManager.IsInRoleAsync(searchUser, UserConstants.AdminRole);
                            if (checkRole)
                            {                           
                                var doubleUserProblem = await _repositoryUserProblem
                                    .GetEntityAsync(problem => problem.ProblemId == request.Id && problem.ProfileId != firstUserProblem.ProfileId);
                                if (doubleUserProblem != null)
                                {
                                    dtoModel.ProfileAdminId = getProfile.Id;
                                    dtoModel.ProfileCreatorId = doubleUserProblem.ProfileId;
                                }
                                else
                                {
                                    dtoModel.ProfileAdminId = getProfile.Id;
                                }
                            }
                            else
                            {
                                var doubleUserProblem = await _repositoryUserProblem
                                    .GetEntityAsync(problem => problem.ProblemId == request.Id && problem.ProfileId != firstUserProblem.ProfileId);
                                if (doubleUserProblem != null)
                                {
                                    dtoModel.ProfileCreatorId = getProfile.Id;
                                    dtoModel.ProfileAdminId = doubleUserProblem.ProfileId;
                                }
                                else
                                {
                                    dtoModel.ProfileCreatorId = getProfile.Id;
                                }
                            }
                        }
                    }
                }

                requestDtos.Add(dtoModel);
            }

            return requestDtos;
        }

        public async Task<RequestDto> GetRequestByIdAsync(int id)
        {
            var request = await _repositoryProblem.GetEntityWithoutTrackingAsync(request => request.Id == id);
            if (request is null)
            {
                return new RequestDto();
            }

            var dtoModel = new RequestDto
            {
                Id = request.Id,
                Theme = request.Theme,
                Description = request.Description,
                Ip = request.Ip,
                IncomingDate = request.IncomingDate,
                StatusId = request.StatusId
            };

            var firstUserProblem = await _repositoryUserProblem.GetEntityAsync(problem => problem.ProblemId == request.Id);

            if (firstUserProblem != null)
            {
                var getProfile = await _repositoryProfile.GetEntityAsync(profile => profile.Id.Equals(firstUserProblem.ProfileId));
                if (getProfile != null)
                {
                    var searchUser = await _userManager.FindByIdAsync(getProfile.UserId);
                    if (searchUser != null)
                    {

                        var checkRole = await _userManager.IsInRoleAsync(searchUser, UserConstants.AdminRole);
                        if (checkRole)
                        {
                            var doubleUserProblem = await _repositoryUserProblem
                                .GetEntityAsync(problem => problem.ProblemId == request.Id && problem.ProfileId != firstUserProblem.ProfileId);
                            dtoModel.ProfileAdminId = getProfile.Id;
                            dtoModel.ProfileCreatorId = doubleUserProblem.ProfileId;
                        }
                        else
                        {
                            var doubleUserProblem = await _repositoryUserProblem
                                .GetEntityAsync(problem => problem.ProblemId == request.Id && problem.ProfileId != firstUserProblem.ProfileId);
                            dtoModel.ProfileCreatorId = getProfile.Id;
                            dtoModel.ProfileAdminId = doubleUserProblem.ProfileId;

                        }
                    }
                }
            }

            return dtoModel;
        }

        public async Task<List<RequestDto>> GetRequestsByUserAsync(int profileId)
        {
            var requestDtos = new List<RequestDto>();

            var userRequests = await _repositoryUserProblem
                .GetAll()
                .AsNoTracking()
                .Where(userrequest => userrequest.ProfileId == profileId)
                .ToListAsync();

            foreach (var requestId in userRequests)
            {
                var request = await _repositoryProblem.GetEntityAsync(problem => problem.Id.Equals(requestId.ProblemId));
                var adminUser = await _repositoryUserProblem
                    .GetEntityAsync(userrequest => userrequest.ProblemId == requestId.ProblemId
                    && userrequest.ProfileId != profileId);
                if (adminUser is null)
                {
                    requestDtos.Add(new RequestDto
                    {
                        Id = request.Id,
                        Theme = request.Theme,
                        Description = request.Description,
                        Ip = request.Ip,
                        IncomingDate = request.IncomingDate,
                        ProfileCreatorId = profileId,
                        StatusId = request.StatusId
                    });
                }
                else
                {
                    requestDtos.Add(new RequestDto
                    {
                        Id = request.Id,
                        Theme = request.Theme,
                        Description = request.Description,
                        Ip = request.Ip,
                        IncomingDate = request.IncomingDate,
                        ProfileCreatorId = profileId,
                        ProfileAdminId = adminUser.ProfileId,
                        StatusId = request.StatusId
                    });
                }
            }

            return requestDtos;
        }
    }
}
