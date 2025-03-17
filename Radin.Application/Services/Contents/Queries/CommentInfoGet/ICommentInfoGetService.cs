using Radin.Application.Interfaces.Contexts;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Contents.Queries.CommentInfoGet
{
    public interface ICommentInfoGetService
    {
        ResultDto<List<GetCommentInfoDto>> Execute();
    }

    public class CommentInfoGetService : ICommentInfoGetService
    {
        private readonly IDataBaseContext _context;
        public CommentInfoGetService(IDataBaseContext Context)
        {
            _context = Context;

        }

        public ResultDto<List<GetCommentInfoDto>> Execute()
        {
            var contents = _context.Contents;

            //int rowsCount = 0;
            var contentsList = contents.Select(p => new GetCommentInfoDto
            {
                ContentId = p.Id,
                ContentTitle = p.ContentTitle,



            }).ToList();
            return new ResultDto<List<GetCommentInfoDto>>
            {
                Data = contentsList,
                IsSuccess = true,
                Message = "",

            };

        }


    }


    public class GetCommentInfoDto

    {
        public long ContentId { get; set; }
        public string ContentTitle { get; set; }


    }
}
