using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Branch.Commands.BranchInfoSetService;
using Radin.Common.Dto;
using Radin.Domain.Entities.Branches;
using System;
using System.Collections.Generic;

namespace Radin.Application.Services.Branch.Commands.BranchRegisterService
{
    public interface IBranchRegisterService
    {
        ResultDto<List<IdLabelDto>> Register(BranchRegisterModel request);
    }

    public class BranchRegisterService : IBranchRegisterService
    {
        private readonly IDataBaseContext _context;

        public BranchRegisterService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<List<IdLabelDto>> Register(BranchRegisterModel request)
        {
            // Perform validation using the custom Validate method
            var validationErrors = request.Validate();
            var BranchRequest = new BranchRegister
            {
                fName=request.fName,
                lName=request.lName,
                city=request.city,
                age=Convert.ToString(request.age),
                phone=request.phone,
                occupation=request.occupation,
                yearsOfService=request.yearsOfService,
                cityOfService=request.cityOfService,
                desiredCity=request.desiredCity,
                currentCompany=request.currentCompany,
                avgCurrSalary=request.avgCurrSalary,
                Btype=request.Btype,
                aboutYou=request.aboutYou,
                


            };
            // If there are validation errors, return them as a failure result
            if (validationErrors.Any())
            {
                return new ResultDto<List<IdLabelDto>>()
                {
                    Data = validationErrors,
                    IsSuccess = false,
                    Message = "خطا در ثبت اطلاعات: برخی فیلدها نادرست وارد شده‌اند"
                };
            }

            try
            {
                // Save the valid BranchRegister entity to the database
                _context.BranchRegisters.Add(BranchRequest);
                _context.SaveChanges();

                return new ResultDto<List<IdLabelDto>>()
                {
                    Data = null,
                    IsSuccess = true,
                    Message = "درخواست نمایندگی شما با موفقیت ثبت شد"
                };
            }
            catch (Exception)
            {
                return new ResultDto<List<IdLabelDto>>()
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "اطلاعات پایه این شعبه ثبت نشد!"
                };
            }
        }
    }
}
