using AfricasTalkingCS;
using AutoMapper;
using InfoTextSMSDashboard.BLL.Mapping_profiles;
using InfoTextSMSDashboard.BLL.Models;
using InfoTextSMSDashboard.DAL.Models;
using InfoTextSMSDashboard.DataModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace InfoTextSMSDashboard.BLL.Services
{
    public class SmsService : ISmsService
    {

        readonly string username = "sandbox";
        readonly string apiKey = "e1f52557ba4b192f302d9ea15e3786333fffe5cbfa73c3d82ad6a2db60ce43a0";


        private readonly sms_dashboardContext _context;
        private readonly IMapper _mapper; 

        public SmsService(sms_dashboardContext context)
        {
            _context = context;

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<SMSProfile>();

            });

            _mapper = mapperConfig.CreateMapper();
        }


        public async Task<OutputResponse> SendSMS(SMS smsObject)
        {
            var gateway = new AfricasTalkingGateway(username, apiKey);
            var recepients = smsObject.Recipients;
            var msg = smsObject.Message;
            var from = smsObject.From;
            
           

            foreach (var phonerNumber in recepients)
            {


                try
                {

                    var sms = gateway.SendMessage(to: phonerNumber,message: msg,from: from);

                    foreach (var res in sms["SMSMessageData"]["Recipients"])
                    {
                        Console.WriteLine((string)res["number"] + ": ");
                        Console.WriteLine((string)res["status"] + ": ");
                        Console.WriteLine((string)res["messageId"] + ": ");
                        Console.WriteLine((string)res["cost"] + ": ");

                        var outgoingMessageDTO = new OutgoingSMSDTO
                        {
                            Message = msg,
                            SenderUsername = "test",
                            RecipientNumber = res["number"],
                            RecipientStatus = res["status"],
                            AtMessageid = res["messageId"],
                            MessageCost = res["cost"]

                        };

                        var outgoingMessage = _mapper.Map<OutgoingSMSDTO, OutgoingSmsList>(outgoingMessageDTO);

                        try
                        {

                            await _context.AddAsync(outgoingMessage);

                            await _context.SaveChangesAsync();


                        }

                        catch (Exception exception)
                        {
                            return new OutputResponse
                            {
                                IsSuccess = false,
                                Message = $"unable to send sms error:{exception}"
                            };

                        }


                    }

                }

                catch (AfricasTalkingGatewayException exception)
                {
                    Console.WriteLine(exception);

                    return new OutputResponse
                    {
                        IsSuccess = false,
                        Message = $"unable to send sms error:{exception}"
                    };
                }
            }

            return new OutputResponse
            {
                IsSuccess = true,
                Message = "Messages sent and recorded with no errors,"
            };


        }

        public async Task<OutputResponse> GetMessageById(int Id)
        {
            var message = await _context.OutgoingSmsLists.SingleOrDefaultAsync(m => m.SmsId == Id);

            if (message == null)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = "No messages in database"
                };

            }

            return new OutputResponse
            {
                IsSuccess = true,
                Message = $"Message for id:{Id}",
                SuccessResult = message
            };
        }



        public async Task<OutputResponse> GetMessages()
        {
            var messages = await _context.OutgoingSmsLists.ToListAsync();

            if (messages == null)
            {
                return new OutputResponse
                {
                    IsSuccess = false,
                    Message = "No messages in database"
                };

            }

            return new OutputResponse
            {
                IsSuccess = true,
                Message = "List of messages",
                SuccessResult = messages
            };
        }
    }
}
