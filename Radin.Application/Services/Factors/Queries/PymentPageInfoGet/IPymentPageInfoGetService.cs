//using Radin.Application.Interfaces.Contexts;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Factors.Queries.PymentPageInfoGet
//{
//    public interface IPymentPageInfoGetService
//    {
//        ResultDto<PymentInfoDto> GetInfo(PymentInfoRequest request);
//        ResultDto<List<CheckPaymentResultDto>> GetCheckInformation(CheckRequestDto request);
//        ResultDto<List<CheckPaymentResultDto>> GetNonCashCheckInformation(CheckRequestDto request);

//    }

//    public class PymentPageInfoGetService: IPymentPageInfoGetService
//    {
//        private readonly IDataBaseContext _context;
//        public  PymentPageInfoGetService(IDataBaseContext context)
//        {
//            _context = context;
//        }

//        public  ResultDto<PymentInfoDto> GetInfo(PymentInfoRequest request)
//        {

//            try 
//            {
//                var factor = _context.MainFactors.FirstOrDefault(f => f.Id == request.FactorID);
//                if (factor == null)
//                {
//                    return new ResultDto<PymentInfoDto>()
//                    {
//                        Message = "فاکتوری با این شماره یافت نشد",
//                        IsSuccess = false,
//                    };
//                }
//                float TotalAmount = factor.TotalAmount.Value;
//                long BranchCode = factor.BranchCode;
//                var branch = _context.BranchINFOs.FirstOrDefault(c => c.BranchCode ==  BranchCode);
//                float DiscountPrice = TotalAmount * branch.BranchDiscount/100;
//                float FinalPrice = TotalAmount - DiscountPrice;
//                var PymentInfoResult = new PymentInfoDto
//                {
//                    TotalAmount = TotalAmount,
//                    BranchDiscount = branch.BranchDiscount,
//                    FinalPrice = FinalPrice,
//                    DiscountPrice = DiscountPrice,
//                    MinimumPrice = FinalPrice * branch.InitialPayment/100,
//                    MinimumCheckPrice= TotalAmount*branch.InitialPayment/100,
//                    NonCashPrice=(float)Math.Floor(Convert.ToDouble(TotalAmount * (100+branch.NonCashAddingPayment)/100)),

//                };
//                return new ResultDto<PymentInfoDto>()
//                {
//                    Data = PymentInfoResult,
//                    IsSuccess = true,
//                    Message = "دریافت اطلاعات صفحه پرداخت"
//                };

//            }
//            catch 
//            {
//                return new ResultDto<PymentInfoDto>()
//                {
//                    Message = "دریافت اطلاعات صفحه پرداخت ناموفق",
//                    IsSuccess = false,
//                };
//            }


//        }













//        public ResultDto<List<CheckPaymentResultDto>> GetCheckInformation(CheckRequestDto request)
//        {
//            try
//            {
//                var MainFactor = _context.MainFactors.Where(p => p.Id == request.factorId && !p.IsRemoved).FirstOrDefault();
//                if (MainFactor == null)
//                {
//                    return new ResultDto<List<CheckPaymentResultDto>>
//                    {
//                        IsSuccess = false,
//                        Message = "فاکتور وجود ندارد"
//                    };
//                }
//                if (MainFactor.TotalAmount == null)
//                {
//                    return new ResultDto<List<CheckPaymentResultDto>>
//                    {
//                        IsSuccess = false,
//                        Message = "مبلغ تعیین نشده است"
//                    };
//                }
//                if ((request.amount + 1000) > MainFactor.TotalAmount)
//                {
//                    return new ResultDto<List<CheckPaymentResultDto>>
//                    {
//                        IsSuccess = false,
//                        Message = $"  مبلغ اولیه پرداختی نباید بزرگتر از مبلغ کل باشد "
//                    };
//                }
//                if ((request.amount+1000) < 0.4 * MainFactor.TotalAmount)
//                {
//                    return new ResultDto<List<CheckPaymentResultDto>>
//                    {
//                        IsSuccess = false,
//                        Message = $" حداقل 40 درصد مبلغ فاکتور حدودا  {0.4 * MainFactor.TotalAmount} تومان  باید بصورت نقد پرداخت شود "
//                    };
//                }
//                int totalAmountRemaining = (int)(MainFactor.TotalAmount - request.amount); // Ensure it's an integer
//                int numberOfChecks = request.number ;
//                int baseEachPrice = totalAmountRemaining / numberOfChecks;
//                int remainder = totalAmountRemaining % numberOfChecks;
//                //float EachPrice = (MainFactor.TotalAmount - request.amount) / request.number ?? 0;
                
//                DateTime now = DateTime.Now;
//                Dictionary<int, List<int>> CheckDictionary = new Dictionary<int, List<int>>
//                {
//                {1,new List<int>{ 105} },
//                {2,new List<int>{ 60,105} },
//                {3,new List<int>{ 60,105,150} },
//                {4,new List<int>{ 60,90,120,150} },
//                {5,new List<int>{ 30,60,105,150,180} },
//                {6,new List<int>{ 30,60,90,120,150,180} },


//                 };
//                if (!CheckDictionary.ContainsKey(request.number))
//                {
//                    return new ResultDto<List<CheckPaymentResultDto>>
//                    {
//                        IsSuccess = false,
//                        Message = "این تعداد چک قابل انتخاب نیست"
//                    };
//                }
//                var daysList = CheckDictionary[request.number];
//                var datesList = daysList.Select(days => now.AddDays(days)).ToList();
//                // PersianCalendar persianCalendar = new PersianCalendar();
//                var Result = new List<CheckPaymentResultDto>();
//                int idCounter = 1; // Start the Id from 1 or any other initial value
//                int I = 1;
//                foreach (var date in datesList)
//                {
//                    var price = baseEachPrice;
//                    // string persianDate = $"{persianCalendar.GetYear(date)}/{persianCalendar.GetMonth(date):00}/{persianCalendar.GetDayOfMonth(date):00}";
//                    if (I == 1) { price = price + remainder; }
//                    Result.Add(new CheckPaymentResultDto
//                    {
//                        Id = idCounter++,
//                        DueDate = ConvertToMilliseconds(date),//persianDate,
//                        price = price
//                    });
//                    I++;
//                }
//                return new ResultDto<List<CheckPaymentResultDto>>
//                {
//                    Data = Result,
//                    IsSuccess = true,
//                    Message = "عملیات موفق"
//                };
//            }
//            catch (Exception ex)
//            {
//                return new ResultDto<List<CheckPaymentResultDto>>
//                {
//                    IsSuccess = false,
//                    Message = "خطا در عملیات"
//                };
//            }


//        }








//        public ResultDto<List<CheckPaymentResultDto>> GetNonCashCheckInformation(CheckRequestDto request)
//        {
//            try
//            {
                
//                var MainFactor = _context.MainFactors.Where(p => p.Id == request.factorId && !p.IsRemoved).FirstOrDefault();
//                if (MainFactor == null)
//                {
//                    return new ResultDto<List<CheckPaymentResultDto>>
//                    {
//                        IsSuccess = false,
//                        Message = "فاکتور وجود ندارد"
//                    };
//                }
//                if (MainFactor.TotalAmount == null)
//                {
//                    return new ResultDto<List<CheckPaymentResultDto>>
//                    {
//                        IsSuccess = false,
//                        Message = "مبلغ تعیین نشده است"
//                    };
//                }
                
//                var branch = _context.BranchINFOs.FirstOrDefault(p => p.BranchCode == MainFactor.BranchCode);
//                if (branch == null)
//                {
//                    return new ResultDto<List<CheckPaymentResultDto>>
//                    {
//                        IsSuccess = false,
//                        Message = "اطلاعات شعبه موجود نیست"
//                    };
//                }
//                float AddedPercent = branch.NonCashAddingPayment;

//                //float EachPrice = (float)Math.Floor(Convert.ToDouble((MainFactor.TotalAmount ?? 0) * (100 + AddedPercent) / 100 / request.number));

//                int totalAmountRemaining = (int)Math.Floor(Convert.ToDouble((MainFactor.TotalAmount ?? 0) * (100 + AddedPercent) / 100));
//                // Ensure it's an integer
//                int numberOfChecks = request.number;
//                int baseEachPrice = totalAmountRemaining / numberOfChecks;
//                int remainder = totalAmountRemaining % numberOfChecks;
//                if (totalAmountRemaining == 0)
//                {
//                    return new ResultDto<List<CheckPaymentResultDto>>
//                    {
//                        IsSuccess = false,
//                        Message = "مبلغ به درستی دریافت نشده است"
//                    };
//                }
//                DateTime now = DateTime.Now;
//                Dictionary<int, List<int>> CheckDictionary = new Dictionary<int, List<int>>
//                {
//                {1,new List<int>{ 105} },
//                {2,new List<int>{ 60,105} },
//                {3,new List<int>{ 60,105,150} },
//                {4,new List<int>{ 60,90,120,150} },
//                {5,new List<int>{ 30,60,105,150,180} },
//                {6,new List<int>{ 30,60,90,120,150,180} },


//                 };
//                if (!CheckDictionary.ContainsKey(request.number))
//                {
//                    return new ResultDto<List<CheckPaymentResultDto>>
//                    {
//                        IsSuccess = false,
//                        Message = "این تعداد چک قابل انتخاب نیست"
//                    };
//                }
//                var daysList = CheckDictionary[request.number];
//                var datesList = daysList.Select(days => now.AddDays(days)).ToList();
//                // PersianCalendar persianCalendar = new PersianCalendar();
//                var Result = new List<CheckPaymentResultDto>();
//                int idCounter = 1; // Start the Id from 1 or any other initial value
//                int I = 1;

//                foreach (var date in datesList)
//                {
//                    var price = baseEachPrice;
//                    // string persianDate = $"{persianCalendar.GetYear(date)}/{persianCalendar.GetMonth(date):00}/{persianCalendar.GetDayOfMonth(date):00}";
//                    if (I == 1) { price = price + remainder; }

//                    Result.Add(new CheckPaymentResultDto
//                    {
//                        Id = idCounter++,
//                        DueDate = ConvertToMilliseconds(date),//persianDate,
//                        price = price
//                    });
//                    I++;
//                }
//                return new ResultDto<List<CheckPaymentResultDto>>
//                {
//                    Data = Result,
//                    IsSuccess = true,
//                    Message = "عملیات موفق"
//                };
//            }
//            catch (Exception ex)
//            {
//                return new ResultDto<List<CheckPaymentResultDto>>
//                {
//                    IsSuccess = false,
//                    Message = "خطا در عملیات"
//                };
//            }


//        }

//        public long ConvertToMilliseconds(DateTime dateTime)
//        {
//            // Convert the DateTime to UTC to ensure consistency
//            DateTimeOffset dateTimeOffset = dateTime.ToUniversalTime();
//            return dateTimeOffset.ToUnixTimeMilliseconds();
//        }


//    }



    
//    public class CheckRequestDto
//    {
//        public int number { get; set; }
//        public long factorId { get; set; }
//        public float amount { get; set; }
//    }

//    public class CheckPaymentResultDto
//    {
//        public int Id { get; set; }
//        public long DueDate { get; set; }
//        public float price { get; set; }
//    }



//    public class PymentInfoRequest
//    {
//        public long FactorID { get; set; }
//    }

//    public class PymentInfoDto
//    {
//        public float TotalAmount { get; set; }
//        public float FinalPrice { get; set; }
//        public float MinimumPrice { get; set; }
//        public float MinimumCheckPrice { get; set; }

//        public float DiscountPrice { get; set; }
//        public float BranchDiscount { get; set; }
//        public float NonCashPrice { get; set; } 
//    }
//}
