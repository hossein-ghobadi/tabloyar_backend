using Endpoint.Site.Areas.Admin.Models.AdminViewModel.User;
using Endpoint.Site.Areas.Proxy.Models;
using Endpoint.Site.Models.ViewModels.CharacterTypeCalculationModels;
using Endpoint.Site.Models.ViewModels.Register;
using Endpoint.Site.Models.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Branch.Commands.BranchInfoSetService;
using Radin.Application.Services.CRM.Commands.UpdateExpiration;
using Radin.Application.Services.Email.Commands;
using Radin.Application.Services.Factors.Commands.Orders;
using Radin.Application.Services.OKR.Queries.OkrGetService;
using Radin.Application.Services.SMS.Commands;
using Radin.Common;
using Radin.Common.Dto;
using Radin.Domain.Entities.Branches;
using Radin.Domain.Entities.Users;
using Sprache;
using System.Security.Claims;

using Radin.Common.StaticClass;

using static Radin.Application.Services.CRM.Commands.UpdateExpiration.ExpirationService;
using CsvHelper;
using System.Globalization;
using Radin.Application.Services.OKR.Commands.TargetDeterminationSet;
using Radin.Application.Services.OKR.Queries.TargetDeterminationGet;

namespace Endpoint.Site.Areas.Proxy.Controllers
{
    [Authorize(Roles = "PROXY,PROXYSELLER")]
    [Route("Proxy/api/[controller]")]
    [ApiController]
    public class ApiBranchController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSendService _emailSendService;
        private readonly ISMSSendService _smsService;
        private readonly ISMSCheckService _SMSCheckService;
        private readonly IOkrGetService _okrGetService;
        private readonly IExpirationService _expirationService;
        private readonly ITargetDeterminationSetService _targetDeterminationSetService;
        private readonly ITargetDeterminationGetService _targetDeterminationGetService;
        private readonly IDataBaseContext _context;
        public ApiBranchController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor httpContextAccessor,
            ISMSSendService sMSSendService,
            ISMSCheckService sMSCheckService,
            IOkrGetService okrGetService,
            IExpirationService expirationService,
            IDataBaseContext context,
            ITargetDeterminationSetService targetDeterminationSetService,
            ITargetDeterminationGetService targetDeterminationGetService

            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _smsService = sMSSendService;
            _SMSCheckService = sMSCheckService;
            _context = context;
            _okrGetService = okrGetService;
            _expirationService = expirationService;
            _targetDeterminationSetService = targetDeterminationSetService;
            _targetDeterminationGetService = targetDeterminationGetService;
        }

        [HttpGet("Statistical_Analysis_Report")]
        public IActionResult Statistical_Analysis_Report()
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    string userEmail = User.FindFirstValue(ClaimTypes.Email);
                    var user = _userManager.FindByEmailAsync(userEmail).Result;
                    if (user.BranchCode == 0)
                    {
                        return BadRequest(new ResultDto { IsSuccess = false, Message = "شما عضو شعبه نیستید" });
                    }
                    var Link=_context.BranchINFOs.Where(p=>p.BranchCode==user.BranchCode).FirstOrDefault();
                    if (Link==null)
                    {
                        return BadRequest( new ResultDto { IsSuccess = false, Message = "چنین شعبه ای وجود ندارد" });
                    }
                    if (Link.DashboardLink == null)
                    {
                        return BadRequest(new ResultDto { IsSuccess = false, Message = "امکان اتصال به داشبورد آماری وجود ندارد" });
                    }
                    return Ok(new ResultDto<string> {Data=Link.DashboardLink, IsSuccess = true, Message = "دریافت موفق" });
                }
                catch
                {
                    // Log the exception (ex)
                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "An error occurred while processing your request." });

                }
            }
            else
            {
                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
            }
        }

        [HttpPost("ExpireTimeAddDays")]
        public IActionResult UpdateExpireTime(ExpireRequest request)
        {
            // Fetching the data grouped by state, ensuring state is between 0 and 4
            try
            {
                var result = _expirationService.UpdateExpireTimeWithDaysInput(request);

                return result.IsSuccess ? Ok(result.Message) : BadRequest(result);

            }
            catch
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        [HttpGet("BranchInformations")]
        public async Task<IActionResult> BranchInformations()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
            }

            try
            {
                // Get the current user
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var currentUser = await _userManager.FindByEmailAsync(userEmail);
                if (currentUser == null || currentUser.BranchCode == 0)
                {
                    return BadRequest(new ResultDto { IsSuccess = false, Message = "شما عضو شعبه نیستید" });
                }

                // Fetch roles containing 'Proxy' using EF.Functions.Like for SQL translation
                var proxyRoles = await _roleManager.Roles
                    .Where(r => EF.Functions.Like(r.Name, "%Proxy%"))
                    .ToListAsync();

                if (!proxyRoles.Any())
                {
                    return NotFound(new ResultDto { IsSuccess = false, Message = "No roles found containing 'Proxy'." });
                }

                // Fetch users for each role and filter by branch code
                var usersInProxyRoles = new List<BranchTemp>();
                foreach (var role in proxyRoles)
                {
                    var users = await _userManager.GetUsersInRoleAsync(role.Name);

                    // Filter users by branch code and avoid duplicates
                    foreach (var user in users.Where(u => u.BranchCode == currentUser.BranchCode))
                    {
                        var existingUser = usersInProxyRoles.FirstOrDefault(u => u.Id == user.Id);
                        if (existingUser != null)
                        {
                            // Add role to existing user
                            if (!existingUser.Roles.Contains(role.Name))
                            {
                                existingUser.Roles.Add(role.Name);
                            }
                        }
                        else
                        {
                            // Add new user entry
                            usersInProxyRoles.Add(new BranchTemp
                            {
                                Id = user.Id,
                                Email = user.Email,
                                FullName=user.FullName,
                                PhoneNumber = user.PhoneNumber,
                                Roles = new List<string> { role.Name }
                            });
                        }
                    }
                }

                if (!usersInProxyRoles.Any())
                {
                    return NotFound(new ResultDto { IsSuccess = false, Message = "No users found in 'Proxy' roles." });
                }
                var finalUsers = usersInProxyRoles.Select(user => new BranchInfos
                {
                    Id=user.Id,
                    Name = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    WorkRole = user.Roles.Contains("Proxy", StringComparer.OrdinalIgnoreCase)
                ? "مدیر شعبه"
                : "فروشنده"
                }).ToList();


                return Ok(new ResultDto<List<BranchInfos>>
                {
                    IsSuccess = true,
                    Message = "Users in 'Proxy' roles retrieved successfully.",
                    Data = finalUsers
                });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new ResultDto { IsSuccess = false, Message = "An error occurred while processing your request." });
            }
        }
        private class BranchTemp
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string FullName { get; set; }
            public string PhoneNumber { get; set; }
            public List<string> Roles { get; set; }
            public string WorkRole { get; set; }
        }
        private class BranchInfos
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; } 
            public string WorkRole { get; set; }
        }



        [HttpGet("OKR")]
        public IActionResult OKR()
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    string userEmail = User.FindFirstValue(ClaimTypes.Email);
                    var user = _userManager.FindByEmailAsync(userEmail).Result;
                    if (user.BranchCode == 0)
                    {
                        return BadRequest(new ResultDto { IsSuccess = false, Message = "شما عضو شعبه نیستید" });
                    }
                    var Result = _okrGetService.Test(Convert.ToInt32(user.BranchCode));
                  
                    
                    
                    return Result.IsSuccess ? Ok(Result) : BadRequest(Result);
                }
                catch
                {
                    // Log the exception (ex)
                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "An error occurred while processing your request." });

                }
            }
            else
            {
                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
            }
    }



        [HttpPost]
        [Route("ProxyRegister1")]
        public IActionResult ProxyRegister1(ProxyRegisterViewModel model)
        {
            try
            {
                var validationErrors = new List<IdLabelDto>();
                int phoneDigits = model.phone.Length;
                int id = 0;

                if (phoneDigits != 11)
                {
                    id = id + 1;
                    validationErrors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!تعدا ارقام شماره تماس باید 11 رقم باشد"
                    });
                }

                bool containsOnlyDigits = model.phone.All(char.IsDigit);
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
                var userID = _userManager.Users.FirstOrDefault(p => p.PhoneNumber == model.phone);
                if (userID == null) return BadRequest("کاربری با این شماره تماس یافت نشد");
                var res = _smsService.Send(new RequestSMSSentDto { PhoneNumber = model.phone });
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
        [Route("ProxyRegister2")]

        public async Task<IActionResult> ProxyRegister2(Step2ProxyRegisterViewModel ViewModel)
        {
            try
            {
                string userEmail = User.FindFirstValue(ClaimTypes.Email);
                var Manageruser = _userManager.FindByEmailAsync(userEmail).Result;
                if (Manageruser.BranchCode == 0)
                {
                    return BadRequest(new ResultDto { IsSuccess = false, Message = "شما عضو شعبه نیستید" });
                }
                var res = _SMSCheckService.Check(new RequestSMSCheckDto { PhoneNumber = ViewModel.phone, Code = ViewModel.verifyCode });
                if (res.IsSuccess == false)
                {
                    return BadRequest("ثبت نام انجام نشد؛ لطفا مجددا تلاش نمایید");
                }

                var validationErrors = new List<IdLabelDto>();
                if (res.Data)
                {
                    var userID = _userManager.Users.FirstOrDefault(p => p.PhoneNumber == ViewModel.phone);
                    if (userID == null) return BadRequest("کاربری با این شماره تماس یافت نشد");
                    var user = _userManager.FindByIdAsync(userID.Id).Result;

                    var result = _userManager.AddToRoleAsync(user, "PROXYSELLER").Result;
                    user.BranchCode = Manageruser.BranchCode;
                    var updateResult = await _userManager.UpdateAsync(user);

                    if (!result.Succeeded || !updateResult.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Concat(updateResult.Errors).Select(e => e.Description));
                        return BadRequest($"Failed to process user registration: {errors}");
                    }
                    return Ok("همکار جدید شما با موفقیت ثبت شد");
                }
                return BadRequest(res.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
            
        }





        [HttpGet]
        [Route("GetNotificationsList")]
        public IActionResult GetNotificationsList([FromQuery] int PageNumber, int PageSize,string? Condition)
        {

            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    string userEmail = User.FindFirstValue(ClaimTypes.Email);
                    var user = _userManager.FindByEmailAsync(userEmail).Result;
                    var data = _context.ProxyNotifications.Where(p => p.BranchCode == user.BranchCode && !p.IsRemoved).AsQueryable();

                    //var dataList =data.ToList();
                    int count = data.Count();
                    int activeCount = data.Where(p => p.IsActive == true).ToList().Count();

                    if (string.IsNullOrEmpty(Condition))
                    {
                        
                        int remainder = count % PageSize;
                        int PageCount = 0;
                        if (remainder > 0)
                        {
                            PageCount = (count / PageSize) + 1;
                        }
                        else
                        {
                            PageCount = count / PageSize;
                        }

                        int skip = (PageNumber - 1) * PageSize;
                        //ConvertToTehran()
                        var Notifications = data
                                .Skip(skip)
                                .Take(PageSize).Select(p => new NotifItem
                                {
                                    Id = p.Id,
                                    FactorId = p.FactorId,
                                    WorkName = p.WorkName,
                                    ExpirationTime = DateToTimeStamp(p.ExpirationTime),
                                    IsActive = p.IsActive,
                                    //Text = $"شماره فاکتور {p.FactorId} با نام {p.WorkName} در تاریخ {p.ExpirationTime}  منقضی می‌شود"
                                    Text = $"هشدار انقضا برای فاکتور {p.WorkName}"
                                })
                                .ToList();
                        
                        var Notifs = new NotifList { Count = activeCount, Notifications = Notifications };

                        return Ok(new ResultDto<NotifList> { Data = Notifs, IsSuccess = true, Message = "دریافت موفق" });
                    }
                    else { return Ok(new ResultDto<int> { Data= activeCount, IsSuccess=true,Message="دریافت موفق"}); }
                }

                catch
                {
                    // Log the exception (ex)
                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
                }
            
            }
            else
            {

                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
            }


        }
        

        [HttpPost]
        [Route("GetNotification")]
        public IActionResult GetNotification (RequestId request)
        {

            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    string userEmail = User.FindFirstValue(ClaimTypes.Email);
                    var user = _userManager.FindByEmailAsync(userEmail).Result;
                    var data = _context.ProxyNotifications.FirstOrDefault(p => p.BranchCode == user.BranchCode && p.Id == request.Id&& !p.IsRemoved);
                    if (data == null) { return BadRequest(new ResultDto { IsSuccess = false, Message = "پیغام یافت نشد" }); }
                    data.SeenTime = DateTime.UtcNow;
                    data.IsActive = false;
                    //var dataList =data.ToList();
                    var jalaliDate = ConvertToJalali( data.ExpirationTime);
                    var Notification = new NotifItem
                           {
                               Id = data.Id,
                               FactorId = data.FactorId,
                               WorkName = data.WorkName,
                               ExpirationTime = DateToTimeStamp(data.ExpirationTime),
                               IsActive = data.IsActive,
                        //Text = $"هشدار انقضا برای کار {data.WorkName}"
                        Text = $"شماره فاکتور {ConvertToPersianNumbers(data.FactorId.ToString())} با نام {data.WorkName} در تاریخ {jalaliDate} منقضی می‌شود"
                        //Text = $"شماره فاکتور {p.FactorId} با نام {p.WorkName} در تاریخ {p.ExpirationTime.ToString("yyyy-MM-dd")} منقضی می‌شود"
                    };

                    _context.ProxyNotifications.Update(data);
                    _context.SaveChanges();
                    return Ok(new ResultDto<NotifItem> { Data = Notification, IsSuccess = true, Message = "دریافت موفق" });
                }
                catch
                {
                    // Log the exception (ex)
                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
                }
            }
            else
            {

                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
            }


        }





        [HttpGet]
        [Route("TargetYearMonth")]
        public IActionResult TargetYearMonth()
        {
            try
            {
                var result = _targetDeterminationGetService.Year_Month_Get();

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        


        [HttpGet]
        [Route("MonthTargetGet")]

        public IActionResult MonthTargetGet(int year, int month)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    string userEmail = User.FindFirstValue(ClaimTypes.Email);
                    var user = _userManager.FindByEmailAsync(userEmail).Result;
                    var result = _targetDeterminationGetService.BranchTargets(year, month, user.BranchCode);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
                }
                catch
                {
                    // Log the exception (ex)
                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
                }
            }
            else
            {

                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
            }

        }




        [HttpGet]
        [Route("BranchTargetsHistory")]

        public IActionResult BranchTargetsHistory()
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    string userEmail = User.FindFirstValue(ClaimTypes.Email);
                    var user = _userManager.FindByEmailAsync(userEmail).Result;
                    var result = _targetDeterminationGetService.BranchTargetsHistory(user.BranchCode);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
                }
                catch
                {
                    // Log the exception (ex)
                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
                }
            }
            else
            {

                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
            }
        }




        [HttpPost]
        [Route("MonthlyTargetSet")]

        public IActionResult MonthlyTargetSet(MonthlyTargetRequestSet request)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    string userEmail = User.FindFirstValue(ClaimTypes.Email);
                    var user = _userManager.FindByEmailAsync(userEmail).Result;
                    request.branchCode = user.BranchCode;
                    var result = _targetDeterminationSetService.SetMonthlyTarget(request);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
                }
                catch
                {
                    // Log the exception (ex)
                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
                }
            }
            else
            {

                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
            }
        }






        private class NotifItem
        {
            public int Id { get; set; }
            public long FactorId { get; set; }
            public string WorkName { get; set; }
            public long ExpirationTime { get; set; }
            public bool IsActive { get; set; }
            public string Text { get; set; }


        }
        private class NotifList
        {
            public List<NotifItem> Notifications { get; set; }
            public int Count { get; set; }  

        }

        private static long DateToTimeStamp(DateTime dateTime)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan elapsedTime = dateTime.ToUniversalTime() - epoch;
            long timestamp = (long)elapsedTime.TotalMilliseconds;
            return timestamp;
        }
        public static string ConvertToPersianNumbers(string input)
        {
            char[] persianDigits = { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
            char[] englishDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            for (int i = 0; i < englishDigits.Length; i++)
            {
                input = input.Replace(englishDigits[i], persianDigits[i]);
            }

            return input;
        }


        private static string ConvertToJalali(DateTime gregorianDate)
        {
            // Create an instance of the PersianCalendar class
            PersianCalendar persianCalendar = new PersianCalendar();

            // Get the Persian year, month, and day
            int year = persianCalendar.GetYear(gregorianDate);
            int month = persianCalendar.GetMonth(gregorianDate);
            int day = persianCalendar.GetDayOfMonth(gregorianDate);

            // Format and return the Jalali date as a string
            return $"{ConvertToPersianNumbers(day.ToString()):D2}-{ConvertToPersianNumbers(month.ToString()):D2}-{ConvertToPersianNumbers(year.ToString())}";
        }



    }
}
