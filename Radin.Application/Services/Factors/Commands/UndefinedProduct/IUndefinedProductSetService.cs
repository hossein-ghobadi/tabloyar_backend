using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Factors.Commands.UndefinedProduct
{
    public interface IUndefinedProductSetService
    {

        Task<ResultDto<long>> UndefinedProductSet(UndefinedProductRequestDto request);



    }
}
