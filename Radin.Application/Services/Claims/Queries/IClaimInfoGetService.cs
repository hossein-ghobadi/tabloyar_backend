using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Queries.ContentGet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Claim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Claims.Queries
{
    public interface IClaimInfoGetService
    {
        ResultDto<ResultClaimInfoGetDto> Execute(RequestClaimInfoGetDto request);
    }

    public class ClaimInfoGetService : IClaimInfoGetService
    {
        private readonly IDataBaseContext _context;
        public ClaimInfoGetService(IDataBaseContext Context)
        {
            _context = Context;

        }

        public ResultDto<ResultClaimInfoGetDto> Execute(RequestClaimInfoGetDto request)
        {
            int rowsCount = 0;
            int count = _context.ClaimInfos.Count();
            var claims = _context.ClaimInfos.AsQueryable();
            var ClaimCategories = _context.ClaimCategories.AsQueryable();
            int remainder = count % request.PageSize;
            int PageCount = 0;


            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                claims = claims.Where(p => p.ClaimName1.Contains(request.SearchKey) || p.ClaimName2.Contains(request.SearchKey));
                count = claims.Count();
                remainder = count % request.PageSize;
                if (remainder > 0)
                {
                    PageCount = (count / request.PageSize) + 1;
                }
                else
                {
                    PageCount = count / request.PageSize;
                }
            }
            
            else
            {
                remainder = count % request.PageSize;
                if (remainder > 0)
                {
                    PageCount = (count / request.PageSize) + 1;
                }
                else
                {
                    PageCount = count / request.PageSize;
                }
            }


            var ClaimsList = claims.Select(p => new GetClaimInfoDto
            {
                id = p.Id,
                category = ClaimCategories.FirstOrDefault(c => c.Id == p.category).Description,
                ClaimName1 = p.ClaimName1,
                ClaimName2 = p.ClaimName2,

            }).ToList();
            int skip = (request.PageNumber - 1) * request.PageSize;

            var ClaimSubList = ClaimsList.Skip(skip).Take(request.PageSize).ToList();


            return new ResultDto<ResultClaimInfoGetDto>
            {
                Data = new ResultClaimInfoGetDto
                {
                    Rows = PageCount,
                    Info = ClaimSubList,
                    count = count,
                },
                IsSuccess = true,
                Message = "",

            };



        }


    }




    public class RequestClaimInfoGetDto
    {
        public string SearchKey { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class ResultClaimInfoGetDto
    {
        public List<GetClaimInfoDto> Info { get; set; }
        public int Rows { get; set; }
        public int count { get; set; }

    }

    public class GetClaimInfoDto
    {
        public long id { get; set; }
        public string? category { get; set; }
        public string ClaimName1 { get; set; }
        public string ClaimName2 { get; set;}

    }

}

 

