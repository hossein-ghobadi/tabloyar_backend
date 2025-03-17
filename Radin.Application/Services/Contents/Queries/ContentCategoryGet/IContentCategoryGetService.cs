using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.ContentCategorySet;
using Radin.Common;
using Radin.Common.Dto;
using Radin.Domain.Entities.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Contents.Queries.ContentCategoryGet
{
    public interface IContentCategoryGetService
    {
        ResultDto<GetContentCategoryDto> Execute(RequestContentCategoryGetDto request);
    }
    
    public class ContentCategoryGetService : IContentCategoryGetService
    {
        private readonly IDataBaseContext _context;
        public ContentCategoryGetService(IDataBaseContext Context)
        {
            _context = Context;

        }

        public ResultDto<GetContentCategoryDto> Execute(RequestContentCategoryGetDto request)
        {

            var categories = _context.Categories.FirstOrDefault(c => c.CategoryUniqeName == request.uniqename);
            if (categories == null)
            {
                return  new ResultDto<GetContentCategoryDto>()
                    {
                    Data=new GetContentCategoryDto
                    {
                        Id = 0,
                        CategoryTitle = "",
                        CategoryUniqeName = "",
                        CategorySorting = 0,
                        CategoryStyle = "",
                        CategoryIsShowMain = false,
                        CategoryIsShowMenu = false,
                        CategoryDescription = "",
                        IsRemoved = false,
                    },
                    IsSuccess = false,
                    Message="خطا در دریافت دسته بندی"
                    
                };
            }

            var categoriesList = new GetContentCategoryDto
            {
                Id = categories.Id,
                CategoryTitle = categories.CategoryTitle,
                CategoryUniqeName = categories.CategoryUniqeName,
                CategorySorting = categories.CategorySorting,
                CategoryStyle = categories.CategoryStyle,
                CategoryIsShowMain = categories.CategoryIsShowMain,
                CategoryIsShowMenu = categories.CategoryIsShowMenu,
                CategoryDescription = categories.CategoryDescription,
                IsRemoved = categories.IsRemoved,
            };



            return new ResultDto<GetContentCategoryDto>()
            {
                Data= categoriesList,
                IsSuccess = true,
                Message=" دریافت موفقیت آمیز "

            };
        }

        
    }

    public class RequestContentCategoryGetDto
    {
        public string uniqename { get; set; }
    }


    public class GetContentCategoryDto
    {
        public long Id { get; set; }

        public string CategoryTitle { get; set; }
        public string CategoryUniqeName { get; set; }
        public int CategorySorting { get; set; }
        public string? CategoryStyle { get; set; }

        public bool CategoryIsShowMain { get; set; }
        public bool CategoryIsShowMenu { get; set; }
        public string CategoryDescription { get; set; }
        public bool IsRemoved { get; set; }

    }
}
