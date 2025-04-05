//using Endpoint.Site.Areas.Admin.Models.AdminViewModel.User;
//using Endpoint.Site.Models.NestingViewModel.ChannelliumViewModel;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Win32;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Branch.Commands.BranchInfoSetService;
//using Radin.Application.Services.Branch.Queries.BranchInfoGetService;
//using Radin.Application.Services.Contents.Queries.ContentCategoryGet;
//using Radin.Application.Services.Factors.Queries.PurchasedFactorGet;
//using Radin.Application.Services.GoesArea.Queries.CityGetService;
//using Radin.Application.Services.GoesArea.Queries.StateGetService;
//using Radin.Application.Services.OKR.Commands.TargetDeterminationSet;
//using Radin.Application.Services.OKR.Queries.TargetDeterminationGet;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Branches;
//using Radin.Domain.Entities.Users;
//using System;
//using NewtonsoftJson = Newtonsoft.Json;

//using static Radin.Application.Services.Branch.Queries.BranchInfoGetService.BranchInfoGetService;
//using Radin.Application.Services.FactorComplementation.Queries;
//using Radin.Application.Services.Factors.Queries.ProductPriceDetailGet;

//namespace Endpoint.Site.Areas.Admin.Controllers
//{
//    [Route("Admin/api/[controller]")]
//    [ApiController]
//    public class ApiBranchProxyController : ControllerBase
//    {
//        private readonly IBranchInfoSetService _branchInfoSetService;
//        private readonly IBranchInfoGetService _branchInfoGetService;
//        private readonly IBranchUniqeGetService _branchUniqeGetService;
//        private readonly IBranchInfoEditService _branchInfoEditService;
//        private readonly IBranchGetCodeService _branchGetCodeService;
//        private readonly UserManager<User> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly IDataBaseContext _context;
//        private readonly IStateGetService _stateGetService;
//        private readonly ICityGetService _cityGetService;
//        private readonly ITargetDeterminationGetService _targetDeterminationGetService;
//        private readonly ITargetDeterminationSetService _targetDeterminationSetService;
//        private readonly IPurchasedFactorGet _purchasedFactorGet;
//        private readonly IFactorComplementationFieldsGetService _factorComplementationFieldsGetService;
//        private readonly IProductPriceDetailGetSevice _productPriceDetailGetSevice;
//        public ApiBranchProxyController(
//            IBranchInfoSetService branchInfoSetService,
//            IBranchInfoGetService branchInfoGetService,
//            IBranchInfoEditService branchInfoEditService,
//            IBranchUniqeGetService branchUniqeGetService,
//            IBranchGetCodeService branchGetCodeService,
//            RoleManager<IdentityRole> roleManager,
//            UserManager<User> userManager,
//            IDataBaseContext dataBaseContext,
//            IStateGetService stateGetService,
//            ICityGetService cityGetService,
//            ITargetDeterminationGetService targetDeterminationGetService,
//            ITargetDeterminationSetService targetDeterminationSetService,
//            IPurchasedFactorGet purchasedFactorGet ,
//            IFactorComplementationFieldsGetService factorComplementationFieldsGetService,
//            IProductPriceDetailGetSevice productPriceDetailGetSevice
//        )
//        {
//            _branchInfoSetService = branchInfoSetService;
//            _branchInfoGetService = branchInfoGetService;
//            _branchInfoEditService = branchInfoEditService;
//            _branchUniqeGetService = branchUniqeGetService;
//            _branchGetCodeService = branchGetCodeService;
//            _userManager = userManager;
//            _roleManager = roleManager;
//            _context = dataBaseContext;
//            _stateGetService = stateGetService;
//            _cityGetService = cityGetService;
//            _targetDeterminationGetService = targetDeterminationGetService;
//            _targetDeterminationSetService = targetDeterminationSetService;
//            _purchasedFactorGet = purchasedFactorGet;
//            _factorComplementationFieldsGetService = factorComplementationFieldsGetService;
//            _productPriceDetailGetSevice = productPriceDetailGetSevice;
//        }

//        [HttpPost]
//        [Route("SetBranchInfo")]
//        public IActionResult SetBranchInfo(RequestBranchInfoSetDto requestBranchInfoSetDto)
//        {
//            try
//            {
//                var res = _branchInfoSetService.Execute(requestBranchInfoSetDto);

//                if (res.IsSuccess)
//                {
//                    return Ok(res);
//                }
//                return BadRequest(res);
//        }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//    }
//}

//        [HttpGet]
//        [Route("GetBranchUniqeInfo")]
//        public IActionResult GetBranchUniqeInfo(long code) 
//        {
//            try
//            {
//                var res = _branchUniqeGetService.Execute(new RequestBranchUniqeGetDto
//                {
//                    BranchCode = code
//                } );
//                if (res.IsSuccess)
//                {
//                    return Ok(res.Data);
//                }
//                return BadRequest(res);
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }

//        [HttpGet]
//        [Route("GetBranchInfo")]
//        public IActionResult GetBranchInfo(int PageNumber, int PageSize, int? WImage, string? search, bool sort = false)
//        {
//            try
//            {
//                var validationErrors = new List<IdLabelDto>();
//                int id = 0;
//                if (PageNumber.GetType() != typeof(int))
//                {
//                    id = id + 1;
//                    validationErrors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!شماره صفحه باید به فرمت عدد باشد  "
//                    });
//                }
//                if (PageSize.GetType() != typeof(int))
//                {
//                    id = id + 1;
//                    validationErrors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!شماره صفحه باید به فرمت عدد باشد  "
//                    });
//                }
//                if (validationErrors.Any())
//                {
//                    return BadRequest(validationErrors);
//                }
//                else
//                {
//                    var requestDto = new RequestBranchInfoGetDto
//                    {
//                        PageNumber = PageNumber,
//                        SearchKey = search,
//                        PageSize = PageSize,
//                        IsSort = sort
//                    };
                    
//                    if (WImage != null)
//                    {
//                        requestDto.WImage = WImage;
//                    }
//                    var res = _branchInfoGetService.Execute(requestDto);
//                    return Ok(res.Data);
//            }
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }

//        [HttpPost]
//        [Route("EditBranchInfo")]
//        public IActionResult EditBranchInfo(RequestBranchInfoEditDto requestBranchInfoEditDto)
//        {
//            try
//            {
//                var res = _branchInfoEditService.Execute(requestBranchInfoEditDto);
//                if (res.IsSuccess)
//                {
//                    return Ok(res);
//                }
//                return BadRequest(res);
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }

//        [HttpGet]
//        [Route("GetBranchCode")]
//        public IActionResult GetBranchCode() {
//            try
//            {
//                return Ok(_branchGetCodeService.Execute());
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }

//        [HttpPost]
//        [Route("ProxyRegister")]
//        public IActionResult ProxyRegister(AdminRegisterView model)
//        {
//            try
//            {
//                var validationErrors = model.Validate();
//                var users = _userManager.Users.ToList();
//                var duplicates = users.FirstOrDefault(u => u.PhoneNumber == model.phone);
//                int phoneDigits = model.phone.Length;
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

//                if (duplicates != null)
//                {
//                    id = id + 1;
//                    validationErrors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!این شماره تماس تکراری است"
//                    });

//                }
//                bool containsOnlyDigits = model.phone.All(char.IsDigit);
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

//                User newUser = new User()
//                {
//                    Email = model.email,
//                    UserName = model.name,
//                    FullName = model.fullName,
//                    PhoneNumber = model.phone
//                };

//                var result = _userManager.CreateAsync(newUser, model.password).Result;
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
//                    foreach (var role in model.UserRole)
//                    {
//                        var res2 = _userManager.AddToRoleAsync(res1, role).Result;
//                    }


//                    //var SellerList = _context.SellerINFOs.Where(c => c.BranchCode == model.BranchCode).ToList();
//                    //var lastBranchCode = _context.SellerINFOs.OrderByDescending(c => c.BranchCode).Select(c => c.BranchCode).FirstOrDefault();
//                    //if (!string.IsNullOrEmpty(lastBranchCode.ToString()) && lastBranchCode.ToString().Length >= 3)
//                    //{
//                    //    var lastThreeDigits = lastBranchCode.ToString().Substring(lastBranchCode.ToString().Length - 3);
//                    //}
//                    //var sellerCount = _context.SellerINFOs.Count(c => c.BranchCode == model.BranchCode);
//                    //sellerCount++;
//                    //long factor = (long)Math.Pow(10, sellerCount.ToString().Length);
//                    //long sellerCode = (long)res1.BranchCode * factor + sellerCount;
//                    _context.SaveChanges();

//                    return CreatedAtAction(nameof(ProxyRegister), new { id = newUser.Id }, newUser);

//                }
//                return BadRequest(validationErrors);
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }

//        [HttpGet]
//        [Route("GetProvince")]
//        public IActionResult GetState()
//        {
//            try { 
//                    return Ok(_stateGetService.Execute());
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }

//        [HttpGet]
//        [Route("GetCity")]
//        public IActionResult GetCity(int ProvinceId)
//        {
//            try
//            {
//                return Ok(_cityGetService.Execute(new GetCityRequest { ProvinceId = ProvinceId }));
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }


       


//        [HttpGet]
//        [Route("TargetYearMonth")]
//        public IActionResult TargetYearMonth()
//        {
//            try
//            {
//                var result = _targetDeterminationGetService.Year_Month_Get();

//                if (result.IsSuccess)
//                {
//                    return Ok(result);
//                }
//                return BadRequest(result);
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }

//        [HttpGet]
//        [Route("BranchListForTarget")]
//        public IActionResult BranchListForTarget(int year, int month)
//        {
//            try
//            {
//                var result = _targetDeterminationGetService.Branch_List(year, month);

//                if (result.IsSuccess)
//                {
//                    return Ok(result);
//                }
//                return BadRequest(result);
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }


//        [HttpGet]
//        [Route("MonthTargetGet")]

//        public IActionResult MonthTargetGet(int year, int month,long branchcode)
//        {
//            try
//            {
//                var result = _targetDeterminationGetService.BranchTargets(year, month, branchcode);

//                if (result.IsSuccess)
//                {
//                    return Ok(result);
//                }
//                return BadRequest(result);
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }




//        [HttpGet]
//        [Route("BranchTargetsHistory")]

//        public IActionResult BranchTargetsHistory(long branchcode)
//        {
//            try
//            {
//                var result = _targetDeterminationGetService.BranchTargetsHistory(branchcode);

//                if (result.IsSuccess)
//                {
//                    return Ok(result);
//                }
//                return BadRequest(result);
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }




//        [HttpPost]
//        [Route("MonthlyTargetSet")]

//        public IActionResult MonthlyTargetSet(MonthlyTargetRequestSet request)
//        {
//            try
//            {
//                var result = _targetDeterminationSetService.SetMonthlyTarget(request);

//                if (result.IsSuccess)
//                {
//                    return Ok(result);
//                }
//                return BadRequest(result);
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }



//        [HttpGet]
//        [Route("PurchasedFactors")]

//        public IActionResult PurchasedFactors(long? branchCode)
//        {
//            try
//            {
//                var result = _purchasedFactorGet.AbstractOfPurchased(branchCode);
                
//                if (result.IsSuccess)
//                {

//                    return Ok(result);
//                }
//                return BadRequest(result);
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }



//        [HttpGet]
//        [Route("PurchasedFactorProducts")]

//        public IActionResult PurchasedFactorProducts(long factorId)
//        {
//            try
//            {
//                var result = _purchasedFactorGet.PurchasedProducts(factorId);

//                if (result.IsSuccess)
//                {


//                    foreach (var Product in result.Data)
//                    {
//                        var Details = NewtonsoftJson.JsonConvert.DeserializeObject<ChannelliumViewModel>(Product.svgAdress);

//                        Product.svgAdress = Details.file ;

//                    }
//                    return Ok(result);
//                }
//                return BadRequest(result);
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }


//        [HttpGet]
//        [Route("PurchasedProductDetail")]

//        public IActionResult PurchasedProductDetail(long productId)
//        {
//            try
//            {
//                var result = _factorComplementationFieldsGetService.GetFactorDetails(productId);

//                if (result.Result.IsSuccess)
//                {
//                    return Ok(result.Result);



//                }
//                return BadRequest(result.Result);
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }


//        [HttpGet]
//        [Route("PurchasedProductPriceDetail")]

//        public IActionResult PurchasedProductPriceDetail(long productId)
//        {
            
//            try
//            {
//                var result = _productPriceDetailGetSevice.ProductPriceDetails(productId);

//                if (result.Result.IsSuccess)
//                {
//                    return Ok(result.Result);



//                }
//                return BadRequest(result.Result);
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }







//    }
//}
