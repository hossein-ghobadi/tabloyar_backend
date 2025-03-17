using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.ContentCategoryEdit;
using Radin.Common.Dto;
using Radin.Domain.Entities.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Ideas.Commands.IdeaCategoryEdit
{
    public interface IIdeaCategoryEditService
    {
        ResultDto<ResultEditIdeaCategoryDto> Execute(EditIdeaCategoryDto request);

    }
    public class IdeaCategoryEditService : IIdeaCategoryEditService
    {
        private readonly IDataBaseContext _context;

        public IdeaCategoryEditService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultEditIdeaCategoryDto> Execute(EditIdeaCategoryDto updateDto)
        {
            var Errors = new List<IdLabelDto>();
            int id = 0;
            try
            {
                var ideaCategory = _context.IdeaCategories.FirstOrDefault(c => c.Id == updateDto.Id);
                if (ideaCategory == null)
                {
                    return new ResultDto<ResultEditIdeaCategoryDto>()
                    {
                        Data = new ResultEditIdeaCategoryDto
                        {
                            Errors = Errors
                        },
                        IsSuccess = false,
                        Message = " !ویرایش دسته بندی ایده انجام نشد"
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

                var IdeaCategories = _context.IdeaCategories.ToList();
                var OtherCategory = IdeaCategories.Remove(ideaCategory);
                if (OtherCategory)
                {
                    var NameDup = IdeaCategories.FirstOrDefault(p => p.IdeaCategoryUniqeName == updateDto.CategoryUniqeName);
                    var TitleDup = IdeaCategories.FirstOrDefault(c => c.IdeaCategoryTitle == updateDto.CategoryTitle);
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
                    ideaCategory.IdeaCategoryTitle = updateDto.CategoryTitle;
                    ideaCategory.IdeaCategoryDescription = updateDto.CategoryDescription;
                    ideaCategory.IdeaCategoryUniqeName = updateDto.CategoryUniqeName;
                    ideaCategory.IdeaCategoryIsShowMenu = updateDto.CategoryIsShowMenu;
                    ideaCategory.IdeaCategorySorting = updateDto.CategorySorting;
                    ideaCategory.IsRemoved = updateDto.isRemoved;
                    _context.SaveChanges();
                    return new ResultDto<ResultEditIdeaCategoryDto>()
                    {
                        Data = new ResultEditIdeaCategoryDto()
                        {
                            Errors = Errors,
                        },
                        IsSuccess = true,
                        Message = "ویرایش دسته بندی با موفقیت انجام شد"
                    };
                }
                else
                {
                    return new ResultDto<ResultEditIdeaCategoryDto>()
                    {
                        Data = new ResultEditIdeaCategoryDto()
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
                return new ResultDto<ResultEditIdeaCategoryDto>()
                {
                    Data = new ResultEditIdeaCategoryDto()
                    {
                        Errors = Errors,
                    },
                    IsSuccess = false,
                    Message = "!ویرایش دسته بندی محتوی انجام نشد"
                };
            }

        }
    }





    public class EditIdeaCategoryDto
    {
        public long Id { get; set; }
        public string CategoryTitle { get; set; }
        public string CategoryUniqeName { get; set; }
        public string? CategoryDescription { get; set; }
        public bool CategoryIsShowMenu { get; set; }
        public int CategorySorting { get; set; }
        public bool isRemoved { get; set; }

    }
    public class ResultEditIdeaCategoryDto
    {
        public List<IdLabelDto> Errors { get; set; }
    }
}



