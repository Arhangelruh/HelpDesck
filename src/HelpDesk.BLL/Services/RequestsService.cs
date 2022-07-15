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
    /// <inheritdoc cref="IRequestsService<T>"/>
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

            var status = await _repositoryStatus.GetEntityWithoutTrackingAsync(status => status.Queue == 1);
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

            var editRequest = await _repositoryProblem.GetEntityWithoutTrackingAsync(q => q.Id.Equals(request.Id));
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

            var requestSearch = await _repositoryProblem.GetEntityWithoutTrackingAsync(q => q.Id.Equals(request.Id));
            if (requestSearch is null)
            {
                return;
            }

            var userconnections = await _repositoryUserProblem
                .GetAll()
                .AsNoTracking()
                .Where(userproblem => userproblem.ProblemId == request.Id)
                .ToListAsync();
           
            if (userconnections.Any())
            {
                foreach (var userconnection in userconnections) {
                          _repositoryUserProblem.Delete(userconnection);
                    await _repositoryUserProblem.SaveChangesAsync();
                }
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

                var userAndAdmin = await GetUserAndAdminProblemAsync(request);
                dtoModel.ProfileCreatorId = userAndAdmin.ProfileCreatorId;
                dtoModel.ProfileAdminId = userAndAdmin.ProfileAdminId;

                requestDtos.Add(dtoModel);
            }

            return requestDtos;
        }

        public async Task<RequestDto> GetRequestByIdAsync(int id)
        {
            var request = await _repositoryProblem.GetEntityWithoutTrackingAsync(request => request.Id == id);
            if (request is null)
            {
                return null;
            }

            var dtoModel = new RequestDto
            {
                Id = request.Id,
                Theme = request.Theme,
                Description = request.Description,
                Ip = request.Ip,
                IncomingDate = request.IncomingDate,
                StatusId = request.StatusId,                
            };

            var userAndAdmin = await GetUserAndAdminProblemAsync(request);
            dtoModel.ProfileCreatorId = userAndAdmin.ProfileCreatorId;
            dtoModel.ProfileAdminId = userAndAdmin.ProfileAdminId;

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
                var request = await _repositoryProblem.GetEntityWithoutTrackingAsync(problem => problem.Id.Equals(requestId.ProblemId));
                var adminUser = await _repositoryUserProblem
                    .GetEntityWithoutTrackingAsync(userrequest => userrequest.ProblemId == requestId.ProblemId
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

        public async Task <RequestDto> GetUserAndAdminProblemAsync(Problem problem)
        {
            var requestDtoModel = new RequestDto();

            var firstUserProblem = await _repositoryUserProblem.GetEntityWithoutTrackingAsync(userproblem => userproblem.ProblemId == problem.Id);

            if (firstUserProblem != null)
            {
                var getProfile = await _repositoryProfile.GetEntityWithoutTrackingAsync(profile => profile.Id.Equals(firstUserProblem.ProfileId));
                if (getProfile != null)
                {
                    var searchUser = await _userManager.FindByIdAsync(getProfile.UserId);
                    if (searchUser != null)
                    {
                        var checkRole = await _userManager.IsInRoleAsync(searchUser, UserConstants.AdminRole);
                        if (checkRole)
                        {
                            var doubleUserProblem = await _repositoryUserProblem
                                .GetEntityWithoutTrackingAsync(userproblem => userproblem.ProblemId == problem.Id && userproblem.ProfileId != firstUserProblem.ProfileId);
                            if (doubleUserProblem != null)
                            {
                                requestDtoModel.ProfileAdminId = getProfile.Id;
                                requestDtoModel.ProfileCreatorId = doubleUserProblem.ProfileId;
                            }
                            else
                            {
                                requestDtoModel.ProfileAdminId = getProfile.Id;
                            }
                        }
                        else
                        {
                            var doubleUserProblem = await _repositoryUserProblem
                                .GetEntityWithoutTrackingAsync(userproblem => userproblem.ProblemId == problem.Id && userproblem.ProfileId != firstUserProblem.ProfileId);
                            if (doubleUserProblem != null)
                            {
                                requestDtoModel.ProfileCreatorId = getProfile.Id;
                                requestDtoModel.ProfileAdminId = doubleUserProblem.ProfileId;
                            }
                            else
                            {
                                requestDtoModel.ProfileCreatorId = getProfile.Id;
                            }
                        }
                    }
                }
            }
            return requestDtoModel;
        }

        public async Task EditRequestAsync(RequestDto request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var requestSearch = await _repositoryProblem.GetEntityWithoutTrackingAsync(q => q.Id.Equals(request.Id));
            if (requestSearch is null)
            {
                return;
            }

            requestSearch.Theme = request.Theme;
            requestSearch.Description = request.Description;
            requestSearch.Ip = request.Ip;
            _repositoryProblem.Update(requestSearch);
            await _repositoryProblem.SaveChangesAsync();
        }

        public async Task AddToWorkAsync(RequestDto request, ProfileDto profile) {
           if(request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
           if(profile is null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            var newConnectionUserProblem = new UserProblem
            {
                ProblemId = request.Id,
                ProfileId = profile.Id
            };

            await _repositoryUserProblem.AddAsync(newConnectionUserProblem);
            await _repositoryUserProblem.SaveChangesAsync();
        }
    }
}
