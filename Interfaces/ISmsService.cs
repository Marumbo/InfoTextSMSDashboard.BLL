using InfoTextSMSDashboard.BLL.Models;
using InfoTextSMSDashboard.DataModels.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace InfoTextSMSDashboard.BLL.Services
{
    public interface ISmsService
    {
        Task<OutputResponse> SendSMS(SMS sms);
        Task<OutputResponse> GetMessages();

        Task<OutputResponse> GetMessageById(int Id);

        Task<OutputResponse> SendMessageTest();
    }
}
