//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Samples.Commands.SampleCategoryEdit;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Samples.Commands.SampleCategoryEdit
//{
//    public interface ISampleCategoryEditService
//    {
//        ResultDto<ResultEditSampleCategoryDto> Execute(EditSampleCategoryDto request);

//    }
//    public class SampleCategoryEditService : ISampleCategoryEditService
//    {
//        private readonly IDataBaseContext _context;

//        public SampleCategoryEditService(IDataBaseContext context)
//        {
//            _context = context;
//        }
//        public ResultDto<ResultEditSampleCategoryDto> Execute(EditSampleCategoryDto updateDto)
//        {
//            var Errors = new List<IdLabelDto>();
//            int id = 0;
//            try
//            {
//                var sampleCategory = _context.SampleCategories.FirstOrDefault(c => c.Id == updateDto.Id);
//                if (sampleCategory == null)
//                {
//                    return new ResultDto<ResultEditSampleCategoryDto>()
//                    {
//                        Data = new ResultEditSampleCategoryDto
//                        {
//                            Errors = Errors
//                        },
//                        IsSuccess = false,
//                        Message = " !ویرایش دسته بندی نمونه کار انجام نشد"
//                    };
//                }
//                if (string.IsNullOrWhiteSpace(updateDto.CategoryTitle))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عنوان دسته را وارد نمایید"
//                    });
//                }
//                if (string.IsNullOrWhiteSpace(updateDto.CategoryUniqeName))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!نام یکتا را وارد نمایید"
//                    });
//                }

//                var sampleCategories = _context.SampleCategories.ToList();
//                var OtherCategory = sampleCategories.Remove(sampleCategory);
//                if (OtherCategory)
//                {
//                    var NameDup = sampleCategories.FirstOrDefault(p => p.SampleCategoryUniqeName == updateDto.CategoryUniqeName);
//                    var TitleDup = sampleCategories.FirstOrDefault(c => c.SampleCategoryTitle == updateDto.CategoryTitle);
//                    if (TitleDup != null)
//                    {
//                        id = id + 1;
//                        Errors.Add(new IdLabelDto
//                        {
//                            id = id,
//                            label = "!این عنوان دسته قبلا ثبت شده است"
//                        });
//                    }
//                    if (NameDup != null)
//                    {
//                        id = id + 1;
//                        Errors.Add(new IdLabelDto
//                        {
//                            id = id,
//                            label = "!این نام یکتای دسته قبلا ثبت شده است"
//                        });
//                    }
//                }
//                if (updateDto.CategorySorting == null)
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عدد مرتب سازی را وارد نمایید"
//                    });
//                }

//                if (updateDto.CategorySorting.GetType() != typeof(int))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عدد مرتب سازی باید مقدار صحیح باشد"
//                    });
//                }

//                if (updateDto.CategorySorting is int & updateDto.CategorySorting < 1)
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
//                    sampleCategory.SampleCategoryTitle = updateDto.CategoryTitle;
//                    sampleCategory.SampleCategoryDescription = updateDto.CategoryDescription;
//                    sampleCategory.SampleCategoryUniqeName = updateDto.CategoryUniqeName;
//                    sampleCategory.SampleCategoryIsShowMenu = updateDto.CategoryIsShowMenu;
//                    sampleCategory.SampleCategorySorting = updateDto.CategorySorting;
//                    sampleCategory.IsRemoved = updateDto.isRemoved;
//                    _context.SaveChanges();
//                    return new ResultDto<ResultEditSampleCategoryDto>()
//                    {
//                        Data = new ResultEditSampleCategoryDto()
//                        {
//                            Errors = Errors,
//                        },
//                        IsSuccess = true,
//                        Message = "ویرایش دسته بندی با موفقیت انجام شد"
//                    };
//                }
//                else
//                {
//                    return new ResultDto<ResultEditSampleCategoryDto>()
//                    {
//                        Data = new ResultEditSampleCategoryDto()
//                        {
//                            Errors = Errors,
//                        },
//                        IsSuccess = false,
//                        Message = "!ویرایش دسته بندی نمونه کار انجام نشد"
//                    };
//                }


//            }

//            catch
//            {
//                return new ResultDto<ResultEditSampleCategoryDto>()
//                {
//                    Data = new ResultEditSampleCategoryDto()
//                    {
//                        Errors = Errors,
//                    },
//                    IsSuccess = false,
//                    Message = "!ویرایش دسته بندی نمونه کار انجام نشد"
//                };
//            }

//        }
//    }





//    public class EditSampleCategoryDto
//    {
//        public long Id { get; set; }
//        public string CategoryTitle { get; set; }
//        public string CategoryUniqeName { get; set; }
//        public string? CategoryDescription { get; set; }
//        public bool CategoryIsShowMenu { get; set; }
//        public int CategorySorting { get; set; }
//        public bool isRemoved { get; set; }

//    }
//    public class ResultEditSampleCategoryDto
//    {
//        public List<IdLabelDto> Errors { get; set; }
//    }
//}
