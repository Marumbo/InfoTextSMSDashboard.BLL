using InfoTextSMSDashboard.BLL.Models;
using InfoTextSMSDashboard.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoTextSMSDashboard.BLL.Interfaces
{
   public interface IGroupContactService
    {
       
        Task<OutputResponse> GetContactsInGroup(int id);



        Task<OutputResponse> AddContactToGroup(GroupContactDTO groupContact);

        Task<OutputResponse> RemoveContactInGroup(GroupContactDTO groupContact);
    }
}
