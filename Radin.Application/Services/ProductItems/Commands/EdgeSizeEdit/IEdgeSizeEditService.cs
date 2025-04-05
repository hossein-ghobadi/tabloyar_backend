//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.ProductItems.Commands.TitleEdit;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.ProductItems.Commands.EdgeSizeEdit
//{
//    public interface IEdgeSizeEditService
//    {
//        ResultDto Execute(UpdateEdgeSizeDto request);

//    }


//    public class EdgeSizeEditService : IEdgeSizeEditService
//    {
//        private readonly IPriceFeeDataBaseContext _context;

//        public EdgeSizeEditService(IPriceFeeDataBaseContext context)
//        {
//            _context = context;
//        }
//        public ResultDto Execute(UpdateEdgeSizeDto updateDto)
//        {
//            var edgeSize = _context.MaterialEdgeSizes.Find(updateDto.Id);
//            if (edgeSize == null)
//            {
//                return new ResultDto
//                {
//                    IsSuccess = false,
//                    Message = " اندازه لبه مورد نظر یافت نشد"
//                };
//            }

//            edgeSize.EdgeSize = updateDto.EdgeSize;

//            _context.SaveChanges();

//            return new ResultDto()
//            {
//                IsSuccess = true,
//                Message = "ویرایش اندازه لبه انجام شد"
//            };

//        }
//    }


//    public class UpdateEdgeSizeDto
//    {
//        public long Id { get; set; }
//        public float EdgeSize { get; set; }


//    }
//}
