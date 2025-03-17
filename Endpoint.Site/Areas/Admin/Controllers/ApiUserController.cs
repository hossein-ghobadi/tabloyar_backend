using Endpoint.Site.Areas.Admin.Models.AdminViewModel.Role;
using Endpoint.Site.Areas.Admin.Models.AdminViewModel.User;
using Endpoint.Site.Areas.Proxy.Models;
using Endpoint.Site.Models.ViewModels.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.SMS.Commands;
using Radin.Common.Dto;
using Radin.Common.StaticClass;
using Radin.Domain.Entities.Branches;
using Radin.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static Radin.Application.Services.ProductItems.Queries.ChannelliumGet.ChannelliumGet;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Route("Admin/api/[controller]")]
    [ApiController]
    public class ApiUserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDataBaseContext _context;
        public ApiUserController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IDataBaseContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }


        [HttpPost]
        [Route("Register")]
        [Authorize(Policy = "Register")]
        public IActionResult Register(AdminRegisterView model)
        {

            var validationErrors = model.Validate();
            var users = _userManager.Users.ToList();
            var duplicates = users.FirstOrDefault(u => u.PhoneNumber == model.phone);
            int phoneDigits = model.phone.Length;
            int id = validationErrors.Count();
            if (phoneDigits != 11)
            {
                id = id +1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = "!تعدا ارقام شماره تماس باید 11 رقم باشد"
                });
            }

            if (duplicates != null)
            {
                id = id +1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = "!این شماره تماس تکراری است"
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

            User newUser = new User()
            {
                Email = model.email,
                UserName = model.name,
                FullName = model.fullName,
                PhoneNumber = model.phone,
                BranchCode = model.BranchCode
            };

            var result = _userManager.CreateAsync(newUser, model.password).Result;
            foreach (var item in result.Errors)
            {
                id = id +1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = item.Description.ToString()
                });
            }
            if (result.Succeeded)
            {
                var res1 = _userManager.FindByIdAsync(newUser.Id).Result;
                foreach(var role in model.UserRole)
                {
                    var res2 = _userManager.AddToRoleAsync(res1, role).Result;
                }
                
                _context.SaveChanges();
                return CreatedAtAction(nameof(Register), new { id = newUser.Id }, newUser);

            }
            return BadRequest(validationErrors);

        }

        [HttpPost]
        [Route("DeactiveUser")]
        [Authorize(Policy = "DeactiveUser")]
        public IActionResult DeactiveUser(DeactiveViewModel username)
        {
            var user = _userManager.FindByNameAsync(username.id).Result;
            if (user != null)
            {
                user.IsActive = false;
                var res =  _userManager.UpdateAsync(user).Result;
                if (res.Succeeded)
                {
                    var Message = new
                    {
                        IsSuccess = true,
                        Message = "کاربر مورد نظر غیر فعال شد"
                    };
                    return Ok(Message);
                }
                else
                {
                     var Message = new
                    {
                        IsSuccess = false,
                        Message = "عملیات با خطا مواجه شد"
                    };
                    //return BadRequest(res.Errors);
                    return BadRequest(Message);

                }

            }
            var Message2 = new
            {
                IsSuccess = false,
                Message = "کاربر مورد نظر وجود ندارد"
            };
            //return NotFound($"User '{username}' not found.");
            return NotFound(Message2);

        }

        [HttpPost]
        [Route("RemoveUser")]
        [Authorize(Policy = "RemoveUser")]
        public IActionResult RemoveUser(DeactiveViewModel username)
        {
            var user = _userManager.FindByNameAsync(username.id).Result;
            if (user != null)
            {
                user.IsRemove = true;
                var res = _userManager.UpdateAsync(user).Result;
                if (res.Succeeded)
                {
                    var Message = new
                    {
                        IsSuccess = true,
                        Message = "کاربر مورد نظر حذف شد"
                    };
                    return Ok(Message);
                    //return Ok("کاربر مورد نظر حذف شد");
                }
                else
                {
                    var Message = new
                    {
                        IsSuccess = false,
                        Message = "عملیات با خطا مواجه شد"
                    };
                    return BadRequest(Message);
                    //return BadRequest(res.Errors);
                }

            }
            var Message2 = new
            {
                IsSuccess = false,
                Message = "کاربر مورد نظر وجود ندارد"
            };
            return NotFound(Message2);
            //return NotFound($"User '{username}' not found.");
        }

        [HttpGet]
        [Route("GetUsers")]
        [Authorize(Policy ="GetUsers")]
        public IActionResult GetUsers([FromQuery]  int PageNumber, int PageSize,string? search,string? RoleName)
        {
            var data = _userManager.Users.ToList();

            var userDtos = new List<UserGetDto>();

            if (!string.IsNullOrEmpty(RoleName))
            {
                data = _userManager.GetUsersInRoleAsync(RoleName).Result.ToList();
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                data = data.Where(p => p.Phone?.Contains(search) ?? false || p.PhoneNumber.Contains(search) || p.FullName.Contains(search)).ToList();
                
            }

            int count = data.Count();
            int remainder = count % PageSize;
            int PageCount = 0;
            if (remainder > 0)
            {
                PageCount = (count / PageSize) + 1;
            }
            else
            {
                 PageCount = count/ PageSize;
            }

            int skip = (PageNumber - 1) * PageSize;

            var users = data
                    .Skip(skip)
                    .Take(PageSize)
                    .ToList();

            foreach (var user in users)
            {
                var role = _userManager.GetRolesAsync(user).Result;
                string rolesString = string.Join(" / ", role);
                userDtos.Add(new UserGetDto
                {
                    id = user.UserName,
                    FullName = user.FullName,
                    Phone = user.PhoneNumber,
                    IsActive = user.IsActive,
                    IsRemove = user.IsRemove,
                    Role = rolesString,
                    BranchCode = user.BranchCode,
                    Email = user.Email,
                });
            }
                var result = new UsersGetInfo
                {
                    UsersInfo = userDtos,
                    count = count,
                    PageCount = PageCount
                };

            
            return Ok(result);
        }
        [HttpGet]
        [Route("GetUser")]
        [Authorize(Policy = "GetUser")]
        public IActionResult GetUser(string username)
        {
            
            var user = _userManager.FindByNameAsync(username).Result;
            var index=0;
            if (user != null)
            {
                var roles = _userManager.GetRolesAsync(user).Result;
                var roleDtos = roles.Select((role, index) => new RoleNameGetDto
                {
                    id = (index + 1).ToString(), // Convert the index + 1 to a string for the ID
                    label = role
                }).ToList();
                
                var ProvinceName=_context.Cities.Where(p=>p.ProvinceId==user.Province).FirstOrDefault()?.province;
                var cityName= _context.Cities.Where(p => p.Id == user.City).FirstOrDefault()?.city;
                

                var usersDtos = new GetUsersDto{

                    username = username,
                    email = user.Email,
                    fullname = user.FullName,
                    phone = user.PhoneNumber,
                    phone2 = user.Phone,
                    gender = user.Gender,
                    job = user.Job,
                    state = new IdLabelDto
                    {
                        id = user.Province ?? 0,
                        label = ProvinceName ?? null

                    },
                    city = new IdLabelDto
                    {
                        id = user.City ?? 0,
                        label = cityName ?? null

                    },
                    
                    address = user.Address,
                    IsActive=user.IsActive,
                    IsRemove = user.IsRemove,
                     role =roleDtos,

                    Latitude=user.Latitude,
                    Longitude=user.Longitude,
                    BranchCode = user.BranchCode,

                };
                
                string UserAge = null;
                if (user.Age != null)
                {
                    
                    UserAge = SimpleMethods.DateTimeToTimeStamp(user.Age.Value).ToString();
                    usersDtos.age = Convert.ToInt64(UserAge);

                }
                return Ok(usersDtos);
            }
            return NotFound($"User '{username}' not found.");

        }

        [HttpPost]
        [Route("EditUser")]
        [Authorize(Policy = "EditUser")]
        public IActionResult EditUser(AdminEditViewModel mdl)
        {
            var validationErrors = mdl.Validate();
            var user = _userManager.FindByEmailAsync(mdl.email).Result;
            var users = _userManager.Users.ToList();
            int phoneDigits = mdl.phone.Length;
            int id = validationErrors.Count();
            if (phoneDigits != 11)
            {
                id = id + 1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = "!تعدا ارقام شماره تماس باید 11 رقم باشد"
                });
            }
            var OtherUsers = users.Remove(user);
            if (OtherUsers)
            {
                var duplicates = users.FirstOrDefault(u => u.PhoneNumber == mdl.phone);
                var DupName = users.FirstOrDefault(u => u.UserName == mdl.username);
                if (duplicates != null)
                {
                    id = id + 1;
                    validationErrors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!این شماره تماس در سیستم به نام شخص دیگری ثبت می باشد"
                    });
                }
                if (DupName != null)
                {
                    id = id + 1;
                    validationErrors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!این نام کاربری در سیستم به نام شخص دیگری ثبت می باشد"
                    });
                }

            }
            bool containsOnlyDigits = mdl.phone.All(char.IsDigit);
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
            
            if (user != null)
            {
                
                var currentRoles = _userManager.GetRolesAsync(user).Result;
                var rolesToAdd = mdl.UserRole.Except(currentRoles).ToList();
                var rolesToRemove = currentRoles.Except(mdl.UserRole).ToList();

                if (rolesToRemove.Any())
                {
                    var removeResult = _userManager.RemoveFromRolesAsync(user, rolesToRemove).Result;
                    if (!removeResult.Succeeded)
                    {
                        return BadRequest(removeResult.Errors);
                    }
                }
                if (rolesToAdd.Any())
                {
                    var addResult = _userManager.AddToRolesAsync(user, rolesToAdd).Result;
                    if (!addResult.Succeeded)
                    {
                        return BadRequest(addResult.Errors);
                    }
                }

                
                user.UserName = mdl.username;
                user.FullName = mdl.fullName;
                user.PhoneNumber = mdl.phone;
                user.Phone = mdl.phone2;
                user.Gender = mdl.gender;
                user.Job = mdl.job;
                user.Province = mdl.state;
                user.City = mdl.city;
                if (mdl.age != null)
                {
                  
                    DateTime AgeDateTime = SimpleMethods.InsertDateTime(mdl.age.ToString());

                    user.Age = AgeDateTime;

                }
                user.Address = mdl.address;
                user.BranchCode = user.BranchCode;
                _context.SaveChanges();
                var res = _userManager.UpdateAsync(user).Result;
                if (res.Succeeded)
                {
                    return Ok("اطلاعات کاربر مورد نظر با موفقیت ویرایش شد");
                }

            }
            return NotFound($"User '{mdl.username}' not found.");

        }

    }

}
