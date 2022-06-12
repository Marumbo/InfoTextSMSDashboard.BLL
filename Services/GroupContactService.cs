using AutoMapper;
using InfoTextSMSDashboard.BLL.Interfaces;
using InfoTextSMSDashboard.BLL.Mapping_profiles;
using InfoTextSMSDashboard.BLL.Models;
using InfoTextSMSDashboard.DAL.Models;
using InfoTextSMSDashboard.DataModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoTextSMSDashboard.BLL.Services
{
    public class GroupContactService : IGroupContactService
    {
        private readonly sms_dashboardContext _context;
        private readonly IContactsService _contactService;
        private readonly IMapper _mapper;
        public GroupContactService(sms_dashboardContext context, IContactsService contactsService)
        {
            _context = context;
            _contactService = contactsService;

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<GroupContactProfile>();
                config.AddProfile<ContactProfile>();

            });

            _mapper = mapperConfig.CreateMapper();
        }
        public async Task<OutputResponse> AddContactToGroup(GroupContactDTO groupContact)
        {
            //find group with id, then take contact id and add to group contact,
            var groupCheck = await _context.Groups.AnyAsync(g => g.GroupId.ToString().ToLower() == groupContact.GroupId.ToString().ToLower());

            if (!groupCheck)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = "group does not exist"
                    
                };

            }

            var contactCheck = await _contactService.GetContactById((int)groupContact.ContactId);

            if (contactCheck.IsSuccess == false)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = "Contact does not exist"

                };

            }

            var groupContactCheck = await _context.GroupContacts.AnyAsync(groupContactDB => groupContactDB.GroupId == groupContact.GroupId
                                                                            && groupContactDB.ContactId == groupContact.ContactId);
            if (groupContactCheck)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = "contact in group aleady exists."

                };

            }


            var addGroupContact = _mapper.Map<GroupContactDTO, GroupContact>(groupContact);

           
            try
            {

                await _context.GroupContacts.AddAsync(addGroupContact);
                await _context.SaveChangesAsync();

                return new OutputResponse
                {
                    IsSuccess = true,
                    Message = $"Contact with id: {groupContact.ContactId} added to group with id: {groupContact.GroupId}",
                    SuccessResult = addGroupContact
                };
            }

            catch (Exception e)
            {

                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = $"Contact with id: {groupContact.ContactId} unable to be added to group with id: {groupContact.GroupId}, Error: {e}",
                };
            }




        }

        public async Task<OutputResponse> GetContactsInGroup(int id)
        {


            var groupContactList = await _context.GroupContacts.Where(groupContact => groupContact.GroupId
                                                                                      == id).ToListAsync();
            var contactIdList = new List<int>();

            var contactList = new List<ContactsDTO>();


            foreach (var contact in groupContactList)
            {
                contactIdList.Add((int)contact.ContactId);
            }

            foreach (var contactId in contactIdList)
            {
                var contact = await _contactService.GetContactById(contactId);

                if (contact.IsSuccess)
                {
                    var contactDTO = _mapper.Map<Contact, ContactsDTO>((Contact)contact.SuccessResult);

                    contactList.Add(contactDTO);
                }

            }
            if (contactList.Count > 0)
            {
                return new OutputResponse
                {
                    IsSuccess = true,
                    Message = $"List of Contacts in group with id {id}",
                    SuccessResult = contactList
                };
            }
            else return new OutputResponse
            {
                IsSuccess = false,
                Message = $"No Contacts in group with id {id}",
                SuccessResult = contactList
            };

        }

        public async Task<OutputResponse> RemoveContactInGroup(GroupContactDTO groupContact)
        {
            if (groupContact.GroupContactId == null)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = "GroupContactId is required to perform removal of contact from group"
                };
            }

            var groupContactIdCheck = await _context.GroupContacts.AnyAsync(gC => gC.GroupContactId == groupContact.GroupContactId);
            if (!groupContactIdCheck)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = $"GroupContactId: {groupContact.GroupContactId} does not exist"
                };
            }

            var groupContactCheck = await _context.GroupContacts.AnyAsync(groupContactDB => groupContactDB.GroupId == groupContact.GroupId
                                                                       && groupContactDB.ContactId == groupContact.ContactId);
            if (!groupContactCheck)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = $"group id: {groupContact.GroupId} and contact id: {groupContact.ContactId} combination does not exist"

                };

            }

            var groupContactRemove = _mapper.Map<GroupContactDTO, GroupContact>(groupContact);

           
                try
                {

                    _context.GroupContacts.Remove(groupContactRemove);
                    _context.SaveChanges();

                    return new OutputResponse
                    {
                        IsSuccess = true,
                        Message = $"Contact with id: {groupContact.ContactId} removed from group with id: {groupContact.GroupId}",

                    };
                }

                catch (Exception e)
                {

                    return new OutputResponse
                    {
                        IsSuccess = false,
                        Message = $"Contact with id: {groupContact.ContactId} unable to be removedto group with id: {groupContact.GroupId}, Error: {e}",
                    };
                }
            }
          
        


    }
}
