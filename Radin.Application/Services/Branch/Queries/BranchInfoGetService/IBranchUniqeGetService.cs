using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Queries.ContentCategoryGet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Branch.Queries.BranchInfoGetService
{
    public interface IBranchUniqeGetService
    {
        ResultDto<GetBranhUniqeDto> Execute(RequestBranchUniqeGetDto request);//// admin/branchProxy
        ResultDto<object> GetBranchInfosInBranchPage(RequestBranchUniqeGetDto request);//// admin/branchProxy

    }

    public class BranchUniqeGetService : IBranchUniqeGetService
    {
        private readonly IDataBaseContext _context;
        public BranchUniqeGetService(IDataBaseContext Context)
        {
            _context = Context;

        }

        public ResultDto<GetBranhUniqeDto> Execute(RequestBranchUniqeGetDto request)
        {
            try
            {
                var branch = _context.BranchINFOs.FirstOrDefault(c => c.BranchCode == request.BranchCode);

                if (branch == null)
                {
                    return new ResultDto<GetBranhUniqeDto>()
                    {
                        Data = null,
                        IsSuccess = false,
                        Message = "خطا در دریافت اطلاعات شعبه"

                    };
                }


                var branchList = new GetBranhUniqeDto
                {
                    Id = branch.Id,
                    BranchCode = branch.BranchCode,
                    BranchName = branch.BranchName,

                    BranchCity = new IdLabelDto
                    {
                        id = branch.BranchCity,
                        label = _context.Cities.FirstOrDefault(c => c.Id == branch.BranchCity).city

                    },

                    BranchProvince = new IdLabelDto
                    {
                        id = branch.BranchProvince,
                        label = _context.Cities.FirstOrDefault(c => c.ProvinceId == branch.BranchProvince).province
                    },

                    BranchCountry = new IdLabelDto
                    {
                        id = branch.BranchCountry,
                        label = _context.Cities.FirstOrDefault(c => c.CountryId == branch.BranchCountry).Country
                    },
                    BranchPhone1 = branch.BranchPhone1,
                    BranchPhone2 = branch.BranchPhone2,
                    TelegramId = branch.TelegramId,
                    WhatsAppId = branch.WhatsAppId,
                    InstagramId = branch.InstagramId,
                    ActivityHistory = branch.ActivityHistory,
                    Star = branch.Star,
                    Description = branch.Description,
                    BranchAddress = branch.BranchAddress,
                    PostalCode = branch.PostalCode,
                    Latitude = branch.Latitude,
                    Longitude = branch.Longitude,
                    MainImage = branch.MainImage,
                    OpeningTime=branch.OpeningTime,
                    CloseingTime=branch.CloseingTime,
                    BranchDiscount = branch.BranchDiscount,
                    InitialPayment=branch.InitialPayment,
                    NonCashAddingPayment=branch.NonCashAddingPayment,


                };



                if (!string.IsNullOrEmpty(branch.Images))
                {
                    List<string> imageUrls = branch.Images
                        .Split(',')
                        .Select(url => url.Trim()) // Trim any whitespace from each URL
                        .ToList();
                    branchList.Images = imageUrls;
                }
                



                return new ResultDto<GetBranhUniqeDto>()
                {
                    Data = branchList,
                    IsSuccess = true,
                    Message = " دریافت موفقیت آمیز "

                };
            }
            catch (Exception ex)
            {
                return new ResultDto<GetBranhUniqeDto>()
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "خطای ناگهانی"

                };

            }
        }




        public ResultDto<object> GetBranchInfosInBranchPage(RequestBranchUniqeGetDto request)
        {
            try
            {
                var branch = _context.BranchINFOs.FirstOrDefault(c => c.BranchCode == request.BranchCode);

                if (branch == null)
                {
                    return new ResultDto<object>()
                    {
                        Data = null,
                        IsSuccess = false,
                        Message = "خطا در دریافت اطلاعات شعبه"

                    };
                }
                List<string> mainImage = null;
                if (branch.MainImage != null)
                {
                    mainImage = (string.IsNullOrEmpty(branch.Images) ? branch.MainImage : $"{branch.MainImage},{branch.Images}").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                string openingHourMinute = $"{branch.OpeningTime.GetValueOrDefault().Hours:D2}:{branch.OpeningTime.GetValueOrDefault().Minutes:D2}";
                string closingHourMinute = $"{branch.CloseingTime.GetValueOrDefault().Hours:D2}:{branch.CloseingTime.GetValueOrDefault().Minutes:D2}";

                var branchList = new 
                {
                    
                    Id = branch.Id,
                    BranchName = branch.BranchName,
                    Images = mainImage,

                    OpeningTime = openingHourMinute ?? null,
                    ClosingTime= closingHourMinute ?? null,
                    Address=branch.BranchAddress ?? null,
                    phone=branch.BranchPhone1 ?? null,
                    latitude=branch.Latitude ,
                    longitude=branch.Longitude ,
                    InstagramId=branch.InstagramId ?? null,
                    WhatsAppId = branch.WhatsAppId ?? null,
                    EitaId = branch.EitaId?? null,
                    TelegramId = branch.TelegramId?? null,
                    BranchCode = branch.BranchCode,

                    
                };




                return new ResultDto<object>()
                {
                    Data = branchList,
                    IsSuccess = true,
                    Message = " دریافت موفقیت آمیز "

                };
            }
            catch (Exception ex)
            {
                return new ResultDto<object>()
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "خطای ناگهانی"

                };

            }
        }





    }


    public class RequestBranchUniqeGetDto
    {
        public long BranchCode { get; set; }
    }

    public class GetBranhUniqeDto
    {
        public long Id { get; set; }

        public long BranchCode { get; set; }
        public string BranchName { get; set; }
        public IdLabelDto BranchCity { get; set; }
        public IdLabelDto BranchProvince { get; set; }
        public IdLabelDto BranchCountry { get; set; }
        public string? BranchPhone1 { get; set; }
        public string? BranchPhone2 { get; set; }
        public string? InstagramId { get; set; }
        public string? TelegramId { get; set; }
        public string? WhatsAppId { get; set; }
        public string? MainImage { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? BranchAddress { get; set; }
        public int? ActivityHistory {  get; set; }
        public int? Star {  get; set; }
        public string? PostalCode { get; set; }
        public string? Description { get; set; }
        public List<string> Images { get; set; } = null;
        public TimeSpan? OpeningTime { get; set; }
        public TimeSpan? CloseingTime { get; set; }
        public float InitialPayment { get; set; }
        public float BranchDiscount { get; set; }
        public float NonCashAddingPayment { get; set; }


    }
}
