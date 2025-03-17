using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Ideas.Commands.IdeaIndex;
using Radin.Common.Dto;
using Radin.Common.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Samples.Commands.SampleIndex
{
    public interface ISampleIndexService
    {
        ResultDto Execute(RequestById_s request);

    }

    public class SampleIndexService : ISampleIndexService
    {
        private readonly IDataBaseContext _context;

        public SampleIndexService(IDataBaseContext context)
        {
            _context = context;
        }


        public ResultDto Execute(RequestById_s request)
        {

            var sample = _context.Samples.FirstOrDefault(c => c.SampleUniqeName == request.id);
            if (sample == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "ایده مورد نظر یافت نشد"
                };
            }
            var msg = sample.IsIndex ? "ایندکس غیر فعال شد" : "ایندکس فعال شد";
            sample.IsIndex = !(sample.IsIndex);
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = msg
            };
        }

    }
}
