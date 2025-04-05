//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Samples.Commands.SampleEdit;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Samples.Commands.SampleEdit
//{
//    public interface ISampleEditService
//    {
//        ResultDto<ResultEditSampleDto> Execute(UpdateSampleDto request);

//    }


//    public class SampleEditService : ISampleEditService
//    {
//        private readonly IDataBaseContext _context;

//        public SampleEditService(IDataBaseContext context)
//        {
//            _context = context;
//        }
//        public ResultDto<ResultEditSampleDto> Execute(UpdateSampleDto updateDto)
//        {
//            var Errors = new List<IdLabelDto>();
//            int id = 0;
//            try
//            {
//                var sample = _context.Samples.FirstOrDefault(c => c.Id == updateDto.Id);
//                if (sample == null)
//                {
//                    return new ResultDto<ResultEditSampleDto>()
//                    {
//                        Data = new ResultEditSampleDto()
//                        {
//                            Errors = Errors,
//                        },
//                        IsSuccess = false,
//                        Message = "!محتوی یافت نشد"
//                    };
//                }

//                if (string.IsNullOrWhiteSpace(updateDto.SampleTitle))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عنوان ایده را وارد نمایید"
//                    });
//                }
//                if (updateDto.SampleTitle.Length > 60 || updateDto.SampleTitle.Length < 35)
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!طول متن عنوان ایده باید بین 35 الی 60 کاراکتر باشد"
//                    });
//                }
//                if (string.IsNullOrWhiteSpace(updateDto.SampleUniqeName))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!نام یکتا را وارد نمایید"
//                    });
//                }

//                if (updateDto.SampleUniqeName.Length > 75 || updateDto.SampleUniqeName.Length < 3)
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!طول متن نام یکتای ایده باید بین 3 الی 75 کاراکتر باشد"
//                    });
//                }

//                if (string.IsNullOrWhiteSpace(updateDto.SampleSorting.ToString()))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عدد مرتب سازی را وارد نمایید"
//                    });
//                }

//                if (updateDto.SampleSorting.GetType() != typeof(int))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عدد مرتب سازی باید مقدار صحیح باشد"
//                    });
//                }
//                if (updateDto.SampleSorting is int & updateDto.SampleSorting < 1)
//                {

//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عدد مرتب سازی باید بزرگتر از صفر باشد"
//                    });
//                }
//                if (string.IsNullOrWhiteSpace(updateDto.SampleLongDescription))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!متن محتوی را وارد نمایید"
//                    });
//                }
//                if (string.IsNullOrWhiteSpace(updateDto.SampleMetaDesc))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!متن توضیحات متا را وارد نمایید"
//                    });
//                }

//                if (updateDto.SampleMetaDesc.Length > 170 || updateDto.SampleMetaDesc.Length < 140)
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!طول متن توضیحات متا  باید بین 140 الی 170 کاراکتر باشد"
//                    });
//                }
//                if (string.IsNullOrWhiteSpace(updateDto.SampleCategoryUniqeName))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!دسته بندی را انتخاب نمایید"
//                    });
//                }
//                var categoryinfo = _context.SampleCategories.FirstOrDefault(c => c.SampleCategoryUniqeName == updateDto.SampleCategoryUniqeName);
//                if (categoryinfo == null)
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!دسته بندی با این نام وجود ندارد"
//                    });
//                }
//                //UpdateSampleDto
//                var samplesTab = _context.Samples.ToList();
//                var OtherSample = samplesTab.Remove(sample);
//                if (OtherSample)
//                {
//                    var NameDup = samplesTab.FirstOrDefault(p => p.SampleUniqeName == updateDto.SampleUniqeName);
//                    var TitleDup = samplesTab.FirstOrDefault(c => c.SampleTitle == updateDto.SampleTitle);
//                    if (TitleDup != null)
//                    {
//                        id = id + 1;
//                        Errors.Add(new IdLabelDto
//                        {
//                            id = id,
//                            label = "!این عنوان ایده قبلا ثبت شده است"
//                        });
//                    }
//                    if (NameDup != null)
//                    {
//                        id = id + 1;
//                        Errors.Add(new IdLabelDto
//                        {
//                            id = id,
//                            label = "!این نام یکتای ایده قبلا ثبت شده است"
//                        });
//                    }
//                }

//                string imageUrlsString = string.Join(",", updateDto.SampleImages.Where(img => !string.IsNullOrWhiteSpace(img)));

//                if (Errors.Count() < 1)
//                {

//                    sample.SampleTitle = updateDto.SampleTitle;
//                    sample.SampleUniqeName = updateDto.SampleUniqeName;
//                    sample.CommentSituation = updateDto.CommentSituation;
//                    sample.CommentShow = updateDto.CommentShow;
//                    sample.SampleSorting = updateDto.SampleSorting;
//                    sample.SampleLongDescription = updateDto.SampleLongDescription;
//                    sample.SampleMetaDesc = updateDto.SampleMetaDesc;
//                    sample.SampleImageAlt = updateDto.SampleImageAlt;
//                    sample.SamplePublish = updateDto.SamplePublish;
//                    sample.MainImage = updateDto.MainImage;
//                    sample.SampleImage = imageUrlsString;
//                    sample.SampleUniqeName = updateDto.SampleUniqeName;
//                    sample.SampleCategoryTitle = categoryinfo.SampleCategoryTitle;
//                    sample.IsIndex=updateDto.IsIndex;
//                    _context.SaveChanges();



//                    return new ResultDto<ResultEditSampleDto>()
//                    {
//                        Data = new ResultEditSampleDto()
//                        {
//                            Errors = Errors,
//                        },
//                        IsSuccess = true,
//                        Message = "!ویرایش محتوی با موفقیت انجام شد"
//                    };
//                }
//                else
//                {
//                    return new ResultDto<ResultEditSampleDto>()
//                    {
//                        Data = new ResultEditSampleDto()
//                        {
//                            Errors = Errors,
//                        },
//                        IsSuccess = false,
//                        Message = "!ویرایش دسته بندی محتوی انجام نشد"
//                    };
//                }

//            }
//            catch
//            {
//                return new ResultDto<ResultEditSampleDto>()
//                {
//                    Data = new ResultEditSampleDto()
//                    {
//                        Errors = Errors,
//                    },
//                    IsSuccess = false,
//                    Message = "!ویرایش دسته بندی محتوی انجام نشد"
//                };
//            }
//        }
//    }


//    public class UpdateSampleDto
//    {
//        public long Id { get; set; }


//        public string SampleTitle { get; set; }
//        public string SampleUniqeName { get; set; }
//        public bool CommentSituation { get; set; }
//        public bool CommentShow { get; set; }
//        public int SampleSorting { get; set; }
//        public string SampleLongDescription { get; set; }
//        public string SampleMetaDesc { get; set; }
//        public string SampleImageAlt { get; set; }
//        public bool SamplePublish { get; set; }
//        public string SampleCategoryUniqeName { get; set; }
//        public string SampleCategoryTitle { get; set; }
//        public bool IsIndex { get; set; }
//        public string MainImage { get; set; }
//        public List<string> SampleImages { get; set; }


//    }

//    public class ResultEditSampleDto
//    {
//        public List<IdLabelDto> Errors { get; set; }
//    }
//}
