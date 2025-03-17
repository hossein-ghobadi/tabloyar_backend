using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Queries.ContentGet;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Radin.Application.Services.Contents.Queries.HomeContentGet
{
    public interface IHomeGroupContentGetService
    {
        ResultDto<List<GetHomeGroupContentDto>> Execute(RequestHomeGroupContentDto request);
        ResultDto<List<GetHomeGroupContentDto>> SelectForTops(RequestHomeGroupContentDto request);

    }

    public class HomeGroupContentGetService: IHomeGroupContentGetService
    {
        private readonly IDataBaseContext _context;
        public HomeGroupContentGetService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<List<GetHomeGroupContentDto>> Execute(RequestHomeGroupContentDto request)
        {
            int rowsCount = 0;
            int cnt = _context.Contents.Count();
            var contents = _context.Contents.Where(p=>!p.IsRemoved).AsQueryable();
            var category = _context.Categories.Where(p => p.CategoryUniqeName == request.CategoryTitle).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(request.CategoryTitle) & category != null)
            {
                if (!category.IsRemoved)
                {
                    contents = contents.Where(p => p.CategoryUniqeName.Equals(request.CategoryTitle));

                }
            }
            
            var contentsList = contents.Select(p => new GetHomeGroupContentDto
            {
                ContentTitle = p.ContentTitle,
                ContentUniqName = p.ContentUniqeName,
                ContentSorting = p.ContentSorting,
                ContentLongDescription = p.ContentMetaDesc.Substring(0, 100),//Regex.Replace(p.ContentLongDescription, "<.*?>", string.Empty).Substring(0, Math.Min(100, Regex.Replace(p.ContentLongDescription, "<.*?>", string.Empty).Length)),
                CateoryTitle = p.CategoryTitle,
                InsertTime = p.InsertTime,
                ContentImage= p.ContentImage,
                
            }).ToList();
            return new ResultDto<List<GetHomeGroupContentDto>>
            {
                Data = contentsList,
                IsSuccess = true,
                Message = "",

            };

        }




        public ResultDto<List<GetHomeGroupContentDto>> SelectForTops(RequestHomeGroupContentDto request)
        {
            int rowsCount = 0;
            int cnt = _context.Contents.Count();
            var contents = _context.Contents.Where(p => !p.IsRemoved && p.ContentSorting==5).AsQueryable();
            //var category = _context.Categories.Where(p => p.CategoryUniqeName == request.CategoryTitle).FirstOrDefault();
            //if (!string.IsNullOrWhiteSpace(request.CategoryTitle) & category != null)
            //{
            //    if (!category.IsRemoved)
            //    {
            //        contents = contents.Where(p => p.CategoryUniqeName.Equals(request.CategoryTitle));

            //    }
            //}

            var contentsList = contents.Select(p => new GetHomeGroupContentDto
            {
                ContentTitle = p.ContentTitle,
                ContentUniqName = p.ContentUniqeName,
                ContentSorting = p.ContentSorting,
                ContentLongDescription = p.ContentMetaDesc.Substring(0, 100),//Regex.Replace(p.ContentLongDescription, "<.*?>", string.Empty).Substring(0, Math.Min(100, Regex.Replace(p.ContentLongDescription, "<.*?>", string.Empty).Length)),
                CateoryTitle = p.CategoryTitle,
                InsertTime = p.InsertTime,
                ContentImage = p.ContentImage,

            }).ToList();
            return new ResultDto<List<GetHomeGroupContentDto>>
            {
                Data = contentsList,
                IsSuccess = true,
                Message = "",

            };

        }


    }

    public class RequestHomeGroupContentDto
    {
        public string CategoryTitle { get; set; }
    }
    public class GetHomeGroupContentDto
    {
        public string CateoryTitle { get; set; }
        public string ContentTitle { get; set; }
        public string ContentUniqName { get; set; }
        public int ContentSorting { get; set; }
        public string ContentLongDescription { get; set; }
        public string ContentImage { get; set; }
        public DateTime InsertTime { get; set; }

    }


}
