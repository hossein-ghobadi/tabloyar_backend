using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.ContentCategorySet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Contents;
using Radin.Domain.Entities.Ideas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Ideas.Commands.IdeaCategorySet
{
    public interface IIdeaCategorySetService
    {
        ResultDto<ResultIdeaCategorySetDto> AdminSet(RequestIdeaCategorySetDto request);

    }
    public class IdeaCategorySetService : IIdeaCategorySetService
    {
        private readonly IDataBaseContext _context;

        public IdeaCategorySetService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultIdeaCategorySetDto> AdminSet(RequestIdeaCategorySetDto request)
        {

            var Errors = new List<IdLabelDto>();
            try
            {
                int id = 0;
                var TitleDup = _context.IdeaCategories.FirstOrDefault(c => c.IdeaCategoryTitle == request.IdeaCategoryTitle);
                var NameDup = _context.IdeaCategories.FirstOrDefault(c => c.IdeaCategoryUniqeName == request.IdeaCategoryUniqeName);

                if (string.IsNullOrWhiteSpace(request.IdeaCategoryTitle))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عنوان دسته را وارد نمایید"
                    });
                }
                if (TitleDup != null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!این عنوان دسته قبلا ثبت شده است"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.IdeaCategoryUniqeName))
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
                        label = "!این نام یکتای دسته قبلا ثبت شده است"
                    });
                }
                if (request.IdeaCategoryUniqeName.Length > 75 || request.IdeaCategoryUniqeName.Length < 3)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!طول متن نام یکتا محتوی باید بین 3 الی 75 کاراکتر باشد"
                    });
                }

                if (request.IdeaCategorySorting == null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی را وارد نمایید"
                    });
                }

                if (request.IdeaCategorySorting.GetType() != typeof(int))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی باید مقدار صحیح باشد"
                    });
                }
                if (request.IdeaCategorySorting is int & request.IdeaCategorySorting < 1)
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
                    IdeaCategory ideaCategory = new IdeaCategory()
                    {
                        IdeaCategoryTitle = request.IdeaCategoryTitle,
                        IdeaCategoryUniqeName = request.IdeaCategoryUniqeName,
                        IdeaCategorySorting = request.IdeaCategorySorting,
                        //IdeaCategoryStyle = request.IdeaCategoryStyle,
                        //IdeaCategoryIsShowMain = request.IdeaCategoryIsShowMain,
                        IdeaCategoryIsShowMenu = request.IdeaCategoryIsShowMenu,
                        IdeaCategoryDescription = request.IdeaCategoryDescription,
                    };
                    ideaCategory.UpdateTime = ideaCategory.InsertTime;
                    _context.IdeaCategories.Add(ideaCategory);

                    _context.SaveChanges();

                    return new ResultDto<ResultIdeaCategorySetDto>()
                    {
                        Data = new ResultIdeaCategorySetDto()
                        {
                            IdeaCategoryId = ideaCategory.Id,
                            Errors = Errors,
                        },
                        IsSuccess = true,
                        Message = "دسته بندی محتوی با موفقیت درج شد",
                    };
                }
                else
                {
                    return new ResultDto<ResultIdeaCategorySetDto>()
                    {
                        Data = new ResultIdeaCategorySetDto()
                        {
                            IdeaCategoryId = 0,
                            Errors = Errors,
                        },
                        IsSuccess = false,
                        Message = "دسته بندی جدید درج نشد !"
                    };

                }
            }
            catch (Exception)
            {
                return new ResultDto<ResultIdeaCategorySetDto>()
                {
                    Data = new ResultIdeaCategorySetDto()
                    {
                        IdeaCategoryId = 0,
                        Errors = Errors,
                    },
                    IsSuccess = false,
                    Message = "دسته بندی جدید درج نشد !"
                };


            }

        }

    }
    public class RequestIdeaCategorySetDto
    {
        public string IdeaCategoryTitle { get; set; }
        public string IdeaCategoryUniqeName { get; set; }
        public int IdeaCategorySorting { get; set; } = 1;
        //public string? IdeaCategoryStyle { get; set; }

        //public bool IdeaCategoryIsShowMain { get; set; } = true;
        public bool IdeaCategoryIsShowMenu { get; set; } = true;
        public string? IdeaCategoryDescription { get; set; }
    }

    public class ResultIdeaCategorySetDto
    {
        public long IdeaCategoryId { get; set; }
        public List<IdLabelDto> Errors { get; set; }
    }
}
