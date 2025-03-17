using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Factors.CRM.Commands.EditWorkName;
using Radin.Common.Dto;
using Radin.Domain.Entities.Factors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.CRM.Commands.ExitCrm
{
    public interface IExitCrmService
    {
        ResultDto Exit(ExitCrmRequest request);
    }


    public class ExitCrmService : IExitCrmService
    {
        private readonly IDataBaseContext _context;

        public ExitCrmService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Exit(ExitCrmRequest request)
        {
            try
            {

                if (request.factorId==null)
                {
                    return new ResultDto()
                    {
                        IsSuccess = false,
                        Message = "شماره فاکتور نامعتبر",

                    };
                }
                var factor = _context.MainFactors.FirstOrDefault(c => !c.IsRemoved && c.Id == request.factorId&& !c.position);
                if (factor == null)
                {
                    return new ResultDto()
                    {
                        IsSuccess = false,
                        Message = "چنین فاکتوری وجود ندارد",

                    };
                }
                if((factor.state==1 || factor.state == 2 || factor.state==3) && !NegotiationCheck(factor))
                {
                    return new ResultDto()
                    {
                        IsSuccess = false,
                        Message = "اطلاعات مذاکره باید تکمیل شود",

                    };

                }
                factor.position = true;
                _context.MainFactors.Update(factor);
                _context.SaveChanges();
                return new ResultDto()
                {
                    IsSuccess = true,
                    Message = "ویرایش با موفقیت انجام شد",
                };
            }
            catch (Exception ex)
            {

                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "خطایی رخ داد",

                };


            }

            
        }
        private bool NegotiationCheck(MainFactor Factor)
        {
            if (Factor.MainsellerID != null && Factor.TotalAmount > 0 && Factor.RecommandedDesign > 0 && !string.IsNullOrEmpty(Factor.ReasonStatus) && Factor.CustomerID != null && Factor.ContactType != 0)
            {

                return true;
            }
            else { return false; }


        }


    }
   
    public class ExitCrmRequest
    {
        public long factorId { get; set; }

    }

}
