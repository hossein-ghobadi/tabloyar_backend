using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Radin.Application.Interfaces.FacadPatterns;
using Radin.Application.Services.Samples.Commands.SampleCategoryEdit;
using Radin.Application.Services.Samples.Commands.SampleCategorySet;
using Radin.Application.Services.Samples.Commands.SampleSet;
using Radin.Application.Services.Samples.Queries.SampleCategoryGet;
using Radin.Application.Services.Samples.Queries.SampleGet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Users;
using static Radin.Application.Services.Samples.Commands.SampleCategoryRemove.SampleCategoryRemoveService;
using System.Security.Claims;
using Endpoint.Site.Areas.Admin.Models.AdminViewModel.Idea;
using Radin.Application.Services.Samples.Commands.SampleEdit;
using static Radin.Application.Services.Ideas.Commands.IIdeaRemove.IdeaRemoveService;
using static Radin.Application.Services.Samples.Commands.SampleRemove.SampleRemoveService;
using static Radin.Application.Services.Ideas.Commands.IdeaCategoryRemove.IdeaCategoryRemoveService;
using Radin.Application.Services.Ideas.Commands.IdeaCategoryEdit;
using Radin.Application.Services.Ideas.FacadPattern;
using static Radin.Application.Services.Ideas.Commands.CommentRemove.IdeaCommentRemoveService;
using static Radin.Application.Services.Ideas.Commands.CommentRemove.IdeaSubCommentRemoveService;
using static Radin.Application.Services.Samples.Commands.CommentRemove.SampleCommentRemoveService;
using static Radin.Application.Services.Samples.Commands.CommentRemove.SampleSubCommentRemoveService;
using Microsoft.AspNetCore.Authorization;
using Radin.Common.Request;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class ApiSampleController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ISampleFacad _sampleFacad;
        public ApiSampleController(ISampleFacad sampleFacad,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _sampleFacad = sampleFacad;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }





        [HttpPost("SetSample")]
        [Authorize(Policy = "SetSample")]
        public IActionResult SetSample(SetIdeaViewModel Request)
        {

            if (User.Identity.IsAuthenticated)
            {
                string userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = _userManager.FindByEmailAsync(userEmail).Result;
                var result = _sampleFacad.SampleSetService.AdminSet(new RequestSampleSetDto
                {

                    SampleTitle = Request.title,
                    SampleOwnerName = user.Email,
                    SampleOwnerId = user.Id,

                    SampleUniqeName = Request.url,
                    CommentSituation = Request.commentable,
                    CommentShow = Request.showComment,
                    SampleSorting = Request.sort,
                    SampleLongDescription = Request.main,
                    SampleMetaDesc = Request.meta,
                    SampleImageAlt = Request.imageDesc,
                    SamplePublish = true,
                    SampleImages = Request.images,
                    MainImage = Request.mainImage,
                    SampleCategoryUniqeName = Request.category,
                    IsIndex =Request.IsIndex ?? true
                });

                if (result.IsSuccess == true)
                {
                    return Ok(result);
                }
                else { return BadRequest(result); }
            }
            else
            {
                return BadRequest("اطلاعات کاربر احراز  نشده است");
            }

        }




        [HttpGet("GetSample")]
        [Authorize(Policy = "GetSample")]
        public IActionResult GetSample(string id)
        {
            var result = _sampleFacad.SampleGetService.SingleSample(new RequestSampleGetDto
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


        [HttpGet("GetSamples")]
        [Authorize(Policy = "GetSamples")]
        public IActionResult GetSamples(int PageNumber, int PageSize, string? search, bool sort = false)
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
                var Samples = _sampleFacad.SampleGetService.SampleList(new RequestSampleListGetDto
                {

                    PageNumber = PageNumber,
                    PageSize = PageSize,
                    SearchKey = search,
                    IsSort = sort
                });
                return Ok(Samples);
            }

        }

        [HttpPost("EditSample")]
        [Authorize(Policy = "EditSample")]
        public IActionResult EditSample(EditIdeaViewModel Request)
        {

            var result = _sampleFacad.SampleEditService.Execute(new UpdateSampleDto
            {
                SampleTitle = Request.title,
                SampleUniqeName = Request.url,
                CommentSituation = Request.commentable,
                CommentShow = Request.showComment,
                SampleSorting = Request.sort,
                SampleLongDescription = Request.main,
                SampleMetaDesc = Request.meta,
                SampleImageAlt = Request.imageDesc,
                SamplePublish = Request.published,
                SampleImages = Request.images,
                MainImage = Request.mainImage,
                SampleCategoryUniqeName = Request.category,
                Id = Request.id,
                IsIndex=Request.IsIndex
            });

            if (result.IsSuccess == true)
            {
                return Ok(result);
            }
            else { return BadRequest(result); }
        }




        [HttpPost("RemoveSample")]
        [Authorize(Policy = "RemoveSample")]
        public IActionResult RemoveSample(RequestIdeaRemoveDto request)
        {
            return Ok(_sampleFacad.SampleRemoveService.Execute(new RequestSampleRemoveDto
            {
                id = request.id
            }));
        }


        [HttpPost("deleteSample")]
        // [Authorize(Policy = "deleteSample")]
        public IActionResult deleteSample(RequestIdeaRemoveDto request)
        {
            var Result=_sampleFacad.SampleRemoveService.delete(new RequestSampleRemoveDto
            {
                id = request.id
            });
            return Result.IsSuccess ? Ok(Result) : BadRequest(Result);

        }


        [HttpPost("SampleCategorySet")]
        [Authorize(Policy = "SampleCategorySet")]
        public IActionResult SampleCategorySet(IdeaCategorySetViewModel Request)
        {
            var validationErrors = Request.Validate();

            if (validationErrors.Any())
            {
                return BadRequest(validationErrors);
            }
            else
            {
                var result = _sampleFacad.SampleCategorySetService.AdminSet(new RequestSampleCategorySetDto
                {
                    SampleCategoryTitle = Request.CategoryTitle,
                    SampleCategoryUniqeName = Request.CategoryUniqeName,
                    SampleCategorySorting = Request.CategorySorting,
                    //CategoryStyle = Request.CategoryStyle,
                    //CategoryIsShowMain = Request.CategoryIsShowMain,
                    SampleCategoryIsShowMenu = Request.CategoryIsShowMenu,
                    SampleCategoryDescription = Request.CategoryDescription,

                });
                if (result.IsSuccess == true)
                {
                    return Ok(result);
                }
                else { return BadRequest(result); }
            }
        }



        [HttpGet("GetCategoryForAdminContentSet")]
        [Authorize(Policy = "GetCategoryForAdminContentSet")]
        public IActionResult GetCategoryForAdminContentSet()
        {
            return Ok(_sampleFacad.SampleCategoryGetService.CategoryForAdminContentSet());
        }


        [HttpGet("CategorySingleGetForAdmin")]
        [Authorize(Policy = "CategorySingleGetForAdmin")]
        public IActionResult CategorySingleGetForAdmin(string uniqname)
        {
            var uniqval = uniqname.ToString();
            var Result = _sampleFacad.SampleCategoryGetService.CategorySingleGetForAdmin(new RequestSampleCategoryGetDto
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
                var CategoryInfo = _sampleFacad.SampleCategoryGetService.CategoryListGetForAdmin(new RequestSampleCategoryListDto
                {
                    PageNumber = PageNumber,
                    SearchKey = search,
                    PageSize = PageSize,
                    IsSort = sort
                });
                return Ok(CategoryInfo);
            }
        }


        [HttpPost("RemoveSampleCategory")]
        [Authorize(Policy = "RemoveSampleCategory")]
        public IActionResult RemoveSampleCategory(RequestIdeaCategoryDto requestCategoryGetuniqname)
        {
            var result = _sampleFacad.SampleCategoryRemoveService.Execute(new RequestSampleCategoryDto { id = requestCategoryGetuniqname.id });

            if (result.IsSuccess == true)
            {
                return Ok(result);
            }
            else { return BadRequest(result); }

        }


        [HttpPost("EditSampleCategory")]
        [Authorize(Policy = "EditSampleCategory")]
        public IActionResult EditSampleCategory(EditIdeaCategoryDto request)
        {
            var result = _sampleFacad.SampleCategoryEditService.Execute(new EditSampleCategoryDto
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
        public IActionResult RemoveComment(RequestSampleCommentGetIdDto requestCommentGetId)
        {
            var result = _sampleFacad.SampleCommentRemoveService.Execute(new RequestSampleCommentGetIdDto { Id = requestCommentGetId.Id });
            if (result.IsSuccess == true)
            {
                return Ok(result);
            }
            else { return BadRequest(result); }
        }

        [HttpPost("IndexSample")]
        //[Authorize(Policy = "IndexSample")]
        public IActionResult IndexSample(RequestById_s request)
        {
            return Ok(_sampleFacad.SampleIndexService.Execute(new RequestById_s { id = request.id }));
        }

        [HttpDelete]
        [Route("RemoveSubComment")]
        [Authorize(Policy = "RemoveComment")]
        public IActionResult RemoveSubComment(RequestSampleSubCommentId request)
        {
            var result = _sampleFacad.SampleSubCommentRemoveService.Execute(new RequestSampleSubCommentId { Id = request.Id });
            if (result.IsSuccess == true)
            {
                return Ok(result);
            }
            else { return BadRequest(result); }
        }
    }
}
