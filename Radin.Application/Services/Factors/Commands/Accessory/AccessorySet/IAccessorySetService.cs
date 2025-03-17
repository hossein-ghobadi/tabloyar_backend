using Radin.Application.Services.Factors.Commands.RecordProduct;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Factors.Commands.Accessory.AccessorySet
{
    public interface IAccessorySetService
    {
         Task<ResultDto<long>> Execute(RequestAccessorySetDto request);

    }
}
