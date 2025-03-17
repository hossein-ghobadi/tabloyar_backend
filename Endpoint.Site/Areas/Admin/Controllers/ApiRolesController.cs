using Endpoint.Site.Areas.Admin.Models.AdminViewModel.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Queries.ContentGet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Users;
using System.Security.Claims;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Route("Admin/api/[controller]")]
    [ApiController]
    
    public class ApiRolesController:Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IDataBaseContext _context;

        public ApiRolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IDataBaseContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }


        [HttpGet]
        [Route("GetRoles")]
        [Authorize(Policy = "GetRoles")]
        public IActionResult GetRoles()
        {
            var tempt = _roleManager.Roles.ToList();
            var rolenames = new List<RoleNameGetDto>();
            foreach (var item in tempt)
            {
                rolenames.Add(new RoleNameGetDto
                {
                    id = item.Id,
                    label = item.Name

                });
            }
            return Ok(rolenames);

        }

        [HttpPost]
        [Route("SetRoles")]
        [Authorize(Policy = "SetRoles")]
        public IActionResult SetRoles(RolesViewModel newRoles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityRole newrole = new IdentityRole()
            {
                Name = newRoles.id,

            };
            var RoleCheck = _roleManager.FindByNameAsync(newRoles.id);

            if (RoleCheck.Result == null)
            {
                var res = _roleManager.CreateAsync(newrole).Result;
                if (res.Succeeded)
                {
                    var claiminfo = _context.ClaimInfos.ToList();
                    var role = _roleManager.FindByNameAsync(newRoles.id).Result;
                    if (role != null)
                    {
                        foreach (var claim in claiminfo)
                        {
                            Claim newClaim = new Claim(claim.ClaimName1, "0", ClaimValueTypes.String);
                            var claimres = _roleManager.AddClaimAsync(role, newClaim);
                            if (claimres.Result == null)
                            {
                                return BadRequest("عملیات ثبت ادعا با مشکل مواجه شد");
                            }
                        }
                        return Ok("نقش جدید با موفقیت ثبت شد");
                    }

                }
                foreach (var item in res.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
            }
            return BadRequest("این نقش قبلا ثبت شده است");
        }

        [HttpPost]
        [Route("RemoveRole")]
        [Authorize(Policy = "RemoveRole")]
        public IActionResult RemoveRole(RolesViewModel existrole)
        {
            if (!ModelState.IsValid)
            {
                var Message = new
                {
                    IsSuccess = false,
                    Message = "Error"
                };
                return BadRequest(Message);
            }

            var role = _roleManager.FindByNameAsync(existrole.id).Result;
            if( role != null)
            {
                if (role.Name != RoleConstantName.SiteUser && role.Name != RoleConstantName.Admin)
                {
                    var users = _userManager.GetUsersInRoleAsync(role.Id).Result;
                    if (!users.Any())
                    {
                        foreach (var u in users)
                        {

                            var currentRoles = _userManager.GetRolesAsync(u).Result;
                            var removeFromRolesResult = _userManager.RemoveFromRolesAsync(u, currentRoles).Result;
                            var SetRole = _userManager.AddToRoleAsync(u, RoleConstantName.SiteUser);

                        }
                    }
                }
                else
                {
                    var Message = new
                    {
                        IsSuccess = false,
                        Message = "نقش های ادمین و کاربر عادی قابل حذف نیستند"
                    };
                    return BadRequest(Message);
                }
                var res = _roleManager.DeleteAsync(role).Result;
                Console.WriteLine(res.Succeeded);
                if (res.Succeeded)
                {
                    var Message = new
                    {
                        IsSuccess = true,
                        Message = "نقش مورد نظر با موفقیت حذف شد"
                    };
                    return Ok(Message);
                }
                else
                {
                    var Message = new
                    {
                        IsSuccess = false,
                        Message = "Error"
                    };
                    return Ok(Message);
                }
            }
            var Result = new
            {
                IsSuccess = false,
                Message = "Error"
            };
            return BadRequest(Result);

        }



        

    }
}
