using Radin.Application.Interfaces.Contexts;
using Radin.Common.Dto;
using Radin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;

namespace Radin.Application.Services.Contents.Queries.ContentCategoryGet
{
    public interface IContentCategoryGetSummary
    {
        ResultDto<ResultContentCategoryGetSummaryDto> Execute(RequestContentCategoryGetSummaryDto request);
    }

    public class ContentCategoryGetSummary : IContentCategoryGetSummary
    {
        private readonly IDataBaseContext _context;
        public ContentCategoryGetSummary(IDataBaseContext Context)
        {
            _context = Context;

        }

        public ResultDto<ResultContentCategoryGetSummaryDto> Execute(RequestContentCategoryGetSummaryDto request)
        {
            //throw new NotImplementedException();
            int rowsCount = 0;
            int count = _context.Categories.Count();
            var categories = _context.Categories.OrderByDescending(n => n.UpdateTime).AsQueryable();
            int remainder = count % request.PageSize;
            int PageCount = 0;

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                categories = categories.Where(p => p.CategoryUniqeName.Contains(request.SearchKey) || p.CategoryTitle.Contains(request.SearchKey));
                count = categories.Count();
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

            var categoriesList = categories.Select(p => new GetContentCategorySummaryDto
            {
                CategoryTitle = p.CategoryTitle,
                id = p.CategoryUniqeName,
                CategorySorting = p.CategorySorting,
                IsRemoved = p.IsRemoved,
            }).ToList();
            if (request.IsSort)
            {
                
                categoriesList=categoriesList.ToList();//.OrderBy(s => s.CategorySorting)

            }
            

            int skip = (request.PageNumber - 1) * request.PageSize;
            
            var CategorySubList = categoriesList.Skip(skip).Take(request.PageSize).ToList();

           
            return new ResultDto<ResultContentCategoryGetSummaryDto>
            {
                Data = new ResultContentCategoryGetSummaryDto
                {
                    Rows = PageCount,
                    Categories = CategorySubList,
                    count = count,
                },
                IsSuccess = true,
                Message = "",

            };
           

        }


    }

    public class RequestContentCategoryGetSummaryDto
    {
        public string SearchKey { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool IsSort { get; set; }
    }

    public class ResultContentCategoryGetSummaryDto
    {
        public List<GetContentCategorySummaryDto> Categories { get; set; }
        public int Rows { get; set; }
        public int count { get; set; }

    }

    public class GetContentCategorySummaryDto
    {

        public string CategoryTitle { get; set; }
        public string id { get; set; }
        public int CategorySorting {  get; set; }
        public bool IsRemoved { get; set; }

    }
}
