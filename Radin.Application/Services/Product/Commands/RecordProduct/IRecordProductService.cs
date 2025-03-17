using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Product.Commands.RecordProduct
{
    public interface IRecordProductService
    {
        Task<ResultDto<string>> HandleRecording(RecordRequest request);
    }
}
