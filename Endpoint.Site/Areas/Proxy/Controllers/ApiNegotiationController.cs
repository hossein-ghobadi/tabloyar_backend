using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.CRM.Commands.ExitCrm;
using Radin.Application.Services.CRM.Commands.UpdateExpiration;
using Radin.Application.Services.CRM.Queries.CrmGet;
using Radin.Application.Services.Factors.Commands.Accessory.AccessorySet;
using Radin.Application.Services.Factors.Commands.Customer;
using Radin.Application.Services.Factors.Commands.Orders.OrdersRemove;
using Radin.Application.Services.Factors.Commands.Orders;
using Radin.Application.Services.Factors.CRM.Commands.EditWorkName;
using Radin.Application.Services.Factors.Queries.AccessoryGet;
using Radin.Application.Services.Factors.Queries.CustomerGet;
using Radin.Application.Services.Factors.Queries.NegotiationGet;
using Radin.Application.Services.Factors.Queries.OrderGet;
using Radin.Domain.Entities.Users;
using static Radin.Application.Services.Factors.Commands.Customer.CustomerService;
using Radin.Application.Services.Factors.Commands.SetConnection;
using Radin.Application.Services.OtherExcelloading;
using static Radin.Application.Services.OtherExcelloading.QuestionService;
using Endpoint.Site.Models.ViewModels.CharacterTypeCalculationModels;
using Radin.Application.Services.Factors.Queries.StatusReasonGet;
using Radin.Application.Services.Factors.Commands.NegotiationSet;
using Endpoint.Site.Models.NestingViewModel.ChannelliumViewModel;
using Radin.Common.Dto;
using Sprache;
using System.Text.Json;
using System;
using Radin.Application.Services.Factors.Queries.ConnectionsGet;
using Radin.Application.Services.Factors.Commands.Pyment;
using static System.Net.WebRequestMethods;
using Radin.Application.Services.Factors.Queries.HesabfaBanksGet;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Endpoint.Site.Areas.Proxy.Controllers
{
    [Authorize(Roles = "PROXY,PROXYSELLER")]
    [Route("Proxy/api/[controller]")]
    [ApiController]
    public class ApiNegotiationController : ControllerBase
    {



        private readonly INegotiationService _negotiationService;
        private readonly ICustomerService _customerService;
        private readonly ICustomerGetService _customerGetService;
        private readonly IConnectionService _connectionService;
        private readonly IConnectionsGetService _connectionsGetService;

        private readonly INegotiationSetService _negotiationSetService;

        private readonly List<DISCQuestion> Questions;
        private readonly QuestionService _questionService;
        private readonly IStatusRasonGetService _statusRasonGetService;
        private static readonly HttpClient client = new HttpClient();

        private readonly UserManager<User> _userManager;

        public ApiNegotiationController(

            INegotiationService negotiationService,
            ICustomerService customerService,
            ICustomerGetService customerGetService,
            IConnectionService connectionService,
            IStatusRasonGetService statusRasonGetService,
            INegotiationSetService negotiationSetService,
            IConnectionsGetService connectionsGetService,
            ICashPymentSaveService cashPymentSaveService,
            IGetBanksService banksService,
            QuestionService questionService,
            UserManager<User> userManager

            )

        {

            _negotiationService = negotiationService;
            _customerService = customerService;
            _customerGetService = customerGetService;
            _connectionService = connectionService;
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CT2.xlsx");
            _questionService = questionService;
            Questions = _questionService.LoadQuestionsFromExcel(filePath);
            _statusRasonGetService = statusRasonGetService;
            _negotiationSetService = negotiationSetService;
            _connectionsGetService = connectionsGetService;           
            _userManager = userManager;

        }


        [HttpGet("GetQuestion")]
        public IActionResult GetQuestions()
        {
            if (Questions == null || !Questions.Any())
            {
                return NotFound("No questions loaded.");
            }
            return Ok(Questions);
        }

        [HttpPost("EstimateDisc")]
        public IActionResult EstimateDisc([FromBody] Dictionary<int, int> answers)
        {
            if (answers == null || !answers.Any())
            {
                return BadRequest("Answers cannot be null or empty.");
            }

            try
            {
                // Delegate processing to the service
                var initialResult = _questionService.CalculateDiscResult(answers, Questions);
                var Result = _questionService.GetHighDiscCharacter(initialResult);
                return Ok(Result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("CustomerGet")]
        public IActionResult CustomerGet(long CustomerId)
        {
            // Fetching the data grouped by state, ensuring state is between 0 and 4
            try
            {
                string userEmail = User.FindFirstValue(ClaimTypes.Email);

                var user = _userManager.FindByEmailAsync(userEmail).Result;
                var result = _customerGetService.GetForEdit(CustomerId,user.BranchCode);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }




        [HttpGet("CustomerListBySearch")]
        public IActionResult CustomerListBySearch(string? search)
        {
            // Fetching the data grouped by state, ensuring state is between 0 and 4
            try
            {
                string userEmail = User.FindFirstValue(ClaimTypes.Email);

                var user = _userManager.FindByEmailAsync(userEmail).Result;
                var result = _customerGetService.GetBySearch(search,user.BranchCode);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("CustomerNeededDataList")]
        public IActionResult CustomerNeededDataList()
        {
            // Fetching the data grouped by state, ensuring state is between 0 and 4
            try
            {
                var result = _customerGetService.GetNeededDataList();
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("AddCustomer")]
        public IActionResult AddCustomer(RequestAddCustomerDto request)
        {
            // Fetching the data grouped by state, ensuring state is between 0 and 4
            try
            {
                var result = _customerService.AddCustomer(request);
                return result.Result.IsSuccess ? Ok(result.Result) : BadRequest(result.Result);
            }
            catch
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        [HttpPost("EditCustomer")]
        public IActionResult EditCustomer(RequestAddCustomerDto request)
        {
            // Fetching the data grouped by state, ensuring state is between 0 and 4
            try
            {
                var result = _customerService.EditCustomer(request);
                return result.Result.IsSuccess ? Ok(result.Result) : BadRequest(result.Result);
            }
            catch
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }





        [HttpGet("GetNegotiationInfo")]
        public IActionResult GetNegotiationInfo(int factorId)
        {
            // Fetching the data grouped by state, ensuring state is between 0 and 4
            try
            {
                string userEmail = User.FindFirstValue(ClaimTypes.Email);

                var user = _userManager.FindByEmailAsync(userEmail).Result;
                if (user.BranchCode == 0)
                {
                    return BadRequest(new ResultDto { IsSuccess = false, Message = "شما عضو شعبه نیستید" });
                }
                var result = _negotiationService.GetInformation(factorId, user.BranchCode);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        [HttpGet("GetNeededForNegotioation")]
        public IActionResult GetNeededForNegotioation()
        {
            // Fetching the data grouped by state, ensuring state is between 0 and 4
            try
            {
                string userEmail = User.FindFirstValue(ClaimTypes.Email);

                var user = _userManager.FindByEmailAsync(userEmail).Result;
                if (user.BranchCode == 0)
                {
                    return BadRequest(new ResultDto { IsSuccess = false, Message = "شما عضو شعبه نیستید" });
                }
                var result = _negotiationService.GetNeededDataList(user.BranchCode);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        [Route("GetStatusReason")]
        public IActionResult GetStatusReason(long FactorId)
        {
            var res = _statusRasonGetService.Execute(new RequestStatusReasonGetDto { FactorId = FactorId });
            return Ok(res);

        }




        [HttpGet]
        [Route("GetContactTypeList")]
        public IActionResult GetContactTypeList()
        {
            var res = _connectionsGetService.GetConactTypeList();
            return res.IsSuccess ? Ok(res) : BadRequest(res);

        }


        [HttpPost("AddConnection")]
        public IActionResult AddConnection(AddConnectionRequest request)
        {
            // Fetching the data grouped by state, ensuring state is between 0 and 4
            try
            {
                var result = _connectionService.AddConnection(request, client);
                return result.Result.IsSuccess ? Ok(result.Result) : BadRequest(result.Result);
            }
            catch
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("ConnectionList")]
        public IActionResult ConnectionList(long factorId)
        {
            // Fetching the data grouped by state, ensuring state is between 0 and 4
            try
            {
                var result = _connectionsGetService.GetConnectionsList(factorId);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpPost("RemoveConnection")]
        public IActionResult RemoveConnection(RemoveConnectionRequest request)
        {
            // Fetching the data grouped by state, ensuring state is between 0 and 4
            try
            {
                var result = _connectionService.RemoveConnection(request, client);
                return result.Result.IsSuccess ? Ok(result.Result) : BadRequest(result.Result);
            }
            catch
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }





        [HttpPost("NegotiationSet")]
        public async Task<IActionResult> NegotiationSet(NegotiationSetRequestDto request)
        {
            // Fetching the data grouped by state, ensuring state is between 0 and 4
            try
            {
                var Result = _negotiationSetService.NegotiationSet(request, client);
                if (!Result.Result.IsSuccess)
                {
                    return Ok(Result.Result);
                }

                // Populate the result object


                return Ok(Result.Result);

            }
            catch
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        [HttpPost("PurchuseProbability")]
        public async Task<IActionResult> PurchuseProbability(ProbabilityRequestDto request)
        {
            // Fetching the data grouped by state, ensuring state is between 0 and 4
            try
            {
                var Result = _negotiationSetService.PurchaseProbability(request, client);
                if (!Result.Result.IsSuccess)
                {
                    return Ok(new ResultDto<string>
                    {
                        IsSuccess = false,
                        Message = "محاسبه احتمال ناموفق."
                    });
                }

                // Populate the result object


                return Ok(new ResultDto<float>
                {
                    Data = Result.Result.Data,
                    IsSuccess = true,
                    Message = "محاسبه احتمال موفق"
                });

            }
            catch
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


    }
}