
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Queries.CategoryGet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Contents.Queries.HomeContentGet
{
    public interface IHomeContentGetService
    {
        ResultDto<List<GetHomeContentDto>> Execute();
    }
    public class HomeContentGetService : IHomeContentGetService
    {
        private readonly IDataBaseContext _context;
        public HomeContentGetService(IDataBaseContext Context)
        {
            _context = Context;

        }

        public ResultDto<List<GetHomeContentDto>> Execute()
        {
            var contents = _context.Contents;

            //int rowsCount = 0;
            var contentsList = contents.Select(p => new GetHomeContentDto
            {
                //ContentId = p.Id,
                //CategoryId = p.CategoryId,
                ContentTitle = p.ContentTitle,
                CateoryTitle = p.CategoryTitle,
                ContentUniqName = p.ContentUniqeName,
                ContentSorting = p.ContentSorting,
                ContentLongDesc = p.ContentLongDescription.Substring(0, 100),
                ContentImage = p.ContentImage,

            }).ToList();
            return new ResultDto<List<GetHomeContentDto>>
            {
                Data = contentsList,
                IsSuccess = true,
                Message = "",

            };

        }


    }

    public class GetHomeContentDto
    {
        //public long ContentId { get; set; }
        //public long CategoryId { get; set; }
        public string CateoryTitle { get; set; }
        public string ContentTitle { get; set; }
        public string ContentUniqName { get; set; }
        public int ContentSorting {  get; set; }
        public string ContentLongDesc { get; set; }
        public string ContentImage { get; set; }

    }


}


