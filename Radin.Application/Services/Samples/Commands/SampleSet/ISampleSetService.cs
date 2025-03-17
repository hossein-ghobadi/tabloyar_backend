using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Samples.Commands.SampleSet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Samples.Commands.SampleSet
{
    public interface ISampleSetService
    {
        ResultDto<ResultSampleSetDto> AdminSet(RequestSampleSetDto request);

    }

    public class SampleSetService : ISampleSetService
    {

        private readonly IDataBaseContext _context;

        public SampleSetService(IDataBaseContext context)
        {
            _context = context;


        }
        public ResultDto<ResultSampleSetDto> AdminSet(RequestSampleSetDto request)
        {

            var Errors = new List<IdLabelDto>();
            try
            {

                int id = 0;
                var TitleDup = _context.Samples.FirstOrDefault(c => c.SampleTitle == request.SampleTitle);
                var NameDup = _context.Samples.FirstOrDefault(c => c.SampleUniqeName == request.SampleUniqeName);

                if (string.IsNullOrWhiteSpace(request.SampleTitle))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عنوان ایده را وارد نمایید"
                    });
                }
                if (TitleDup != null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!این عنوان ایده قبلا ثبت شده است"
                    });
                }

                //if (request.SampleTitle.Length > 60 || request.SampleTitle.Length < 35)
                //{
                //    id = id + 1;
                //    Errors.Add(new IdLabelDto
                //    {
                //        id = id,
                //        label = "!طول متن عنوان ایده باید بین 35 الی 60 کاراکتر باشد"
                //    });
                //}

                if (string.IsNullOrWhiteSpace(request.SampleUniqeName))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!نام یکتا را وارد نمایید"
                    });
                }
                if (NameDup != null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!این نام یکتا قبلا ثبت شده است"
                    });
                }
                if (request.SampleUniqeName.Length > 75 || request.SampleUniqeName.Length < 3)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!طول متن نام یکتا  باید بین 3 الی 75 کاراکتر باشد"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.SampleSorting.ToString()))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی را وارد نمایید"
                    });
                }

                if (request.SampleSorting.GetType() != typeof(int))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی باید مقدار صحیح باشد"
                    });
                }

                if (request.SampleSorting is int & request.SampleSorting < 1)
                {

                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی باید بزرگتر از صفر باشد"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.SampleLongDescription))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!متن ایده را وارد نمایید"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.SampleMetaDesc))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!متن توضیحات متا را وارد نمایید"
                    });
                }

                //if (request.SampleMetaDesc.Length > 170 || request.SampleMetaDesc.Length < 140)
                //{
                //    id = id + 1;
                //    Errors.Add(new IdLabelDto
                //    {
                //        id = id,
                //        label = "!طول متن توضیحات متا محتوی باید بین 140 الی 170 کاراکتر باشد"
                //    });
                //}
                if (string.IsNullOrWhiteSpace(request.SampleUniqeName))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!دسته بندی را انتخاب نمایید"
                    });
                }

                var categoryinfo = _context.SampleCategories.FirstOrDefault(c => c.SampleCategoryUniqeName == request.SampleCategoryUniqeName);

                if (categoryinfo == null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!دسته بندی با این نام وجود ندارد"
                    });
                }

                if (Errors.Count() < 1)
                {
                    string imageUrlsString = string.Join(",", request.SampleImages.Where(img => !string.IsNullOrWhiteSpace(img)));

                    Sample sample = new Sample()
                    {

                        SampleTitle = request.SampleTitle,
                        OnlinePrice = false,
                        SampleOwnerName = request.SampleOwnerName,
                        SampleOwnerId = request.SampleOwnerId,
                        AverageStar = 0,
                        SumStar = 0,
                        CountStar = 0,
                        SampleUniqeName = request.SampleUniqeName,
                        CommentSituation = request.CommentSituation,
                        CommentShow = request.CommentShow,
                        SampleSorting = request.SampleSorting,
                        SampleLongDescription = request.SampleLongDescription,
                        SampleMetaDesc = request.SampleMetaDesc,
                        SampleImageAlt = request.SampleImageAlt,
                        SamplePublish = request.SamplePublish,
                        MainImage = request.MainImage,
                        SampleImage = imageUrlsString,
                        SampleCategoryUniqeName = categoryinfo.SampleCategoryUniqeName,
                        SampleCategoryTitle = categoryinfo.SampleCategoryTitle,
                        IsIndex=request.IsIndex,
                        IsRemoved=true
                    };
                    sample.UpdateTime = sample.InsertTime;
                    _context.Samples.Add(sample);
                    _context.SaveChanges();



                    return new ResultDto<ResultSampleSetDto>()
                    {
                        Data = new ResultSampleSetDto()
                        {
                            SampleId = sample.Id,
                            Errors = Errors,
                        },
                        IsSuccess = true,
                        Message = " محتوی جدید با موفقیت درج شد",
                    };
                }

                else
                {
                    return new ResultDto<ResultSampleSetDto>()
                    {
                        Data = new ResultSampleSetDto()
                        {
                            SampleId = 0,
                            Errors = Errors,
                        },
                        IsSuccess = false,
                        Message = "!محتوی جدید درج نشد"
                    };

                }

            }

            catch (Exception)
            {
                return new ResultDto<ResultSampleSetDto>()
                {
                    Data = new ResultSampleSetDto()
                    {
                        SampleId = 0,
                        Errors = Errors,
                    },
                    IsSuccess = false,
                    Message = "!محتوی جدید درج نشد"
                };

            }
        }
    }


    public class RequestSampleSetDto
    {
        public string SampleTitle { get; set; }
        public string SampleOwnerName { get; set; }
        public string SampleOwnerId { get; set; }

        public string SampleUniqeName { get; set; }
        public bool CommentSituation { get; set; }
        public bool CommentShow { get; set; }
        public int SampleSorting { get; set; }
        public string SampleLongDescription { get; set; }
        public string SampleMetaDesc { get; set; }
        public string SampleImageAlt { get; set; }
        public bool SamplePublish { get; set; }
        public string MainImage { get; set; }
        public List<string> SampleImages { get; set; }
        public string SampleCategoryUniqeName { get; set; }
        public string SampleCategoryTitle { get; set; }
        public bool IsIndex { get; set; }   
    }
    public class ResultSampleSetDto
    {
        public long SampleId { get; set; }
        public List<IdLabelDto> Errors { get; set; }
    }
}
