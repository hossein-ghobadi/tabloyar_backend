//using Endpoint.Site.Areas.Admin.Models.AdminViewModel.Claim;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Claims.Commands.ClaimCategorySetService;
//using Radin.Application.Services.Claims.Commands.ClaimSetService;
//using Radin.Application.Services.Claims.Queries.ClaimCategoryGetService;
//using Radin.Application.Services.Claims.Queries;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Users;
//using System;
//using System.IO;
//using System.Security.Claims;

//namespace Endpoint.Site.Areas.Admin.Controllers
//{
//    [Route("Admin/api/[controller]")]
//    [ApiController]
//    public class ApiClaimController : ControllerBase
//    {
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly UserManager<User> _userManager;
//        private readonly IClaimSetService _claimSetService;
//        private readonly IClaimGetService _claimGetService;
//        private readonly IClaimInfoGetService _claimInfoGetService;
//        private readonly IClaimCategorySetService _claimCategorySetService;
//        private readonly IClaimCategoryGetService _claimCategoryGetService;
//        private readonly IDataBaseContext _context;

//        public ApiClaimController(
//            RoleManager<IdentityRole> roleManager, 
//            UserManager<User> userManager,
//            IClaimSetService claimSetService,
//            IClaimGetService claimGetService,
//            IClaimInfoGetService claimInfoGetService,
//            IDataBaseContext context,
//            IClaimCategorySetService claimCategorySetService,
//            IClaimCategoryGetService claimCategoryGetService
//            )
//        {
//            _roleManager = roleManager;
//            _userManager = userManager;
//            _claimSetService = claimSetService;
//            _claimGetService = claimGetService;
//            _claimInfoGetService = claimInfoGetService;
//            _claimCategorySetService = claimCategorySetService;
//            _claimCategoryGetService = claimCategoryGetService;
//            _context = context;
//        }


//        [HttpPost]
//        [Route("SetClaimCategory")]
//        //[Authorize(Roles = "ADMIN")]
//        public IActionResult SetClaimCategory(RequestClaimCategorySetDto requestClaimCategorySetDto)
//        {
//            var res = _claimCategorySetService.Execute( requestClaimCategorySetDto );
//            if(res.IsSuccess)
//            {
//                return Ok(res);
//            }
//            return BadRequest("درج دسته بندی ادعا ناموفق");
            
//        }

//        [HttpGet]
//        [Route("GetClaimCategory")]
//        [Authorize(Roles = "ADMIN")]
//        public IActionResult GetClaimCategory()
//        {
//            return Ok(_claimCategoryGetService.Execute());
//        }

//        [HttpDelete]
//        [Route("DeleteClaimCategory")]
//        [Authorize(Roles = "ADMIN")]
//        public IActionResult DeleteClaimCategory(long Id)
//        {
//            var result=new{
//                     IsSuccess = false,
//                    Message ="",

//                };
//            var ClaimCategory = _context.ClaimCategories.FirstOrDefault(x => x.Id == Id);
//            if (ClaimCategory == null)
//            {
//                 result =new {
//                     IsSuccess = false,
//                    Message ="دسته ای با این آیدی یافت نشد"

//                };
//                return NotFound(result);
//            }
//            if (ClaimCategory.CategoryName==ClaimCategoryConstant.FixCategory)
//            {
//                   result =new {
//                     IsSuccess = false,
//                    Message = "!این دسته  قابلیت حذف ندارد"

//                };
//                return BadRequest(result);
//            }
//            _context.ClaimCategories.Remove(ClaimCategory);
//            _context.SaveChanges();
//            var ClaimCategoryInfo = _context.ClaimCategories.FirstOrDefault(c => c.CategoryName == ClaimCategoryConstant.FixCategory);
//            long FixCategoryId = 0;
//            if (ClaimCategoryInfo == null)
//            {
//                  result =new {
//                     IsSuccess = false,
//                    Message = $@"error={FixCategoryId}"

//                };
//                return BadRequest(result);
//            }
//            FixCategoryId = ClaimCategoryInfo.Id;
//            var claiminfo = _context.ClaimInfos.Where(c => c.category == Id).ToList();
//            if (claiminfo.Any())
//            {
//                foreach (var claim in claiminfo)
//                {
//                    claim.category = FixCategoryId;
//                }

//                _context.SaveChanges();
//            }
//              result =new {
//                     IsSuccess = false,
//                    Message ="دسته مورد نظر با موفقیت حذف شد"

//                };
//            return Ok(result);
//        }




//        [HttpPost]
//        [Route("SetClaim")]
//        [Authorize(Roles ="ADMIN")]
//        public IActionResult SetClaim(RequestClaimSetDto request)
//        {
//            var res = _claimSetService.Execute(new RequestClaimSetDto
//            {
//                ClaimName1 = request.ClaimName1,
//                ClaimName2 = request.ClaimName2,
//                category = request.category

//            });
//            if(res.IsSuccess)
//            {
//                var roles = _roleManager.Roles.ToList();
//                foreach (var role in roles)
//                {
//                    Claim newClaim = new Claim(request.ClaimName1,"0", ClaimValueTypes.String);
//                    var claimres = _roleManager.AddClaimAsync(role, newClaim);
//                    if (claimres.Result == null)
//                    {
//                        return BadRequest("عملیات ثبت ادعا با مشکل مواجه شد");
//                    }
//                }

//                return Ok("عملیات ثبت ادعا برای تمامی نقش ها با موفقیت انجام شد");
//            }
//            return BadRequest("عملیات ثبت ادعا ناموفق");
//        }

//        [HttpGet]
//        [Route("GetAccess")]
//        [Authorize(Roles = "ADMIN")]
//        public IActionResult GetAccess(string rolename)
//        {
//            var roleClaims = _claimGetService.Execute(new RequestClaimesDto
//            {
//                rolename = rolename
//            });
//            return Ok(roleClaims);
//        }

//        [HttpGet]
//        [Route("GetClaimInfo")]
//        [Authorize(Roles = "ADMIN")]
//        public IActionResult GetClaimInfo(int PageNumber, int PageSize, string? search)
//        {
//            var validationErrors = new List<IdLabelDto>();
//            int id = 0;
//            if (PageNumber.GetType() != typeof(int))
//            {
//                id = id + 1;
//                validationErrors.Add(new IdLabelDto
//                {
//                    id = id,
//                    label = "!شماره صفحه باید به فرمت عدد باشد  "
//                });
//            }
//            if (PageSize.GetType() != typeof(int))
//            {
//                id = id + 1;
//                validationErrors.Add(new IdLabelDto
//                {
//                    id = id,
//                    label = "!شماره صفحه باید به فرمت عدد باشد  "
//                });
//            }
//            if (validationErrors.Any())
//            {
//                return BadRequest(validationErrors);
//            }
//            else
//            {
//                var ClaimInfo = _claimInfoGetService.Execute(new RequestClaimInfoGetDto
//                {
//                    PageNumber = PageNumber,
//                    SearchKey = search,
//                    PageSize = PageSize,
//                });
//                return Ok(ClaimInfo);
//            }
//        }

//        [HttpDelete]
//        [Route("DeleteClaimInfo")]
//        [Authorize(Roles = "ADMIN")]
//        public IActionResult DeleteClaimInfo(long Id)
//        {
//            var result = new
//            {
//                IsSuccess = false,
//                Message = "",

//            };
//            var claiminfo = _context.ClaimInfos.FirstOrDefault(x => x.Id == Id);
//            if (claiminfo == null)
//            {
//                result = new
//                {
//                    IsSuccess = false,
//                    Message = "ادعایی با این آیدی یافت نشد"

//                };
//                return NotFound(result);
//            }

//            _context.ClaimInfos.Remove(claiminfo);
//            var claimname = claiminfo.ClaimName1;
//            var roles = _roleManager.Roles.ToList();
//            foreach (var role in roles)
//            {
//                var claimrole = _roleManager.GetClaimsAsync(role).Result;
//                if (claimrole != null)
//                {
//                    var claim = claimrole.FirstOrDefault(c => c.Type == claimname);
//                    var res = _roleManager.RemoveClaimAsync(role, claim).Result;
//                    if(!res.Succeeded)
//                    {
//                        result = new
//                        {
//                            IsSuccess = false,
//                            Message = "حذف ادعا ناموفق"

//                        };
//                        return BadRequest(result);

//                    }

//                }
//            }

//            _context.SaveChanges();
//            result = new
//            {
//                IsSuccess = false,
//                Message = "ادعای مورد نظر با موفقیت حذف شد"

//            };

//            return Ok(result);
//        }


//        [HttpPost]
//        [Route("SetAccess")]
//        [Authorize(Roles = "ADMIN")]
//        public IActionResult UpdateClaimValue(RoleClaimUpdate model)
//        {
//            try
//            {
//                var role = _roleManager.FindByNameAsync(model.rolename).Result;
//                if (role == null)
//                {
//                    return NotFound("این نقش یافت نشد");
//                }
//                var allAccesses = model.AccessCategory
//                .Where(ca => ca.access != null)
//                .SelectMany(ca => ca.access)
//                .ToList();
                
               
//                var access = allAccesses;
//                var ClaimList = _roleManager.GetClaimsAsync(role).Result.ToList();
//                if (ClaimList == null || !ClaimList.Any())
//                {
//                    return NotFound("ادعایی برای این نقش یافت نشد");
//                }
                
//                foreach (var acc in access)
//                {
//                    var claim = ClaimList.FirstOrDefault(c => c.Type == acc.type);
//                    if (claim != null)
//                    {
//                        var removeResult =  _roleManager.RemoveClaimAsync(role, claim).Result;
//                        if (!removeResult.Succeeded)
//                        {
//                            return BadRequest("خطا در حذف ادعا");
//                        }

//                    }
//                    Claim newClaim = new Claim(acc.type, acc.value.ToString(), ClaimValueTypes.String);
//                    var res = _roleManager.AddClaimAsync(role, newClaim);
//                    if (res.Result == null)
//                    {
//                       return BadRequest("سطح دسترسی این نقش با مشکل مواجه شد");
//                    }
//                }

//                return Ok("سطح درسترسی این نقش به روزرسانی شد");
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500,"خطا در عملیات");
//            }
//        }


//    }
//}
