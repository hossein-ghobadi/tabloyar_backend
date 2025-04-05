//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Samples.Queries.SampleCategoryGet;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Samples.Queries.SampleCategoryGet
//{
//    public interface ISampleCategoryGetService
//    {
//        List<GetDto> CategoryForAdminContentSet();
//        ResultDto<GetSampleCategoryDto> CategorySingleGetForAdmin(RequestSampleCategoryGetDto request);
//        ResultDto<ResultSampleCategoryListDto> CategoryListGetForAdmin(RequestSampleCategoryListDto request);

//    }


//    public class SampleCategoryGetService : ISampleCategoryGetService
//    {
//        private readonly IDataBaseContext _context;
//        public SampleCategoryGetService(IDataBaseContext Context)
//        {
//            _context = Context;

//        }

//        public List<GetDto> CategoryForAdminContentSet()
//        {
//            var categories = _context.SampleCategories.Where(p => !(p.IsRemoved));

//            //int rowsCount = 0;
//            var categoriesList = categories.Select(p => new GetDto
//            {
//                id = p.SampleCategoryUniqeName,
//                label = p.SampleCategoryTitle,



//            }).ToList();

//            return categoriesList;
//        }



//        public ResultDto<GetSampleCategoryDto> CategorySingleGetForAdmin(RequestSampleCategoryGetDto request)
//        {

//            var sampleCategories = _context.SampleCategories.FirstOrDefault(c => c.SampleCategoryUniqeName == request.uniqename);
//            if (sampleCategories == null)
//            {
//                return new ResultDto<GetSampleCategoryDto>()
//                {
//                    Data = new GetSampleCategoryDto
//                    {
//                        Id = 0,
//                        SampleCategoryTitle = "",
//                        SampleCategoryUniqeName = "",
//                        SampleCategorySorting = 0,
//                        SampleCategoryIsShowMenu = false,
//                        SampleCategoryDescription = "",
//                        IsRemoved = false,
//                    },
//                    IsSuccess = false,
//                    Message = "خطا در دریافت دسته بندی"

//                };
//            }

//            var sampleCategoriesList = new GetSampleCategoryDto
//            {
//                Id = sampleCategories.Id,
//                SampleCategoryTitle = sampleCategories.SampleCategoryTitle,
//                SampleCategoryUniqeName = sampleCategories.SampleCategoryUniqeName,
//                SampleCategorySorting = sampleCategories.SampleCategorySorting,
//                SampleCategoryIsShowMenu = sampleCategories.SampleCategoryIsShowMenu,
//                SampleCategoryDescription = sampleCategories.SampleCategoryDescription,
//                IsRemoved = sampleCategories.IsRemoved,
//            };



//            return new ResultDto<GetSampleCategoryDto>()
//            {
//                Data = sampleCategoriesList,
//                IsSuccess = true,
//                Message = " دریافت موفقیت آمیز "

//            };
//        }






//        public ResultDto<ResultSampleCategoryListDto> CategoryListGetForAdmin(RequestSampleCategoryListDto request)
//        {
//            //throw new NotImplementedException();
//            int rowsCount = 0;
//            int count = _context.SampleCategories.Count();
//            var sampleCategories = _context.SampleCategories.OrderByDescending(n => n.UpdateTime).AsQueryable();
//            int remainder = count % request.PageSize;
//            int PageCount = 0;

//            if (!string.IsNullOrWhiteSpace(request.SearchKey))
//            {
//                sampleCategories = sampleCategories.Where(p => p.SampleCategoryUniqeName.Contains(request.SearchKey) || p.SampleCategoryTitle.Contains(request.SearchKey));
//                count = sampleCategories.Count();
//                remainder = count % request.PageSize;
//                if (remainder > 0)
//                {
//                    PageCount = (count / request.PageSize) + 1;
//                }
//                else
//                {
//                    PageCount = count / request.PageSize;
//                }
//            }
//            else
//            {
//                remainder = count % request.PageSize;
//                if (remainder > 0)
//                {
//                    PageCount = (count / request.PageSize) + 1;
//                }
//                else
//                {
//                    PageCount = count / request.PageSize;
//                }
//            }

//            var categoriesList = sampleCategories.Select(p => new AbstractedSampleCategoryDto
//            {
//                SampleCategoryTitle = p.SampleCategoryTitle,
//                id = p.SampleCategoryUniqeName,
//                SampleCategorySorting = p.SampleCategorySorting,
//                IsRemoved = p.IsRemoved,
//            }).ToList();
//            if (request.IsSort)
//            {

//                categoriesList = categoriesList.ToList();//.OrderBy(s => s.SampleCategorySorting)

//            }


//            int skip = (request.PageNumber - 1) * request.PageSize;

//            var CategorySubList = categoriesList.Skip(skip).Take(request.PageSize).ToList();


//            return new ResultDto<ResultSampleCategoryListDto>
//            {
//                Data = new ResultSampleCategoryListDto
//                {
//                    Rows = PageCount,
//                    SampleCategories = CategorySubList,
//                    count = count,
//                },
//                IsSuccess = true,
//                Message = "",

//            };


//        }

//    }

















//    public class GetDto
//    {
//        public string id { get; set; }
//        public string label { get; set; }


//    }
//    //........................................................................




//    public class RequestSampleCategoryGetDto
//    {
//        public string uniqename { get; set; }
//    }


//    public class GetSampleCategoryDto
//    {
//        public long Id { get; set; }

//        public string SampleCategoryTitle { get; set; }
//        public string SampleCategoryUniqeName { get; set; }
//        public int SampleCategorySorting { get; set; }

//        public bool SampleCategoryIsShowMenu { get; set; }
//        public string SampleCategoryDescription { get; set; }
//        public bool IsRemoved { get; set; }

//    }
//    //........................................................................



//    public class RequestSampleCategoryListDto
//    {
//        public string SearchKey { get; set; }
//        public int PageNumber { get; set; }
//        public int PageSize { get; set; }
//        public bool IsSort { get; set; }
//    }

//    public class ResultSampleCategoryListDto
//    {
//        public List<AbstractedSampleCategoryDto> SampleCategories { get; set; }
//        public int Rows { get; set; }
//        public int count { get; set; }

//    }

//    public class AbstractedSampleCategoryDto
//    {

//        public string SampleCategoryTitle { get; set; }
//        public string id { get; set; }
//        public int SampleCategorySorting { get; set; }
//        public bool IsRemoved { get; set; }

//    }
//}
