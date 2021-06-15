using HelpDesk.BLL.Interfaces;
using HelpDesk.BLL.Models;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.BLL.Services
{
    /// <inheritdoc cref="IGetUserFromAD<T>"/>
    public class GetUserFromAD : IGetUserFromAD
    {
        public async Task <List<UserDto>> ADGetUsers()
        {
            List<UserDto> users = new List<UserDto>();

            DirectoryEntry dir = new DirectoryEntry("");
            DirectorySearcher search = new DirectorySearcher(dir);

            try
            {
                search.Filter = "(&(objectCategory=person)(objectClass=user)(!userAccountControl:1.2.840.113556.1.4.803:=2))";
                search.SearchScope = SearchScope.Subtree;

                byte[] domainSIdArray = (byte[])dir.Properties["objectSid"].Value;
                SecurityIdentifier domainSId = new SecurityIdentifier(domainSIdArray, 0);
                SecurityIdentifier domainAdminsSId = new SecurityIdentifier(WellKnownSidType.AccountDomainAdminsSid, domainSId);
                DirectoryEntry groupEntry = new DirectoryEntry(string.Format("LDAP://<SID={0}>", BuildOctetString(domainAdminsSId).GetAwaiter().GetResult()));
                string adminDn = groupEntry.Properties["distinguishedname"].Value as string;

                await Task.Run(() =>
                {
                    foreach (SearchResult result in search.FindAll())
                    {
                        var entry = result.GetDirectoryEntry();

                        List<string> group = new List<string>();

                        var memberOfGroups = entry.Properties["memberOf"];

                        var primaryGroupId = entry.Properties["primaryGroupID"].Value;
                        var primaryGroupSID = domainSId + "-" + primaryGroupId;
                        var resultSearshPrimaryGrop = new DirectoryEntry(string.Format("LDAP://<SID={0}>", primaryGroupSID));
                        string namePrimaryGroup = resultSearshPrimaryGrop.Properties["distinguishedname"].Value as string;
                        group.Add(namePrimaryGroup);

                        foreach (var g in memberOfGroups)
                        {
                            group.Add(g.ToString());
                        }

                        bool isAdmin = group.Contains(adminDn);

                        byte[] userSIdArray = (byte[])entry.Properties["objectSid"].Value;
                        SecurityIdentifier userSId = new SecurityIdentifier(userSIdArray, 0);
                        var userSID = BuildOctetString(userSId).GetAwaiter().GetResult();

                        users.Add(new UserDto()
                        {
                            Login = entry.Properties["samAccountName"].Value != null ? entry.Properties["samAccountName"].Value.ToString() : "NoN",
                            ADName = entry.Properties["cn"].Value != null ? entry.Properties["cn"].Value.ToString() : "NoN",
                            DisplayName = entry.Properties["displayName"].Value != null ? entry.Properties["displayName"].Value.ToString() : "NoN",
                            FirstName = entry.Properties["givenName"].Value != null ? entry.Properties["givenName"].Value.ToString() : "NoN",
                            LastName = entry.Properties["sn"].Value != null ? entry.Properties["sn"].Value.ToString() : "NoN",
                            EMail = entry.Properties["mail"].Value != null ? entry.Properties["mail"].Value.ToString() : "NoN",
                            MobileNumber = entry.Properties["mobile"].Value != null ? entry.Properties["mobile"].Value.ToString() : "NoN",
                            NumberFull = entry.Properties["telephoneNumber"].Value != null ? entry.Properties["telephoneNumber"].Value.ToString() : "NoN",
                            IsAdmin = isAdmin,
                            UserSID = userSID
                        });
                    }
                });
                return users;
            }
            catch
            {
                return users;
            }
        }

        public async Task <string> BuildOctetString(SecurityIdentifier sid)
        {
            byte[] items = new byte[sid.BinaryLength];
            sid.GetBinaryForm(items, 0);
            StringBuilder sb = new StringBuilder();
            await Task.Run(() =>
            {
                foreach (byte b in items)
                {
                    sb.Append(b.ToString("X2"));
                }
            });
            return sb.ToString();
        }
    }
}
