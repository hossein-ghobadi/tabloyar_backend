using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Queries.ContentGet;
using Radin.Application.Services.Contents.Queries.HomeContentGet;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Ideas.Queries.IdeaGet
{
    public interface IIdeaGetService
    {
        GetIdeaDto SingleIdea(RequestIdeaGetDto request);
        ResultDto<ResultIdeaListGetDto> IdeaList(RequestIdeaListGetDto request);
        ResultDto<List<ResultIdeaListInIdeaPageDto>> IdeaListInIdeaPage(RequestIdeaListInIdeaPageDto request);
        ResultDto<List<IdeaSliderInHomeDto>> IdeaSliderInHomePage();
        ResultDto<IdeaResult> IdeaInIdeaPage(RequestIdeaGetDto request);

    }

    public class IdeaGetService : IIdeaGetService
    {
        private readonly IDataBaseContext _context;
        public IdeaGetService(IDataBaseContext Context)
        {
            _context = Context;

        }

        public GetIdeaDto SingleIdea(RequestIdeaGetDto request)
        {
            //var contents = _context.Contents.AsQueryable();

            var Ideas = _context.Ideas.FirstOrDefault(c => c.IdeaUniqeName == request.uniqename );
            if (Ideas == null)
            {
                return new GetIdeaDto
                {
                    IdeaTitle = "",
                    IdeaUniqeName = "",
                    CommentSituation = false,
                    CommentShow = false,
                    IdeaSorting = 0,
                    IdeaLongDescription = "",
                    IdeaMetaDesc = "",
                    IdeaImageAlt = "",
                    IdeaPublish = false,
                    IdeaImages = null,
                    IdeaCategory = new GetDto
                    {
                        id = "",
                        label = ""

                    },
                    Id = 0,
                    IsRemoved = false,
                    IsIndex = false,
                };
            }
            //List<string> imageUrls = Ideas.IdeaImage
            //    .Split(',')
            //    .Select(url => url.Trim()) // Trim any whitespace from each URL
            //    .ToList();

            List<string> imageUrls = string.IsNullOrWhiteSpace(Ideas.IdeaImage)
              ? new List<string>() // Return an empty list if the string is null or empty
              : Ideas.IdeaImage
              .Split(',', StringSplitOptions.RemoveEmptyEntries) // Split by comma, removing empty entries
              .Select(url => url.Trim()) // Trim whitespace from each URL
              .ToList();
            var IdeasList = new GetIdeaDto
            {
                
                IdeaTitle = Ideas.IdeaTitle,
                IdeaOwnerName=Ideas.IdeaOwnerName,
                AverageStar=Ideas.AverageStar,
                SumStar=Ideas.SumStar,
                CountStar=Ideas.CountStar,
                IdeaUniqeName = Ideas.IdeaUniqeName,
                CommentSituation = Ideas.CommentSituation,
                CommentShow = Ideas.CommentShow,
                IdeaSorting = Ideas.IdeaSorting,
                IdeaLongDescription = Ideas.IdeaLongDescription,
                IdeaMetaDesc = Ideas.IdeaMetaDesc,
                IdeaImageAlt = Ideas.IdeaImageAlt,
                IdeaPublish = Ideas.IdeaPublish,
                IdeaImages = imageUrls,
                MainImage = Ideas.MainImage,
                IdeaCategory = new GetDto
                {
                    id = Ideas.IdeaCategoryUniqeName,
                    label = Ideas.IdeaCategoryTitle

                },
                Id = Ideas.Id,
                IsRemoved = Ideas.IsRemoved,
                IsIndex = Ideas.IsIndex,
            };
            return IdeasList;

        }




        public ResultDto<ResultIdeaListGetDto> IdeaList(RequestIdeaListGetDto request)
        {
            int rowsCount = 0;
            var Ideas = _context.Ideas.OrderByDescending(n=>n.UpdateTime).AsQueryable();
            int count = _context.Ideas.Count();
            int remainder = count % request.PageSize;
            int PageCount = 0;
           


            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                Ideas = Ideas.Where(p => p.IdeaUniqeName.Contains(request.SearchKey) || p.IdeaTitle.Contains(request.SearchKey));
                count = Ideas.Count();
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

            var IdeasList = Ideas.Select(p => new GetIdeaListDto
            {

                IdeaOwnerName = p.IdeaOwnerName,
                IdeaTitle = p.IdeaTitle,
                id = p.IdeaUniqeName,
                IdeaCategoryTitle = p.IdeaCategoryTitle,
                InsertTime = p.InsertTime,
                IdeaSorting = p.IdeaSorting,
                IsRemoved = p.IsRemoved,
                IsIndex= p.IsIndex,

            }).ToList();
            if (request.IsSort)
            {
                IdeasList = IdeasList.ToList();//.OrderBy(s => s.IdeaSorting)
            }
            int skip = (request.PageNumber - 1) * request.PageSize;

            var IdeaSubList = IdeasList.Skip(skip).Take(request.PageSize).ToList();


            return new ResultDto<ResultIdeaListGetDto>
            {
                Data = new ResultIdeaListGetDto
                {
                    Rows = PageCount,
                    Ideas = IdeaSubList,
                    count = count,
                },
                IsSuccess = true,
                Message = "",

            };



        }




        public ResultDto<List<ResultIdeaListInIdeaPageDto>> IdeaListInIdeaPage(RequestIdeaListInIdeaPageDto request)
        {
            int rowsCount = 0;
            var ideas = _context.Ideas.Where(p => !(p.IsRemoved)).AsQueryable();
            int cnt = _context.Ideas.Where(p => !(p.IsRemoved)).Count();
            if (!string.IsNullOrWhiteSpace(request.IdeaCategoryTitle))
            {
                ideas = ideas.Where(p => p.IdeaCategoryUniqeName.Equals(request.IdeaCategoryTitle));
            }
            var ideasList = ideas.Select(p => new ResultIdeaListInIdeaPageDto
            {
                IdeaTitle = p.IdeaTitle,
                IdeaUniqName = p.IdeaUniqeName,
                IdeaSorting = p.IdeaSorting,
                IdeaLongDescription = p.IdeaLongDescription.Substring(0, 100),
                IdeaCateoryTitle = p.IdeaCategoryTitle,
                InsertTime = p.InsertTime,
                MainImage = p.MainImage,
            }).ToList();
            return new ResultDto<List<ResultIdeaListInIdeaPageDto>>
            {
                Data = ideasList,
                IsSuccess = true,
                Message = "",

            };

        }





        public ResultDto<List<IdeaSliderInHomeDto>> IdeaSliderInHomePage( )
        {
            int rowsCount = 0;
            int cnt = _context.Ideas.Where(p => !(p.IsRemoved)).Count();
            var ideas = _context.Ideas.Where(p => !(p.IsRemoved)).AsQueryable();
           
            var ideasList = ideas.Select(p => new IdeaSliderInHomeDto
            {
                IdeaTitle = p.IdeaTitle,
                IdeaUniqeName = p.IdeaUniqeName,
                IdeaSorting = p.IdeaSorting,
                IdeaMetaDesc = p.IdeaMetaDesc.Substring(0, 100),
                MainImage = p.MainImage,
            }).ToList();
            return new ResultDto<List<IdeaSliderInHomeDto>>
            {
                Data = ideasList,
                IsSuccess = true,
                Message = "",

            };

        }







        public ResultDto<IdeaResult> IdeaInIdeaPage(RequestIdeaGetDto request)
        {
            var ideas = _context.Ideas.Where(p => !(p.IsRemoved)).FirstOrDefault(c => c.IdeaUniqeName == request.uniqename);
            var AttachedIdeas = new List<ResultIdeaListInIdeaPageDto>();

            if (ideas != null)
            {
                var ideaCategory = ideas.IdeaCategoryUniqeName;
                 AttachedIdeas = _context.Ideas.Where(p => p.IdeaCategoryUniqeName == ideaCategory& !(p.IsRemoved)).Select(p =>  new ResultIdeaListInIdeaPageDto
                {
                    IdeaTitle = p.IdeaTitle,
                    IdeaUniqName = p.IdeaUniqeName,
                    IdeaSorting = p.IdeaSorting,
                    IdeaLongDescription = p.IdeaLongDescription.Substring(0, 100),
                    IdeaCateoryTitle = p.IdeaCategoryTitle,
                    InsertTime = p.InsertTime,
                    MainImage = p.MainImage,
                    
                }).ToList();
                var initialString = $"{ideas.IdeaImage}";
                List<string> imageUrls = string.IsNullOrWhiteSpace(initialString)
              ? new List<string>() // Return an empty list if the string is null or empty
              : initialString
              .Split(',', StringSplitOptions.RemoveEmptyEntries) // Split by comma, removing empty entries
              .Select(url => url.Trim()) // Trim whitespace from each URL
              .ToList();



                var ideaComment = _context.IdeaComments.AsQueryable();
                var ideaSubcomment = _context.IdeaSubComments.AsQueryable();



                var comments = ideaComment.Where(c => c.IdeaId == ideas.Id).Select(c => new CommentDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    role = c.UserRole,
                    main = c.CommentText,
                    date = c.InsertTime,
                    SubComments = ideaSubcomment.Where(sc => sc.CommentID == c.Id).Select(sc => new SubCommentDto
                    {
                        Id = sc.Id,
                        Name = sc.Name,
                        Email = sc.Email,
                        role = sc.UserRole,
                        reply = sc.ReplyMsg,
                        date = sc.InsertTime,
                    }).ToList(),
                }).ToList();
                return new ResultDto<IdeaResult>
                {
                    Data = new IdeaResult()
                    {
                        IdeaData = new GetIdeaCompleteDto
                        {
                            Id = ideas.Id,
                            IdeaTitle = ideas.IdeaTitle,
                            IdeaUniqeName = ideas.IdeaUniqeName,
                            IdeaLongDescription = ideas.IdeaLongDescription,
                            IdeaCategoryUniqeName = ideas.IdeaCategoryUniqeName,
                            MainImage = ideas.MainImage,
                            IdeaImages = imageUrls,
                            IdeaCategoryTitle = ideas.IdeaCategoryTitle,
                            AverageStar = ideas.AverageStar,
                            date = ideas.InsertTime,
                            Comments = comments,
                            IsIndex = ideas.IsIndex,
                        },
                        AttachedIdeas = AttachedIdeas


                    },
                    IsSuccess = true,
                    Message = "",

                };
            }
           

           

            else
            {
                return new ResultDto<IdeaResult>()
                {
                    Data = new IdeaResult()
                    {
                        IdeaData = new GetIdeaCompleteDto
                        {
                            Id = 0,
                            IdeaTitle = "",
                            IdeaUniqeName = "",
                            IdeaLongDescription = "",
                            IdeaCategoryUniqeName = "",
                            IdeaCategoryTitle = "",
                            AverageStar = 0,
                            IdeaImages = null,
                            date = DateTime.Now,
                            Comments = null,
                            IsIndex = false,

                        },
                        AttachedIdeas = null



                    },
                    IsSuccess = false,
                    Message = "محتوی پیدا نشد !"
                };
            }
        }


    }

    public class RequestIdeaGetDto
    {
        public string uniqename { get; set; }
    }
    public class GetDto
    {
        public string id { get; set; }
        public string label { get; set; }
    }
    public class GetIdeaDto
    {
        public long Id { get; set; }
        public string IdeaTitle { get; set; }
        public string IdeaOwnerName { get; set; }
        public float AverageStar {  get; set; }
        public int SumStar { get; set; }
        public int CountStar { get; set; }
        public string IdeaUniqeName { get; set; }
        public bool CommentSituation { get; set; }
        public bool CommentShow { get; set; }
        public int IdeaSorting { get; set; }
        public string IdeaLongDescription { get; set; }
        public string IdeaMetaDesc { get; set; }
        public string IdeaImageAlt { get; set; }
        public bool IdeaPublish { get; set; }
        public string MainImage {  get; set; }
        public List<string> IdeaImages { get; set; }
        public GetDto IdeaCategory { get; set; }
        public bool IsRemoved { get; set; }
        public bool IsIndex { get; set; }
    }
    //............................................................................
    //............................................................................
    //............................................................................






    public class RequestIdeaListGetDto
    {
        
        public string SearchKey { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool IsSort { get; set; }
        
    }

    public class ResultIdeaListGetDto
    {
        public List<GetIdeaListDto> Ideas { get; set; }
        public int Rows { get; set; }
        public int count { get; set; }

    }

    public class GetIdeaListDto
    {
        //public long Id { get; set; }
        public string IdeaTitle { get; set; }

        public string IdeaOwnerName { get; set; }
        public string id { get; set; }
        //public long CategoryId { get; set; }
        public string IdeaCategoryTitle { get; set; }
        public int IdeaSorting { get; set; }
        public DateTime InsertTime { get; set; }
        public bool IsRemoved { get; set; }
        public bool IsIndex { get; set; }
    }
    //............................................................................
    //............................................................................
    //............................................................................
    public class RequestIdeaListInIdeaPageDto
    {
        public string? IdeaCategoryTitle { get; set; }
        
    }
    public class ResultIdeaListInIdeaPageDto
    {
        public string IdeaCateoryTitle { get; set; }
        public string IdeaTitle { get; set; }
        public string IdeaUniqName { get; set; }
        public int IdeaSorting { get; set; }
        public string IdeaLongDescription { get; set; }
        public string MainImage { get; set; }
        public DateTime InsertTime { get; set; }

    }
    //............................................................................
    //............................................................................
    //............................................................................


    public class IdeaSliderInHomeDto
    {
        public string IdeaTitle { get; set; }
        public string IdeaUniqeName { get; set; }
        public int IdeaSorting { get; set; }
        public string IdeaMetaDesc { get; set; }
        public string MainImage { get; set; }

    }

    //............................................................................
    //............................................................................
    //............................................................................
    public class IdeaResult
    {
        public GetIdeaCompleteDto IdeaData { get; set; }
        public List<ResultIdeaListInIdeaPageDto> AttachedIdeas {  get; set; }
    }

    public class GetIdeaCompleteDto
    {
        public long Id { get; set; }
        public string IdeaTitle { get; set; }
        public string IdeaUniqeName { get; set; }
        public string IdeaLongDescription { get; set; }
        public string IdeaCategoryUniqeName { get; set; }
        public string IdeaCategoryTitle { get; set; }
        public string MainImage { get; set; }
        public List<string> IdeaImages { get; set; }

        public float AverageStar {  get; set; }
        public DateTime date { get; set; }
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
        public bool IsIndex { get; set; }

    }

    
    public class CommentDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string role { get; set; }
        public string main { get; set; }
        public DateTime date { get; set; }
        public List<SubCommentDto> SubComments { get; set; } = new List<SubCommentDto>();

    }
    
    public class SubCommentDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string role { get; set; }
        public string reply { get; set; }
        public DateTime date { get; set; }
    }
    //............................................................................
    //............................................................................
    //............................................................................

}
