//using Microsoft.EntityFrameworkCore;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Contents.Queries.CategoryGet;
//using Radin.Application.Services.Contents.Queries.HomeContentGet;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Products.Aditional;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Contents.Queries.HomePageContentGet
//{
//    public interface IHomePageContentGetService
//    {
//        ResultDto<ContentResult> Execute();

//    }
//    public class HomePageContentGetService : IHomePageContentGetService
//    {
//        private readonly IDataBaseContext _context;
//        public HomePageContentGetService(IDataBaseContext context)
//        {
//            _context = context;
//        }

//        public ResultDto<ContentResult> Execute()
//        {
//            int rowsCount = 0;
//            int cnt = _context.Contents.Count();
//            var contents = _context.Contents.AsQueryable();
//            //if (!string.IsNullOrWhiteSpace(request.CategoryTitle))
//            //{
//            //    contents = contents.Where(p => p.CategoryUniqeName.Equals(request.CategoryTitle));
//            //}
//            var InitialList = contents.GroupBy(p => p.CategoryUniqeName)
//                .Select(g => g.First())
//                .AsEnumerable()
//                    .Select((item, index) => new Dto

//                    {
//                        id = index + 1,
//                        UniqeName = item.CategoryUniqeName,
//                        title = item.CategoryTitle,
//                    }).ToList();
           


//            var CategoryTemp = _context.Categories.Where(p => p.CategoryIsShowMain == true && p.IsRemoved==false).Select(p=>p.CategoryUniqeName ).ToList();
//            var CategoryList = new List<Dto>();
//            foreach (var category in InitialList)
//            {
//                if ((CategoryTemp.Contains(category.UniqeName))==true )
//                {
//                    CategoryList.Add(category);  

//                }
                
//            }

//            var SubContents = new List<CategorycalContent>();
//            foreach (var category in CategoryList)
//            {
//                var temp = new CategorycalContent
//                {
//                    Title = category.title,
//                    ContentsList = contents.Where(p => p.CategoryUniqeName.Equals(category.UniqeName) && !p.IsRemoved).OrderByDescending(n => n.InsertTime).Take(5).Select(p => new ContentDto//contents.Where(p => p.CategoryUniqeName.Equals(category.UniqeName)).OrderBy(n => n.ContentSorting).Take(5).Select(p => new ContentDto
//                    {
//                        ContentTitle = p.ContentTitle,
//                        ContentUniqName = p.ContentUniqeName,
//                        ContentSorting = p.ContentSorting,
//                        ContentLongDescription = p.ContentMetaDesc.Substring(0, 100),//Regex.Replace(p.ContentLongDescription, "<.*?>", string.Empty).Substring(0, Math.Min(100, Regex.Replace(p.ContentLongDescription, "<.*?>", string.Empty).Length)),
//                        CateoryTitle = p.CategoryTitle,
//                        InsertTime = p.InsertTime,
//                        ContentImage = p.ContentImage,
//                    }).ToList()
//                };

//                SubContents.Add(temp);

//            }

        
//        var MainContents = contents.Where(p=>!p.IsRemoved).OrderByDescending(n => n.InsertTime).Take(12).Select(p => new ContentDto//contents.OrderBy(n => n.ContentSorting).Take(12).Select(p => new ContentDto
//        {
//            ContentTitle = p.ContentTitle,
//            ContentUniqName = p.ContentUniqeName,
//            ContentSorting = p.ContentSorting,
//            ContentLongDescription = p.ContentMetaDesc.Substring(0, 100),//Regex.Replace(p.ContentLongDescription, "<.*?>", string.Empty).Substring(0, Math.Min(100, Regex.Replace(p.ContentLongDescription, "<.*?>", string.Empty).Length)),
//            CateoryTitle = p.CategoryTitle,
//            InsertTime = p.InsertTime,
//            ContentImage = p.ContentImage,
//        }).ToList();
//            return new ResultDto<ContentResult>
//            {
//                Data =new ContentResult{
//                MainContents=MainContents,
//                SubContents=SubContents

//                },
//                IsSuccess = true,
//                Message = "",

//            };

//        }


//    }

    
    
//    public class Dto
//    {
//        public int id { get; set; }
//        public string UniqeName { get; set; }
//        public string title { get; set; }   
//    }
//    public class CategorycalContent
//    {
//        public string Title { get; set; }
//        public List<ContentDto> ContentsList { get; set; }
//    }
//        public class ContentDto
//        {
//            public string CateoryTitle { get; set; }
//            public string ContentTitle { get; set; }
//            public string ContentUniqName { get; set; }
//            public int ContentSorting { get; set; }
//            public string ContentLongDescription { get; set; }
//            public string ContentImage { get; set; }
//            public DateTime InsertTime { get; set; }

//    }
//    public class ContentResult
//    {
//        public List<ContentDto> MainContents { get; set; }
//        public List<CategorycalContent> SubContents { get; set; }
//    }
    
//}
