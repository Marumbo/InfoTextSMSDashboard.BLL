using InfoTextSMSDashboard.BLL.Models;
using InfoTextSMSDashboard.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoTextSMSDashboard.BLL.Interfaces
{
   public interface IGroupService
    {
        Task<OutputResponse> GetAllGroups();
        Task<OutputResponse> GetGroupById(int id);

        Task<OutputResponse> UpdateGroup(GroupsDTO group);

        Task<OutputResponse> AddGroup(GroupsDTO group);

        Task<OutputResponse> RemoveGroup(int id);
    }
}
