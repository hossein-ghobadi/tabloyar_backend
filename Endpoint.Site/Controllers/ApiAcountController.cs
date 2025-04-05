//using Endpoint.Site.Areas.Admin.Models.AdminViewModel.Role;
//using Endpoint.Site.Areas.Admin.Models.AdminViewModel.User;
//using Endpoint.Site.Models.ViewModels.Register;
//using Endpoint.Site.Models.ViewModels.User;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using NuGet.Protocol;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Users;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;
//using System.Web;
//using System.IdentityModel.Tokens.Jwt;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.SignalR;
//using Radin.Application.Services.Email.Commands;
//using Radin.Application.Services.SMS.Commands;
//using Endpoint.Site.Models.NestingViewModel.ChannelliumViewModel;
//using Microsoft.Extensions.Caching.Memory;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Common.StaticClass;
//using Microsoft.EntityFrameworkCore;

//namespace Endpoint.Site.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ApiAcountController : ControllerBase
//    {
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly UserManager<User> _userManager;
//        private readonly SignInManager<User> _signInManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly IEmailSendService _emailSendService;
//        private readonly ISMSSendService _smsService;
//        private readonly ISMSCheckService _SMSCheckService;
//        private readonly IMemoryCache _memoryCache;
//        private readonly IDataBaseContext _context;

       


//        public ApiAcountController(
//            UserManager<User> userManager,
//            SignInManager<User> signInManager,
//            RoleManager<IdentityRole> roleManager,
//            IHttpContextAccessor httpContextAccessor,
//            IEmailSendService emailSendService,
//            ISMSSendService sMSSendService,
//            ISMSCheckService sMSCheckService,
//            IMemoryCache memoryCache,
//            IDataBaseContext context

//            )
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _roleManager = roleManager;
//            _httpContextAccessor = httpContextAccessor;
//            _emailSendService = emailSendService;
//            _smsService = sMSSendService;
//            _SMSCheckService = sMSCheckService;
//            _memoryCache = memoryCache;
//            _context = context;

//        }


//        [HttpPost]
//        [Route("SMSSender")]
//        public IActionResult SMSSender(RequestSMSSentDto PhoneNumber)
//        {
//            var res = _smsService.Send( PhoneNumber);
//            return Ok(res);
//        }

//        [HttpPost]
//        [Route("SMSCheck")]
//        public IActionResult SMSCheck(RequestSMSCheckDto requestSMSCheckDto)
//        {
//            var res = _SMSCheckService.Check( requestSMSCheckDto );
//            return Ok(res);
//        }

//        [HttpPost]
//        [Route("Register1")]
//        public IActionResult Register1(RegisterViewModel model)
//        {
//            var validationErrors = model.Validate();
//            var users = _userManager.Users.ToList();
//            var duplicates = users.FirstOrDefault(u => u.PhoneNumber == model.phone);
//            //var duplicateEmail = users.FirstOrDefault(u => u.Email == model.email);
//            int phoneDigits = model.phone.Length;
//            int id = validationErrors.Count();

//            //if (duplicateEmail != null)
//            //{
//            //    id = id + 1;
//            //    validationErrors.Add(new IdLabelDto
//            //    {
//            //        id = id,
//            //        label = "!این ایمیل قبلا در سیستم ثبت شده است"
//            //    });

//            //}

//            if (phoneDigits != 11)
//            {
//                id = id + 1;
//                validationErrors.Add(new IdLabelDto
//                {
//                    id = id,
//                    label = "!تعدا ارقام شماره تماس باید 11 رقم باشد"
//                });
//            }

//            if (duplicates != null)
//            {
//                id = id + 1;
//                validationErrors.Add(new IdLabelDto
//                {
//                    id = id,
//                    label = "!این شماره تماس قبلا در سیستم ثبت شده است"
//                });

//            }
//            bool containsOnlyDigits = model.phone.All(char.IsDigit);
//            if (!containsOnlyDigits)
//            {
//                id = id + 1;
//                validationErrors.Add(new IdLabelDto
//                {
//                    id = id,
//                    label = "! شماره تماس باید شامل اعداد باشد"
//                });
//            }

//            if (validationErrors.Any())
//            {
//                return BadRequest(validationErrors);
//            }
//            User newUser = new User()
//            {
//                Email = model.email,
//                UserName = model.name,
//                FullName = model.fullName,
//                PhoneNumber = model.phone,
//            };

//            var res = _smsService.Send(new RequestSMSSentDto { PhoneNumber = model.phone});
//            if (res.IsSuccess == false)
//            {
//                return BadRequest("خطای داخلی سرویسِ!‍ مجدد تلاش کنید");
//            }
//            if (res.Data==false)
//            {
//                return BadRequest(res.Message);
//            }

//            return Ok(newUser);

//        }

//        [HttpPost]
//        [Route("Register2")]

//        public IActionResult Register2(FinalRegisterStep ViewModel)
//        {

//            var res = _SMSCheckService.Check(new RequestSMSCheckDto { PhoneNumber=ViewModel.phone,Code=ViewModel.verifyCode});
//            if (res.IsSuccess == false)
//            {
//                return BadRequest("ثبت نام انجام نشد؛ لطفا مجددا تلاش نمایید");
//            }

//            var validationErrors = new List<IdLabelDto>();
//            if (res.Data)
//            {
//                User newUser = new User()
//                {
//                    Email = ViewModel.email,
//                    UserName = ViewModel.name,
//                    FullName = ViewModel.fullName,
//                    PhoneNumber = ViewModel.phone,
//                    PhoneNumberConfirmed = true,
//                    BranchCode = 122
//                };
                
//                int id = validationErrors.Count();
//                var result = _userManager.CreateAsync(newUser, ViewModel.password).Result;
//                foreach (var item in result.Errors)
//                {
//                    id = id + 1;
//                    validationErrors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = item.Description.ToString()
//                    });
//                }
//                if (result.Succeeded)
//                {

//                    var res1 = _userManager.FindByIdAsync(newUser.Id).Result;
//                    var res2 = _userManager.AddToRoleAsync(res1, RoleConstantName.SiteUser).Result;
//                    LoginViewModel UserInfo = new LoginViewModel
//                        {
//                            Password = ViewModel.password,
//                            Email = ViewModel.email
//                        };
//                        return Login(UserInfo);
//                }
//                else
//                {
//                    return BadRequest(validationErrors);
//                }

//            }
//            return BadRequest(res.Message);
//        }

//        public IActionResult Login(string returnUrl = "/")
//        {
//            return Ok(new LoginViewModel
//            {
//                ReturnUrl = returnUrl,
//            });
//        }
//        [HttpGet]
//        [Route("Cookie")]
//         // Ensure authentication is required to access this endpoint
//        // Ensure authentication is required to access this endpoint
//        public IActionResult Cookie()
//        {
//            var cookieString = Request.Headers["Cookie"];
//            // Retrieve the user's identity from the HttpContext
//            var user1 = Request.HttpContext.User;
//            var isSuccess = false;
//            // Extract the user's name and role from claims
//            var user = user1.FindFirst(ClaimTypes.Name)?.Value;
//            var userRole = user1.FindFirst(ClaimTypes.Role)?.Value;
//            if (user1.Identity.IsAuthenticated)
//            {
//                isSuccess = true;
//                return Ok(new { IsSuccess= isSuccess, UserName = user, UserRole = userRole ,CookieString= cookieString });
//            }
//            else
//            {
//                // Return the user-related information in the response
//                return Ok(new { IsSuccess = isSuccess });
//            }
//        }

//        [HttpPost]
//        [Route("Login")]
//        public IActionResult Login(LoginViewModel model)
//        {
//            // LogRequest(HttpContext.Request);
            
//            if (!ModelState.IsValid)
//            {
//                return BadRequest("AccessDenied");
//            }

//            //var user = _userManager.FindByEmailAsync(model.Email).Result;
//            var users = _userManager.Users
//            .Where(u => u.Email == model.Email || u.PhoneNumber == model.Email);
//            if (users.ToList().Count() >1)
//            {
//                //ModelState.AddModelError("", "کاربر یافت نشد!");
//                return BadRequest("تداخل در کاربری!");
//            }
//            var user = users.FirstOrDefault();  
//            if (user == null)
//            {
//                //ModelState.AddModelError("", "کاربر یافت نشد!");
//                return BadRequest("کاربر یافت نشد!");
//            }

//            // Add rate limiting to prevent brute force attacks
//            // You can use a library like Polly for this purpose
//            // Example: https://github.com/App-vNext/Polly
//            // Here, I'm assuming a maximum of 5 login attempts in a 5-minute window

//            bool isLockedOut = _userManager.IsLockedOutAsync(user).Result;
//            //if (isLockedOut)
//            //{
//            //    ModelState.AddModelError("", "Too many failed login attempts. Please try again later.");
//            //    return BadRequest(ModelState);
//            //}

//            _signInManager.SignOutAsync();

//            var signInResult = _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true).Result;

//            if (signInResult.Succeeded)
//            {

//                //var token = _userManager.GenerateUserTokenAsync(user, "Default", "Login").Result;
//                // Reset lockout count upon successful login
//                _userManager.ResetAccessFailedCountAsync(user).Wait();
//                var userRole = _userManager.GetRolesAsync(user).Result;



//                var Data = new
//                {
//                    IsLockedOut = signInResult.IsLockedOut,
//                    Succeeded = signInResult.Succeeded,
//                    IsNotAllowed = signInResult.IsNotAllowed,
//                    RequiresTwoFactor = signInResult.RequiresTwoFactor,
//                    userRole = userRole[0],
//                    //token = token

//                };
//                var Result = new
//                {
//                    Data = Data,
//                    IsSuccess = true,
//                    Message = " ورود موفق"

//                };

//                return Ok(Result);
//            }
//            else if (signInResult.IsLockedOut)
//            {
//                // Increment access failed count upon failed login attempt
//                _userManager.AccessFailedAsync(user).Wait();
//            }

//            // Log the failed login attempt
//            // LogFailedLoginAttempt(model.Email);

//            return BadRequest("Invalid login attempt.");
//        }


//        [HttpGet]
//        [Route("State")]
//        public IActionResult State()
//        {
//            var Result = new UserState();
//            var IsSuccess = false;
//            Result.IsSuccess = IsSuccess;
//            if (User.Identity.IsAuthenticated)
//            {
//                IsSuccess = true;
//                string userEmail = User.FindFirstValue(ClaimTypes.Email);
//                var user = _userManager.FindByEmailAsync(userEmail).Result;
//                var userrole = _userManager.GetRolesAsync(user).Result;
//                if (userrole != null)
//                {

//                    Result.Email = userEmail;
//                    Result.Name = user.UserName;
//                    Result.FullName = user.FullName;
//                    Result.Role = userrole.ToList();
//                    Result.IsSuccess = IsSuccess;
//                    Result.BranchCode = user.BranchCode;
                    
//                    return Ok(Result);
//                }
//                else
//                {
//                    return NotFound($"User '{user.UserName}' not found.");
//                }

//            }
//            else
//            {
//                _signInManager.SignOutAsync();
                
//                return BadRequest(Result);

//            }

//        }
//        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
//        [HttpPost]
//        [Route("ForgetPassword")]
//        public IActionResult ForgetPassword(ForgetPasswordViewModel model)
//        {
//            var validationErrors = model.Validate();
//            var users = _userManager.Users.ToList();
//            var duplicates = users.FirstOrDefault(u => u.PhoneNumber == model.phone);
//            int phoneDigits = model.phone.Length;
//            int id = validationErrors.Count();
//            if (phoneDigits != 11)
//            {
//                id = id + 1;
//                validationErrors.Add(new IdLabelDto
//                {
//                    id = id,
//                    label = "!تعدا ارقام شماره تماس باید 11 رقم باشد"
//                });
//            }

//            if (duplicates == null)
//            {
//                id = id + 1;
//                validationErrors.Add(new IdLabelDto
//                {
//                    id = id,
//                    label = "!کاربری بااین شماره تماس یافت نشد"
//                });

//            }
//            bool containsOnlyDigits = model.phone.All(char.IsDigit);
//            if (!containsOnlyDigits)
//            {
//                id = id + 1;
//                validationErrors.Add(new IdLabelDto
//                {
//                    id = id,
//                    label = "! شماره تماس باید شامل اعداد باشد"
//                });
//            }
//            if (validationErrors.Any())
//            {
//                return BadRequest(validationErrors);
//            }
//            var user = users.Find(user => user.PhoneNumber == model.phone);
//            var res = _smsService.Send(new RequestSMSSentDto { PhoneNumber = user.PhoneNumber });
//            if (res.IsSuccess == false)
//            {
//                return BadRequest("خطای داخلی سرویسِ!‍ مجدد تلاش کنید");
//            }
//            if (res.Data == false)
//            {
//                return BadRequest(res.Message);
//            }

//            var output = new List<OUTPUT>();
//            output.Add(new OUTPUT
//            {
//                id = user.Id,
//                Msg = res.Message
//            });

//            return Ok(output);
//        }

//        [HttpPost]
//        [Route("ResetPassword")]
//        public IActionResult ResetPassword(ResetPasswordDto reset)
//        {
//            var validationErrors = reset.Validate();
//            int id = validationErrors.Count();
//            var user = _userManager.FindByIdAsync(reset.UserId).Result;

//            if (user == null)
//            {
//                id = id + 1;
//                validationErrors.Add(new IdLabelDto
//                {
//                    id = id,
//                    label = "!کاربر یافت نشد"
//                });
//            }

//            if (validationErrors.Any())
//            {
//                return BadRequest(validationErrors);
//            }

//            var res = _SMSCheckService.Check(new RequestSMSCheckDto { PhoneNumber = user.PhoneNumber, Code = reset.Code });
//            if (res.IsSuccess == false)
//            {
//                return BadRequest("!خطاُ مجددا تلاش کنید");
//            }
//            if (!res.Data)
//            {
//                return BadRequest(res.Message);
//            }

//            string token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
//            var Result = _userManager.ResetPasswordAsync(user, token, reset.Password).Result;
//            if (Result.Succeeded)
//            {
//                return Ok("گذرواژه با موفقیت تغییر یافت");
//            }
//            else
//            {
//                return BadRequest("تغییر گذرواژه با مشکل مواجه شد");
//            }
//        }


//        [HttpPost]
//        [Route("LogOut")]
//        public IActionResult LogOut()
//        {
//            _signInManager.SignOutAsync();

//            return Ok(true);
//        }

//        [HttpGet]
//        [Route("GetUser")]
//        public IActionResult GetUser()
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                var username = User.Identity.Name;
//                var user = _userManager.FindByNameAsync(username).Result;
//                if (user != null)
//                {

//                    var ProvinceName = _context.Cities.Where(p => p.ProvinceId == user.Province).FirstOrDefault()?.province;
//                    var cityName = _context.Cities.Where(p => p.Id == user.City).FirstOrDefault()?.city;
//                    var roles = _userManager.GetRolesAsync(user).Result;
//                    string UserAge = null;
                    
//                    var usersDtos = new GetUsersDto
//                    {
//                        username = user.UserName,
//                        email = user.Email,
//                        fullname = user.FullName,
//                        phone = user.PhoneNumber,
//                        phone2 = user.Phone,
//                        gender = user.Gender,
//                        state = new IdLabelDto
//                        {
//                            id = user.Province ?? 0,
//                            label = ProvinceName ?? null

//                        },
//                        city = new IdLabelDto
//                        {
//                            id = user.City ?? 0,
//                            label = cityName ?? null

//                        },
                        
//                        address = user.Address,
//                        IsActive = user.IsActive,
//                        IsRemove = user.IsRemove,
//                        role = roles.Select((role, index) => new RoleNameGetDto
//                            {
//                                id = (index + 1).ToString(), // Convert the index + 1 to a string for the ID
//                                label = role
//                            }).ToList(),
                       
//                    };
//                    if (user.Age != null)
//                    {
//                        string reversedTimestamp = SimpleMethods.DateTimeToTimeStamp(user.Age.Value).ToString();
//                        usersDtos.age = Convert.ToInt64(reversedTimestamp);

//                    }
//                    return Ok(usersDtos);

//                }
//                else
//                {
//                    return NotFound($"User '{username}' not found.");
//                }

//            }
//            return BadRequest("لطفا ابتدا وارد سایت شوید!");
//        }

//        [HttpPost]
//        [Route("EditUser")]
//        public IActionResult EditUser(EditViewModel mdl)
//        {
//            var validationErrors = mdl.Validate();
//            if(User.Identity.IsAuthenticated)
//            {
//                string userEmail = User.FindFirstValue(ClaimTypes.Email);
//                var user = _userManager.FindByEmailAsync(userEmail).Result;
//                var users = _userManager.Users.ToList();
//                int phoneDigits = mdl.phone.Length;
//                int id = validationErrors.Count();
//                if (phoneDigits != 11)
//                {
//                    id = id + 1;
//                    validationErrors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!تعدا ارقام شماره تماس باید 11 رقم باشد"
//                    });
//                }
//                var OtherUsers = users.Remove(user);
//                if (OtherUsers)
//                {
//                    var duplicates = users.FirstOrDefault(u => u.PhoneNumber == mdl.phone);
//                    var DupName = users.FirstOrDefault(u => u.UserName == mdl.username);
//                    if (duplicates != null)
//                    {
//                        id = id + 1;
//                        validationErrors.Add(new IdLabelDto
//                        {
//                            id = id,
//                            label = "!این شماره تماس در سیستم به نام شخص دیگری ثبت می باشد"
//                        });
//                    }
//                    if (DupName != null)
//                    {
//                        id = id + 1;
//                        validationErrors.Add(new IdLabelDto
//                        {
//                            id = id,
//                            label = "!این نام کاربری در سیستم به نام شخص دیگری ثبت می باشد"
//                        });
//                    }

//                }
//                bool containsOnlyDigits = mdl.phone.All(char.IsDigit);
//                if (!containsOnlyDigits)
//                {
//                    id = id + 1;
//                    validationErrors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "! شماره تماس باید شامل اعداد باشد"
//                    });
//                }
//                if (validationErrors.Any())
//                {
//                    return BadRequest(validationErrors);
//                }
//                if (user != null)
//                {
//                    user.UserName = mdl.username;
//                    user.FullName = mdl.fullName;
//                    user.PhoneNumber = mdl.phone;
//                    user.Phone = mdl.phone2;
//                    user.Gender = mdl.gender;
//                    user.Province = mdl.state;
//                    user.City = mdl.city;
//                    if(user.Age!=null) { user.Age = mdl.age; }
//                    user.Address = mdl.address;

//                    var res = _userManager.UpdateAsync(user).Result;
//                    if (res.Succeeded)
//                    {
//                        return Ok("اطلاعات کاربر مورد نظر با موفقیت ویرایش شد");
//                    }

//                }
//                else
//                {
//                    return NotFound($"User '{mdl.username}' not found.");
//                }

//            }       
//            return BadRequest("لطفا ابتدا وارد سایت شوید!");
//        }

//    }
//}




