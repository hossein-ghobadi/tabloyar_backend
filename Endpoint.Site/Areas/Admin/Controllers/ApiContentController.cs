

using Endpoint.Site.Areas.Admin.Models.AdminViewModel.Content;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Radin.Application.Interfaces.FacadPatterns;
using Radin.Application.Services.Contents.Commands.CommentRemove;
using Radin.Application.Services.Contents.Commands.ContentCategoryEdit;
using Radin.Application.Services.Contents.Commands.ContentCategoryRemove;
using Radin.Application.Services.Contents.Commands.ContentCategorySet;
using Radin.Application.Services.Contents.Commands.ContentChangeIsIndex;
using Radin.Application.Services.Contents.Commands.ContentEdit;
using Radin.Application.Services.Contents.Commands.ContentRemove;
using Radin.Application.Services.Contents.Commands.ContentSet;
using Radin.Application.Services.Contents.Queries.CategoryGet;
using Radin.Application.Services.Contents.Queries.ContentCategoryGet;
using Radin.Application.Services.Contents.Queries.ContentGet;
using Radin.Common.Dto;
using Radin.Common.Request;
using Radin.Domain.Entities.Contents;
using System.Runtime.Intrinsics.X86;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Radin.Application.Services.Contents.Commands.CommentRemove.CommentRemoveService;
using static Radin.Application.Services.Contents.Commands.CommentRemove.SubCommentRemoveService;
using static Radin.Application.Services.Contents.Commands.ContentCategoryRemove.ContentCategoryRemoveService;
using static Radin.Application.Services.Contents.Commands.ContentRemove.ContentRemoveService;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Route("Admin/api/[controller]")]
    [ApiController]

    public class ApiContentController : ControllerBase
    {
        private readonly IContentFacad _contentFacad;
        
        private readonly IContentEditService _contentEditService;
        private readonly ICommentRemoveService _commentRemoveService;
       
        public ApiContentController(
        
        IContentFacad contentFacad
        )
        {
           
            _contentFacad= contentFacad;
        }

        [HttpGet("GetCategory")]    // دریافت اطلاعات  کامل هر دسته بندی در پنل ادمین
        [Authorize(Policy = "GetCategory")]
        public IActionResult GetCategory(string uniqname)
        {
            var uniqval = uniqname.ToString();
            var Result = _contentFacad.ContentCategoryGetService.Execute(new RequestContentCategoryGetDto
            {
                uniqename = uniqval,
            });

            if (Result.IsSuccess==true )
            {
                return Ok( Result.Data );
            }
            else
            {
                return BadRequest(Result.IsSuccess);
            }
        }

        [HttpGet("GetCategorySummary")] // دریافت لیست اطلاعات مختصر دسته بندی ها برای نمایش در صفحه دسته بندی های پنل ادمین
        [Authorize(Policy = "GetCategorySummary")]
        public IActionResult GetCategorySummary(int PageNumber, int PageSize, string? search,bool sort=false)
        {
            var validationErrors = new List<IdLabelDto>();
            int id = 0;
            if (PageNumber.GetType() != typeof(int))
            {
                id = id + 1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = "!شماره صفحه باید به فرمت عدد باشد  "
                });
            }
            if (PageSize.GetType() != typeof(int))
            {
                id = id + 1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = "!شماره صفحه باید به فرمت عدد باشد  "
                });
            }
            if (validationErrors.Any())
            {
                return BadRequest(validationErrors);
            }
            else
            {
                var CategoryInfo = _contentFacad.ContentCategoryGetSummary.Execute(new RequestContentCategoryGetSummaryDto
                {
                    PageNumber = PageNumber,
                    SearchKey = search,
                    PageSize = PageSize,
                    IsSort = sort
                });
                return Ok(CategoryInfo);
            }
        }


        [HttpPost("SetContentCategory")]
        [Authorize(Policy = "SetContentCategory")]
        public IActionResult SetContentCategory(ContentCategorySetDtoViewModel requestContentCategorySetDto)
        {
            var validationErrors = requestContentCategorySetDto.Validate();
 
            if (validationErrors.Any())
            {
                return BadRequest(validationErrors);
            }
            else
            {
                var result = _contentFacad.ContentCategorySetService.Execute(new RequestContentCategorySetDto
                {
                    CategoryTitle = requestContentCategorySetDto.CategoryTitle,
                    CategoryUniqeName = requestContentCategorySetDto.CategoryUniqeName,
                    CategorySorting = requestContentCategorySetDto.CategorySorting,
                    CategoryStyle = requestContentCategorySetDto.CategoryStyle,
                    CategoryIsShowMain = requestContentCategorySetDto.CategoryIsShowMain,
                    CategoryIsShowMenu = requestContentCategorySetDto.CategoryIsShowMenu,
                    CategoryDescription = requestContentCategorySetDto.CategoryDescription,

                });
                if (result.IsSuccess == true)
                {
                    return Ok(result);
                }
                else { return BadRequest(result); }
            }

        }

        [HttpPost("RemoveCategory")]
        [Authorize(Policy = "RemoveCategory")]
        public IActionResult RemoveCategory(RequestCategoryGetIdDto requestCategoryGetuniqname)
        {
            var result= _contentFacad.ContentCategoryRemoveService.Execute(new RequestCategoryGetIdDto { id = requestCategoryGetuniqname.id});

            if (result.IsSuccess == true)
            {
                return Ok(result);
            }
            else { return BadRequest(result); }

        }

        [HttpPost("EditCategory")]
        [Authorize(Policy = "EditCategory")]
        public IActionResult EditCategory(UpdateContentCategoryDto request)
        {
            var result= _contentFacad.ContentCategoryEditService.Execute(new UpdateContentCategoryDto
            {
                Id = request.Id,
                CategoryTitle = request.CategoryTitle,
                CategoryUniqeName = request.CategoryUniqeName,
                CategoryStyle = request.CategoryStyle,
                CategorySorting = request.CategorySorting,
                CategoryDescription = request.CategoryDescription,
                CategoryIsShowMenu = request.CategoryIsShowMenu,
                CategoryIsShowMain = request.CategoryIsShowMain,
                isRemoved = request.isRemoved
            });

            if (result.IsSuccess == true)
            {
                return Ok(result);
            }
            else { return BadRequest(result); }
        }



        //[HttpGet("GetCategoryList")]
        //public IActionResult GetCategoryList()
        //{
            
        //    return Ok(_contentFacad.CategoryGetService.Execute());
        //}

        [HttpGet("GetCategoryForContent")]

        public IActionResult GetCategoryForContent()
        {
            return Ok(_contentFacad.CategoryGetForContentService.InPublic());
        }

        [HttpPost("SetContent")]
        [Authorize(Policy = "SetContent")]
        public IActionResult SetContent(SetContentViewModel Request

        )
        {

            var result = _contentFacad.ContentSetService.Execute(new RequestContentSetDto
            {

                ContentTitle = Request.title,
                ContentUniqeName = Request.url,
                CommentSituation = Request.commentable,
                CommentShow = Request.showComment,
                ContentSorting = Request.sort,
                ContentLongDescription =Request.main ,
                ContentMetaDesc = Request.meta,
                ContentImageAlt = Request.imageDesc,
                ContentImageTitle = Request.imageTitle,
                ContentPublish = true,
                ContentImage = Request.image,
                CategoryUniqeName = Request.category,
                Canonical=Request.canonical,
                IsIndex=Request.IsIndex ?? true,
                
            });

            if (result.IsSuccess == true)
            {
                return Ok(result);
            }
            else { return BadRequest(result); }
        }

        [HttpGet("GetAll")]
        [Authorize(Policy = "GetAll")]
        public IActionResult GetAll(int PageNumber, int PageSize, string? search, bool sort = false)
        {
            var validationErrors = new List<IdLabelDto>();
            int id = 0;
            if(string.IsNullOrEmpty(PageNumber.ToString()))
            {
                id = id + 1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = "!شماره صفحه را وارد کنید  "
                });

            }
            if (PageNumber.GetType() != typeof(int))
            {
                id = id + 1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = "!شماره صفحه باید به فرمت عدد باشد  "
                });
            }
            if (PageSize.GetType() != typeof(int))
            {
                id = id + 1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = "!شماره صفحه باید به فرمت عدد باشد  "
                });
            }
            if (validationErrors.Any())
            {
                return BadRequest(validationErrors);
            }
            else
            {
                var contents = _contentFacad.ContentTotalGetService.Execute(new RequestContentTotalGetDto
                {
                    PageNumber = PageNumber,
                    PageSize = PageSize,
                    SearchKey = search,
                    IsSort = sort
                });
                return Ok(contents);
            }
            
        }

        [HttpGet("GetContent")]
        [Authorize(Policy = "GetContent")]
        public IActionResult GetContent(string id)
        {
            var result = _contentFacad.ContentGetService.Execute(new RequestContentGetDto
            {
                uniqename = id,
            });
            if(result.Id == 0)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            } 
        }

        [HttpPost("RemoveContent")]
        [Authorize(Policy = "RemoveContent")]
        public IActionResult RemoveContent(RequestContentGetIdDto requestContentGetId)
        {
            return Ok(_contentFacad.ContentRemoveService.Execute(new RequestContentGetIdDto { id = requestContentGetId.id}));
        }

        [HttpPost("deleteContent")]
        // [Authorize(Policy = "RemoveContent")]
        public IActionResult deleteContent(RequestContentGetIdDto requestContentGetId)
        {
            var Result=_contentFacad.ContentRemoveService.delete(new RequestContentGetIdDto { id = requestContentGetId.id});
            return Result.IsSuccess ? Ok(Result) : BadRequest(Result);
        }


        [HttpPost("IndexContent")]
        //[Authorize(Policy = "IndexContent")]
        public IActionResult IndexContent(RequestById_s request)
        {
            return Ok(_contentFacad.ContentIndexService.Execute(new RequestById_s { id = request.id }));
        }


        [HttpPost("EditContent")]
        [Authorize(Policy = "EditContent")]
        public IActionResult EditContent(EditContentViewModel Request)
        {

            var result= _contentFacad.ContentEditService.Execute(new UpdateContentDto
            {
                ContentTitle = Request.title,
                ContentUniqeName = Request.url,
                CommentSituation = Request.commentable,
                CommentShow = Request.showComment,
                ContentSorting = Request.sort,
                ContentLongDescription = Request.main,
                ContentMetaDesc = Request.meta,
                ContentImageAlt = Request.imageDesc,
                ContentPublish = Request.published,
                ContentImage = Request.image,
                CategoryUniqeName = Request.category,
                Canonical=Request.canonical,
                Id=Request.id,
                IsIndex=Request.IsIndex,
                ContentImageTitle = Request.imageTitle,
                
            });

            if (result.IsSuccess == true)
            {
                return Ok(result);
            }
            else { return BadRequest(result); }
        }
        [HttpDelete]
        [Route("RemoveComment")]
        [Authorize(Policy = "RemoveComment")]
        public IActionResult RemoveComment(long Id)
        {
            var result= _contentFacad.CommentRemoveService.Execute(new RequestCommentGetIdDto { Id = Id });
            if (result.IsSuccess == true)
            {
                return Ok(result);
            }
            else { return BadRequest(result); }
        }

        [HttpDelete]
        [Route("RemoveSubComment")]
        [Authorize(Policy = "RemoveSubComment")]
        public IActionResult RemoveSubComment(long Id)
        {
            var result = _contentFacad.SubCommentRemoveService.Execute(new RequestSubCommentId { Id = Id });
            if (result.IsSuccess == true)
            {
                return Ok(result);
            }
            else { return BadRequest(result); }
        }


    }
}
