using Microsoft.AspNetCore.Identity;
using Radin.Common.Dto;
using Radin.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Email.Commands
{
    public interface IEmailSendService
    { 
        ResultDto<ResultEmailSentDto> Execute(RequestEmailSentDto RequestEmailSentDto);
    
    }

    public class EmailSendService: IEmailSendService
    {
        private readonly UserManager<User> _userManager;

        public EmailSendService (UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public ResultDto<ResultEmailSentDto> Execute(RequestEmailSentDto RequestEmailSentDto)
        {
            var Errors = new List<IdLabelDto>();
            try
            {
                int id = 0;

                if (string.IsNullOrWhiteSpace(RequestEmailSentDto.UserEmail))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "ایمیل خود را وارد نمایید"
                    });
                }

                var user = _userManager.FindByEmailAsync(RequestEmailSentDto.UserEmail).Result;
                if (user == null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "کاربری با این ایمیل آدرس یافت نشد!"
                    });
                }
                Console.WriteLine("1");
                if (Errors.Count() < 1)
                {
                    Console.WriteLine("2");

                    SmtpClient client = new SmtpClient();
                    client.Port = 587;
                    client.Host = "smtp.c1.liara.email";
                    client.EnableSsl = true;
                    client.Timeout = 1000000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("friendly_dhawan_k7f30e", "a33f00a5-57f2-4e7d-b064-a66d612cfb2d");
                    MailMessage mailMessage = new MailMessage("mail@test.tabloradin.com", RequestEmailSentDto.UserEmail, RequestEmailSentDto.Subject, RequestEmailSentDto.MsgBody);
                    Console.WriteLine("3");
                    mailMessage.IsBodyHtml = true;
                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                    client.Send(mailMessage);
                    

                    return new ResultDto<ResultEmailSentDto>()
                    {
                        Data = new ResultEmailSentDto()
                        {
                            Errors = Errors
                        },
                        IsSuccess = true,
                        Message = "ایمیل ارسال شد",
                    };
                }
                else
                {
                    return new ResultDto<ResultEmailSentDto>()
                    {
                        Data = new ResultEmailSentDto()
                        {
                            Errors = Errors
                        },
                        IsSuccess = false,
                        Message = "ایمیل ارسال نشد",
                    };
                }

            }
            catch (Exception)
            {
                return new ResultDto<ResultEmailSentDto>()
                {
                    Data = new ResultEmailSentDto()
                    {
                        Errors = Errors
                    },
                    IsSuccess = false,
                    Message = "!یه جا مشکل داره",
                };

            }
        }
    }


    public class RequestEmailSentDto
    {
        public string UserEmail { get; set; }
        public string MsgBody { get; set; }
        public string Subject { get; set; }
    }

    public class ResultEmailSentDto
    {
        public List<IdLabelDto> Errors { get; set; }
    }



}
