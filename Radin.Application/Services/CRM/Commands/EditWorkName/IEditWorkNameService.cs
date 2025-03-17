using Radin.Application.Interfaces.Contexts;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Factors.CRM.Commands.EditWorkName
{
    public interface IEditWorkNameService
    {


        ResultDto Edit(IdLabelDto request);


    }

    public class EditWorkNameService: IEditWorkNameService
    {
        private readonly IDataBaseContext _context;

        public EditWorkNameService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Edit(IdLabelDto request)
        {
            try
            {

                if (string.IsNullOrEmpty(request.label))
                {
                    return new ResultDto()
                    {
                        IsSuccess = false,
                        Message = "نام کار را وارد کنید",

                    };
                }
                var factor = _context.MainFactors.FirstOrDefault(c => !c.IsRemoved && c.Id == request.id);
                if (factor == null)
                {
                    return new ResultDto()
                    {
                        IsSuccess = false,
                        Message = "چنین فاکتوری وجود ندارد",

                    };
                }

                factor.WorkName = request.label;
                _context.MainFactors.Update(factor);
                _context.SaveChanges();
                return new ResultDto()
                {
                    IsSuccess = true,
                    Message = "ویرایش با موفقیت انجام شد",
                };
            }
            catch (Exception ex) {

                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "خطایی رخ داد",

                };


            }


        }



    }





}
