//using Microsoft.AspNetCore.Identity;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Factors.Commands.Orders;
//using Radin.Common.Dto;
//using Radin.Common.StaticClass;
//using Radin.Domain.Entities.Users;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Factors.Queries.OrderGet
//{
//    public interface IOrderGetService
//    {
//        ResultDto<Step1Result> OrderGetInStep1(Step1Request request);
//    }
//    public class OrderGetService : IOrderGetService
//    {
//        private readonly IDataBaseContext _context;
//        private readonly UserManager<User> _userManager;

//        public OrderGetService(IDataBaseContext context, UserManager<User> userManager)
//        {
//            _context = context;
//            _userManager = userManager;

//        }
//        public ResultDto<Step1Result> OrderGetInStep1(Step1Request request)
//        {
//            try
//            {
//                var user = _userManager.FindByIdAsync(request.UserId).Result;
//                long BranchCode = user.BranchCode;
//                if (request.FactorId <= 0) {
//                    return new ResultDto<Step1Result>
//                    {
//                        Data = new Step1Result(),
//                        IsSuccess = false,
//                        Message = "فاکتور نامتعبر",
//                    };

//                }
//                    var existingFactor = _context.MainFactors.FirstOrDefault(f => f.Id == request.FactorId && f.IsRemoved == false);
//                    //Console.WriteLine(existingFactor.Id);

//                    if (existingFactor != null)
//                    {

//                    //if (existingFactor.BranchCode != BranchCode)
//                    //{
//                    //    return new ResultDto<Step1Result>
//                    //    {
//                    //        Data = new Step1Result(),
//                    //        IsSuccess = false,
//                    //        Message = "شما مجاز به دریافت اطلاعات نیستید",
//                    //    };

//                    //}
//                    long reversedTimestamp = SimpleMethods.DateTimeToTimeStamp(existingFactor.InitialConnectionTime);
//                    string InitialConnectionTime = reversedTimestamp.ToString();
                        

//                        return new ResultDto<Step1Result>
//                        {
//                            Data=new Step1Result
//                            {
//                                WorkName = existingFactor.WorkName,
//                                InitialConnectionTime = Convert.ToInt64(InitialConnectionTime) ,
//                                FactorId = request.FactorId,
//                            },
//                            IsSuccess = true,
//                            Message="دریافت موفق"
                            

//                        };
//                    }
//                    else
//                    {
//                        return new ResultDto<Step1Result>
//                        {
//                            Data = new Step1Result(),
//                            IsSuccess = false,
//                            Message = "اطلاعاتی برای این شماره فاکتور وجود ندارد",
//                        };
//                    }
                


//            }
//            catch
//            {
//                return new ResultDto<Step1Result>
//                {
//                    Data = new Step1Result(),
//                    IsSuccess = false,
//                    Message = "اشکال در دریافت اطلاعات",
//                };

//            }
//        }


//    }
//    //var existingFactor = _context.MainFactors.FirstOrDefault(f => f.Id == request.FactorId);
//    public class Step1Request
//    {
//        public long? FactorId { get; set; }
//        public string UserId { get; set; }
//    }
//    public class Step1Result
//    {
//        public long? FactorId { get; set; }
//        public long InitialConnectionTime { get; set; }
//        public string WorkName { get; set; }

//    }


//}
