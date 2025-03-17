using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Queries.CommentInfoGet;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Branch.Queries.BranchInfoGetService
{
    public interface IBranchGetCodeService
    {
        ResultDto<List<GetBranchCodeDto>> Execute();/// public using
    }

    public class BranchGetCodeService : IBranchGetCodeService
    {
        private readonly IDataBaseContext _context;
        public BranchGetCodeService(IDataBaseContext Context)
        {
            _context = Context;

        }

        public ResultDto<List<GetBranchCodeDto>> Execute()
        {
            var branches = _context.BranchINFOs;

            //int rowsCount = 0;
            var branchesList = branches.Select(p => new GetBranchCodeDto
            {
                BranchCode = p.BranchCode,
                BranchName = p.BranchName,



            }).ToList();
            return new ResultDto<List<GetBranchCodeDto>>
            {
                Data = branchesList,
                IsSuccess = true,
                Message = "",

            };

        }


    }

    public class GetBranchCodeDto

    {
        public long BranchCode { get; set; }
        public string BranchName { get; set; }


    }
}
