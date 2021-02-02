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
    public class ContactsService : IContactsService
    {

        private readonly sms_dashboardContext _context;

        private readonly IMapper _mapper;
        public ContactsService(sms_dashboardContext context)
        {
            _context = context;

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<ContactProfile>();

            });

            _mapper = mapperConfig.CreateMapper();
        }

        public async Task<OutputResponse> AddContact(ContactsDTO contact)
        {
            var contactCheck = await _context.Contacts.AnyAsync(c => c.ContactId.ToString().ToLower() == contact.ContactId.ToString().ToLower());

            if (contactCheck)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = "Cannot add contact as contact already exists."

                };

            }

            var addContact = _mapper.Map<ContactsDTO, Contact>(contact);

            try
            {
               await  _context.AddAsync(addContact);
                await _context.SaveChangesAsync();

                return new OutputResponse
                {
                    IsSuccess = true,
                    Message = $"Contact {contact.FirstName} added to database."
                };
            }
            catch (Exception e)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = $"Error in adding contact. {e}"
                };
            }


        }

        public async Task<OutputResponse> GetContacts()
        {
            var contactList = await _context.Contacts.ToListAsync();

            var count = contactList.Count();


            if(contactList == null)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = $"No contacts in database"
                };

            }

            return new OutputResponse
            {
                IsSuccess = true,
                Message = "List of contacts",
                SuccessResult = contactList
            };
        }

     

        public async Task<OutputResponse> GetContactById(int id)
        {
            var contactExists = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactId == id);

            if(contactExists == null)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = $"No contact with given id: {id}"
                };

            }

            return new OutputResponse
            {
                IsSuccess = true,
                Message = $"Contact using {id} ",
                SuccessResult = contactExists
            };

        }

       

        public async Task<OutputResponse> UpdateContact(ContactsDTO contact)
        {
            var contactCheck = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactId == contact.ContactId);

            if (contactCheck == null)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = $"No contact with id:{contact.ContactId} in database"
                };
            }

            try
            {
                _mapper.Map<ContactsDTO, Contact>(contact, contactCheck);

                await _context.SaveChangesAsync();

                return new OutputResponse
                {
                    IsSuccess = true,
                    Message = $" updated contact {contact.ContactId}"

                };

            }
            catch (Exception error)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = $"unable to update contact id: {contact.ContactId}, error: {error}"
                };
            }
        }

        public async Task<OutputResponse> DeleteContact(int id)
        {
            var contactCheck = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactId == id);

            if(contactCheck == null)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = $"Unable to delete because no contact wwith id: {id}"
                };
            }

            try
            {
                _context.Contacts.Remove(contactCheck);

                _context.SaveChanges();

                return new OutputResponse
                {
                    IsSuccess = true,
                    Message = $"Contact removed. Id {id}",

                };

            }
            catch(Exception e)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = $"Failed to delete contact. id: {id}. Error: {e}"
                };
            }

        }
    }
}
