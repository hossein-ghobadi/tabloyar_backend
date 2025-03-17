using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.ContentCategorySet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Contents.Commands.ContentCategoryEdit
{
    public interface IContentCategoryEditService
    {
        ResultDto<ResultUpdateContentCategoryDto> Execute(UpdateContentCategoryDto request);
    }
    public class ContentCategoryEditService : IContentCategoryEditService
    {
        private readonly IDataBaseContext _context;

        public ContentCategoryEditService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultUpdateContentCategoryDto> Execute(UpdateContentCategoryDto updateDto)
        {
            var Errors = new List<IdLabelDto>();
            int id = 0;
            try
            {
                var category = _context.Categories.FirstOrDefault(c => c.Id == updateDto.Id);
                if (category == null)
                {
                    return new ResultDto<ResultUpdateContentCategoryDto>()
                    {
                        Data = new ResultUpdateContentCategoryDto
                        {
                            Errors = Errors
                        },
                        IsSuccess = false,
                        Message = " !ویرایش دسته بندی محتوی انجام نشد"
                    };
                }
                if (string.IsNullOrWhiteSpace(updateDto.CategoryTitle))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عنوان دسته را وارد نمایید"
                    });
                }
                if (string.IsNullOrWhiteSpace(updateDto.CategoryUniqeName))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!نام یکتا را وارد نمایید"
                    });
                }

                var categories = _context.Categories.ToList();
                var OtherCategory = categories.Remove(category);
                if (OtherCategory)
                {
                    var NameDup = categories.FirstOrDefault(p=>p.CategoryUniqeName==updateDto.CategoryUniqeName);
                    var TitleDup = categories.FirstOrDefault(c => c.CategoryTitle == updateDto.CategoryTitle);
                    if (TitleDup != null)
                    {
                        id = id + 1;
                        Errors.Add(new IdLabelDto
                        {
                            id = id,
                            label = "!این عنوان دسته قبلا ثبت شده است"
                        });
                    }
                    if (NameDup != null)
                    {
                        id = id + 1;
                        Errors.Add(new IdLabelDto
                        {
                            id = id,
                            label = "!این نام یکتای دسته قبلا ثبت شده است"
                        });
                    }
                }
                if (updateDto.CategorySorting == null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی را وارد نمایید"
                    });
                }

                if (updateDto.CategorySorting.GetType() != typeof(int))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی باید مقدار صحیح باشد"
                    });
                }

                if (updateDto.CategorySorting is int & updateDto.CategorySorting < 1)
                {

                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی باید بزرگتر از صفر باشد"
                    });
                }

                if (Errors.Count() < 1)
                {
                    category.CategoryTitle = updateDto.CategoryTitle;
                    category.CategoryDescription = updateDto.CategoryDescription;
                    category.CategoryUniqeName = updateDto.CategoryUniqeName;
                    category.CategoryIsShowMenu = updateDto.CategoryIsShowMenu;
                    category.CategoryIsShowMain = updateDto.CategoryIsShowMain;
                    category.CategorySorting = updateDto.CategorySorting;
                    category.CategoryStyle = updateDto.CategoryStyle;
                    category.IsRemoved = updateDto.isRemoved;
                    _context.SaveChanges();
                    return new ResultDto<ResultUpdateContentCategoryDto>()
                    {
                        Data = new ResultUpdateContentCategoryDto()
                        {
                            Errors = Errors,
                        },
                        IsSuccess = true,
                        Message = "ویرایش دسته بندی با موفقیت انجام شد"
                    };
                }
                else
                {
                    return new ResultDto<ResultUpdateContentCategoryDto>()
                    {
                        Data = new ResultUpdateContentCategoryDto()
                        {
                            Errors = Errors,
                        },
                        IsSuccess = false,
                        Message = "!ویرایش دسته بندی محتوی انجام نشد"
                    };
                }


            }

            catch
            {
                return new ResultDto<ResultUpdateContentCategoryDto>()
                {
                    Data = new ResultUpdateContentCategoryDto()
                    {
                        Errors = Errors,
                    },
                    IsSuccess = false,
                    Message = "!ویرایش دسته بندی محتوی انجام نشد"
                };
            }

        }
    }


    public class UpdateContentCategoryDto
    {
        public long Id { get; set; }
        public string CategoryTitle { get; set; }
        public string CategoryUniqeName { get; set; }
        public string? CategoryDescription { get; set; }
        public bool CategoryIsShowMenu { get; set; }
        public bool CategoryIsShowMain { get; set; }
        public int CategorySorting { get; set; }
        public string? CategoryStyle { get; set; }
        public bool isRemoved {  get; set; }

    }
    public class ResultUpdateContentCategoryDto
    {
        public List<IdLabelDto> Errors { get; set; }
    }


}
