//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Contents.Commands.ContentSet;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Comments;
//using Radin.Domain.Entities.Contents;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.Design;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;

//namespace Radin.Application.Services.Contents.Queries.HomeContentGet
//{
//    public interface IHomeUniqeContentGetService
//    {
//        ResultDto<GetContentUniqeDto> Execute(RequestContentUniqeGetDto request);
//    }
//    public class HomeUniqeContentGetService : IHomeUniqeContentGetService
//    {
//        private readonly IDataBaseContext _context;
//        public HomeUniqeContentGetService(IDataBaseContext Context)
//        {
//            _context = Context;

//        }

//        public ResultDto<GetContentUniqeDto> Execute(RequestContentUniqeGetDto request)
//        {
//            var contents = _context.Contents.FirstOrDefault(c => c.ContentUniqeName == request.id & !c.IsRemoved);

           

//            if (contents != null)
//            {
//                var comment = _context.Comments.AsQueryable();
//                var subcomment = _context.SubComments.AsQueryable();

//                var comments = comment.Where(c => c.ContentId == contents.Id).Select(c => new CommentDto
//                {
//                    Id = c.Id,
//                    Name = c.Name,
//                    Email = c.Email,
//                    role = c.UserRole,
//                    main = c.CommentText,
//                    date = c.InsertTime,
//                    SubComments = subcomment.Where(sc => sc.CommentID == c.Id).Select(sc => new SubCommentDto
//                    {
//                        Id = sc.Id,
//                        Name = sc.Name,
//                        Email = sc.Email,
//                        role = sc.UserRole,
//                        reply = sc.ReplyMsg,
//                        date = sc.InsertTime,
//                    }).ToList(),
//                }).ToList();
//                return new ResultDto<GetContentUniqeDto>
//                {
//                    Data = new GetContentUniqeDto
//                    {
//                        Id = contents.Id,
//                        ContentTitle = contents.ContentTitle,
//                        ContentUniqeName = contents.ContentUniqeName,
//                        ContentLongDesc = contents.ContentLongDescription,
//                        CategoryUniqeName = contents.CategoryUniqeName,
//                        ContentImage = contents.ContentImage,
//                        ContentImageAlt = contents.ContentImageAlt,
//                        CategoryTitle = contents.CategoryTitle,
//                        date = contents.InsertTime,
//                        Comments = comments,
//                        IsIndex=contents.IsIndex,
//                        ContentImageTitle=contents.ContentImageTitle
//                    },
//                    IsSuccess = true,
//                    Message = "",

//                };
//            }
//            else
//            {
//                return new ResultDto<GetContentUniqeDto>()
//                {
//                    Data = new GetContentUniqeDto()
//                    {
//                        Id = 0,
//                        ContentTitle = "",
//                        ContentUniqeName = "",
//                        ContentLongDesc = "",
//                        CategoryUniqeName = "",
//                        CategoryTitle = "",
//                        date = DateTime.Now,
//                        Comments = null,
//                        IsIndex =false ,
//                    },
//                    IsSuccess = false,
//                    Message = "محتوی پیدا نشد !"
//                };
//            }
//        }


//    }

//    public class RequestContentUniqeGetDto
//    {
//        public string id { get; set; }
//    }


//    public class GetContentUniqeDto
//    {
//        public long Id { get; set; }
//        public string ContentTitle { get; set; }
//        public string ContentUniqeName { get; set; }
//        public string ContentLongDesc { get; set; }
//        public string CategoryUniqeName { get; set; }
//        public string CategoryTitle { get; set; }
//        public string ContentImage { get; set; }
//        public string ContentImageAlt { get; set; }
//        public string ContentImageTitle { get; set; }
//        public bool IsIndex { get; set; }
//        public DateTime date { get; set; }
//        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
        
//    }

//    public class CommentDto
//    {
//        public long Id { get; set; }
//        public string Name { get; set; }
//        public string Email { get; set; }
//        public string role { get; set; }
//        public string main { get; set; }
//        public DateTime date { get; set; }
//        public List<SubCommentDto> SubComments { get; set; } = new List<SubCommentDto>();

//    }

//    public class SubCommentDto
//    {
//        public long Id { get; set; }
//        public string Name { get; set; }
//        public string Email { get; set; }
//        public string role { get; set; }
//        public string reply { get; set; }
//        public DateTime date { get; set; }
//    }
//}
