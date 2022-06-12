using InfoTextSMSDashboard.DAL.Models;
using InfoTextSMSDashboard.DataModels.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfoTextSMSDashboard.BLL.Mapping_profiles
{
    public class GroupContactProfile : Profile
    {
        public GroupContactProfile()
        {
            CreateMap<GroupContactDTO, GroupContact>();
            CreateMap<GroupContact, GroupContactDTO>();

        }

    }
}
