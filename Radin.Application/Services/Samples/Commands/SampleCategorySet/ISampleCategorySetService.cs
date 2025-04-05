//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Samples.Commands.SampleCategorySet;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Samples;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Samples.Commands.SampleCategorySet
//{
//    public interface ISampleCategorySetService
//    {
//        ResultDto<ResultSampleCategorySetDto> AdminSet(RequestSampleCategorySetDto request);

//    }

//    public class SampleCategorySetService : ISampleCategorySetService
//    {
//        private readonly IDataBaseContext _context;

//        public SampleCategorySetService(IDataBaseContext context)
//        {
//            _context = context;
//        }
//        public ResultDto<ResultSampleCategorySetDto> AdminSet(RequestSampleCategorySetDto request)
//        {

//            var Errors = new List<IdLabelDto>();
//            try
//            {
//                int id = 0;
//                var TitleDup = _context.SampleCategories.FirstOrDefault(c => c.SampleCategoryTitle == request.SampleCategoryTitle);
//                var NameDup = _context.SampleCategories.FirstOrDefault(c => c.SampleCategoryUniqeName == request.SampleCategoryUniqeName);

//                if (string.IsNullOrWhiteSpace(request.SampleCategoryTitle))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عنوان دسته را وارد نمایید"
//                    });
//                }
//                if (TitleDup != null)
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!این عنوان دسته قبلا ثبت شده است"
//                    });
//                }

//                if (string.IsNullOrWhiteSpace(request.SampleCategoryUniqeName))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!نام یکتا را وارد نمایید"
//                    });
//                }
//                if (NameDup != null)
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!این نام یکتای دسته قبلا ثبت شده است"
//                    });
//                }
//                if (request.SampleCategoryUniqeName.Length > 75 || request.SampleCategoryUniqeName.Length < 3)
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!طول متن نام یکتا محتوی باید بین 3 الی 75 کاراکتر باشد"
//                    });
//                }

//                if (request.SampleCategorySorting == null)
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عدد مرتب سازی را وارد نمایید"
//                    });
//                }

//                if (request.SampleCategorySorting.GetType() != typeof(int))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عدد مرتب سازی باید مقدار صحیح باشد"
//                    });
//                }
//                if (request.SampleCategorySorting is int & request.SampleCategorySorting < 1)
//                {

//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عدد مرتب سازی باید بزرگتر از صفر باشد"
//                    });
//                }

//                if (Errors.Count() < 1)
//                {
//                    SampleCategory sampleCategory = new SampleCategory()
//                    {
//                        SampleCategoryTitle = request.SampleCategoryTitle,
//                        SampleCategoryUniqeName = request.SampleCategoryUniqeName,
//                        SampleCategorySorting = request.SampleCategorySorting,
//                        //SampleCategoryStyle = request.SampleCategoryStyle,
//                        //SampleCategoryIsShowMain = request.SampleCategoryIsShowMain,
//                        SampleCategoryIsShowMenu = request.SampleCategoryIsShowMenu,
//                        SampleCategoryDescription = request.SampleCategoryDescription,
//                    };
//                    sampleCategory.UpdateTime = sampleCategory.InsertTime;
//                    _context.SampleCategories.Add(sampleCategory);

//                    _context.SaveChanges();

//                    return new ResultDto<ResultSampleCategorySetDto>()
//                    {
//                        Data = new ResultSampleCategorySetDto()
//                        {
//                            SampleCategoryId = sampleCategory.Id,
//                            Errors = Errors,
//                        },
//                        IsSuccess = true,
//                        Message = "دسته بندی محتوی با موفقیت درج شد",
//                    };
//                }
//                else
//                {
//                    return new ResultDto<ResultSampleCategorySetDto>()
//                    {
//                        Data = new ResultSampleCategorySetDto()
//                        {
//                            SampleCategoryId = 0,
//                            Errors = Errors,
//                        },
//                        IsSuccess = false,
//                        Message = "دسته بندی جدید درج نشد !"
//                    };

//                }
//            }
//            catch (Exception)
//            {
//                return new ResultDto<ResultSampleCategorySetDto>()
//                {
//                    Data = new ResultSampleCategorySetDto()
//                    {
//                        SampleCategoryId = 0,
//                        Errors = Errors,
//                    },
//                    IsSuccess = false,
//                    Message = "دسته بندی جدید درج نشد !"
//                };


//            }

//        }

//    }
//    public class RequestSampleCategorySetDto
//    {
//        public string SampleCategoryTitle { get; set; }
//        public string SampleCategoryUniqeName { get; set; }
//        public int SampleCategorySorting { get; set; } = 1;
//        //public string? SampleCategoryStyle { get; set; }

//        //public bool SampleCategoryIsShowMain { get; set; } = true;
//        public bool SampleCategoryIsShowMenu { get; set; } = true;
//        public string? SampleCategoryDescription { get; set; }
//    }

//    public class ResultSampleCategorySetDto
//    {
//        public long SampleCategoryId { get; set; }
//        public List<IdLabelDto> Errors { get; set; }
//    }
//}
