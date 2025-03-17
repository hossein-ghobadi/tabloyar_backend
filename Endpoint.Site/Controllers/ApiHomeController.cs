using Endpoint.Site.Areas.Proxy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Interfaces.FacadPatterns;
using Radin.Application.Services.Branch.Commands.BranchRegisterService;
using Radin.Application.Services.Branch.Queries.BranchInfoGetService;
using Radin.Application.Services.ContactUs.Commands.ContactMessageSet;
using Radin.Application.Services.Contents.Queries.CategoryGet;
using Radin.Application.Services.Contents.Queries.HomeContentGet;
using Radin.Application.Services.Contents.Queries.HomePageContentGet;
using Radin.Application.Services.HomePage.Queries.HomePageSlider;
using Radin.Application.Services.SMS.Commands;
using Radin.Common.Dto;
using Radin.Domain.Entities.Branches;
using Sprache;
using static Radin.Application.Services.Branch.Queries.BranchInfoGetService.BranchInfoGetService;
using static Radin.Application.Services.ContactUs.Commands.ContactMessageSet.ContactMessageSet;

namespace Endpoint.Site.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiHomeController : ControllerBase
    {
        private readonly IContentFacad _contentFacad;
        private readonly IIdeaFacad _ideaFacad;
        private readonly ISampleFacad _sampleFacad;

        private readonly IBranchUniqeGetService _branchUniqeGetService;
        private readonly IBranchInfoGetService _branchInfoGetService;
        private readonly IDataBaseContext _context;
        private readonly IHomePageSliderGet _homePageSliderGet;
        //private readonly IHomePageContentGetService _homePageContentGetService;
        private readonly IContactMessageSet _contactMessageSet;
        private readonly IBranchRegisterService _branchRegisterService;
        private readonly ISMSSendService _smsSendService;
        private readonly ISMSCheckService _SMSCheckService;
        public ApiHomeController(
         IHomePageSliderGet homePageSliderGet,
         //IHomePageContentGetService homePageContentGetService,
         IContactMessageSet contactMessageSet,
         IContentFacad contentFacad,
         IIdeaFacad ideaFacad,
         ISampleFacad sampleFacad,
         IBranchUniqeGetService branchUniqeGetService,
         IBranchInfoGetService branchInfoGetService,
         IDataBaseContext context,
         IBranchRegisterService branchRegisterService,
         ISMSSendService sMSSendService,
         ISMSCheckService sMSCheckService

         )
        {
            _homePageSliderGet = homePageSliderGet;
            //_homePageContentGetService = homePageContentGetService;
            _contactMessageSet = contactMessageSet;
            _contentFacad = contentFacad;
            _ideaFacad = ideaFacad;
            _branchUniqeGetService = branchUniqeGetService;
            _branchInfoGetService = branchInfoGetService;
            _context = context;
            _branchRegisterService = branchRegisterService;
            _smsSendService = sMSSendService;
             _SMSCheckService = sMSCheckService;
            _sampleFacad = sampleFacad;
        }

        [HttpGet("HomePageContent")]
        public IActionResult HomePageContent()
        {
            var Content = _contentFacad.HomePageContentGetService.Execute();
            return Ok(Content);

        }

        [HttpGet("HomeSlider")]
        public IActionResult HomeSlider()
        {
            var SliderData = _homePageSliderGet.MainPageGet().Data.OrderBy(n => n.Sorting).Take(5);
            return Ok(SliderData);

        }

        [HttpGet("HomeIdeaSlider")]
        public IActionResult HomeIdeaSlider()
        {
            //var IdeaSliderData = _ideaFacad.IdeaGetService.IdeaSliderInHomePage().Data.Take(10);//.OrderBy(n => n.IdeaSorting)
            var SampleSliderData = _sampleFacad.SampleGetService.SampleSliderInHomePage().Data.Take(10);//.OrderBy(n => n.IdeaSorting)
            return Ok(SampleSliderData);

        }
        [HttpPost("ContactMessageSet")]
        public IActionResult ContactMessageSet(MessageDto request)
        {
            var Message = _contactMessageSet.Execute(new MessageDto
            {
                Subject = request.Subject,
                Name = request.Name,
                Description = request.Description,
                Department = request.Department,
                Phone = request.Phone,
                Email = request.Email,
            }
                );
            if (Message.IsSuccess == true)
            {
                return Ok(Message);

            }
            else { return BadRequest(Message); }

        }




        [HttpGet("ContactMessageList")]
        public IActionResult ContactMessageList()
        {
            try
            {
                var MessageList = _context.ContactMessages.ToList();
               
                    return Ok(MessageList);

               
            }
            catch { return StatusCode(500, "An error occurred while processing your request."); }
        }



        [HttpGet("BranchRegistersList")]
        public IActionResult BranchRegistersList()
        {
            try
            {
                var MessageList = _context.BranchRegisters.ToList();
                if (MessageList.Count > 0)
                {
                    return Ok(MessageList);
                }
                return BadRequest(" درخواستی برای نمایندگی وجود ندارد"); 

            }
            catch { return StatusCode(500, "An error occurred while processing your request."); }
        }



        [HttpGet]
        [Route("BranchDetails")]
        public IActionResult BranchDetails(long code)
        {
            try
            {
                var res = _branchUniqeGetService.GetBranchInfosInBranchPage(new RequestBranchUniqeGetDto
                {
                    BranchCode = code
                });
                if (res.IsSuccess)
                {
                    return Ok(res.Data);
                }
                return BadRequest(res);
            }
            catch
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        [Route("BranchList")]
        public IActionResult BranchList( string? search)
        {
            try
            {
                
                   
                    var res = _branchInfoGetService.GetBranchsInBranchPage(search);
                    return res.IsSuccess ? Ok(res) : BadRequest(res);
                //}
            }
            catch
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        //[HttpPost]
        //[Route("BranchRegister")]
        //public IActionResult BranchRegister(BranchRegisterModel request)
        //{
        //    try
        //    {


        //       var Result=_branchRegisterService.Register(request);
        //        return Result.IsSuccess ? Ok(Result) : BadRequest(Result);
        //        //}
        //    }
        //    catch
        //    {
        //        // Log the exception (ex)
        //        return StatusCode(500, "An error occurred while processing your request.");
        //    }
        //}




        [HttpPost]
        [Route("BranchRegister1")]
        public IActionResult ProxyRegister1(Phone Number)
        {
            try
            {
                int id = 0;
                var validationErrors = new List<IdLabelDto>();                //var validationErrors = model.Validate();
                var duplicates = _context.BranchRegisters.Where(u => u.phone == Number.phoneNumber).ToList();
                int phoneDigits = Number.phoneNumber.Length;
                if (duplicates.Count >0)
                {
                    return BadRequest("شماره تماس قبلا ثبت شده است");
                }
                if (phoneDigits != 11)
                {
                    id = id + 1;
                    validationErrors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!تعدا ارقام شماره تماس باید 11 رقم باشد"
                    });
                }

                bool containsOnlyDigits = Number.phoneNumber.All(char.IsDigit);
                if (!containsOnlyDigits)
                {
                    id = id + 1;
                    validationErrors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "! شماره تماس باید شامل اعداد باشد"
                    });
                }

                if (validationErrors.Any())
                {
                    return BadRequest(validationErrors);

                }

                var res = _smsSendService.Send(new RequestSMSSentDto { PhoneNumber = Number.phoneNumber });
                if (res.IsSuccess == false)
                {
                    return BadRequest("خطای داخلی سرویسِ!‍ مجدد تلاش کنید");
                }
                if (res.Data == false)
                {
                    return BadRequest(res.Message);
                }

                return Ok("کد تایید ارسال شد");
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        [Route("BranchRegister2")]

        public IActionResult ProxyRegister2(BranchRegisterModel ViewModel)
        {
            try
            {
                var validationErrors = ViewModel.Validate();
                var res = _SMSCheckService.Check(new RequestSMSCheckDto { PhoneNumber = ViewModel.phone, Code = ViewModel.verifyCode });
                if (validationErrors.Any())
                {
                    return BadRequest(validationErrors);

                }
                if (res.IsSuccess == false)
                {
                    return BadRequest("خطا در ثبت نام");
                }
                

                if (res.Data)
                {
                    var Result = _branchRegisterService.Register(ViewModel);

                    return Result.IsSuccess ? Ok(Result) : BadRequest(Result);
                }
                return BadRequest(res.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }





    }
}
