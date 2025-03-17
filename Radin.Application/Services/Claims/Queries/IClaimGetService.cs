using Microsoft.AspNetCore.Identity;
using Radin.Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Claims.Queries
{
    public interface IClaimGetService
    {
        List<CategoricalAccess> Execute(RequestClaimesDto requestClaimesDto);
    }

    public class ClaimGetService : IClaimGetService
    {
        private readonly IDataBaseContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ClaimGetService(IDataBaseContext Context, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _context = Context;

        }

        public List<CategoricalAccess> Execute(RequestClaimesDto requestClaimesDto)
        {
            var role = _roleManager.FindByNameAsync(requestClaimesDto.rolename).Result;
            var roleClaims = _roleManager.GetClaimsAsync(role).Result;
            //var result = new Result();
            var ClaimCategoryInfo = _context.ClaimCategories.ToList();
            var calimsInfo = _context.ClaimInfos.AsQueryable();
            var result = new List<CategoricalAccess>();
            foreach (var category in ClaimCategoryInfo)
            {
                var claimsList = calimsInfo.Where(c => c.category==category.Id).ToList();
                var access = new List<Access>();
                //Console.WriteLine($@"Category={category.CategoryName}");
                //Console.WriteLine("???????????????????????????????????");
                foreach (var info in claimsList)
                {
                    //Console.WriteLine($@"info={info.ClaimName1}");
                    //Console.WriteLine($@"Result={roleClaims.FirstOrDefault(rc => rc.Type == info.ClaimName1)}");
                    var roleClaimValue = roleClaims.FirstOrDefault(rc => rc.Type == info.ClaimName1).Value;
                    var accessDto = new Access
                    {
                        type = info.ClaimName1,
                        description = info.ClaimName2,
                        value = roleClaimValue,
                    };
                    access.Add(accessDto);
                }
                var categoryAccessDto = new CategoricalAccess
                {
                    id = category.CategoryName,
                    label = category.Description,
                    access = access,
                };

                
                result.Add(categoryAccessDto);

            }
            return result;
        }


    }

   

    public class CategoricalAccess
    {
        public string id { get; set; }
        public string label { get; set; }
        public List<Access> access { get; set; }
    }

    public class Access
    {
        public string type { get; set; }
        public string description { get; set; }
        public string value { get; set; }
        
    }

    public class RequestClaimesDto
    {
        public string rolename { get; set; }
    }

}
