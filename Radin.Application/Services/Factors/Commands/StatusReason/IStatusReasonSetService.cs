using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.ContentCategorySet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Contents;
using Radin.Domain.Entities.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Factors.Commands.StatusReason
{
    public interface IStatusReasonSetService
    {
        ResultDto<ResultStatusReasonSetDto> Execute(RequestStatusReasonSetDto request);
    }

    public class StatusReasonSetService : IStatusReasonSetService
    {
        private readonly IDataBaseContext _context;

        public StatusReasonSetService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultStatusReasonSetDto> Execute(RequestStatusReasonSetDto request)
        {

            var Errors = new List<IdLabelDto>();
            try
            {
                int id = 0;
                var ReasonDup = _context.StatusReasons.FirstOrDefault(c => c.Reason == request.reason);

                if (string.IsNullOrWhiteSpace(request.reason))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!علت مورد نظر را وارد نمایید"
                    });
                }
                if (ReasonDup != null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!این علت دسته قبلا ثبت شده است"
                    });
                }



                if (Errors.Count() < 1)
                {
                    StatusReasons Reason = new StatusReasons()
                    {
                        status = request.status,
                        Reason = request.reason,

                    };
                    Reason.UpdateTime = Reason.InsertTime;
                    _context.StatusReasons.Add(Reason);

                    _context.SaveChanges();

                    return new ResultDto<ResultStatusReasonSetDto>()
                    {
                        Data = new ResultStatusReasonSetDto()
                        {
                            ReasonId = Reason.Id,
                            Errors = Errors,
                        },
                        IsSuccess = true,
                        Message = "علت مورد نظر با موفقیت درج شد",
                    };
                }
                else
                {
                    return new ResultDto<ResultStatusReasonSetDto>()
                    {
                        Data = new ResultStatusReasonSetDto()
                        {
                            ReasonId = 0,
                            Errors = Errors,
                        },
                        IsSuccess = false,
                        Message = "علت جدید درج نشد !"
                    };

                }
            }
            catch (Exception)
            {
                return new ResultDto<ResultStatusReasonSetDto>()
                {
                    Data = new ResultStatusReasonSetDto()
                    {
                        ReasonId = 0,
                        Errors = Errors,
                    },
                    IsSuccess = false,
                    Message = "دسته بندی جدید درج نشد !"
                };


            }

        }

    }




    public class RequestStatusReasonSetDto
    {
        public bool status { get; set; }
        public string reason { get; set; }
    }

    public class ResultStatusReasonSetDto
    {
        public long ReasonId { get; set; }
        public List<IdLabelDto> Errors { get; set; }
    }


}
