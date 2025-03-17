using Radin.Application.Interfaces.Contexts;
using Radin.Common.Dto;
using Radin.Domain.Entities.ClaimsInfo;
using Radin.Domain.Entities.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Claims.Commands.ClaimCategorySetService
{
    public interface IClaimCategorySetService
    {
        ResultDto<ResultClaimCategorySetDto> Execute(RequestClaimCategorySetDto request);

    }

    public class ClaimCategorySetService : IClaimCategorySetService
    {
        private readonly IDataBaseContext _context;

        public ClaimCategorySetService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultClaimCategorySetDto> Execute(RequestClaimCategorySetDto request)
        {

            var Errors = new List<IdLabelDto>();
            try
            {
                int id = 0;
                var TitleDup = _context.ClaimCategories.FirstOrDefault(c => c.CategoryName == request.CategoryName);

                if (string.IsNullOrWhiteSpace(request.CategoryName))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!نام دسته را وارد نمایید"
                    });
                }
                if (TitleDup != null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!این نام دسته قبلا ثبت شده است"
                    });
                }

                if (Errors.Count() < 1)
                {
                    ClaimCategoryInfo category = new ClaimCategoryInfo()
                    {
                        CategoryName = request.CategoryName,
                        Description = request.Description,
                    };

                    _context.ClaimCategories.Add(category);

                    _context.SaveChanges();

                    return new ResultDto<ResultClaimCategorySetDto>()
                    {
                        Data = new ResultClaimCategorySetDto()
                        {
                            CategoryId = category.Id,
                            Errors = Errors,
                        },
                        IsSuccess = true,
                        Message = "دسته بندی محتوی با موفقیت درج شد",
                    };
                }
                else
                {
                    return new ResultDto<ResultClaimCategorySetDto>()
                    {
                        Data = new ResultClaimCategorySetDto()
                        {
                            CategoryId = 0,
                            Errors = Errors,
                        },
                        IsSuccess = false,
                        Message = "دسته بندی جدید درج نشد !"
                    };

                }
            }
            catch (Exception)
            {
                return new ResultDto<ResultClaimCategorySetDto>()
                {
                    Data = new ResultClaimCategorySetDto()
                    {
                        CategoryId = 0,
                        Errors = Errors,
                    },
                    IsSuccess = false,
                    Message = "دسته بندی جدید درج نشد !"
                };


            }

        }

    }
    public class RequestClaimCategorySetDto
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
    
    }

    public class ResultClaimCategorySetDto
    {
        public long CategoryId { get; set; }
        public List<IdLabelDto> Errors { get; set; }
    }

}
