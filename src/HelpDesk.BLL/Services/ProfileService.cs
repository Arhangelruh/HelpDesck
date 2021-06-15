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
                    Email = CheckFromNull(userAD.EMail).GetAwaiter().GetResult(),
                    PhoneNumber = CheckFromNull(userAD.MobileNumber).GetAwaiter().GetResult(),
                    UserName = CheckFromNull(userAD.Login).GetAwaiter().GetResult(),                    
                };

                var searchUser = _userManager.FindByNameAsync(userAD.Login).GetAwaiter().GetResult();
                
                if (searchUser == null)
                {
                    if (userAD.UserSID != null)
                    {
                        var searchUserbySid = GetUserBySid(userAD.UserSID).GetAwaiter().GetResult();

                        if (searchUserbySid == null)
                        {

                            var result = _userManager.CreateAsync(user, UserConstants.FirstPassword).GetAwaiter().GetResult();

                            if (result.Succeeded)
                            {
                                if (userAD.IsAdmin)
                                {
                                    var checkRole = _roleManager.FindByNameAsync(UserConstants.AdminRole).GetAwaiter().GetResult();
                                    if (checkRole != null)
                                    {
                                         _userManager.AddToRoleAsync(user, UserConstants.AdminRole).GetAwaiter().GetResult();
                                    }
                                    else
                                    {
                                         _roleManager.CreateAsync(new IdentityRole(UserConstants.AdminRole)).GetAwaiter().GetResult();
                                         _userManager.AddToRoleAsync(user, UserConstants.AdminRole).GetAwaiter().GetResult();
                                    }
                                    
                                }
                                else
                                {
                                    var checkRole = _roleManager.FindByNameAsync(UserConstants.UserRole).GetAwaiter().GetResult();
                                    if (checkRole != null)
                                    {
                                         _userManager.AddToRoleAsync(user, UserConstants.UserRole).GetAwaiter().GetResult();
                                    }
                                    else
                                    {
                                         _roleManager.CreateAsync(new IdentityRole(UserConstants.UserRole)).GetAwaiter().GetResult();
                                        _userManager.AddToRoleAsync(user, UserConstants.UserRole).GetAwaiter().GetResult();
                                    }                                   
                                }

                                var userProfile = new Profile
                                {
                                    UserId = CheckFromNull(user.Id).GetAwaiter().GetResult(),
                                    FirstName = CheckFromNull(userAD.FirstName).GetAwaiter().GetResult(),
                                    LastName = CheckFromNull(userAD.LastName).GetAwaiter().GetResult(),
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
                    var searchProfile =  _repository.GetEntityAsync(profile => profile.UserId.Equals(searchUser.Id)).GetAwaiter().GetResult();

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
                            FirstName = CheckFromNull(userAD.FirstName).GetAwaiter().GetResult(),
                            LastName = CheckFromNull(userAD.LastName).GetAwaiter().GetResult(),
                            UserSid = CheckFromNull(userAD.UserSID).GetAwaiter().GetResult()
                        };
                        searchProfile.FirstName = CheckFromNull(userAD.FirstName).GetAwaiter().GetResult();                        
                        searchProfile.LastName = CheckFromNull(userAD.LastName).GetAwaiter().GetResult(); 
                        searchProfile.MiddleName = null;
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
                return null;
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

        public async Task <string> CheckFromNull (string value) {

            if (value is null) {
                return null;
            }

             var result =  await Task.Run(() =>
            {
                if (value == "NoN" || value == "null")
                {
                    return null;
                }
                else
                {
                    return value;
                }
            });

            return result;
        }
    }
}
