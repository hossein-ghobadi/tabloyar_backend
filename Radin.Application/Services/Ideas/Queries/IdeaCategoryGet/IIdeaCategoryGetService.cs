using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Queries.CategoryGet;
using Radin.Application.Services.Contents.Queries.ContentCategoryGet;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Ideas.Queries.IdeaCategoryGet
{
    public interface IIdeaCategoryGetService
    {
        List<GetDto> CategoryForAdminContentSet();
        ResultDto<GetIdeaCategoryDto> CategorySingleGetForAdmin(RequestIdeaCategoryGetDto request);
        ResultDto<ResultIdeaCategoryListDto> CategoryListGetForAdmin(RequestIdeaCategoryListDto request);


    }

    public class IdeaCategoryGetService : IIdeaCategoryGetService
    {
        private readonly IDataBaseContext _context;
        public IdeaCategoryGetService(IDataBaseContext Context)
        {
            _context = Context;

        }

        public List<GetDto> CategoryForAdminContentSet()
        {
            var categories = _context.IdeaCategories.Where(p => !(p.IsRemoved));

            //int rowsCount = 0;
            var categoriesList = categories.Select(p => new GetDto
            {
                id = p.IdeaCategoryUniqeName,
                label = p.IdeaCategoryTitle,



            }).ToList();

            return categoriesList;
        }



        public ResultDto<GetIdeaCategoryDto> CategorySingleGetForAdmin(RequestIdeaCategoryGetDto request)
        {

            var ideaCategories = _context.IdeaCategories.FirstOrDefault(c => c.IdeaCategoryUniqeName == request.uniqename);
            if (ideaCategories == null)
            {
                return new ResultDto<GetIdeaCategoryDto>()
                {
                    Data = new GetIdeaCategoryDto
                    {
                        Id = 0,
                        IdeaCategoryTitle = "",
                        IdeaCategoryUniqeName = "",
                        IdeaCategorySorting = 0,
                        IdeaCategoryIsShowMenu = false,
                        IdeaCategoryDescription = "",
                        IsRemoved = false,
                    },
                    IsSuccess = false,
                    Message = "خطا در دریافت دسته بندی"

                };
            }

            var ideaCategoriesList = new GetIdeaCategoryDto
            {
                Id = ideaCategories.Id,
                IdeaCategoryTitle = ideaCategories.IdeaCategoryTitle,
                IdeaCategoryUniqeName = ideaCategories.IdeaCategoryUniqeName,
                IdeaCategorySorting = ideaCategories.IdeaCategorySorting,
                IdeaCategoryIsShowMenu = ideaCategories.IdeaCategoryIsShowMenu,
                IdeaCategoryDescription = ideaCategories.IdeaCategoryDescription,
                IsRemoved = ideaCategories.IsRemoved,
            };



            return new ResultDto<GetIdeaCategoryDto>()
            {
                Data = ideaCategoriesList,
                IsSuccess = true,
                Message = " دریافت موفقیت آمیز "

            };
        }






        public ResultDto<ResultIdeaCategoryListDto> CategoryListGetForAdmin(RequestIdeaCategoryListDto request)
        {
            //throw new NotImplementedException();
            int rowsCount = 0;
            int count = _context.IdeaCategories.Count();
            var ideaCategories = _context.IdeaCategories.OrderByDescending(n => n.UpdateTime).AsQueryable();
            int remainder = count % request.PageSize;
            int PageCount = 0;

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                ideaCategories = ideaCategories.Where(p => p.IdeaCategoryUniqeName.Contains(request.SearchKey) || p.IdeaCategoryTitle.Contains(request.SearchKey));
                count = ideaCategories.Count();
                remainder = count % request.PageSize;
                if (remainder > 0)
                {
                    PageCount = (count / request.PageSize) + 1;
                }
                else
                {
                    PageCount = count / request.PageSize;
                }
            }
            else
            {
                remainder = count % request.PageSize;
                if (remainder > 0)
                {
                    PageCount = (count / request.PageSize) + 1;
                }
                else
                {
                    PageCount = count / request.PageSize;
                }
            }

            var categoriesList = ideaCategories.Select(p => new AbstractedIdeaCategoryDto
            {
                IdeaCategoryTitle = p.IdeaCategoryTitle,
                id = p.IdeaCategoryUniqeName,
                IdeaCategorySorting = p.IdeaCategorySorting,
                IsRemoved = p.IsRemoved,
            }).ToList();
            if (request.IsSort)
            {

                categoriesList = categoriesList.ToList();//.OrderBy(s => s.IdeaCategorySorting)

            }


            int skip = (request.PageNumber - 1) * request.PageSize;

            var CategorySubList = categoriesList.Skip(skip).Take(request.PageSize).ToList();


            return new ResultDto<ResultIdeaCategoryListDto>
            {
                Data = new ResultIdeaCategoryListDto
                {
                    Rows = PageCount,
                    IdeaCategories = CategorySubList,
                    count = count,
                },
                IsSuccess = true,
                Message = "",

            };


        }

    }



    













    public class GetDto
    {
        public string id { get; set; }
        public string label { get; set; }


    }
//........................................................................




    public class RequestIdeaCategoryGetDto
    {
        public string uniqename { get; set; }
    }


    public class GetIdeaCategoryDto
    {
        public long Id { get; set; }

        public string IdeaCategoryTitle { get; set; }
        public string IdeaCategoryUniqeName { get; set; }
        public int IdeaCategorySorting { get; set; }

        public bool IdeaCategoryIsShowMenu { get; set; }
        public string IdeaCategoryDescription { get; set; }
        public bool IsRemoved { get; set; }

    }
    //........................................................................



    public class RequestIdeaCategoryListDto
    {
        public string SearchKey { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool IsSort { get; set; }
    }

    public class ResultIdeaCategoryListDto
    {
        public List<AbstractedIdeaCategoryDto> IdeaCategories { get; set; }
        public int Rows { get; set; }
        public int count { get; set; }

    }

    public class AbstractedIdeaCategoryDto
    {

        public string IdeaCategoryTitle { get; set; }
        public string id { get; set; }
        public int IdeaCategorySorting { get; set; }
        public bool IsRemoved { get; set; }

    }
}
