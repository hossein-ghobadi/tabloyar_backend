//using Radin.Application.Interfaces.Contexts;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Others;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Factors.Commands.StatusReason
//{
//    public interface IStatusReasonRecoveryService
//    {
//        ResultDto<ResultStatusReasonRecoveryDto> Execute(RequestStatusReasonRecoveryDto request);
//    }
//    public class StatusReasonRecoveryService : IStatusReasonRecoveryService
//    {
//        private readonly IDataBaseContext _context;

//    public StatusReasonRecoveryService (IDataBaseContext context)
//    {
//        _context = context;
//    }

//    public ResultDto<ResultStatusReasonRecoveryDto> Execute(RequestStatusReasonRecoveryDto request)
//    {

//        var Errors = new List<IdLabelDto>();
//        try
//        {
//            int id = 0;
//            var factor = _context.MainFactors.FirstOrDefault(c => c.Id == request.FactorId);

//            if (factor == null)
//            {
//                id = id + 1;
//                Errors.Add(new IdLabelDto
//                {
//                    id = id,
//                    label = "!فاکتوری با این شماره یافت نشد"
//                });
//            }


//            if (Errors.Count() < 1)
//            {
//                    factor.position = false;
//                    factor.ReasonStatus = null;
//                   _context.SaveChanges();

//                return new ResultDto<ResultStatusReasonRecoveryDto>()
//                {
//                    Data = new ResultStatusReasonRecoveryDto()
//                    {
//                        reason = factor.ReasonStatus,
//                        Errors = Errors,
//                    },
//                    IsSuccess = true,
//                    Message = "فاکتور مورد نظر مجددا به لیست مدیریت ارتباط مشتریان بازگشت.",
//                };
//            }
//            else
//            {
//                return new ResultDto<ResultStatusReasonRecoveryDto>()
//                {
//                    Data = new ResultStatusReasonRecoveryDto()
//                    {
//                        reason = factor.ReasonStatus,
//                        Errors = Errors,
//                    },
//                    IsSuccess = false,
//                    Message = "بازگشت این مجدد این فاکتور به لیست مدیریت ارتباط با مشتریان با خطا مواجه شد!"
//                };

//            }
//        }
//        catch (Exception)
//        {
//            return new ResultDto<ResultStatusReasonRecoveryDto>()
//            {
//                Data = new ResultStatusReasonRecoveryDto()
//                {
//                    reason = "",
//                    Errors = Errors,
//                },
//                IsSuccess = false,
//                Message = "بازگشت این مجدد این فاکتور به لیست مدیریت ارتباط با مشتریان با خطا مواجه شد !"
//            };


//        }

//    }

//}



//public class RequestStatusReasonRecoveryDto
//    {
//        public long FactorId { get; set; }
//    }

//    public class ResultStatusReasonRecoveryDto
//    {
//        public string reason { get; set; }
//        public List<IdLabelDto> Errors { get; set; }
//    }
//}
