using Endpoint.Site.Models.ViewModels.Comment;
using Endpoint.Site.Models.ViewModels.Content;
using Endpoint.Site.Models.ViewModels.Idea;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Protocol;
using Radin.Application.Interfaces.FacadPatterns;
using Radin.Application.Services.Contents.Commands.CommentSet;
using Radin.Application.Services.Contents.Commands.SubCommentSet;
using Radin.Application.Services.Contents.FacadPattern;
using Radin.Application.Services.Contents.Queries.HomeContentGet;
using Radin.Application.Services.Ideas.Commands.CommentSet;
using Radin.Application.Services.Ideas.Queries.IdeaGet;
using Radin.Domain.Entities.Users;
using Sprache;
using System.Security.Claims;
using static Radin.Application.Services.Ideas.Commands.IdeaRankSet.IdeaRatingService;

namespace Endpoint.Site.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiIdeaController : ControllerBase
    {
        private readonly IIdeaFacad _ideaFacad;
        private readonly UserManager<User> _userManager;

        public ApiIdeaController(

            IIdeaFacad ideaFacad,
            UserManager<User> userManager

            )
        {

            _ideaFacad = ideaFacad;
            _userManager = userManager;

        }


        [HttpGet("IdeaList")]
        public IActionResult IdeaList([FromQuery] int PageNumber, int PageSize, string? category)
        {
            try
            {
                int skip = (PageNumber - 1) * PageSize;
                var InitialContentList = _ideaFacad.IdeaGetService.IdeaListInIdeaPage(new RequestIdeaListInIdeaPageDto { IdeaCategoryTitle = category }).Data.OrderByDescending(n => n.InsertTime).AsQueryable();
                var IdeaList = InitialContentList.Skip(skip).Take(PageSize).ToList();
                int fraction = InitialContentList.Count() / PageSize;
                int res = InitialContentList.Count() % PageSize;
                int Count = 0;
                if (res == 0)
                {
                    Count = fraction;
                }
                else
                {
                    Count = fraction + 1;
                }
                var ideaDtos = new List<IdeaListDto>();
                foreach (var idea in IdeaList)
                {
                    ideaDtos.Add(new IdeaListDto
                    {
                        IdeaTitle = idea.IdeaTitle,
                        IdeaUniqName = idea.IdeaUniqName,
                        IdeaSorting = idea.IdeaSorting,
                        MainImage = idea.MainImage,

                    });

                }
                //ideaDtos.OrderBy(n => n.IdeaSorting);
                var Result = new { Count = Count, ContentDtos = ideaDtos };
                return Ok(Result);
            }
            
                catch (Exception ex)
            {
                return StatusCode(500, "خطا");
            }
        
        }


        [HttpGet("Idea")]
        public IActionResult HomeContentUniqe(string id)
        {
            var Result=_ideaFacad.IdeaGetService.IdeaInIdeaPage(new RequestIdeaGetDto { uniqename = id });
            return Result.IsSuccess ? Ok(Result) : BadRequest(Result.Message);

        }

        [HttpGet("GetCategoryList")]

        public IActionResult GetCategoryForContent()
        {
            return Ok(_ideaFacad.IdeaCategoryGetService.CategoryForAdminContentSet());
        }




        [Authorize]
        [HttpPost("SetComment")]
        public IActionResult SetComment(SetIdeaCommentViewModel requestCommentSetDto)
        {
            var username = User.Identity.Name;
            var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userrole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            //var fullName = User.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;

            var result = _ideaFacad.IdeaCommentSetService.Execute(new RequestIdeaCommentSetDto
            {
                IdeaId = requestCommentSetDto.IdeaId,
                Name = username,
                Email = useremail,
                UserRole = userrole,
                CommentText = requestCommentSetDto.CommentText,
                Situation = requestCommentSetDto.Situation,


            });
            return Ok(result);
        }



        [Authorize]
        [HttpPost("SetIdeaRank")]
        public async Task<IActionResult> SetIdeaRank(IdeaRankRequest request)
        {
            if (User.Identity.IsAuthenticated)
            {
                //var user = await _userManager.FindByIdAsync(request.userId);
                string userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user =  await _userManager.FindByEmailAsync(userEmail);

                if (user == null)
                {
                    return NotFound($"اطلاعات کاربر وجود ندارد '{_userManager.GetUserId(User)}'.");
                }
                var userId = user.Id;
                


                try
                {
                    await _ideaFacad.IdeaRatingService.AddRatingAsync(new IdeaRankRequest
                    {
                        ideaId = request.ideaId,
                        userId = userId,
                        starPoint = request.starPoint
                    });

                    return Ok("امتیاز دهی موفق");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("5");
                    return NotFound("ایده مورد نظر پیدا نشد"); // If idea not found
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("6");
                    return BadRequest("شما قبلا نظر خود را ثبت کرده اید "); // If user has already rated
                }
                catch (Exception)
                {
                    return StatusCode(500, "خطا");
                }
                

            }
            else
            {
                return Unauthorized("خطا در احراز هویت");
            }

        }


        [Authorize]
        [HttpPost("SetSubComment")]
        public IActionResult SetSubComment(IdeaSubCommentSetViewModel requestSubCommentSetDto)
        {
            var username = User.Identity.Name;
            var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userrole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var result = _ideaFacad.IdeaSubCommentSetService.Execute(new RequestIdeaSubCommentSetDto
            {
                CommentId = requestSubCommentSetDto.reply,
                Name = username,
                Email = useremail,
                UserRole = userrole,
                ReplyMsg = requestSubCommentSetDto.CommentText,


            });
            return Ok(result);
        }

    }
}
