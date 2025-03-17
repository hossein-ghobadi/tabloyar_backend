using Radin.Application.Interfaces.Contexts;
using Radin.Common.Dto;
using Radin.Domain.Entities.Branches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Branch.Commands.BranchInfoSetService
{
    public interface IBranchInfoEditService
    {
        ResultDto<ResultBranchInfoEditDto> Execute(RequestBranchInfoEditDto result);// admin/branchProxy
    }

    public class BranchInfoEditService : IBranchInfoEditService
    {
        private readonly IDataBaseContext _context;

        public BranchInfoEditService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultBranchInfoEditDto> Execute(RequestBranchInfoEditDto request)
        {
            

            try
            {
                    var info = _context.BranchINFOs.FirstOrDefault(c => c.BranchCode == request.BranchCode);
                info.BranchName = request.BranchName;
                info.BranchPhone1 = request.BranchPhone1 ?? info.BranchPhone1;
                info.BranchPhone2 = request.BranchPhone2 ?? info.BranchPhone2;
                info.BranchCity = request.BranchCity ;
                info.BranchProvince = request.BranchState ;
                info.BranchCountry = request.BranchCountry?? 1 ;
                info.BranchAddress = request.BranchAddress ?? info.BranchAddress;
                info.PostalCode = request.PostalCode ?? info.PostalCode;
                info.Latitude = request.Latitude;
                info.Longitude = request.Longitude;
                info.TelegramId = request.TelegramId ?? info.TelegramId;
                info.WhatsAppId = request.WhatsAppId ?? info.WhatsAppId;
                info.InstagramId = request.InstagramId ?? info.InstagramId;
                info.Description = request.Description ?? info.Description;
                info.MainImage = request.MainImage ?? info.MainImage;
                info.Star = request.Star ?? info.Star;
                info.ActivityHistory = request.ActivityHistory ?? info.ActivityHistory;
                if(request.OpeningTime != null&& request.CloseingTime != null)
                {
                    info.OpeningTime = request?.OpeningTime;
                    info.CloseingTime = request?.CloseingTime;
                }
                
                info.BranchDiscount=request?.BranchDiscount ?? info.BranchDiscount;
                info.InitialPayment=request?.InitialPayment ?? info.InitialPayment;
                info.NonCashAddingPayment = request?.NonCashAddingPayment ?? info.NonCashAddingPayment;
                if (request.Images != null)
                {
                    string imageUrlsString = string.Join(",", request.Images.Where(img => !string.IsNullOrWhiteSpace(img)));

                    if (!string.IsNullOrEmpty(imageUrlsString)) { info.Images = imageUrlsString; }else { info.Images = null; }
                   
                }
                else
                {
                    info.Images = null;
                }
                _context.SaveChanges();

                    return new ResultDto<ResultBranchInfoEditDto>()

                    {
                        Data = new ResultBranchInfoEditDto()
                        {
                            BranchCode = info.BranchCode,
                        },
                        IsSuccess = true,
                        Message = "اطلاعات پایه این شعبه با موفقیت ویریش شد",
                    };
                
            }
            catch (Exception)
            {
                return new ResultDto<ResultBranchInfoEditDto>()
                {
                    Data = new ResultBranchInfoEditDto()
                    {
                        BranchCode = 0,
                    },
                    IsSuccess = false,
                    Message = "اطلاعات پایه این شعبه ویرایش نشد !"
                };


            }

        }

    }



    public class RequestBranchInfoEditDto
    {
        public string BranchName {  get; set; }
        public long BranchCode { get; set; }
        public int BranchCity { get; set; }
        public int BranchState { get; set; }
        public int? BranchCountry { get; set; }
        public string? BranchAddress { get; set; }
        public string? PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? BranchPhone1 { get; set; }
        public string? BranchPhone2 { get; set; }
        public string? TelegramId { get; set; }
        public string? WhatsAppId { get; set; }
        public string? InstagramId { get; set; }
        public string? Description { get; set; }
        public string? MainImage { get; set; }
        public List<string>? Images { get; set; }
        public int? Star { get; set; }
        public int? ActivityHistory { get; set; }
        public TimeSpan? OpeningTime { get; set; }
        public TimeSpan? CloseingTime { get; set;}
        public float? BranchDiscount { get; set; }
        public float? InitialPayment { get; set; }
        public float? NonCashAddingPayment { get; set; }

    }

    public class ResultBranchInfoEditDto
    {
        public long BranchCode { get; set; }
        //public List<IdLabelDto> Errors { get; set; }
    }
}
