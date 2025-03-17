using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Factors.Commands.Orders;
using Radin.Common.Dto;
using Radin.Domain.Entities.Customers;
using Radin.Domain.Entities.Users;
using System.Security.Claims;
using static Radin.Application.Services.Factors.Commands.Customer.CustomerService;

namespace Endpoint.Site.Areas.Proxy.Controllers
{
    [Route("Proxy/api/[controller]")]
    [ApiController]
    public class ApiTempController : ControllerBase
    {
        private readonly IDataBaseContext _context;
        private readonly UserManager<User> _userManager;

        public ApiTempController(IDataBaseContext context,

            UserManager<User> userManager


            ) {  _context = context;
            _userManager = userManager;


        }



        [HttpPost("ChangeUserBranchCode")]
        public async Task<IActionResult> ChangeUserBranchCode(BranchRequest11 request)
        {
            // Fetching the data grouped by state, ensuring state is between 0 and 4
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var currentUser = await _userManager.FindByEmailAsync(userEmail);
                if (currentUser == null || currentUser.BranchCode == 0)
                {
                    return BadRequest(new ResultDto { IsSuccess = false, Message = "شما عضو شعبه نیستید" });
                }
                var Branch = _context.BranchINFOs.FirstOrDefault(p => p.BranchCode == currentUser.BranchCode);
                if (Branch==null)
                {
                    return Ok(new ResultDto { IsSuccess = false, Message = "شعبه موجود نیست" });
                }
                currentUser.BranchCode=request.BranchCode;
                _context.SaveChanges();
                return Ok(new ResultDto { IsSuccess = true, Message = "کاربر به شعبه جدید منتقل شد" });
            }
            catch
            {
                // Log the exception (ex)w
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        [HttpPost("ChangeIframe")]
        public async Task<IActionResult> ChangeIframe(IframeRequest11 request)
        {
            // Fetching the data grouped by state, ensuring state is between 0 and 4
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var currentUser = await _userManager.FindByEmailAsync(userEmail);
                if (currentUser == null || currentUser.BranchCode == 0)
                {
                    return BadRequest(new ResultDto { IsSuccess = false, Message = "شما عضو شعبه نیستید" });
                }
                var Branch = _context.BranchINFOs.FirstOrDefault(p => p.BranchCode == currentUser.BranchCode);
                if (Branch == null)
                {
                    return Ok(new ResultDto { IsSuccess = false, Message = "شعبه موجود نیست" });
                }
                Branch.DashboardLink = request.Iframe;
                _context.SaveChanges();
                return Ok(new ResultDto { IsSuccess = true, Message = "کاربر به شعبه جدید منتقل شد" });
            }
            catch
            {
                // Log the exception (ex)w
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        [HttpPost("ChangeProbability")]
        public async Task<IActionResult> ChangeProbability(ProbRequest request)
        {
            // Fetching the data grouped by state, ensuring state is between 0 and 4
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var currentUser = await _userManager.FindByEmailAsync(userEmail);
                if (currentUser == null || currentUser.BranchCode == 0)
                {
                    return BadRequest(new ResultDto { IsSuccess = false, Message = "شما عضو شعبه نیستید" });
                }
                var Branch = _context.BranchINFOs.FirstOrDefault(p => p.BranchCode == currentUser.BranchCode);
                var factor = _context.MainFactors.FirstOrDefault(p => p.Id == request.FactorId);
                if (Branch == null)
                {
                    return Ok(new ResultDto { IsSuccess = false, Message = "شعبه موجود نیست" });
                }
                if (factor == null)
                {
                    return Ok(new ResultDto { IsSuccess = false, Message = "فاکتور موجود نیست" });
                }
                factor.PurchaseProbability = request.Probability;
                _context.SaveChanges();
                return Ok(new ResultDto { IsSuccess = true, Message = "کاربر به شعبه جدید منتقل شد" });
            }
            catch
            {
                // Log the exception (ex)w
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        [HttpGet("CustomerList")]
        public async Task<IActionResult> CustomerList(int number)
        {
            // Fetching the data grouped by state, ensuring state is between 0 and 4
            try
            {
                var CustomerList = _context.CustomerInfo.Select(p=>new CustomerData { Lastname=p.LastName,id=p.Id,CharacterType=_context.PersonalityCharacterType.First(m=>m.Id==p.CharacterType).Type}).Take(number).ToList();

                return Ok(new ResultDto<List<CustomerData>> { IsSuccess = true, Message = "دریافت موفق" ,Data= CustomerList });
            }
            catch
            {
                // Log the exception (ex)w
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }




        public class BranchRequest11
        {
            public long BranchCode { get; set;}
        }
        public class IframeRequest11
        {
            public string Iframe { get; set; }
        }
        public class ProbRequest
        {
            public int Probability { get; set; }
            public long FactorId { get; set; }
        }
        public class CustomerData
        {
            public long id { get; set; }
            public string Lastname { get; set; }
            public string CharacterType { get; set; }
        }
    }
}
