using InfoTextSMSDashboard.BLL.Models;
using InfoTextSMSDashboard.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoTextSMSDashboard.BLL.Interfaces
{
   public interface IContactsService
    {
        Task<OutputResponse> AddContact(ContactsDTO contact);
        Task<OutputResponse> GetContacts();
        Task<OutputResponse> GetContactById(int id);
        Task<OutputResponse> UpdateContact(ContactsDTO contact);
        Task<OutputResponse> DeleteContact(int id);

    }
}
