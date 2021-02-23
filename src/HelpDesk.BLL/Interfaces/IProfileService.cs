using HelpDesk.BLL.Models;
using System.Collections.Generic;

namespace HelpDesk.BLL.Interfaces
{
    public interface IProfileService
    {
        public List<UserDto> ADOrgStructSync();
    }
}