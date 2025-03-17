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
    public interface IBranchInfoSetService
    {
        ResultDto<ResultBranchInfoSetDto> Execute(RequestBranchInfoSetDto result);/// Admin/branchProxy
    }


    public class BranchInfoSetService : IBranchInfoSetService
    {
        private readonly IDataBaseContext _context;

        public BranchInfoSetService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultBranchInfoSetDto> Execute(RequestBranchInfoSetDto request)
        {

            var Errors = new List<IdLabelDto>();
            try
            {
                int id = 0;
                var Data = _context.BranchINFOs.ToList();
                var BCodeDup = Data.FirstOrDefault(c => c.BranchCode == request.BranchCode);
                var BNameDup = Data.FirstOrDefault(c => c.BranchName == request.BranchName);



                if (request.BranchCode == null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!کد شعبه را وارد نمایید"
                    });
                }
                if (BCodeDup != null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!این کد به نام شعبه دیگری قبلا در سیستم ثبت شده است"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.BranchName))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!نام شعبه را وارد نمایید"
                    });
                }
                if (BNameDup != null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!این نام شعبه قبلا در سیستم ثبت شده است"
                    });
                }

                if (request.BranchDiscount == null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!مقدار تخفیف نقدی را وارد نمایید"
                    });
                }

                if (request.BranchDiscount.GetType() != typeof(float))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "! مقدار تخفیف نقدی باید بصورت عدد باشد"
                    });
                }

                if (request.InitialPayment == null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!مقدار پرداخت اولیه را وارد نمایید"
                    });
                }

                if (request.InitialPayment.GetType() != typeof(float))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "! مقدار پرداخت اولیه باید بصورت عدد باشد"
                    });
                }
              

                if (Errors.Count() < 1)
            {
                BranchINFO branch = new BranchINFO()
                {
                    BranchCode = request.BranchCode,
                    BranchName = request.BranchName,
                    BranchCity = request.BranchCity,
                    BranchProvince = request.BranchState,
                    BranchCountry = request.BranchCountry?? 1,
                    BranchAddress = request.BranchAddress,
                    PostalCode = request.PostalCode,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    BranchPhone1 = request.BranchPhone1,
                    BranchPhone2 = request.BranchPhone2,
                    TelegramId = request.TelegramId,
                    WhatsAppId = request.WhatsAppId,
                    InstagramId = request.InstagramId ?? null,
                    BranchDiscount = request.BranchDiscount,
                    InitialPayment = request.InitialPayment,
                    NonCashAddingPayment = request.NonCashAddingPayment,
                    Description = request.Description,
                    MainImage = request.MainImage,
                    Star = request.Star,
                    ActivityHistory = request.ActivityHistory,
                    CloseingTime=request.CloseingTime,
                    OpeningTime=request.OpeningTime
                    
                };
                if (request?.Images != null) { 
                    string imageUrlsString = string.Join(",", request.Images.Where(img => !string.IsNullOrWhiteSpace(img)));
                    branch.Images = imageUrlsString;    
                }
                _context.BranchINFOs.Add(branch);

                    _context.SaveChanges();

                    return new ResultDto<ResultBranchInfoSetDto>()
                    {
                        Data = new ResultBranchInfoSetDto()
                        {
                            BranchCode = branch.BranchCode,
                            Errors = Errors,
                        },
                        IsSuccess = true,
                        Message = "اطلاعات پایه این شعبه با موفقیت درج شد",
                    };
                }
                else
                {
                    return new ResultDto<ResultBranchInfoSetDto>()
                    {
                        Data = new ResultBranchInfoSetDto()
                        {
                            BranchCode = 0,
                            Errors = Errors,
                        },
                        IsSuccess = false,
                        Message = "اطلاعات پایه این شعبه درج نشد !"
                    };

                }
        }
            catch (Exception)
            {
                return new ResultDto<ResultBranchInfoSetDto>()
                {
                    Data = new ResultBranchInfoSetDto()
        {
            BranchCode = 0,
                        Errors = Errors,
                    },
                    IsSuccess = false,
                    Message = "اطلاعات پایه این شعبه درج نشد !"
                };


}

        }

    }


    public class RequestBranchInfoSetDto
    {
        public long BranchCode { get; set; }
        public string BranchName { get; set; }
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
        public float BranchDiscount { get; set; }
        public float InitialPayment { get; set; }
        public float NonCashAddingPayment { get; set; }
        public string? Description { get; set; }
        public string? MainImage { get; set; }
        public List<string>? Images { get; set; }
        public int? Star { get; set; } = 1;
        public int? ActivityHistory { get; set; }
        public TimeSpan? OpeningTime { get; set; }
        public TimeSpan? CloseingTime { get; set;}
    }

    public class ResultBranchInfoSetDto
    {
        public long BranchCode { get; set; }
        public List<IdLabelDto> Errors { get; set; }
    }
}
