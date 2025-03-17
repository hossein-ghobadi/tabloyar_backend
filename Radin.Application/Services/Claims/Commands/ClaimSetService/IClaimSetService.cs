using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.CommentSet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Claim;
using Radin.Domain.Entities.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Claims.Commands.ClaimSetService
{
    public interface IClaimSetService
    {
        ResultDto<ResultClaimSetDto> Execute(RequestClaimSetDto request);

    }

    public class ClaimSetService : IClaimSetService
    {

        private readonly IDataBaseContext _context;

        public ClaimSetService(IDataBaseContext context)
        {
            _context = context;

        }
        public ResultDto<ResultClaimSetDto> Execute(RequestClaimSetDto request)
        {
            try
            {

                if (string.IsNullOrEmpty(request.ClaimName1))
                {
                    return new ResultDto<ResultClaimSetDto>()
                    {
                        Data = new ResultClaimSetDto()
                        {
                            ClaimId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام اختصاصی ادعا را وارد کنید"

                    };



                }
                if (string.IsNullOrEmpty(request.ClaimName2))
                {
                    return new ResultDto<ResultClaimSetDto>()
                    {
                        Data = new ResultClaimSetDto()
                        {
                            ClaimId = 0,
                        },
                        IsSuccess = false,
                        Message = "عنوان ادعا را وارد کنید"

                    };
                }

                //var claims = _context.ClaimInfos.FirstOrDefault(c => c.ClaimName1 == request.ClaimName1);
                //if (claims != null)
                //{
                //    return new ResultDto<ResultClaimSetDto>()
                //    {
                //        Data = new ResultClaimSetDto()
                //        {
                //            ClaimId = 0,
                //        },
                //        IsSuccess = false,
                //        Message = "این اعا در گذشته به ثبت رسیده است"

                //    };
                //}

                ClaimInfo claim = new ClaimInfo()
                {
                    Id = request.Id,
                    ClaimName1 = request.ClaimName1,
                    ClaimName2 = request.ClaimName2,
                    category = request.category

                };

                _context.ClaimInfos.Add(claim);

                _context.SaveChanges();



                return new ResultDto<ResultClaimSetDto>()
                {
                    Data = new ResultClaimSetDto()
                    {
                        ClaimId = claim.Id,

                    },
                    IsSuccess = true,
                    Message = " ادعای مورد نظر شما با موفقیت ثبت شد",
                };

            }


            catch (Exception)
            {
                return new ResultDto<ResultClaimSetDto>()
                {
                    Data = new ResultClaimSetDto()
                    {
                        ClaimId = 0,
                    },
                    IsSuccess = false,
                    Message = "ثبت ادعا ناموفق !"
                };

            }


        }



    }




    public class RequestClaimSetDto
    {
        public long Id { get; set; }
        public string ClaimName1 { get; set; }
        public string ClaimName2 { get; set; }
        public long category {  get; set; }

    }

    public class ResultClaimSetDto
    {
        public long ClaimId { get; set; }
    }

}
