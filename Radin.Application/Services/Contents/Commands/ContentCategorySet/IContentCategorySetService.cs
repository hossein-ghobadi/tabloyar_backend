//using Radin.Application.Interfaces.Contexts;
//using Radin.Common;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Contents;
//using Radin.Domain.Entities.Users;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Contents.Commands.ContentCategorySet
//{
//    public interface IContentCategorySetService
//    {
//        ResultDto<ResultContentCategorySetDto> Execute(RequestContentCategorySetDto request);
        
//    }

//    public class ContentCategorySetService : IContentCategorySetService
//    {
//        private readonly IDataBaseContext _context;

//        public ContentCategorySetService(IDataBaseContext context)
//        {
//            _context = context;
//        }
//        public ResultDto<ResultContentCategorySetDto> Execute(RequestContentCategorySetDto request)
//        {
            
//            var Errors = new List<IdLabelDto>();
//            try
//            {
//                int id = 0;
//                var TitleDup = _context.Categories.FirstOrDefault(c => c.CategoryTitle == request.CategoryTitle);
//                var NameDup = _context.Categories.FirstOrDefault(c => c.CategoryUniqeName == request.CategoryUniqeName);

//                if (string.IsNullOrWhiteSpace(request.CategoryTitle))
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

//                if (string.IsNullOrWhiteSpace(request.CategoryUniqeName))
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
//                if (request.CategoryUniqeName.Length > 75 || request.CategoryUniqeName.Length < 3)
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!طول متن نام یکتا محتوی باید بین 3 الی 75 کاراکتر باشد"
//                    });
//                }

//                if (request.CategorySorting == null)
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عدد مرتب سازی را وارد نمایید"
//                    });
//                }

//                if (request.CategorySorting.GetType() != typeof(int))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عدد مرتب سازی باید مقدار صحیح باشد"
//                    });
//                }
//                if (request.CategorySorting is int & request.CategorySorting < 1)
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
//                    Category category = new Category()
//                    {
//                        CategoryTitle = request.CategoryTitle,
//                        CategoryUniqeName = request.CategoryUniqeName,
//                        CategorySorting = request.CategorySorting,
//                        CategoryStyle = request.CategoryStyle,
//                        CategoryIsShowMain = request.CategoryIsShowMain,
//                        CategoryIsShowMenu = request.CategoryIsShowMenu,
//                        CategoryDescription = request.CategoryDescription,
//                    };
//                    category.UpdateTime = category.InsertTime;
//                    _context.Categories.Add(category);

//                    _context.SaveChanges();

//                    return new ResultDto<ResultContentCategorySetDto>()
//                    {
//                        Data = new ResultContentCategorySetDto()
//                        {
//                            CategoryId = category.Id,
//                            Errors = Errors,
//                        },
//                        IsSuccess = true,
//                        Message = "دسته بندی محتوی با موفقیت درج شد",
//                    };
//                }
//                else
//                {
//                    return new ResultDto<ResultContentCategorySetDto>()
//                    {
//                        Data = new ResultContentCategorySetDto()
//                        {
//                            CategoryId = 0,
//                            Errors = Errors,
//                        },
//                        IsSuccess = false,
//                        Message = "دسته بندی جدید درج نشد !"
//                    };

//                }
//            }
//            catch (Exception ) {
//                return new ResultDto<ResultContentCategorySetDto>()
//                {
//                    Data = new ResultContentCategorySetDto()
//                    {
//                        CategoryId = 0,
//                        Errors = Errors,
//                    },
//                    IsSuccess = false,
//                    Message = "دسته بندی جدید درج نشد !"
//                };


//            }

//        }

//    }
//    public class RequestContentCategorySetDto
//    {
//        public string CategoryTitle { get; set; }
//        public string CategoryUniqeName { get; set; }
//        public int CategorySorting { get; set; } = 1;
//        public string? CategoryStyle { get; set; }

//        public bool CategoryIsShowMain { get; set; } = true;
//        public bool CategoryIsShowMenu { get; set; } = true;
//        public string? CategoryDescription { get; set; }
//    }

//    public class ResultContentCategorySetDto
//    {
//        public long CategoryId { get; set; }
//        public List<IdLabelDto> Errors { get; set; }
//    }

//}
