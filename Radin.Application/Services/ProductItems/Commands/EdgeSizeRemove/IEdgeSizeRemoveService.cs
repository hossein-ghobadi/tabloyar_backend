using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.ProductItems.Commands.TitleRemove;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.ProductItems.Commands.EdgeSizeRemove
{
    public interface IEdgeSizeRemoveService
    {
        ResultDto Execute(long EdgeSizeId);


    }
    public class EdgeSizeRemoveService : IEdgeSizeRemoveService
    {
        private readonly IPriceFeeDataBaseContext _context;

        public EdgeSizeRemoveService(IPriceFeeDataBaseContext context)
        {
            _context = context;
        }


        public ResultDto Execute(long EdgeSizeId)
        {

            var edgeSize = _context.MaterialEdgeSizes.Find(EdgeSizeId);
            if (edgeSize == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "اندازه لبه مورد نظر یافت نشد"
                };
            }
            //title.RemoveTime = DateTime.Now;
            //title.IsRemoved = true;
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "اندازه لبه مورد نظر با موفقیت حذف شد"
            };
        }
    }
}
