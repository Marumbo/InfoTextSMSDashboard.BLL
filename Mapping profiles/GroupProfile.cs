using AutoMapper;
using InfoTextSMSDashboard.DAL.Models;
using InfoTextSMSDashboard.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfoTextSMSDashboard.BLL.Mapping_profiles
{
    class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<GroupsDTO, Group>();
            CreateMap<Group, GroupsDTO>();

        }
    }
}
