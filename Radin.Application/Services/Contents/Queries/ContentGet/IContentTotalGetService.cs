//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Contents.Queries.ContentCategoryGet;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Contents.Queries.ContentGet
//{
//    public interface IContentTotalGetService
//    {
//        ResultDto<ResultContentTotalGetDto> Execute(RequestContentTotalGetDto request);
//    }
//    public class ContentTotalGetService : IContentTotalGetService
//    {
//        private readonly IDataBaseContext _context;
//        public ContentTotalGetService(IDataBaseContext Context)
//        {
//            _context = Context;

//        }

//        public ResultDto<ResultContentTotalGetDto> Execute(RequestContentTotalGetDto request)
//        {
//            int rowsCount = 0;
//            int count = _context.Contents.Count();
//            var contents = _context.Contents.OrderByDescending(n => n.UpdateTime).AsQueryable();
//            int remainder = count % request.PageSize;
//            int PageCount = 0;
            

//                if (!string.IsNullOrWhiteSpace(request.SearchKey))
//            {
//                contents = contents.Where(p => p.ContentUniqeName.Contains(request.SearchKey) || p.ContentTitle.Contains(request.SearchKey));
//                count = contents.Count();
//                remainder = count % request.PageSize;
//                if (remainder > 0)
//                {
//                    PageCount = (count / request.PageSize) + 1;
//                }
//                else
//                {
//                    PageCount = count / request.PageSize;
//                }
//            }
//            else
//            {
//                remainder = count % request.PageSize;
//                if (remainder > 0)
//                {
//                    PageCount = (count / request.PageSize) + 1;
//                }
//                else
//                {
//                    PageCount = count / request.PageSize;
//                }
//            }


//            var contentsList = contents.Select(p => new GetContentTotalDto
//            {
//                ContentTitle = p.ContentTitle,
//                id = p.ContentUniqeName,
//                CategoryTitle =  p.CategoryTitle,
//                InsertTime = p.InsertTime,
//                ContentSorting = p.ContentSorting,
//                IsRemoved = p.IsRemoved,
//                IsIndex = p.IsIndex,
//            }).ToList();
//            if (request.IsSort)
//            {
//                contentsList=contentsList.OrderBy(s => s.ContentSorting).ToList();
//            }
//            int skip = (request.PageNumber - 1) * request.PageSize;

//            var ContentSubList = contentsList.Skip(skip).Take(request.PageSize).ToList();

           
//                return new ResultDto<ResultContentTotalGetDto>
//                {
//                    Data = new ResultContentTotalGetDto
//                    {
//                        Rows = PageCount,
//                        Contents = ContentSubList,
//                        count = count,
//                    },
//                    IsSuccess = true,
//                    Message = "",

//                };
           
          

//        }


//    }

//    public class RequestContentTotalGetDto
//    {
//        public string SearchKey { get; set; }
//        public int PageNumber { get; set; }
//        public int PageSize { get; set; }
//        public bool IsSort { get; set; }
//    }

//    public class ResultContentTotalGetDto
//    {
//        public List<GetContentTotalDto> Contents { get; set; }
//        public int Rows { get; set; }
//        public int count { get; set; }

//    }

//    public class GetContentTotalDto
//    {
//        //public long Id { get; set; }
//        public string ContentTitle { get; set; }
//        public string id { get; set; }
//        //public long CategoryId { get; set; }
//        public string CategoryTitle { get; set; }
//        public int ContentSorting { get; set; }
//        public DateTime InsertTime { get; set; }
//        public bool IsRemoved { get; set; }
//        public bool IsIndex { get; set; }

//    }
//}
