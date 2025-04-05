//using CsvHelper;
//using Endpoint.Site.Areas.Admin.Models.AdminViewModel.User;
//using Endpoint.Site.Areas.Proxy.Models;
//using Endpoint.Site.Models.NestingViewModel.ChannelliumViewModel;
//using Endpoint.Site.Models.ViewModels.User;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Build.Construction;
//using Microsoft.CodeAnalysis;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Branch.Commands.BranchInfoSetService;
//using Radin.Application.Services.Branch.Commands.BranchRegisterService;
//using Radin.Application.Services.Branch.Queries.BranchInfoGetService;
//using Radin.Application.Services.CRM.Commands.ExitCrm;
//using Radin.Application.Services.CRM.Commands.UpdateExpiration;
//using Radin.Application.Services.CRM.Queries.CrmGet;
//using Radin.Application.Services.FactorComplementation.Commands;
//using Radin.Application.Services.FactorComplementation.Queries;
//using Radin.Application.Services.Factors.Commands.Accessory.AccessorySet;
//using Radin.Application.Services.Factors.Commands.Customer;
//using Radin.Application.Services.Factors.Commands.FactorContractSet;
//using Radin.Application.Services.Factors.Commands.Orders;
//using Radin.Application.Services.Factors.Commands.Orders.FinalizeOrder;
//using Radin.Application.Services.Factors.Commands.Orders.OrdersRemove;
//using Radin.Application.Services.Factors.Commands.Pyment;
//using Radin.Application.Services.Factors.Commands.RecordProduct;
//using Radin.Application.Services.Factors.Commands.Service.ServiceProductSet;
//using Radin.Application.Services.Factors.Commands.UndefinedProduct;
//using Radin.Application.Services.Factors.CRM.Commands.EditWorkName;
//using Radin.Application.Services.Factors.Queries.AccessoryGet;
//using Radin.Application.Services.Factors.Queries.CustomerGet;
//using Radin.Application.Services.Factors.Queries.HesabfaBanksGet;
//using Radin.Application.Services.Factors.Queries.NegotiationGet;
//using Radin.Application.Services.Factors.Queries.OrderGet;
//using Radin.Application.Services.Factors.Queries.PymentPageInfoGet;
//using Radin.Application.Services.SMS.Commands;
//using Radin.Common;
//using Radin.Common.Dto;
//using Radin.Common.Request;
//using Radin.Domain.Entities.Factors;
//using Radin.Domain.Entities.Ideas;
//using Radin.Domain.Entities.Users;
//using Sprache;
//using System.Buffers.Text;
//using System.Net.Http;
//using System.Security.Claims;
//using System.Text.Json;
//using System.Xml.Linq;
//using static Radin.Application.Services.CRM.Commands.UpdateExpiration.ExpirationService;
//using static Radin.Application.Services.Factors.Commands.Customer.CustomerService;
//using static Radin.Application.Services.Factors.Commands.Orders.OrdersRemove.FactorRemoveService;
//using static Radin.Application.Services.Factors.Commands.Orders.OrdersRemove.ProductFactorRemove;
//using static Radin.Application.Services.Factors.Commands.Orders.OrdersRemove.SubFactorRemoveService;
//using static Radin.Application.Services.Factors.Commands.RecordProduct.RecordProductService;
//using NewtonsoftJson = Newtonsoft.Json;
//namespace Endpoint.Site.Areas.Proxy.Controllers
//{
//    [Authorize(Roles = "PROXY,PROXYSELLER")]
//    [Route("Proxy/api/[controller]")]
//    [ApiController]
//    public class ApiOrdersController : ControllerBase
//    {
//        private readonly UserManager<User> _userManager;
//        private readonly IInitialOrderService _orderService;
//        private readonly IOrderGetService _orderGetService;
//        private readonly ISubFactorGetService _subFactorGetService;
//        private readonly IGetProductFactors _getProductFactors;
//        private readonly IGetProductFactorDetiles _getProductFactorDetiles;
//        private readonly IFactorRemoveService _factorRemoveService;
//        private readonly ISubFactorRemoveService _subFactorRemoveService;
//        private readonly IProductFactorRemove _productFactorRemoveService;
//        private readonly IDataBaseContext _context;
//        private readonly IAccessoryGetService _accessoryGetService;
//        private readonly IAccessorySetService _accessorySetService;
//        private readonly ICountDiscountChangingService _countDiscountChangingService;
//        private readonly IEditWorkNameService _editWorkNameService;
//        private readonly ICrmGetService _crmGetService;
//        private readonly IExitCrmService _exitCrmService;
//        private readonly IExpirationService _expirationService;
//        private readonly IRecordProductService _recordProductService;
//        private readonly IFinalizeOrderService _finalizeOrderService;
//        private readonly ICashPymentSaveService _cashPymentSaveService;
//        private readonly IGetBanksService _getBanksService;
//        private readonly IPymentPageInfoGetService _pymentPageInfoGetService;
//        private readonly IUndefinedProductSetService _undefinedProductSetService;
//        private readonly IServiceProductSet _serviceProductSet;
//        private readonly IPaymentService _paymentService;
//        private readonly IFactorComplementationFieldsGetService _factorComplementationFieldsGetService;
//        private readonly IFactorComplementarySetService _factorComplementarySetService;
//        private readonly IFactorContractSetService _factorContractSetService;
//        private readonly ICustomerGetService _customerGetService; 
//        private static readonly HttpClient client = new HttpClient();


//        public ApiOrdersController(
//            IInitialOrderService orderService,
//            UserManager<User> userManager,
//            IOrderGetService orderGetService,
//            ISubFactorGetService subFactorGetService,
//            IGetProductFactors getProductFactors,
//            IGetProductFactorDetiles getProductFactorDetiles,
//            IFactorRemoveService factorRemoveService,
//            ISubFactorRemoveService subFactorRemoveService,
//            IProductFactorRemove productFactorRemove,
//            IDataBaseContext context,
//            IAccessoryGetService accessoryGetService,
//            IAccessorySetService accessorySetService,
//            ICountDiscountChangingService countDiscountChangingService,
//            IEditWorkNameService editWorkNameService,
//            ICrmGetService crmGetService,
//            IExitCrmService exitCrmService,
//            IExpirationService expirationService,
//            IRecordProductService recordProductService,
//            IFinalizeOrderService finalizeOrderService,
//            ICashPymentSaveService cashPymentSaveService,
//            IGetBanksService banksService,
//            IPymentPageInfoGetService pymentPageInfoGetService,
//            IUndefinedProductSetService undefinedProductSetService,
//            IServiceProductSet serviceProductSet,
//            IPaymentService paymentService,
//            IFactorComplementationFieldsGetService factorComplementationFieldsGetService,
//            IFactorComplementarySetService factorComplementarySetService,
//            IFactorContractSetService factorContractSetService,
//            ICustomerGetService customerGetService

//            )

//        {
//            _orderService = orderService;
//            _userManager = userManager;
//            _orderGetService = orderGetService;
//            _subFactorGetService = subFactorGetService;
//            _getProductFactors = getProductFactors;
//            _getProductFactorDetiles = getProductFactorDetiles;
//            _factorRemoveService = factorRemoveService;
//            _subFactorRemoveService = subFactorRemoveService;
//            _productFactorRemoveService = productFactorRemove;
//            _context = context;
//            _accessoryGetService = accessoryGetService;
//            _accessorySetService = accessorySetService;
//            _countDiscountChangingService = countDiscountChangingService;
//            _editWorkNameService = editWorkNameService;
//            _crmGetService = crmGetService;
//            _exitCrmService = exitCrmService;
//            _expirationService = expirationService;
//            _recordProductService = recordProductService;
//            _finalizeOrderService = finalizeOrderService;
//            _getBanksService = banksService;
//            _cashPymentSaveService = cashPymentSaveService;
//            _pymentPageInfoGetService = pymentPageInfoGetService;
//            _undefinedProductSetService = undefinedProductSetService;
//            _serviceProductSet= serviceProductSet;
//            _paymentService = paymentService;
//            _factorComplementationFieldsGetService = factorComplementationFieldsGetService;
//            _factorComplementarySetService = factorComplementarySetService;
//            _factorContractSetService = factorContractSetService;
//            _customerGetService = customerGetService;

//        }

//        [HttpPost]
//        [Route("InitialOrder")]
//        public IActionResult InitialOrder(InitialOrderViewModel mdl)
//        {

//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    string userEmail = User.FindFirstValue(ClaimTypes.Email);
//                    string UserId = _userManager.FindByEmailAsync(userEmail).Result.Id;
//                    var res = _orderService.Execute(new RequestInitialOrderDto { InitialConnectionTime = mdl.InitialConnectionTime, WorkName = mdl.WorkName, UserId = UserId, FactorId = mdl.FactorId });

//                    return res.IsSuccess ? Ok(res) : BadRequest(res.Message);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, "An error occurred while processing your request.");
//                }
//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }
//        }

//        [HttpPost]
//        [Route("EditingWorkName")]
//        public IActionResult EditingWorkName(IdLabelDto request)
//        {

//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {

//                    var result = _editWorkNameService.Edit(request);

//                    return result.IsSuccess ? Ok(result) : BadRequest(result.Message);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, "An error occurred while processing your request.");
//                }
//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }
//        }






//        [HttpPost]
//        [Route("UpdateQualityFactor")]
//        public async Task<IActionResult> UpdateQualityFactorAsync(UpdateQualityFactorRequest request)
//        {

//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    if (request.QualityFactor == "A%2B")
//                    {
//                        request.QualityFactor = ConstantMaterialName.QualityFactor_Aplus;
//                    }
//                    if (request.QualityFactor == "A%2B%2B")
//                    {
//                        request.QualityFactor = ConstantMaterialName.QualityFactor_A2plus;
//                    }
//                    var result = _recordProductService.ChangeQualityFactor(request);

//                    return result.Result.IsSuccess ? Ok(result.Result) : BadRequest(result.Result);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "An error occurred while processing your request." });

//                }
//            }
//            else
//            {
//                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
//            }
//        }


//        [HttpGet("GetForCRM")]
//        public IActionResult GetFactorsByState()
//        {
//            // Fetching the data grouped by state, ensuring state is between 0 and 4
//            try
//            {
//                string userEmail = User.FindFirstValue(ClaimTypes.Email);

//                var user = _userManager.FindByEmailAsync(userEmail).Result;
//                if (user.BranchCode == 0)
//                {
//                    return BadRequest(new ResultDto { IsSuccess = false, Message = "شما عضو شعبه نیستید" });
//                }
//                var result = _crmGetService.GetForCrm(user.BranchCode);

//                return result.IsSuccess ? Ok(result) : BadRequest(result);
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }




//        [HttpPost("UpdateExpireTime")]
//        public IActionResult UpdateExpireTime(ExpireRequest request)
//        {
//            // Fetching the data grouped by state, ensuring state is between 0 and 4
//            try
//            {
//                var result = _expirationService.UpdateExpireTime(request);

//                return result.IsSuccess ? Ok(result.Message) : BadRequest(result);

//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }





//        [HttpPost("SendExpirationMessages")]
//        public IActionResult SendExpirationMessages()
//        {
//            // Fetching the data grouped by state, ensuring state is between 0 and 4
//            //try
//            //{
//            var result = _expirationService.CheckExpireMessages();

//            return result.IsSuccess ? Ok(result.Message) : BadRequest(result);

//            //}
//            //catch
//            //{
//            //    // Log the exception (ex)
//            //    return StatusCode(500, "An error occurred while processing your request.");
//            //}
//        }



//        [HttpPost("CycleExpireTime")]
//        public IActionResult CycleExpireTime(ExpireRequest request)
//        {
//            // Fetching the data grouped by state, ensuring state is between 0 and 4
//            try
//            {
//                var result = _expirationService.CycleExpireTime(request);

//                return result.IsSuccess ? Ok(result.Message) : BadRequest(result);

//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }


//        [HttpPost("ExitCRM")]
//        public IActionResult ExitCRM(ExitCrmRequest request)
//        {
//            // Fetching the data grouped by state, ensuring state is between 0 and 4
//            try
//            {
//                var Result = _exitCrmService.Exit(request);

//                return Result.IsSuccess ? Ok(Result) : BadRequest(Result);
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }




//        [HttpGet]
//        [Route("GetInitial")]
//        public IActionResult GetInitial(int? FactorId)
//        {

//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    string userEmail = User.FindFirstValue(ClaimTypes.Email);
//                    string UserId = _userManager.FindByEmailAsync(userEmail).Result.Id;
//                    var Result = _orderGetService.OrderGetInStep1(new Step1Request
//                    {
//                        FactorId = FactorId,
//                        UserId = UserId
//                    }

//                        );
//                    return Result.IsSuccess ? Ok(Result.Data) : BadRequest(Result.Message);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, "An error occurred while processing your request.");
//                }
//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }
//        }

//        [HttpGet]
//        [Route("GetSubFactors")]
//        public IActionResult GetSubFactors(int FactorId)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var Result = _subFactorGetService.Execute(new SubFactorGetRequest { FactorId = FactorId });
//                    return Result.IsSuccess ? Ok(Result.Data) : BadRequest(Result.Message);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, "An error occurred while processing your request.");
//                }
//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }
//        }
//        [HttpGet]
//        [Route("GetQualityFactor")]
//        public IActionResult GetQualityFactor(int SubFactorID)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var QualityFactor = _context.SubFactors.Where(p => p.Id == SubFactorID).Select(p => p.QualityFactor).FirstOrDefault();

//                    if (QualityFactor == null)
//                    {
//                        QualityFactor = "A";
//                    }
//                    var allQualityFactors = new List<string> { ConstantMaterialName.QualityFactor_A2plus,ConstantMaterialName.QualityFactor_Aplus, ConstantMaterialName.QualityFactor_A, ConstantMaterialName.QualityFactor_B };

//                    // Create the result list with IsDefault set according to the retrieved QualityFactor
//                    var result = allQualityFactors.Select(qf => new
//                    {
//                        id = qf.Replace("+", "%2B"), // Example to generate a simple ID, you can replace it with actual IDs
//                        label = qf,
//                        isDefault = qf == QualityFactor // Set true if it matches the retrieved QualityFactor, false otherwise
//                    }).ToList();
//                    return Ok(result);

//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, "An error occurred while processing your request.");
//                }

//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }
//        }





//        [HttpGet]
//        [Route("GetProductFactors")]
//        public IActionResult GetProductFactors(int FactorId, int? SubFactorID, string? QualityFactor)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    if(SubFactorID!=null && QualityFactor != null)
//                    {
//                        var Result = _getProductFactors.Execute(new ProductFactorGetRequest { FactorId = FactorId, SubFactorID = SubFactorID ?? 0, QualityFactor = QualityFactor, IsAccessory = false });
//                        return Result.IsSuccess ? Ok(Result.Data) : BadRequest(Result.Message);

//                    }
//                    else
//                    {
//                        var Result = _getProductFactors.GetProductForComplementation(FactorId);
//                        return Result.IsSuccess ? Ok(Result.Data) : BadRequest(Result.Message);
//                    }


          
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, "An error occurred while processing your request.");
//                }
//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }
//        }


//        [HttpGet]
//        [Route("GetProductDetails")]
//        public async Task<IActionResult> GetProductDetails(int FactorId, int SubFactorID, int ProductID)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var Input = _getProductFactorDetiles.Execute(new ProductFactorDetilesGetRequest { FactorId = FactorId, SubFactorID = SubFactorID, ProductFactorID = ProductID });
//                    if (!Input.IsSuccess)
//                    {
//                        return BadRequest(new ResultDto {  IsSuccess = true, Message = "دریافت ناموفق" });

//                    }

//                    var Result = NewtonsoftJson.JsonConvert.DeserializeObject<ChannelliumViewModel>(Input.Data);
//                    if (Result.QualityFactor == "A%2B")
//                    {
//                        Result.QualityFactor = ConstantMaterialName.QualityFactor_Aplus;
//                    }

//                    //var Base64 = _getProductFactorDetiles.SvgToBase64(Result.file, client).Result;
//                    //if (!Base64 .IsSuccess)
//                    //{
//                    //    return BadRequest(new ResultDto {  IsSuccess = false, Message = Base64.Message });
//                    //}

//                    //Result.ImageString = Base64.Data;
//                    return Ok(new ResultDto<ChannelliumViewModel> { Data = Result, IsSuccess = true, Message = "دریافت موفق" });


//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, "An error occurred while processing your request.");
//                }
//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }
//        }

//        private class PythonApiResponse
//        {
//            public string jpg_base64 { get; set; }
//        }













//        [HttpGet("GetSvg")]
//        public async Task<IActionResult> GetSvg([FromQuery] string url)
//        {
//            if (string.IsNullOrEmpty(url))
//            {
//                return BadRequest(new { Message = "The 'url' query parameter is required." });
//            }

//            try
//            {
//                //// Python API endpoint
//                //string pythonApiUrl = $"https://flask-svg.liara.run/upload-svg?file_url={url}";

//                //// Forward the request to the Python API
//                //HttpResponseMessage response = await client.GetAsync(pythonApiUrl);
//                //Console.WriteLine(response.Content);
//                //// Ensure the response is successful
//                //if (!response.IsSuccessStatusCode)
//                //{
//                //    string errorContent = await response.Content.ReadAsStringAsync();
//                //    return StatusCode((int)response.StatusCode, new
//                //    {
//                //        Message = "Error from Python API",
//                //        Details = errorContent
//                //    });
//                //}

//                //// Read the response from the Python API
//                //string responseContent = await response.Content.ReadAsStringAsync();

//                //// Deserialize the Python API's JSON response
//                //var pythonApiResponse = JsonSerializer.Deserialize<PythonApiResponse>(responseContent);

//                //// Transform or process the response if needed
//                //if (pythonApiResponse?.jpg_base64 == null)
//                //{
//                //    return BadRequest(new { Message = " 'maybe Wrong format, image should be Svg." });
//                //}

//                string pythonApiResponse = "";

//                using (HttpClient client = new HttpClient())
//                {
//                    // Download the file from the URL
//                    byte[] fileBytes = await client.GetByteArrayAsync(url);

//                    // Convert the byte array to a Base64 string
//                    pythonApiResponse = Convert.ToBase64String(fileBytes);
//                }
//                //Return the processed response to the client
//                return Ok(new
//                {
//                    Message = "SVG processed successfully.",
//                    EncodedData = pythonApiResponse // Forward 'data' field from Python API
//                });
//            }
//            catch (Exception ex)
//            {
//                // Handle unexpected errors
//                return StatusCode(500, new
//                {
//                    Message = "An error occurred while processing the request.",
//                    Details = ex.Message
//                });
//            }
//        }

//        // Define a class for deserializing Python API responses





//        [HttpPost]
//        [Route("UnProductSet")]
//        public IActionResult UnProductSet(UndefinedProductRequestDto model)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                //try
//                //{
//                    var Result = _undefinedProductSetService.UndefinedProductSet(model);
//                    return Result.Result.IsSuccess ? Ok(Result.Result) : BadRequest(Result.Result);
//                //}
//                //catch
//                //{
//                //    // Log the exception (ex)
//                //    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                //}
//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }


//        }



//        [HttpPost]
//        [Route("ServiceSet")]
//        public IActionResult ServiceSet(ServiceProductRequestDto model)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var Result = _serviceProductSet.ProductSet(model);
//                    return Result.Result.IsSuccess ? Ok(Result.Result) : BadRequest(Result.Result.Message);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }


//        }







//        [HttpPost]
//        [Route("AccessorySet")]
//        public IActionResult AccessorySet(RequestAccessorySetDto model)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var Result = _accessorySetService.Execute(model);
//                    return Result.Result.IsSuccess ? Ok(Result.Result) : BadRequest(Result.Result.Message);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }


//        }


//        [HttpGet]
//        [Route("GetAccessories")]/// دریافت لیست محصولات جانبی ثبت شده در فاکتور
//        public IActionResult GetAccessories(int FactorId, int SubFactorID)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var factror = _context.ProductFactors
//                        .Where(p => p.FactorID == FactorId && !p.IsRemoved).AsQueryable();
//                    var result = new ProductFactorGetResult();
//                    var items = factror
//                    .Where(p => p.SubFactorID == SubFactorID && p.IsAccessory && !p.IsUndefinedProduct)
//                    .Select(p => new ProductFactorGetDto
//                    {
//                        id = p.Id,
//                        Name = p.Name, // Add other properties as needed
//                        price = p.price,
//                        count = p.count,
//                        Discount = p.Discount,
//                        fee = p.fee * (1 - p.Discount * 0.01f),
//                        InsertTime = p.InsertTime,
//                        purchaseFee = _context.Accessories.FirstOrDefault(q => q.Name == p.Name).purchaseFee,
//                        purchasePrice = _context.Accessories.FirstOrDefault(q => q.Name == p.Name).purchaseFee * p.count
//                    })
//                        .ToList();

//                    result.ProductFactorsInfo = items;

//                    if (factror != null)
//                    {
//                        return Ok(result);
//                    }
//                    else
//                    {
//                        return BadRequest("آیتمی یافت نشد");
//                    }
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }
//        }



//        [HttpGet]
//        [Route("GetUndefinedProducts")]/// دریافت لیست خدمات در فاکتور
//        public IActionResult GetUndefinedProducts(int FactorId, int SubFactorId)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var factror = _context.ProductFactors
//                        .Where(p => p.FactorID == FactorId && !p.IsRemoved).AsQueryable();
//                    var result = new ProductFactorGetResult();
//                    var items = factror
//                    .Where(p => p.SubFactorID == SubFactorId && !p.IsAccessory && p.IsUndefinedProduct)
//                    .Select(p => new ProductFactorGetDto
//                    {
//                        id = p.Id,
//                        Name = p.Name, // Add other properties as needed
//                        price = p.price,
//                        count = p.count,
//                        Discount = p.Discount,
//                        fee = p.fee*(1 - p.Discount * 0.01f),
//                        InsertTime = p.InsertTime,
//                        purchaseFee =0,// p.fee/2,
//                        purchasePrice = 0//p.fee * p.count / 2
//                    })
//                        .ToList();

//                    result.ProductFactorsInfo = items;

//                    if (factror != null)
//                    {
//                        return Ok(result);
//                    }
//                    else
//                    {
//                        return BadRequest("آیتمی یافت نشد");
//                    }
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }
//        }


//        [HttpGet]
//        [Route("GetServiceProducts")]/// دریافت لیست خدمات در فاکتور
//        public IActionResult GetServiceProducts(int FactorId, int SubFactorId)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var factror = _context.ProductFactors
//                        .Where(p => p.FactorID == FactorId && !p.IsRemoved).AsQueryable();
//                    var result = new ProductFactorGetResult();
//                    var items = factror
//                    .Where(p => p.SubFactorID == SubFactorId && !p.IsAccessory && !p.IsUndefinedProduct && p.IsService)
//                    .Select(p => new ProductFactorGetDto
//                    {
//                        id = p.Id,
//                        Name = p.Name, // Add other properties as needed
//                        price = p.price,
//                        count = p.count,
//                        Discount = p.Discount,
//                        fee = p.fee * (1 - p.Discount * 0.01f),
//                        InsertTime = p.InsertTime,
//                        purchaseFee = 0,// p.fee/2,
//                        purchasePrice = 0//p.fee * p.count / 2
//                    })
//                        .ToList();

//                    result.ProductFactorsInfo = items;

//                    if (factror != null)
//                    {
//                        return Ok(result);
//                    }
//                    else
//                    {
//                        return BadRequest("آیتمی یافت نشد");
//                    }
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }
//        }


//        [HttpGet]
//        [Route("AccessoryList")] //// دریافت لیست محصولات جانبی برای انتخاب کردن در قسمت افزودن محصولات جانبی
//        public IActionResult AccessoryList()
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var Accessory = _accessoryGetService.GetAccessories();
//                    return Accessory.IsSuccess ? Ok(Accessory) : BadRequest(Accessory);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }
//        }
//        [HttpGet]
//        [Route("ServiceProductList")] //// دریافت لیست محصولات جانبی برای انتخاب کردن در قسمت افزودن محصولات جانبی
//        public IActionResult ServiceProductList()
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var AccessoryList = _context.Services.Select(p=>new IdLabelDto
//                    {
//                        id = p.Id,
//                        label=p.ServiceName
//                    }
//                    ).ToList();
//                    if (AccessoryList.Count == 0) { BadRequest(new ResultDto { IsSuccess = false, Message = "آیتمی وجود ندارد" }); }
//                    return Ok(new ResultDto<List<IdLabelDto>> {Data= AccessoryList,IsSuccess=true,Message="دریافت موفق" }) ;

                    

//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }
//        }
//        //

//        [HttpGet]
//        [Route("AccessoryGet")] //// دریافت  محصول جانبی
//        public IActionResult AccessoryGet(long FactorId, long SubfactorId, long ProductId)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var Accessory = _accessoryGetService.GetForEdit(FactorId, SubfactorId, ProductId);
//                    return Accessory.IsSuccess ? Ok(Accessory) : BadRequest(Accessory);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }
//        }






//        [HttpGet]
//        [Route("ServiceProductGet")] //// دریافت  محصول جانبی
//        public IActionResult ServiceProductGet(long FactorId, long SubfactorId, long ProductId)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var Product = _context.ProductFactors.Where(p => p.FactorID == FactorId && p.SubFactorID == SubfactorId && p.Id == ProductId && !p.IsRemoved && !p.IsAccessory && !p.IsUndefinedProduct && p.IsService)
//                .FirstOrDefault();
//                    if (Product == null)
//                    {
//                        return BadRequest(new ResultDto { IsSuccess = false, Message = "آیتمی موجود نیست" });
//                    }
//                    var productDetails = NewtonsoftJson.JsonConvert.DeserializeObject<DescriptionImage>(Product.ProductDetails);
//                    List<string> imageUrls = string.IsNullOrWhiteSpace(productDetails.Image)
//                     ? new List<string>() // Return an empty list if the string is null or empty
//                     : productDetails.Image
//                     .Split(',', StringSplitOptions.RemoveEmptyEntries) // Split by comma, removing empty entries
//                     .Select(url => url.Trim()) // Trim whitespace from each URL
//                     .ToList();
//                    var ProductItem = new IdLabelString { id = productDetails.Id, label = Product.Name };
//                    var Result = new UndefinedProduct
//                    {
//                        ProductItem = ProductItem,
//                        fee = Product.fee,
//                        price = Product.price,
//                        Discount = Product.Discount,
//                        count = Product.count,
//                        Description = productDetails.Description,
//                        Image = imageUrls,//imageUrls,productDetails.Image

//                    };
//                    return Ok(new ResultDto<UndefinedProduct> { Data = Result, IsSuccess = true, Message = "دریافت موفق" });
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }
//        }




//        [HttpGet]
//        [Route("UndefinedProductGet")] //// دریافت  محصول جانبی
//        public IActionResult UndefinedProductGet(long FactorId, long SubfactorId, long ProductId)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var Product = _context.ProductFactors.Where(p => p.FactorID == FactorId && p.SubFactorID == SubfactorId && p.Id == ProductId && !p.IsRemoved && !p.IsAccessory && p.IsUndefinedProduct)
//                .FirstOrDefault();
//                    if (Product == null)
//                    {
//                        return BadRequest(new ResultDto { IsSuccess = false, Message = "آیتمی موجود نیست" });
//                    }
//                    var productDetails = NewtonsoftJson.JsonConvert.DeserializeObject<DescriptionImage>(Product.ProductDetails);

//                    List<string> imageUrls = string.IsNullOrWhiteSpace(productDetails.Image)
//                     ? new List<string>() // Return an empty list if the string is null or empty
//                     : productDetails.Image
//                     .Split(',', StringSplitOptions.RemoveEmptyEntries) // Split by comma, removing empty entries
//                     .Select(url => url.Trim()) // Trim whitespace from each URL
//                     .ToList();
//                    var ProductItem = new IdLabelString { id = productDetails.Id, label = Product.Name };
//                    var Result = new UndefinedProduct
//                    {
//                        ProductItem = ProductItem,
//                        fee=Product.fee,
//                        price=Product.price,
//                        Discount=Product.Discount,
//                        count = Product.count,
//                        Description= productDetails.Description,
//                        Image= imageUrls,//imageUrls,productDetails.Image

//                    };
//                    return Ok(new ResultDto<UndefinedProduct> {Data= Result, IsSuccess = true, Message = "دریافت موفق" });
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }
//        }
//        private class UndefinedProduct
//        {
//            public IdLabelString ProductItem { get; set; }
//            public float fee { get; set; }
//            public float Discount { set; get; }
//            public int count { get; set; }
//            public float price { set; get; }
//            public string Description { get; set; }
//            //public string Image {  get; set; }
//            public List<string> Image { get; set; }

//        }

//        [HttpPost]
//        [Route("RemoveFactor")]
//        public IActionResult RemoveFactor(FactorRemoveRequest request)
//        {

//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var Result = _factorRemoveService.Execute(new FactorRemoveRequest { FactorId = request.FactorId });
//                    return Result.IsSuccess ? Ok(Result.Message) : BadRequest(Result.Message);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }

//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }
//        }

//        [HttpPost]
//        [Route("RemoveSubFactor")]
//        public async Task<IActionResult> RemoveSubFactorAsync(SubFactorRemoveRequest request)
//        {

//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var Result = await _subFactorRemoveService.Execute(new SubFactorRemoveRequest { SubFactorId = request.SubFactorId });
//                    return Result.IsSuccess ? Ok(Result.Message) : BadRequest(Result.Message);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {
//                return Unauthorized("احراز هویت انجام نشده است");
//            }
//        }

//        [HttpPost]
//        [Route("RemoveProductFactor")]
//        public async Task<IActionResult> RemoveProductFactorAsync(ProductFactorRemoveRequest request)
//        {

//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var result = await _productFactorRemoveService.ExecuteAsync(new ProductFactorRemoveRequest { ProductFactorId = request.ProductFactorId });
//                    return result.IsSuccess ? Ok(result.Message) : BadRequest(result.Message);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {

//                return Unauthorized("احراز هویت انجام نشده است");
//            }
//        }



//        [HttpPost]
//        [Route("CountDiscount")]
//        public async Task<IActionResult> CountDiscountAsync(RequestCountDiscountChanging request)
//        {

//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var result = await _countDiscountChangingService.ExecuteAsync(request);
//                    return result.IsSuccess ? Ok(result) : BadRequest(result);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {

//                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
//            }
//        }

//        [HttpPost]
//        [Route("FinalizeOrder")]
//        public IActionResult FinalizeOrder(FinalizeOrderRequest request)
//        {
//            if (User.Identity.IsAuthenticated)
//            {


//                var Result = _finalizeOrderService.Finalize(request);
//                return Result.Result.IsSuccess ? Ok(Result.Result) : BadRequest(Result.Result);


//            }
//            else
//            {
//                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
//            }

//        }



//        [HttpPost("AcessoryServiceRecord")]
//        public async Task<IActionResult> AcessoryServiceRecord()
//        {
//            // Fetching the data grouped by state, ensuring state is between 0 and 4
//            try
//            {
//                //var Result = _cashPymentSaveService.Execute(request, client);
//                var Result = _paymentService.AcessoryServiceAddItems( client, 126);

//                return Result.Result.IsSuccess ? Ok(Result.Result) : BadRequest(Result.Result);
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }


//        [HttpPost("Pyment")]
//        public async Task<IActionResult> Pyment(PymentRequestService request)
//        {
//            // Fetching the data grouped by state, ensuring state is between 0 and 4
//            try
//            {
//                //var Result = _cashPymentSaveService.Execute(request, client);
//                var Result = _paymentService.Payment(request, client);

//                return Result.Result.IsSuccess ? Ok(Result.Result) : BadRequest(Result.Result);
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }


//        [HttpPost("NonCashPyment")]
//        public async Task<IActionResult> NonCashPyment(NonCashRequestService request)
//        {
//            // Fetching the data grouped by state, ensuring state is between 0 and 4
//            try
//            {
//                //var Result = _cashPymentSaveService.NonCashPayment(request, client);
//                var Result = _paymentService.TotalCheckPayment(request, client);

//                return Result.Result.IsSuccess ? Ok(Result.Result) : BadRequest(Result.Result);
//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }


//        [HttpPost("CheckPyment")]
//        public async Task<IActionResult> CheckPyment(CheckPymentRequestService request)
//        {
//            // Fetching the data grouped by state, ensuring state is between 0 and 4
//            try
//            {
//                var Result = _cashPymentSaveService.ExecuteCheck(request, client);
//                if (!Result.Result.IsSuccess)
//                {
//                    return Ok(Result.Result);
//                }

//                // Populate the result object


//                return Ok(Result.Result);

//            }
//            catch
//            {
//                // Log the exception (ex)
//                return StatusCode(500, "An error occurred while processing your request.");
//            }
//        }





//        [HttpGet("BanksGet")]
//        public async Task<IActionResult> BanksGet()
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    string userEmail = User.FindFirstValue(ClaimTypes.Email);

//                    var user = _userManager.FindByEmailAsync(userEmail).Result;
//                    if (user.BranchCode == 0)
//                    {
//                        return BadRequest(new ResultDto { IsSuccess = false, Message = "شما عضو شعبه نیستید" });
//                    }
//                    var result = _getBanksService.BanksGetList(client, user.BranchCode);
//                    return result.Result.IsSuccess ? Ok(result.Result) : BadRequest(result.Result);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {

//                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
//            }

//        }

//        [HttpGet("PymentPageInfo")]
//        public IActionResult PymentPageInfo(long factorId)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var result = _pymentPageInfoGetService.GetInfo(new PymentInfoRequest { FactorID = factorId });
//                    return result.IsSuccess ? Ok(result) : BadRequest(result);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {

//                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
//            }
//        }



//        [HttpGet("CheckInfoGet")]
//        public IActionResult CheckInfoGet(int Number, long FactorId, float Amount)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var result = _pymentPageInfoGetService.GetCheckInformation(new CheckRequestDto { number = Number, factorId = FactorId, amount = Amount });
//                    return result.IsSuccess ? Ok(result) : BadRequest(result);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {

//                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
//            }
//        }




//        [HttpGet("NonCashCheck")]
//        public IActionResult NonCashCheck(int Number, long FactorId)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var result = _pymentPageInfoGetService.GetNonCashCheckInformation(new CheckRequestDto { number = Number, factorId = FactorId });
//                    return result.IsSuccess ? Ok(result) : BadRequest(result);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {

//                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
//            }
//        }





//        [HttpGet("FactorComplementationGet")]
//        public IActionResult FactorComplementationGet(long productId,int complementaryType)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var result = _factorComplementationFieldsGetService.GetFields( productId, complementaryType);
//                    return result.Result.IsSuccess ? Ok(result.Result) : BadRequest(result.Result);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {

//                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
//            }
//        }


//        [HttpGet("FactorComplementationType")]
//        public IActionResult FactorComplementationType()
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var result = _factorComplementationFieldsGetService.GetComplementaryTypes();
//                    return result.Result.IsSuccess ? Ok(result.Result) : BadRequest(result.Result);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {

//                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
//            }
//        }


        
//        [HttpPost("FactorComplementationSet")]
//        public IActionResult FactorComplementationSet(SetFactorComplementaryRequest request)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var result = _factorComplementarySetService.SetComplementary(request);
//                    return result.Result.IsSuccess ? Ok(result.Result) : BadRequest(result.Result);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {

//                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
//            }
//        }

//        [HttpPost("FactorComplementationRemove")]
//        public IActionResult FactorComplementationRemove(RequestId request)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var result = _factorComplementarySetService.RemoveComplementary(request);
//                    return result.Result.IsSuccess ? Ok(result.Result) : BadRequest(result.Result);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {

//                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
//            }
//        }

//        [HttpPost("FactorContractSet")]
//        public IActionResult FactorContractSet(ContractSetRequestDto request)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    var result = _factorContractSetService.ContractSet(request);
//                    return result.IsSuccess ? Ok(result) : BadRequest(result);
//                }
//                catch
//                {
//                    // Log the exception (ex)
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//                }
//            }
//            else
//            {

//                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
//            }
//        }

//        [HttpGet("PurchasedFactors")]
//        public IActionResult PurchasedFactors([FromQuery] int pageNumber, int pageSize, string? search)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                try
//                {
//                    string userEmail = User.FindFirstValue(ClaimTypes.Email);

//                    var user = _userManager.FindByEmailAsync(userEmail).Result;
//                    if (user.BranchCode == 0)
//                    {
//                        return BadRequest(new ResultDto { IsSuccess = false, Message = "شما عضو شعبه نیستید" });
//                    }

//                    var data = from factor in _context.MainFactors
//                                join customer in _context.CustomerInfo
//                                on factor.CustomerID equals customer.CustomerID into customerGroup
//                                from customer in customerGroup.DefaultIfEmpty() // Left join for handling null customers
//                                where factor.BranchCode == user.BranchCode && factor.position && factor.status
//                                // Apply search filtering before projection
//                                && (string.IsNullOrWhiteSpace(search) ||
//                                    factor.WorkName.Contains(search) ||
//                                    (customer != null && (
//                                        customer.Name.Contains(search) ||
//                                        customer.LastName.Contains(search) ||
//                                        customer.phone.Contains(search))
//                                    )
//                                )
//                                select new
//                                {
//                                    factorId=factor.Id,
//                                    workName = factor.WorkName,
//                                    price = factor.TotalAmount,
//                                    customerName = customer != null ? $"{customer.Name} {customer.LastName}" : null,
//                                    customerPhone = customer != null ? customer.phone : null,
//                                    purchaseTime=factor.LastConnectionTime,
//                                };
                    
//                    // Get total count before pagination
//                    int totalRecords = data.Count();
//                    int pageCount = (totalRecords + pageSize - 1) / pageSize; // Alternative to `Math.Ceiling`

//                    // Apply pagination
//                    int skip = (pageNumber - 1) * pageSize;
//                    var pagedFactors = data.OrderByDescending(p=>p.purchaseTime).Skip(skip).Take(pageSize).ToList();

//                    // Prepare response
//                    var factorData = new
//                    {
//                        Factors = pagedFactors,
//                        PageCount = pageCount,
//                        Count = totalRecords
//                    };

//                    return Ok(new ResultDto<object>
//                    {
//                        Data = factorData,
//                        IsSuccess = true,
//                        Message = "دریافت موفق"
//                    });
//                }
//                catch
//                {
//                    return StatusCode(500, new ResultDto { IsSuccess = false, Message = "دریافت ناموفق" });
//                }
//            }
//            else
//            {
//                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
//            }
//        }


//        [HttpGet("GetCustomers")]//         نمایش مشتریان بصورت لیستی در بخش سفارشات پنل نماینده
//        public async Task<IActionResult> GetCustomers([FromQuery] int pageNumber, int pageSize, string? search)
//        {
//            if (!User.Identity.IsAuthenticated)
//            {
//                return Unauthorized(new ResultDto { IsSuccess = false, Message = "احراز هویت انجام نشده است" });
//            }

//            try
//            {
//                string userEmail = User.FindFirstValue(ClaimTypes.Email);
//                var result = await _customerGetService.BranchCustomersAsync(userEmail, pageNumber, pageSize, search);

//                if (result.IsSuccess)
//                {
//                    return Ok(result);
//                }
//                return BadRequest(result);
//            }
//            catch (Exception ex)
//            {
//                // Log the exception (ex)
//                return StatusCode(500, new ResultDto { IsSuccess = false, Message = "خطای ناگهانی" });
//            }
//        }






        


//    }
//}
