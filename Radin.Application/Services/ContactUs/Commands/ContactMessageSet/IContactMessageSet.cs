//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.HomePage.Commands.HomePageSliderRemove;
//using Radin.Application.Services.HomePage.Commands.HomePageSliderSet;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.ContactUs;
//using Radin.Domain.Entities.HomePage;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Radin.Application.Services.ContactUs.Commands.ContactMessageSet.ContactMessageSet;
//using static Radin.Application.Services.HomePage.Queries.HomePageSlider.HomePageSliderGet;

//namespace Radin.Application.Services.ContactUs.Commands.ContactMessageSet
//{
//    public interface IContactMessageSet
//    {
//        ResultDto<ResultContactMessageSetDto> Execute(MessageDto request);
//    }
//    public class ContactMessageSet : IContactMessageSet
//    {
//        private readonly IDataBaseContext _context;

//        public ContactMessageSet(IDataBaseContext context)
//        {
//            _context = context;
//        }


//        public ResultDto<ResultContactMessageSetDto> Execute(MessageDto request)
//        {
//            var Errors = new List<IdLabelDto>();

//            try
//            {
//                int id = 0;
                
//                if (string.IsNullOrWhiteSpace(request.Email))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!لطفا ایمیل خود را وارد نمایید"
//                    });
//                }
//                if (string.IsNullOrWhiteSpace(request.Name))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!لطفا نام خود را وارد نمایید"
//                    });
//                }

//                if (string.IsNullOrWhiteSpace(request.Description))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!لطفا توضیحات خود را وارد نمایید"
//                    });
//                }
//                if (string.IsNullOrWhiteSpace(request.Department))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!لطفا واحد مربوطه را انتخاب نمایید"
//                    });
//                }
//                if (Errors.Count() < 1)
//                {

//                    ContactMessage Message = new ContactMessage()
//                    {
//                        ReleventUnit=request.Department,
//                        Title = request.Subject,
//                        Name = request.Name,
//                        Description = request.Description,
//                        Phone = request.Phone,
//                        Email = request.Email,
//                    };

//                    _context.ContactMessages.Add(Message);
//                    _context.SaveChanges();

//                    return new ResultDto<ResultContactMessageSetDto>()
//                    {
//                        Data = new ResultContactMessageSetDto()
//                        {
//                            MessageId = Message.Id,
//                            Errors = Errors,
//                        },
//                        IsSuccess = true,
//                        Message = " پیام جدید با موفقیت درج شد",
//                    };
//                }
//                else
//                {
//                    return new ResultDto<ResultContactMessageSetDto>()
//                    {
//                        Data = new ResultContactMessageSetDto()
//                        {
//                            MessageId = 0,
//                            Errors = Errors,
//                        },
//                        IsSuccess = false,
//                        Message = "!پیام درج نشد"
//                    };

//                }
//            }
//            catch (Exception ex) {
//                return new ResultDto<ResultContactMessageSetDto>()
//                {
//                    Data = new ResultContactMessageSetDto()
//                    {
//                        MessageId = 0,
//                        Errors = Errors,
//                    },
//                    IsSuccess = false,
//                    Message = "!پیام درج نشد"
//                };
//            }
//        }

//        public class MessageDto
//        {
//            public string Department { get; set; }
//            public string? Subject { get; set; }
//            public string Email { get; set; }
//            public string Name { get; set; }
//            public string? Phone { get; set; }
//            public string Description { get; set; }
//        }
//        public class ResultContactMessageSetDto
//        {
//            public long MessageId { get; set; }
//            public List<IdLabelDto> Errors { get; set; }
//        }
//    }
//}
