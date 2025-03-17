using Microsoft.VisualBasic;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Branch.Commands.BranchInfoSetService;
using Radin.Application.Services.Contents.Queries.ContentCategoryGet;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Radin.Application.Services.Branch.Queries.BranchInfoGetService.BranchInfoGetService;

namespace Radin.Application.Services.Branch.Queries.BranchInfoGetService
{
    public interface IBranchInfoGetService
    {
        ResultDto<object> Execute(RequestBranchInfoGetDto request);//// admin/branchProxy

        ResultDto<List<BranchInfoDto>> GetBranchsInBranchPage(string? search);//Home

    }


    public class BranchInfoGetService : IBranchInfoGetService
    {
        private readonly IDataBaseContext _context;
        public BranchInfoGetService(IDataBaseContext Context)
        {
            _context = Context;
        }

        public ResultDto<object> Execute(RequestBranchInfoGetDto request)
        {
            int count = _context.BranchINFOs.Count();
            var branches = _context.BranchINFOs.OrderByDescending(n => n.UpdateTime).AsQueryable();
            //.OrderByDescending(n => n.UpdateTime)
            int PageCount = (count + request.PageSize - 1) / request.PageSize; // Simplified page count calculation

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                branches = branches.Where(p => p.BranchName.Contains(request.SearchKey));
                count = branches.Count();
                PageCount = (count + request.PageSize - 1) / request.PageSize;
            }

            List<object> BranchList;

            if (request.WImage == null)
            {
                // Include MainImage in the response
                BranchList = branches.Select(p => new
                {
                    id = p.BranchCode,
                    BranchName = p.BranchName,
                    BranchCity = _context.Cities.FirstOrDefault(c => c.Id == p.BranchCity).city ?? "Unknown City",
                    BranchProvince = _context.Cities.FirstOrDefault(c => c.ProvinceId == p.BranchProvince).province ?? "Unknown Province",
                    BranchCountry = _context.Cities.FirstOrDefault(c => c.Id == p.BranchCountry).Country ?? "Unknown Country",
                    BranchPhone1 = p.BranchPhone1,
                    InstagramId = p.InstagramId,
                    MainImage = p.MainImage // Include MainImage
                }).ToList<object>();
            }
            else
            {
                // Exclude MainImage from the response
                BranchList = branches.Select(p => new
                {
                    id = p.BranchCode,
                    BranchName = p.BranchName,
                    BranchCity = _context.Cities.FirstOrDefault(c => c.Id == p.BranchCity).city ?? "Unknown City",
                    BranchProvince = _context.Cities.FirstOrDefault(c => c.ProvinceId == p.BranchProvince).province ?? "Unknown Province",
                    BranchCountry = _context.Cities.FirstOrDefault(c => c.Id == p.BranchCountry).Country ?? "Unknown Country",
                    BranchPhone1 = p.BranchPhone1,
                    InstagramId = p.InstagramId
                    // MainImage is not included here
                }).ToList<object>();
            }

            // Sort and paginate results
            //BranchList = BranchList.OrderBy(s => ((dynamic)s).id).ToList();
            int skip = (request.PageNumber - 1) * request.PageSize;
            var BranchSubList = BranchList.Skip(skip).Take(request.PageSize).ToList();

            // Return result
            return new ResultDto<object>
            {
                Data = new
                {
                    Rows = PageCount,
                    Branches = BranchSubList,
                    count = count,
                },
                IsSuccess = true,
                Message = "",
            };
        }




        public ResultDto<List<BranchInfoDto>> GetBranchsInBranchPage(string? search)
        {
            try
            {
                var branches = _context.BranchINFOs.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    branches = branches.Where(p => p.BranchName.Contains(search));
                }

                List<BranchInfoDto> BranchList;
                var branchList = branches.Where(p=>p.BranchCode!=7792).Select(p => new BranchInfoDto
                {
                    Id = p.BranchCode,
                    BranchName = p.BranchName,
                    BranchAddress = p.BranchAddress,
                    BranchPhone1 = p.BranchPhone1,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    InstagramId = p.InstagramId,
                    WhatsAppId = p.WhatsAppId,
                    TelegramId = p.TelegramId,
                    EitaId = p.EitaId,
                    
                }).ToList();
                // Include MainImage in the response
                //BranchList = branches.Select(p => new
                //{
                //    id = p.BranchCode,
                //    BranchName = p.BranchName,
                //    BranchAddress = p.BranchAddress,
                //    BranchPhone1 = p.BranchPhone1,
                //    Latitude = p.Latitude,
                //    Longitude = p.Longitude,
                //    InstagramId = p.InstagramId,
                //    WhatsAppId = p.WhatsAppId,
                //    TelegramId = p.TelegramId,
                //    EitaId = p.EitaId,
                //    //MainImage = (string.IsNullOrEmpty(p.Images) ? p.MainImage : $"{p.MainImage},{p.Images}").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
                //}).ToList<object>();


                // Return result
                return new ResultDto<List<BranchInfoDto>>
                {
                    Data = branchList,
                    IsSuccess = true,
                    Message = "",
                };
            }
            catch (Exception ex) {
                return new ResultDto<List<BranchInfoDto>>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "خطا در دریافت اطلاعات",
                };



            }
}



        


        public class BranchInfoDto
        {
            public long Id { get; set; }
            public string BranchName { get; set; }
            public string BranchAddress { get; set; }
            public string BranchPhone1 { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
            public string InstagramId { get; set; }
            public string WhatsAppId { get; set; }
            public string TelegramId { get; set; }
            public string EitaId { get; set; }
        }


        public class RequestBranchInfoGetDto
        {
            public string SearchKey { get; set; } // The search key to filter branch names.
            public int PageNumber { get; set; } // The current page number for pagination.
            public int PageSize { get; set; } // The number of items to be returned per page.
            public bool IsSort { get; set; } // A boolean indicating if sorting is applied.
            public int? WImage { get; set; } // A flag to determine if the MainImage should be included in the result.
        }


    }
}