using Microsoft.Extensions.Configuration;
using Radin.Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Radin.Application.Services.PolicyRequirementsService;

namespace Radin.Application.Services
{
    public interface IPolicyRequirementsService
    {
        IEnumerable<PolicyRequirement> GetPolicyRequirements();
    }

    public class PolicyRequirementsService : IPolicyRequirementsService
    {
        private readonly IDataBaseContext _context;

        public PolicyRequirementsService(IDataBaseContext context)
        {
            _context = context;
        }

        public IEnumerable<PolicyRequirement> GetPolicyRequirements()
        {
            // Load from configuration, database, or other data source
            var Result= _context.ClaimInfos.Select(p=>new  PolicyRequirement {

                PolicyName=p.ClaimName1,
                ClaimType=p.ClaimName1,
                ClaimValue= "1"


            }).ToList();
            
            return Result;
        }

        public class PolicyRequirement
        {
            public string PolicyName { get; set; }
            public string ClaimType { get; set; }
            public string ClaimValue { get; set; }
        }
       
    }
}
