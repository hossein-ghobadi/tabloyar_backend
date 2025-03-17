using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Claims.Commands.ClaimCategorySetService;
using Radin.Application.Services.Claims.Commands.ClaimSetService;
using Radin.Application.Services.Claims.Queries.ClaimCategoryGetService;
using Radin.Application.Services.Claims.Queries;
using Radin.Application.Services.Factors.Commands.StatusReason;
using Radin.Domain.Entities.Users;
using Radin.Application.Services.Factors.Queries.StatusReasonGet;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Route("Admin/api/[controller]")]
    [ApiController]
    public class ApiFactorController : ControllerBase
    {
        private readonly IStatusReasonSetService _statusReasonSet;
        private readonly IStatusRasonGetService _statusRasonGetService;
        private readonly IStatusReasonRecoveryService _statusReasonRecoveryService;
        public ApiFactorController(
            IStatusReasonSetService statusReasonSet,
            IStatusRasonGetService statusRasonGetService,
            IStatusReasonRecoveryService statusReasonRecoveryService
            )
        {
            _statusReasonSet = statusReasonSet;
            _statusRasonGetService = statusRasonGetService;
            _statusReasonRecoveryService = statusReasonRecoveryService;
        }

        [HttpPost]
        [Route("SetStatusReason")]
        public IActionResult SetStatusReason(RequestStatusReasonSetDto requestStatusReasonSetDto)
        {
            var res = _statusReasonSet.Execute(requestStatusReasonSetDto);
            if (res.IsSuccess)
            {
                return Ok(res);
            }
            return BadRequest("درج علت ناموفق");
        }

        [HttpGet]
        [Route("GetStatusReason")]
        public IActionResult GetStatusReason(long FactorId)
        {
            var res = _statusRasonGetService.Execute(new RequestStatusReasonGetDto { FactorId = FactorId });
            return Ok(res);

        }

        [HttpPost]
        [Route("RecoveryStatusReason")]
        public IActionResult RecoveryStatusReason(long FactorId)
        {
            var res = _statusReasonRecoveryService.Execute(new RequestStatusReasonRecoveryDto { FactorId = FactorId });
            return Ok(res);

        }

    }
}
