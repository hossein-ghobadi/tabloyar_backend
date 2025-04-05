//using Microsoft.AspNetCore.Identity;
//using Radin.Application.Services.Email.Commands;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Users;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Mail;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using Newtonsoft.Json;

//namespace Radin.Application.Services.SMS.Commands
//{
//    public interface ISMSSendService
//    {
//        ResultDto<bool> Send(RequestSMSSentDto RequestSMSSentDto);
//    }


//    public class SMSSendService : ISMSSendService
//    {

//        public ResultDto<bool> Send(RequestSMSSentDto RequestSMSSentDto)
//        {
//            var Result = false;
//            try
//            {

//                var client = new HttpClient();
//                var data = new
//                {
//                    Mobile = RequestSMSSentDto.PhoneNumber,
//                    Footer = "",
//                };

//                var url = "https://api.limosms.com/api/sendcode";
//                client.DefaultRequestHeaders.Add("ApiKey", "573a1201-c534-4c55-948f-f40c2912b574");
//                var objectStr = JsonConvert.SerializeObject(data);
//                var content = new StringContent(objectStr, Encoding.UTF8, "application/json");
//                var response = client.PostAsync(url, content).Result;

//                string resultContent = response.Content.ReadAsStringAsync().Result;
//                Response response2 = JsonConvert.DeserializeObject<Response>(resultContent);
//                Result = response2.Success;
//                Console.WriteLine(resultContent);
//                Console.WriteLine(Result);
//                string message = "";
//                if (Result)
//                {
//                    message = response2.Message;

//                }
//                else
//                {
//                    message = response2.Message;

//                }

//                return new ResultDto<bool>()
//                {

//                    Data = Result,
//                    IsSuccess = true,
//                    Message = message,
//                };

//            }
//            catch (Exception)
//            {
//                return new ResultDto<bool>()
//                {
//                    Data = Result,
//                    IsSuccess = false,
//                    Message = "خطا سرویس",
//                };

//            }
//        }
//    }


//    public class RequestSMSSentDto
//    {
//        public string PhoneNumber { get; set; }
//    }










//}
