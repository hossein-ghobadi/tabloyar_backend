using Endpoint.Site.Models.ViewModels.Idea;
using Endpoint.Site.Models.ViewModels.Sample;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Radin.Application.Interfaces.FacadPatterns;
using Radin.Application.Services.Ideas.Commands.CommentSet;
using Radin.Application.Services.Ideas.FacadPattern;
using Radin.Application.Services.Samples.Commands.CommentSet;
using Radin.Application.Services.Samples.Queries.SampleGet;
using Radin.Domain.Entities.Users;
using Sprache;
using System.Security.Claims;
using static Radin.Application.Services.Samples.Commands.SampleRankSet.SampleRatingService;

namespace Endpoint.Site.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiSampleController : ControllerBase
    {

        private readonly ISampleFacad _sampleFacad;
        private readonly UserManager<User> _userManager;

        public ApiSampleController(

            ISampleFacad sampleFacad,
            UserManager<User> userManager
            )
        {

            _sampleFacad = sampleFacad;
            _userManager = userManager;

        }


        [HttpGet("SampleList")]
        public IActionResult SampleList([FromQuery] int PageNumber, int PageSize, string? category)
        {
            try
            {
                int skip = (PageNumber - 1) * PageSize;
                var InitialContentList = _sampleFacad.SampleGetService.SampleListInSamplePage(new RequestSampleListInSamplePageDto { SampleCategoryTitle = category }).Data.OrderByDescending(n => n.InsertTime).AsQueryable();
                var SampleList = InitialContentList.Skip(skip).Take(PageSize).ToList();
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
                var sampleDtos = new List<SampleListDto>();
                foreach (var sample in SampleList)
                {
                    sampleDtos.Add(new SampleListDto
                    {
                        SampleTitle = sample.SampleTitle,
                        SampleUniqName = sample.SampleUniqName,
                        SampleSorting = sample.SampleSorting,
                        MainImage = sample.MainImage,

                    });

                }


                //InitialConnectionTime = (long)(m.InitialConnectionTime - new DateTime(1970, 1, 1)).TotalMilliseconds, // Convert DateTime to milliseconds since Unix epoch
                //                WorkName = m.WorkName,
                //                TotalAmount = Convert.ToString(m.TotalAmount) ?? "قیمت نامعین", // Ensure TotalAmount has a default value if null
                //                PurchaseProbability = Convert.ToString(m.PurchaseProbability) ?? "احتمال نامعین",
                //                CustomerName = _context.CustomerInfo
                //                    .Where(c => c.CustomerID == m.CustomerID)
                //                    .Select(c => c.Name)
                //                    .FirstOrDefault() ?? "نام نامعین", // Get the first match or null if none exists



                //sampleDtos.OrderBy(n => n.SampleSorting);
                var Result = new { Count = Count, ContentDtos = sampleDtos };
                return Ok(Result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "خطا");
            }
        }


        [HttpGet("Sample")]
        public IActionResult HomeContentUniqe(string id)
        {
            Console.WriteLine(id);
            var Result = _sampleFacad.SampleGetService.SampleInSamplePage(new RequestSampleGetDto { uniqename = id });
            return Result.IsSuccess ? Ok(Result) : BadRequest(Result.Message);

        }

        [HttpGet("GetCategoryList")]

        public IActionResult GetCategoryForContent()
        {
            return Ok(_sampleFacad.SampleCategoryGetService.CategoryForAdminContentSet());
        }




        [Authorize]
        [HttpPost("SetComment")]
        public IActionResult SetComment(SetSampleCommentViewModel requestCommentSetDto)
        {
            var username = User.Identity.Name;
            var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userrole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            //var fullName = User.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;

            var result = _sampleFacad.SampleCommentSetService.Execute(new RequestSampleCommentSetDto
            {
                SampleId = requestCommentSetDto.SampleId,
                Name = username,
                Email = useremail,
                UserRole = userrole,
                CommentText = requestCommentSetDto.CommentText,
                Situation = requestCommentSetDto.Situation,


            });
            return Ok(result);
        }

        [Authorize]
        [HttpPost("SetSampleRank")]
        public async Task<IActionResult> SetSampleRank(SampleRankRequest request)
        {
            if (User.Identity.IsAuthenticated)
            {

                //var user = await _userManager.FindByIdAsync(request.userId);
                string userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = await _userManager.FindByEmailAsync(userEmail);
                if (user == null)
                {
                    return NotFound($"اطلاعات کاربر وجود ندارد '{_userManager.GetUserId(User)}'.");
                }
                var userId = user.Id;



                try
                {
                    await _sampleFacad.SampleRatingService.AddRatingAsync(new SampleRankRequest
                    {
                        sampleId = request.sampleId,
                        userId = userId,
                        starPoint = request.starPoint
                    });

                    return Ok("امتیاز دهی موفق");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("5");
                    return NotFound("نمونه کار مورد نظر پیدا نشد"); // If idea not found
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
        public IActionResult SetSubComment(SampleSubCommentSetViewModel requestSubCommentSetDto)
        {
            var username = User.Identity.Name;
            var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userrole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var result = _sampleFacad.SampleSubCommentSetService.Execute(new RequestSampleSubCommentSetDto
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
