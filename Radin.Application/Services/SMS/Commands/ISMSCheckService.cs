using Newtonsoft.Json;
using Radin.Application.Services.Claims.Queries;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace Radin.Application.Services.SMS.Commands
{
    public interface ISMSCheckService
    {
        ResultDto<bool> Check(RequestSMSCheckDto RequestSMSCheckDto);
    }

    public class SMSCheckService : ISMSCheckService
    {

        public ResultDto<bool> Check(RequestSMSCheckDto RequestSMSCheckDto)
        {
            var Result = false;
            try
            {

                var client = new HttpClient();
                var data = new
                {
                    Mobile = RequestSMSCheckDto.PhoneNumber,
                    Code = RequestSMSCheckDto.Code,
                };

                var url = "https://api.limosms.com/api/checkcode";
                client.DefaultRequestHeaders.Add("ApiKey", "573a1201-c534-4c55-948f-f40c2912b574");
                var objectStr = JsonConvert.SerializeObject(data);
                var content = new StringContent(objectStr, Encoding.UTF8, "application/json");
                var response = client.PostAsync(url, content).Result;
                string resultContent = response.Content.ReadAsStringAsync().Result;
                Response response2 = JsonConvert.DeserializeObject<Response>(resultContent);
                Result = response2.Success;
                string message = "";
                if (Result)
                {
                    message = response2.Message;

                }
                else
                {
                    message = response2.Message;

                }

                return new ResultDto<bool>()
                {
                    
                    Data= Result,
                    IsSuccess = true,
                    Message = message,
                };

            }
            catch (Exception)
            {
                return new ResultDto<bool>()
                {
                    Data = Result,
                    IsSuccess = false,
                    Message = "خطا سرویس",
                };

            }
        }
    }



    public class RequestSMSCheckDto
    {
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
    }

    public class ResultSMSCheckDto
    {
        public List<IdLabelDto> Results { get; set; }
    }

    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

}
