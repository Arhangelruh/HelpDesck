using HelpDesk.BLL.Interfaces;
using HelpDesk.BLL.Models;
using HelpDesk.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.DirectoryServices;

namespace HelpDesk.BLL.Services
{
    /// <inheritdoc cref="IProfileService<T>"/>
    public class ProfileService : IProfileService
    {
        private readonly UserManager<User> _userManager;

        public ProfileService(UserManager<User> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public List<UserDto> ADOrgStructSync()
        {
            List<UserDto> users = new List<UserDto>();

            DirectoryEntry dir = new DirectoryEntry("");
            DirectorySearcher search = new DirectorySearcher(dir);

            search.Filter = "(&(objectCategory=person)(objectClass=user)(!userAccountControl:1.2.840.113556.1.4.803:=2))";

            search.SearchScope = SearchScope.Subtree;

            try
            {
                foreach (SearchResult result in search.FindAll())
                {
                    var entry = result.GetDirectoryEntry();

                    users.Add(new UserDto()
                    {
                        ADName = entry.Properties["cn"].Value != null ? entry.Properties["cn"].Value.ToString() : "NoN",
                        DisplayName = entry.Properties["displayName"].Value != null ? entry.Properties["displayName"].Value.ToString() : "NoN",
                        FirstName = entry.Properties["givenName"].Value != null ? entry.Properties["givenName"].Value.ToString() : "NoN",
                        LastName = entry.Properties["sn"].Value != null ? entry.Properties["sn"].Value.ToString() : "NoN",
                        EMail = entry.Properties["mail"].Value != null ? entry.Properties["mail"].Value.ToString() : "NoN",
                        //    MobileNumber = entry.Properties["mobile"].Value != null ? long.Parse(entry.Properties["mobile"].Value.ToString()) : 0,
                        //    NumberFull = entry.Properties["telephoneNumber"].Value != null ? long.Parse(entry.Properties["telephoneNumber"].Value.ToString()) : 0,
                        MobileNumber = entry.Properties["mobile"].Value != null ? entry.Properties["mobile"].Value.ToString() : "NoN",
                        NumberFull = entry.Properties["telephoneNumber"].Value != null ? entry.Properties["telephoneNumber"].Value.ToString() : "NoN",
                    });
                }
                return users;
            }
            catch {
                return users;
            }
        }
    }
}
