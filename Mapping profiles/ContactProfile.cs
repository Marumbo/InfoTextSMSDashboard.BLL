using AutoMapper;
using InfoTextSMSDashboard.DAL.Models;
using InfoTextSMSDashboard.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfoTextSMSDashboard.BLL.Mapping_profiles
{
    class ContactProfile : Profile
    {
        public ContactProfile()
        {
             CreateMap<ContactsDTO, Contact>();

             CreateMap<Contact, ContactsDTO>();
                
        }
    }
}
