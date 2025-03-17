using OfficeOpenXml.Drawing.Style.Fill;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Branch.Queries.BranchInfoGetService;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Factors.Queries.OrderGet
{
    public interface ISubFactorGetService
    {

        ResultDto<SubFactorGetResult> Execute(SubFactorGetRequest request);

    }

    public class SubFactorGetService : ISubFactorGetService
    {

        private readonly IDataBaseContext _context;
        public SubFactorGetService(IDataBaseContext Context)
        {
            _context = Context;

        }

        public ResultDto<SubFactorGetResult> Execute(SubFactorGetRequest request)
        {
            string workname = _context.MainFactors.FirstOrDefault(i => i.Id ==request.FactorId).WorkName;
            var subfactors = _context.SubFactors.AsQueryable();
            var SubFactors = _context.SubFactors.Where(f => f.FactorID == request.FactorId && f.IsRemoved == false).ToList();
            var SubFactorsList = SubFactors.Select(p => new SubFactorGetDto
            {
                id=p.Id,
                WorkName = workname,
                description = p.Description,
                Amount = p.Amount,
                InsertTime = p.InsertTime

            }).ToList().OrderByDescending(o => o.InsertTime).ToList();


                return new ResultDto<SubFactorGetResult>()
                {
                    Data = new SubFactorGetResult
                    {
                        SubFactorsInfo = SubFactorsList,
                    },
                    IsSuccess = true,
                    Message = " دریافت موفقیت آمیز "

                };
        }



    }

        public class SubFactorGetRequest
        {
            public long FactorId { get; set; }
        }

        public class SubFactorGetDto
        {
        public long id { get; set; }
            public string WorkName { get; set; }
            public string description { get; set; }
            public float Amount { get; set; }
            public DateTime InsertTime {  get; set; }
            
        }

        public class SubFactorGetResult
        {
            public List<SubFactorGetDto> SubFactorsInfo { get; set; }
        }

    }
