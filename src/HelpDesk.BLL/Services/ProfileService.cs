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
    /// <inheritdoc cref="IProfileService<T>"/>
    public class ProfileService : IProfileService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepository<Profile> _repository;
        private readonly IRepository<UserProblem> _repositoryUserProblem;

        public ProfileService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IRepository<Profile> repository, IRepository<UserProblem> repositoryUserProblem)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _repositoryUserProblem = repositoryUserProblem ?? throw new ArgumentNullException(nameof(repositoryUserProblem));
        }

        public async Task AddAsyncUsers(List<UserDto> users)
        {
            if (users is null)
            {
                throw new ArgumentNullException(nameof(users));
            }

            foreach (var userAD in users)
            {
                var user = new User
                {
                    Email = userAD.EMail,
                    PhoneNumber = userAD.MobileNumber,
                    UserName = userAD.Login,                    
                };

                var searchUser = await _userManager.FindByNameAsync(userAD.Login);
                
                if (searchUser == null)
                {
                    if (userAD.UserSID != null)
                    {
                        var searchUserbySid = await GetUserBySid(userAD.UserSID);

                        if (searchUserbySid == null)
                        {

                            var result = await _userManager.CreateAsync(user, UserConstants.FirstPassword);

                            if (result.Succeeded)
                            {
                                if (userAD.IsAdmin)
                                {
                                    var checkRole = await _roleManager.FindByNameAsync(UserConstants.AdminRole);
                                    if (checkRole != null)
                                    {
                                        await _userManager.AddToRoleAsync(user, UserConstants.AdminRole);
                                    }
                                    else
                                    {
                                        await _roleManager.CreateAsync(new IdentityRole(UserConstants.AdminRole));
                                        await _userManager.AddToRoleAsync(user, UserConstants.AdminRole);
                                    }
                                    
                                }
                                else
                                {
                                    var checkRole = await _roleManager.FindByNameAsync(UserConstants.UserRole);
                                    if (checkRole != null)
                                    {
                                        await _userManager.AddToRoleAsync(user, UserConstants.UserRole);
                                    }
                                    else
                                    {
                                        await _roleManager.CreateAsync(new IdentityRole(UserConstants.UserRole));
                                        await _userManager.AddToRoleAsync(user, UserConstants.UserRole);
                                    }                                   
                                }

                                var userProfile = new Profile
                                {
                                    UserId = user.Id,
                                    FirstName = userAD.FirstName,
                                    LastName = userAD.LastName,
                                };
                                await _repository.AddAsync(userProfile);
                                await _repository.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            await _userManager.UpdateAsync(user);
                        }
                    }
                }
                else
                {
                    var searchProfile = await _repository.GetEntityAsync(profile => profile.UserId.Equals(searchUser.Id));

                    if (searchProfile.UserSid != userAD.UserSID)
                    {
                        
                        var usersProblem = await _repositoryUserProblem
                        .GetAll()
                        .AsNoTracking()
                        .Where(problem => problem.ProfileId == searchProfile.Id)
                        .ToListAsync();

                        if (usersProblem.Any())
                        {
                            foreach (var userProblem in usersProblem)
                            {
                                _repositoryUserProblem.Delete(userProblem);
                                await _repositoryUserProblem.SaveChangesAsync();
                            }
                        }

                        var userProfile = new Profile
                        {
                            FirstName = userAD.FirstName,
                            LastName = userAD.LastName,
                            UserSid = userAD.UserSID
                        };
                        searchProfile.FirstName = userAD.FirstName;                        
                        searchProfile.LastName = userAD.LastName;
                        searchProfile.MiddleName = "null";
                        _repository.Update(searchProfile);
                        await _repository.SaveChangesAsync();                       
                    }
                   
                }
            }
        }

        public async Task AddProfileAsync(ProfileDto profile)
        {
            if (profile is null)
            {
                throw new ArgumentNullException(nameof(profile));
            }
            var userProfile = new Profile
            {
                UserId = profile.UserId,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                MiddleName = profile.MiddleName,
            };

            await _repository.AddAsync(userProfile);
            await _repository.SaveChangesAsync();
        }

        public async Task EditUserAsync(UserDto user)
        {

            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var searchUser = await _userManager.FindByIdAsync(user.Id);
            var searchProfile = await _repository.GetEntityAsync(profile => profile.UserId == searchUser.Id);
            searchProfile.FirstName = user.FirstName;
            searchProfile.MiddleName = user.MiddleName;
            searchProfile.LastName = user.LastName;
            _repository.Update(searchProfile);
            await _repository.SaveChangesAsync();
           
            if (user.Role != null) {
                var checkRole = await _userManager.IsInRoleAsync(searchUser, user.Role);
                if (!checkRole) {
                    var userRoles = await _userManager.GetRolesAsync(searchUser);
                    foreach( var roles in userRoles)
                    {
                        await _userManager.RemoveFromRoleAsync(searchUser, roles);
                    }
                    await _userManager.AddToRoleAsync(searchUser, user.Role);
                }
            }
            searchUser.UserName = user.Login;
            searchUser.Email = user.EMail;
            searchUser.PhoneNumber = user.MobileNumber;
            
            await _userManager.UpdateAsync(searchUser);
        }

        public async Task<User> GetUserBySid(string sid)
        {
            var profileDataModel = await _repository.GetEntityAsync(profileModel => profileModel.UserSid == sid);

            if (profileDataModel is null)
            {
                return new User();
            }

            var userDataModel = await _userManager.FindByIdAsync(profileDataModel.UserId);
            
            return userDataModel;
        }

        public async Task<ProfileDto> GetProfileByUserId(string userId)
        {
            if (userId is null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var profiles = await _repository.GetAll().AsNoTracking().ToListAsync();
            var profileDataModel = profiles.FirstOrDefault(c => c.UserId.Equals(userId));
            if (profileDataModel is null)
            {
                return new ProfileDto();
            }

            var user = await _userManager.FindByIdAsync(userId);
            string mobileNumber = user.PhoneNumber;
            string Email = user.Email;
            if (user == null)
            {
                mobileNumber = "Null";
                Email = "Null";
            }

            var profile = new ProfileDto
            {
                UserId = profileDataModel.UserId,
                FirstName = profileDataModel.FirstName,
                LastName = profileDataModel.LastName,
                MiddleName = profileDataModel.MiddleName,
                UserSid = profileDataModel.UserSid,
                MobileNumber = mobileNumber,
                Email = Email
            };
            profile.Id = profileDataModel.Id;
            return profile;
        }

        public async Task EditProfile(ProfileDto profile)
        {
            if (profile is null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            var editProfile = await _repository.GetEntityAsync(q => q.Id.Equals(profile.Id));
            editProfile.FirstName = profile.FirstName;
            editProfile.MiddleName = profile.MiddleName;
            editProfile.LastName = profile.LastName;
            _repository.Update(editProfile);
            await _repository.SaveChangesAsync();
        }

        public async Task Locking(int profileId)
        {               
            var getProfile = await _repository.GetEntityAsync(q => q.Id.Equals(profileId));
            var getUser = await _userManager.FindByIdAsync(getProfile.UserId);
            getUser.LockoutEnd = DateTime.UtcNow.AddYears(200);
            await _userManager.UpdateAsync(getUser);
        }

        public async Task UnLock(int profileId)
        {
            var getProfile = await _repository.GetEntityAsync(q => q.Id.Equals(profileId));
            var getUser = await _userManager.FindByIdAsync(getProfile.UserId);
            getUser.LockoutEnd = DateTime.UtcNow.AddYears(-1);
            await _userManager.UpdateAsync(getUser);
        }

        public async Task<IEnumerable<ProfileDto>> GetAsyncProfiles()
        {
            var profileDtos = new List<ProfileDto>();
            var usersDto = _userManager.Users;

            if (!usersDto.Any())
            {
                return profileDtos;
            }
            var profiles = await _repository.GetAll().AsNoTracking().ToListAsync();

            foreach (var user in usersDto)
            {
                var profile = profiles.FirstOrDefault(c => c.UserId.Equals(user.Id));

                profileDtos.Add(new ProfileDto { 
                 Id = profile.Id,
                 UserId = profile.UserId,
                 FirstName = profile.FirstName,
                 LastName = profile.LastName,
                 MiddleName = profile.MiddleName,
                 Email = user.Email,
                 MobileNumber = user.PhoneNumber,
                });                
            }
           
            return profileDtos;
        }

        public async Task<ProfileDto> GetProfileByIdAsync(int id)
        {            
            var getProfile = await _repository.GetEntityAsync(q => q.Id.Equals(id));
            var profile = new ProfileDto
            {
                Id = getProfile.Id,
                UserId = getProfile.UserId,
                FirstName = getProfile.FirstName,
                LastName = getProfile.LastName,
                MiddleName = getProfile.MiddleName
            };
            return profile;
        }

        public async Task DeleteUserAsync(int id)
        {
            var profile = await _repository.GetEntityAsync(profile => profile.Id.Equals(id));
            if (profile != null)
            {
                var user = await _userManager.FindByIdAsync(profile.UserId);
                var userProblems = await _repositoryUserProblem
                    .GetAll()
                    .AsNoTracking()
                    .Where(problem => problem.ProfileId == profile.Id)
                    .ToListAsync();

                if (userProblems.Any())
                {
                    foreach (var userProblem in userProblems)
                    {
                        _repositoryUserProblem.Delete(userProblem);
                        await _repositoryUserProblem.SaveChangesAsync();
                    }
                }

                _repository.Delete(profile);
                await _repository.SaveChangesAsync();

                if(user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
            }            
        }
    }
}
