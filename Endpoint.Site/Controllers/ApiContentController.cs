//using Endpoint.Site.Areas.Admin.Models.AdminViewModel.User;
//using Endpoint.Site.Models.ViewModels.Content;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Radin.Application.Interfaces.FacadPatterns;
//using Radin.Application.Services.Contents.Queries.CategoryGet;
//using Radin.Application.Services.Contents.Queries.ContentGet;
//using Radin.Application.Services.Contents.Queries.HomeContentGet;
//using Radin.Application.Services.Contents.Queries.HomePageContentGet;
//using Radin.Domain.Entities.Contents;
//using Radin.Domain.Entities.Users;
//using Sprache;
//using System.Drawing.Printing;

//namespace Endpoint.Site.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ApiContentController : ControllerBase
//    {
//        private readonly IContentFacad _contentFacad;
        
//        public ApiContentController(
         
//         IContentFacad contentFacad
//         )
//        {
            
//            _contentFacad = contentFacad;
//        }

//        [HttpGet("HomeContent")]//دریافت  لیست مقالات با فیلتر دسته بندی در صفحه مقالات
//        public IActionResult HomeContent() 
//        {
//            var GroupContent = _contentFacad.HomeGroupContentGetService.SelectForTops(new RequestHomeGroupContentDto { CategoryTitle = null }).Data.OrderByDescending(n => n.InsertTime).Take(4).ToList();//.OrderBy(n => n.ContentSorting)
//            return Ok(GroupContent);
        
//        }


        

//        [HttpGet("HomeContentUniqe")] //دریافت اطلاعات تک مقاله
//        public IActionResult HomeContentUniqe(string id)
//        {
//            var Result=_contentFacad.HomeUniqeContentGetService.Execute(new RequestContentUniqeGetDto { id = id});
//            return Result.IsSuccess ? Ok(Result) : BadRequest(Result.Message);

//        }


//        [HttpGet("GetCategoryList")]
//        public IActionResult GetCategoryList()
//        {
//            return Ok(_contentFacad.CategoryGetForContentService.InPublic());
//        }

//        [HttpGet("HomeGroupContent")]//دریافت  لیست مقالات با فیلتر دسته بندی و صفحه بندی در صفحه مقالات
//        public IActionResult HomeGroupContent([FromQuery] int PageNumber, int PageSize,string? category)
//        {
//            int skip = (PageNumber - 1) * PageSize;
//            var InitialContentList= _contentFacad.HomeGroupContentGetService.Execute(new RequestHomeGroupContentDto { CategoryTitle = category }).Data.OrderByDescending(n => n.InsertTime).AsQueryable();//.OrderBy(n => n.ContentSorting)
//            var GroupContent = InitialContentList.Skip(skip).Take(PageSize).ToList();
//            int fraction= InitialContentList.Count()/ PageSize;
//            int res= InitialContentList.Count() % PageSize;
//            int Count = 0;
//            if (res==0)
//            {
//                Count = fraction;
//            }
//            else
//            {
//                Count = fraction+1;
//            }
//            var contentDtos = new List<ContentGroupDto>();
//            foreach (var content in GroupContent)
//            {
//                contentDtos.Add(new ContentGroupDto
//                {
//                    CateoryTitle = content.CateoryTitle,
//                    ContentTitle = content.ContentTitle,
//                    ContentUniqName = content.ContentUniqName,
//                    ContentSorting = content.ContentSorting,
//                    ContentLongDescription = content.ContentLongDescription,
//                    ContentImage = content.ContentImage,
//                    InsertTime = content.InsertTime,

//                });

//            }
//            contentDtos.OrderBy(n => n.ContentSorting);
//            var Result=new { Count=Count, ContentDtos=contentDtos};
//            return Ok(Result);
//        }

//    }
//}
