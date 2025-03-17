using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Queries.CategoryGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Factors.Queries.StatusReasonGet
{
    public interface IStatusRasonGetService
    {
        List<GetDto> Execute(RequestStatusReasonGetDto request);
    }

    public class StatusReasonGetService : IStatusRasonGetService
    {
        private readonly IDataBaseContext _context;
        public StatusReasonGetService(IDataBaseContext Context)
        {
            _context = Context;

        }

        public List<GetDto> Execute(RequestStatusReasonGetDto request)
        {

            var factor = _context.MainFactors.FirstOrDefault(i => i.Id == request.FactorId);

            var Reasons = _context.StatusReasons.Where(s => s.status == factor.status);

            //int rowsCount = 0;
            var ReasonsList = Reasons.Select(r => new GetDto
            {
                id = r.Id,
                label = r.Reason,
            }).ToList();

            return ReasonsList;
        }

    }

    public class RequestStatusReasonGetDto
    {
        public long FactorId { get; set; }
    }

    public class GetDto
    {
        public long id { get; set; }
        public string label { get; set; }


    }
}
