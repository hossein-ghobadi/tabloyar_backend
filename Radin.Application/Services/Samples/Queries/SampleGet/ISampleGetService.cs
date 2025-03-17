using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Samples.Queries.SampleGet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Ideas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Samples.Queries.SampleGet
{
    public interface ISampleGetService
    {

        GetSampleDto SingleSample(RequestSampleGetDto request);
        ResultDto<ResultSampleListGetDto> SampleList(RequestSampleListGetDto request);
        ResultDto<List<ResultSampleListInSamplePageDto>> SampleListInSamplePage(RequestSampleListInSamplePageDto request);
        ResultDto<List<SampleSliderInHomeDto>> SampleSliderInHomePage();
        ResultDto<SampleResult> SampleInSamplePage(RequestSampleGetDto request);
    }


    public class SampleGetService : ISampleGetService
    {
        private readonly IDataBaseContext _context;
        public SampleGetService(IDataBaseContext Context)
        {
            _context = Context;

        }

        public GetSampleDto SingleSample(RequestSampleGetDto request)
        {
            //var contents = _context.Contents.AsQueryable();

            var Samples = _context.Samples.FirstOrDefault(c => c.SampleUniqeName == request.uniqename);
            if (Samples == null)
            {
                return new GetSampleDto
                {
                    SampleTitle = "",
                    SampleUniqeName = "",
                    CommentSituation = false,
                    CommentShow = false,
                    SampleSorting = 0,
                    SampleLongDescription = "",
                    SampleMetaDesc = "",
                    SampleImageAlt = "",
                    SamplePublish = false,
                    SampleImages = null,
                    SampleCategory = new GetDto
                    {
                        id = "",
                        label = ""

                    },
                    Id = 0,
                    IsRemoved = false,
                    IsIndex = false,
                };
            }
          
            List<string> imageUrls = string.IsNullOrWhiteSpace(Samples.SampleImage)
              ? new List<string>() // Return an empty list if the string is null or empty
              : Samples.SampleImage
              .Split(',', StringSplitOptions.RemoveEmptyEntries) // Split by comma, removing empty entries
              .Select(url => url.Trim()) // Trim whitespace from each URL
              .ToList();

            var SamplesList = new GetSampleDto
            {

                SampleTitle = Samples.SampleTitle,
                SampleOwnerName = Samples.SampleOwnerName,
                AverageStar = Samples.AverageStar,
                SumStar = Samples.SumStar,
                CountStar = Samples.CountStar,
                SampleUniqeName = Samples.SampleUniqeName,
                CommentSituation = Samples.CommentSituation,
                CommentShow = Samples.CommentShow,
                SampleSorting = Samples.SampleSorting,
                SampleLongDescription = Samples.SampleLongDescription,
                SampleMetaDesc = Samples.SampleMetaDesc,
                SampleImageAlt = Samples.SampleImageAlt,
                SamplePublish = Samples.SamplePublish,
                SampleImages = imageUrls,
                MainImage = Samples.MainImage,
                SampleCategory = new GetDto
                {
                    id = Samples.SampleCategoryUniqeName,
                    label = Samples.SampleCategoryTitle

                },
                Id = Samples.Id,
                IsRemoved = Samples.IsRemoved,
                IsIndex = Samples.IsIndex,
            };
            return SamplesList;

        }




        public ResultDto<ResultSampleListGetDto> SampleList(RequestSampleListGetDto request)
        {
            int rowsCount = 0;
            int count = _context.Samples.Count();
            var Samples = _context.Samples.OrderByDescending(n => n.UpdateTime).AsQueryable();
            int remainder = count % request.PageSize;
            int PageCount = 0;



            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                Samples = Samples.Where(p => p.SampleUniqeName.Contains(request.SearchKey) || p.SampleTitle.Contains(request.SearchKey));
                count = Samples.Count();
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

            var SamplesList = Samples.Select(p => new GetSampleListDto
            {

                SampleOwnerName = p.SampleOwnerName,
                SampleTitle = p.SampleTitle,
                id = p.SampleUniqeName,
                SampleCategoryTitle = p.SampleCategoryTitle,
                InsertTime = p.InsertTime,
                SampleSorting = p.SampleSorting,
                IsRemoved = p.IsRemoved,
                IsIndex = p.IsIndex,

            }).ToList();
            if (request.IsSort)
            {
                SamplesList = SamplesList.ToList();//.OrderBy(s => s.SampleSorting)
            }
            int skip = (request.PageNumber - 1) * request.PageSize;

            var SampleSubList = SamplesList.Skip(skip).Take(request.PageSize).ToList();


            return new ResultDto<ResultSampleListGetDto>
            {
                Data = new ResultSampleListGetDto
                {
                    Rows = PageCount,
                    Samples = SampleSubList,
                    count = count,
                },
                IsSuccess = true,
                Message = "",

            };



        }




        public ResultDto<List<ResultSampleListInSamplePageDto>> SampleListInSamplePage(RequestSampleListInSamplePageDto request)
        {
            int rowsCount = 0;
            var samples = _context.Samples.Where(p => !(p.IsRemoved)).AsQueryable();
            int cnt = _context.Samples.Count();
            if (!string.IsNullOrWhiteSpace(request.SampleCategoryTitle))
            {
                samples = samples.Where(p => p.SampleCategoryUniqeName.Equals(request.SampleCategoryTitle));
            }
            var samplesList = samples.Select(p => new ResultSampleListInSamplePageDto
            {
                SampleTitle = p.SampleTitle,
                SampleUniqName = p.SampleUniqeName,
                SampleSorting = p.SampleSorting,
                SampleLongDescription = p.SampleLongDescription.Substring(0, 100),
                SampleCateoryTitle = p.SampleCategoryTitle,
                InsertTime = p.InsertTime,
                MainImage = p.MainImage,
            }).ToList();
            return new ResultDto<List<ResultSampleListInSamplePageDto>>
            {
                Data = samplesList,
                IsSuccess = true,
                Message = "",

            };

        }





        public ResultDto<List<SampleSliderInHomeDto>> SampleSliderInHomePage()
        {
            int rowsCount = 0;
            int cnt = _context.Samples.Where(p => !(p.IsRemoved)).Count();
            var samples = _context.Samples.Where(p => !(p.IsRemoved)).AsQueryable();

            var samplesList = samples.Select(p => new SampleSliderInHomeDto
            {
                SampleTitle = p.SampleTitle,
                SampleUniqeName = p.SampleUniqeName,
                SampleSorting = p.SampleSorting,
                SampleMetaDesc = p.SampleMetaDesc.Substring(0, 100),
                MainImage = p.MainImage,
            }).ToList();
            return new ResultDto<List<SampleSliderInHomeDto>>
            {
                Data = samplesList,
                IsSuccess = true,
                Message = "",

            };

        }







        public ResultDto<SampleResult> SampleInSamplePage(RequestSampleGetDto request)
        {
            var samples = _context.Samples.Where(p => !(p.IsRemoved)).FirstOrDefault(c => c.SampleUniqeName == request.uniqename);
            var AttachedSamples = new List<ResultSampleListInSamplePageDto>();
            Console.WriteLine($@"adawd===={samples}");

            if (samples != null)
            {
                var sampleCategory = samples.SampleCategoryUniqeName;
                AttachedSamples = _context.Samples.Where(p => p.SampleCategoryUniqeName == sampleCategory& !(p.IsRemoved)).Select(p => new ResultSampleListInSamplePageDto
                {
                    SampleTitle = p.SampleTitle,
                    SampleUniqName = p.SampleUniqeName,
                    SampleSorting = p.SampleSorting,
                    SampleLongDescription = p.SampleLongDescription.Substring(0, 100),
                    SampleCateoryTitle = p.SampleCategoryTitle,
                    InsertTime = p.InsertTime,
                    MainImage = p.MainImage,
                }).ToList();
                var initialString = $"{samples.SampleImage}";
                List<string> imageUrls = string.IsNullOrWhiteSpace(initialString)
                ? new List<string>() // Return an empty list if the string is null or empty
                : initialString
                .Split(',', StringSplitOptions.RemoveEmptyEntries) // Split by comma, removing empty entries
                .Select(url => url.Trim()) // Trim whitespace from each URL
                .ToList();


                var sampleComment = _context.SampleComments.AsQueryable();
                var sampleSubcomment = _context.SampleSubComments.AsQueryable();

                
                var comments = sampleComment.Where(c => c.SampleId == samples.Id).Select(c => new CommentDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    role = c.UserRole,
                    main = c.CommentText,
                    date = c.InsertTime,
                    SubComments = sampleSubcomment.Where(sc => sc.CommentID == c.Id).Select(sc => new SubCommentDto
                    {
                        Id = sc.Id,
                        Name = sc.Name,
                        Email = sc.Email,
                        role = sc.UserRole,
                        reply = sc.ReplyMsg,
                        date = sc.InsertTime,
                    }).ToList(),
                }).ToList();

                return new ResultDto<SampleResult>
                {
                    Data = new SampleResult()
                    {
                        SampleData = new GetSampleCompleteDto
                        {
                            Id = samples.Id,
                            SampleTitle = samples.SampleTitle,
                            SampleUniqeName = samples.SampleUniqeName,
                            SampleLongDescription = samples.SampleLongDescription,
                            SampleCategoryUniqeName = samples.SampleCategoryUniqeName,
                            MainImage = samples.MainImage,
                            SampleImages = imageUrls,
                            SampleCategoryTitle = samples.SampleCategoryTitle,
                            AverageStar = samples.AverageStar,
                            date = samples.InsertTime,
                            Comments = comments,
                            IsIndex=samples.IsIndex,
                        },
                        AttachedSamples = AttachedSamples


                    },
                    IsSuccess = true,
                    Message = "",

                };
            }


           

            
            else
            {
                return new ResultDto<SampleResult>()
                {
                    Data = new SampleResult()
                    {
                        SampleData = new GetSampleCompleteDto
                        {
                            Id = 0,
                            SampleTitle = "",
                            SampleUniqeName = "",
                            SampleLongDescription = "",
                            SampleCategoryUniqeName = "",
                            SampleCategoryTitle = "",
                            AverageStar = 0,
                            SampleImages = null,
                            date = DateTime.Now,
                            Comments = null,
                            IsIndex=false,

                        },
                        AttachedSamples = null



                    },
                    IsSuccess = false,
                    Message = "محتوی پیدا نشد !"
                };
            }
        }


    }

    public class RequestSampleGetDto
    {
        public string uniqename { get; set; }
    }
    public class GetDto
    {
        public string id { get; set; }
        public string label { get; set; }
    }
    public class GetSampleDto
    {
        public long Id { get; set; }
        public string SampleTitle { get; set; }
        public string SampleOwnerName { get; set; }
        public float AverageStar { get; set; }
        public int SumStar { get; set; }
        public int CountStar { get; set; }
        public string SampleUniqeName { get; set; }
        public bool CommentSituation { get; set; }
        public bool CommentShow { get; set; }
        public int SampleSorting { get; set; }
        public string SampleLongDescription { get; set; }
        public string SampleMetaDesc { get; set; }
        public string SampleImageAlt { get; set; }
        public bool SamplePublish { get; set; }
        public string MainImage { get; set; }
        public List<string> SampleImages { get; set; }
        public GetDto SampleCategory { get; set; }
        public bool IsRemoved { get; set; }
        public bool IsIndex { get; set; }
    }
    //............................................................................
    //............................................................................
    //............................................................................






    public class RequestSampleListGetDto
    {

        public string SearchKey { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool IsSort { get; set; }
    }

    public class ResultSampleListGetDto
    {
        public List<GetSampleListDto> Samples { get; set; }
        public int Rows { get; set; }
        public int count { get; set; }

    }

    public class GetSampleListDto
    {
        //public long Id { get; set; }
        public string SampleTitle { get; set; }

        public string SampleOwnerName { get; set; }
        public string id { get; set; }
        //public long CategoryId { get; set; }
        public string SampleCategoryTitle { get; set; }
        public int SampleSorting { get; set; }
        public DateTime InsertTime { get; set; }
        public bool IsRemoved { get; set; }
        public bool IsIndex { get; set; }

    }
    //............................................................................
    //............................................................................
    //............................................................................
    public class RequestSampleListInSamplePageDto
    {
        public string? SampleCategoryTitle { get; set; }

    }
    public class ResultSampleListInSamplePageDto
    {
        public string SampleCateoryTitle { get; set; }
        public string SampleTitle { get; set; }
        public string SampleUniqName { get; set; }
        public int SampleSorting { get; set; }
        public string SampleLongDescription { get; set; }
        public string MainImage { get; set; }
        public DateTime InsertTime { get; set; }

    }
    //............................................................................
    //............................................................................
    //............................................................................


    public class SampleSliderInHomeDto
    {
        public string SampleTitle { get; set; }
        public string SampleUniqeName { get; set; }
        public int SampleSorting { get; set; }
        public string SampleMetaDesc { get; set; }
        public string MainImage { get; set; }

    }

    //............................................................................
    //............................................................................
    //............................................................................
    public class SampleResult
    {
        public GetSampleCompleteDto SampleData { get; set; }
        public List<ResultSampleListInSamplePageDto> AttachedSamples { get; set; }
    }

    public class GetSampleCompleteDto
    {
        public long Id { get; set; }
        public string SampleTitle { get; set; }
        public string SampleUniqeName { get; set; }
        public string SampleLongDescription { get; set; }
        public string SampleCategoryUniqeName { get; set; }
        public string SampleCategoryTitle { get; set; }
        public string MainImage { get; set; }

        public bool IsIndex {  get; set; }
        public List<string> SampleImages { get; set; }

        public float AverageStar { get; set; }
        public DateTime date { get; set; }
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
        
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
