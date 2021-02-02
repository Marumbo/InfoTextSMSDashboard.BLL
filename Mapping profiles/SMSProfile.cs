using AutoMapper;
using InfoTextSMSDashboard.DAL.Models;
using InfoTextSMSDashboard.DataModels.Models;

namespace InfoTextSMSDashboard.BLL.Mapping_profiles
{
    public class SMSProfile : Profile
    {
        public SMSProfile()
        {
            CreateMap<OutgoingSMSDTO, OutgoingSmsList>();
            CreateMap<OutgoingSmsList, OutgoingSMSDTO>();
            

        }
    }
}
