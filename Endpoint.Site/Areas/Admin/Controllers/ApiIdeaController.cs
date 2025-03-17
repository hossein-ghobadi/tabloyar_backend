using Endpoint.Site.Areas.Admin.Models.AdminViewModel.Content;
using Endpoint.Site.Areas.Admin.Models.AdminViewModel.Idea;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Radin.Application.Interfaces.FacadPatterns;
using Radin.Application.Services.Contents.Commands.ContentCategoryEdit;
using Radin.Application.Services.Contents.Commands.ContentCategorySet;
using Radin.Application.Services.Contents.Commands.ContentEdit;
using Radin.Application.Services.Contents.Commands.ContentSet;
using Radin.Application.Services.Contents.FacadPattern;
using Radin.Application.Services.Contents.Queries.ContentCategoryGet;
using Radin.Application.Services.Contents.Queries.ContentGet;
using Radin.Application.Services.Ideas.Commands.IdeaCategoryEdit;
using Radin.Application.Services.Ideas.Commands.IdeaCategorySet;
using Radin.Application.Services.Ideas.Commands.IdeaSet;
using Radin.Application.Services.Ideas.Commands.IIdeaEdit;
using Radin.Application.Services.Ideas.FacadPattern;
using Radin.Application.Services.Ideas.Queries.IdeaCategoryGet;
using Radin.Application.Services.Ideas.Queries.IdeaGet;
using Radin.Common.Dto;
using Radin.Common.Request;
using Radin.Domain.Entities.Users;
using System.Security.Claims;
using static Radin.Application.Services.Contents.Commands.CommentRemove.CommentRemoveService;
using static Radin.Application.Services.Contents.Commands.CommentRemove.SubCommentRemoveService;
using static Radin.Application.Services.Contents.Commands.ContentCategoryRemove.ContentCategoryRemoveService;
using static Radin.Application.Services.Contents.Commands.ContentRemove.ContentRemoveService;
using static Radin.Application.Services.Ideas.Commands.CommentRemove.IdeaCommentRemoveService;
using static Radin.Application.Services.Ideas.Commands.CommentRemove.IdeaSubCommentRemoveService;
using static Radin.Application.Services.Ideas.Commands.IdeaCategoryRemove.IdeaCategoryRemoveService;
using static Radin.Application.Services.Ideas.Commands.IIdeaRemove.IdeaRemoveService;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class ApiIdeaController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IIdeaFacad _ideaFacad;
        public ApiIdeaController(IIdeaFacad ideaFacad,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _ideaFacad = ideaFacad;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        




        [HttpPost("SetIdea")]
        [Authorize(Policy = "SetIdea")]
        public IActionResult SetIdea(SetIdeaViewModel Request)
        {

            //if (User.Identity.IsAuthenticated)
            //{
                string userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = _userManager.FindByEmailAsync(userEmail).Result;
                var result = _ideaFacad.IdeaSetService.AdminSet(new RequestIdeaSetDto
                {

                    IdeaTitle = Request.title,
                    IdeaOwnerName =user.Email,
                    IdeaOwnerId = user.Id,

                    IdeaUniqeName = Request.url,
                    CommentSituation = Request.commentable,
                    CommentShow = Request.showComment,
                    IdeaSorting = Request.sort,
                    IdeaLongDescription = Request.main,
                    IdeaMetaDesc = Request.meta,
                    IdeaImageAlt = Request.imageDesc,
                    IdeaPublish = true,
                    IdeaImages = Request.images,
                    MainImage=Request.mainImage,
                    IdeaCategoryUniqeName = Request.category,
                    IsIndex=Request.IsIndex ?? true
                });

                if (result.IsSuccess == true)
                {
                    return Ok(result);
                }
                else { return BadRequest(result); }
            //}
            //else
            //{
            //    return BadRequest("اطلاعات کاربر احراز  نشده است");
            //}
            
        }




        [HttpGet("GetIdea")]
        [Authorize(Policy = "GetIdea")]
        public IActionResult GetIdea(string id)
        {
            var result = _ideaFacad.IdeaGetService.SingleIdea(new RequestIdeaGetDto
            {
                uniqename = id,
            });
            if (result.Id == 0)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }


        [HttpGet("GetIdeas")]
        [Authorize(Policy = "GetIdeas")]
        public IActionResult GetIdeas(int PageNumber, int PageSize, string? search, bool sort = false)
        {
            var validationErrors = new List<IdLabelDto>();
            int id = 0;
            if (string.IsNullOrEmpty(PageNumber.ToString()))
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
                var Ideas = _ideaFacad.IdeaGetService.IdeaList(new RequestIdeaListGetDto
                {
                    
                    PageNumber = PageNumber,
                    PageSize = PageSize,
                    SearchKey = search,
                    IsSort = sort
                });
                return Ok(Ideas);
            }

        }

        [HttpPost("EditIdea")]
        [Authorize(Policy = "EditIdea")]
        public IActionResult EditIdea(EditIdeaViewModel Request)
        {

            var result = _ideaFacad.IdeaEditService.Execute(new UpdateIdeaDto
            {
                IdeaTitle = Request.title,
                IdeaUniqeName = Request.url,
                CommentSituation = Request.commentable,
                CommentShow = Request.showComment,
                IdeaSorting = Request.sort,
                IdeaLongDescription = Request.main,
                IdeaMetaDesc = Request.meta,
                IdeaImageAlt = Request.imageDesc,
                IdeaPublish = Request.published,
                IdeaImages = Request.images,
                MainImage=Request.mainImage,
                IdeaCategoryUniqeName = Request.category,
                Id = Request.id,
                IsIndex=Request.IsIndex

            });

            if (result.IsSuccess == true)
            {
                return Ok(result);
            }
            else { return BadRequest(result); }
        }




        [HttpPost("RemoveIdea")]
        [Authorize(Policy = "RemoveIdea")]
        public IActionResult RemoveIdea(RequestIdeaRemoveDto request)
        {
            return Ok(_ideaFacad.IdeaRemoveService.Execute(new RequestIdeaRemoveDto
            {
                id = request.id
            }));
        }
        //
        [HttpPost("deleteIdea")]
        // [Authorize(Policy = "RemoveIdea")]
        public IActionResult deleteIdea(RequestIdeaRemoveDto request)
        {
            var Result=_ideaFacad.IdeaRemoveService.delete(new RequestIdeaRemoveDto
            {
                id = request.id
            });
            return Result.IsSuccess ? Ok(Result) : BadRequest(Result);

        }



        [HttpPost("IndexIdea")]
        //[Authorize(Policy = "IndexIdea")]
        public IActionResult IndexIdea(RequestById_s request)
        {
            return Ok(_ideaFacad.IdeaIndexService.Execute(new RequestById_s { id = request.id }));
        }

        [HttpPost("IdeaCategorySet")]
        [Authorize(Policy = "IdeaCategorySet")]
        public IActionResult IdeaCategorySet(IdeaCategorySetViewModel Request)
        {
            var validationErrors = Request.Validate();

            if (validationErrors.Any())
            {
                return BadRequest(validationErrors);
            }
            else
            {
                var result = _ideaFacad.IdeaCategorySetService.AdminSet(new RequestIdeaCategorySetDto
                {
                    IdeaCategoryTitle = Request.CategoryTitle,
                    IdeaCategoryUniqeName = Request.CategoryUniqeName,
                    IdeaCategorySorting = Request.CategorySorting,
                    //CategoryStyle = Request.CategoryStyle,
                    //CategoryIsShowMain = Request.CategoryIsShowMain,
                    IdeaCategoryIsShowMenu = Request.CategoryIsShowMenu,
                    IdeaCategoryDescription = Request.CategoryDescription,

                });
                if (result.IsSuccess == true)
                {
                    return Ok(result);
                }
                else { return BadRequest(result); }
            }
        }

        [HttpGet("GetCategoryForAdminContentSet")]
        public IActionResult GetCategoryForAdminContentSet()
        {
            return Ok(_ideaFacad.IdeaCategoryGetService.CategoryForAdminContentSet());
        }


        [HttpGet("CategorySingleGetForAdmin")]
        [Authorize(Policy = "CategorySingleGetForAdmin")]
        public IActionResult CategorySingleGetForAdmin(string uniqname)
        {
            var uniqval = uniqname.ToString();
            var Result = _ideaFacad.IdeaCategoryGetService.CategorySingleGetForAdmin(new RequestIdeaCategoryGetDto
            {
                uniqename = uniqval,
            });

            if (Result.IsSuccess == true)
            {
                return Ok(Result.Data);
            }
            else
            {
                return BadRequest(Result.IsSuccess);
            }
        }


        [HttpGet("CategoryListGetForAdmin")]
        [Authorize(Policy = "CategoryListGetForAdmin")]
        public IActionResult CategoryListGetForAdmin(int PageNumber, int PageSize, string? search, bool sort = false)
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
                var CategoryInfo = _ideaFacad.IdeaCategoryGetService.CategoryListGetForAdmin(new RequestIdeaCategoryListDto
                {
                    PageNumber = PageNumber,
                    SearchKey = search,
                    PageSize = PageSize,
                    IsSort = sort
                });
                return Ok(CategoryInfo);
            }
        }


        [HttpPost("RemoveIdeaCategory")]
        [Authorize(Policy = "RemoveIdeaCategory")]
        public IActionResult RemoveIdeaCategory(RequestIdeaCategoryDto requestCategoryGetuniqname)
        {
            var result = _ideaFacad.IdeaCategoryRemoveService.Execute(new RequestIdeaCategoryDto { id = requestCategoryGetuniqname.id });

            if (result.IsSuccess == true)
            {
                return Ok(result);
            }
            else { return BadRequest(result); }

        }


        [HttpPost("EditIdeaCategory")]
        [Authorize(Policy = "EditIdeaCategory")]
        public IActionResult EditCategory(EditIdeaCategoryDto request)
        {
            var result = _ideaFacad.IdeaCategoryEditService.Execute(new EditIdeaCategoryDto
            {
                Id = request.Id,
                CategoryTitle = request.CategoryTitle,
                CategoryUniqeName = request.CategoryUniqeName,
                CategorySorting = request.CategorySorting,
                CategoryDescription = request.CategoryDescription,
                CategoryIsShowMenu = request.CategoryIsShowMenu,
                isRemoved = request.isRemoved
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
        public IActionResult RemoveComment(RequestIdeaCommentGetIdDto requestCommentGetId)
        {
            var result = _ideaFacad.IdeaCommentRemoveService.Execute(new RequestIdeaCommentGetIdDto { Id = requestCommentGetId.Id });
            if (result.IsSuccess == true)
            {
                return Ok(result);
            }
            else { return BadRequest(result); }
        }


        [HttpDelete]
        [Route("RemoveSubComment")]
        [Authorize(Policy = "RemoveSubComment")]
        public IActionResult RemoveSubComment(RequestIdeaSubCommentId request)
        {
            var result = _ideaFacad.IdeaSubCommentRemoveService.Execute(new RequestIdeaSubCommentId { Id = request.Id });
            if (result.IsSuccess == true)
            {
                return Ok(result);
            }
            else { return BadRequest(result); }
        }

    }
}
