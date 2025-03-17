using Radin.Application.Interfaces.Contexts;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Operations.Check
{
    public interface IBanckCheckService
    {
        ResultDto AverageDueDateValidation(CheckRequestDto request);
    }
    public class BanckCheckService : IBanckCheckService
    {

        
        public ResultDto AverageDueDateValidation(CheckRequestDto request)
        {
            try
            {
                
                var StandardDate = (request.MaxPaymentMonth + 1) * 15;
                var TotalAmount = request.CheckItems.Sum(x => x.Amount);
                float AverageDuedate = 0;
                if (request.CheckItems.Count == 0)
                {
                    return new ResultDto
                    {
                        IsSuccess = false,
                        Message = "لیست چک‌ها خالی است"
                    };

                }
                foreach (var Item in request.CheckItems)
                {
                    AverageDuedate += Item.Amount * (float)(Item.DueDate - request.PurchaseDate).TotalDays / TotalAmount;
                }
                if (AverageDuedate < (StandardDate + 3))
                {
                    return new ResultDto
                    {
                        IsSuccess = true,
                        Message = "تاریخ  و اعداد چک ها معتبر هستند"
                    };
                }
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "اعداد و تاریخ باید اصلاح شوند"
                };
            }
            catch  {

                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "خطا"
                };
            }

        }
    }
    public class CheckItem
    {
        public DateTime DueDate { get; set; }
        public float Amount { get; set; }

    }
    public class CheckRequestDto
    {
        public int MaxPaymentMonth { get; set; } = 6;
        public DateTime PurchaseDate { get; set; }
        public List<CheckItem> CheckItems { get; set;}
    }
}
