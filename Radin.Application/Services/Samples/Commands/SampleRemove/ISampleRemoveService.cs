//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Samples.Commands.SampleRemove;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Contents;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Radin.Application.Services.Samples.Commands.SampleRemove.SampleRemoveService;

//namespace Radin.Application.Services.Samples.Commands.SampleRemove
//{
//    public interface ISampleRemoveService
//    {
//        ResultDto Execute(RequestSampleRemoveDto request);
//        ResultDto delete(RequestSampleRemoveDto request);

//    }

//    public class SampleRemoveService : ISampleRemoveService
//    {
//        private readonly IDataBaseContext _context;

//        public SampleRemoveService(IDataBaseContext context)
//        {
//            _context = context;
//        }


//        public ResultDto Execute(RequestSampleRemoveDto request)
//        {

//            var sample = _context.Samples.FirstOrDefault(c => c.SampleUniqeName == request.id);
//            if (sample == null)
//            {
//                return new ResultDto
//                {
//                    IsSuccess = false,
//                    Message = "ایده مورد نظر یافت نشد"
//                };
//            }
            
//            var msg = "";
//            if (sample.IsRemoved)
//            {
//                msg = "منتشر شد";
//            }
//            else
//            {
//                msg = "حالت پیش نویس";
//            }
//            sample.RemoveTime = DateTime.Now;
//            sample.IsRemoved = !(sample.IsRemoved);
//            _context.SaveChanges();
//            return new ResultDto()
//            {
//                IsSuccess = true,
//                Message = msg
//            };
//        }
        

//        public ResultDto delete(RequestSampleRemoveDto request)
//        {

//            var sample = _context.Samples.FirstOrDefault(c => c.SampleUniqeName == request.id);
//            if (sample == null)
//            {
//                return new ResultDto
//                {
//                    IsSuccess = false,
//                    Message = "ایده مورد نظر یافت نشد"
//                };
//            }
            
//            _context.Samples.Remove(sample);
//            _context.SaveChanges();
//            return new ResultDto()
//            {
//                IsSuccess = true,
//                Message = " حذف با موفقیت انجام شد"
//            };
//        }
//        public class RequestSampleRemoveDto
//        {
//            public string id { get; set; }
//        }
//    }
//}
