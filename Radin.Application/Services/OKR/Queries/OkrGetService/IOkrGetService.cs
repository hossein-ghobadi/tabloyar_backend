using Microsoft.EntityFrameworkCore;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Ideas.Commands.CommentRemove;
using Radin.Application.Services.OKR.Queries.TargetDeterminationGet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Factors;
using Radin.Domain.Entities.OKR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
 
namespace Radin.Application.Services.OKR.Queries.OkrGetService
{
    public interface IOkrGetService
    {
        ResultDto<OkrResult> Execute(int BranchCode);
        ResultDto<List<object>> DateTest(int BranchCode);
        ResultDto<OkrResult> Test(int BranchCode);

    }


    public class OkrGetService : IOkrGetService
    {
        private readonly IDataBaseContext _context;

        public OkrGetService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<OkrResult> Execute(int BranchCode)
        {

            DateTime now = DateTime.Now;

            // Convert to Persian (Jalali) date
            PersianCalendar persianCalendar = new PersianCalendar();
            int persianYear = persianCalendar.GetYear(now);
            int persianMonth = persianCalendar.GetMonth(now);

            // Get the number of days in the current Persian month
            int daysInMonth = persianCalendar.GetDaysInMonth(persianYear, persianMonth);

            // Determine the first day of the month
            DateTime firstDayOfMonth = new DateTime(persianYear, persianMonth, 1, persianCalendar);
            DateTime lastDayOfMonth = firstDayOfMonth.AddDays(daysInMonth - 1); // Last day of Jalali month
                                                                                // Extract Gregorian months
            int startGregorianMonth = firstDayOfMonth.Month; // Gregorian month for the first day






            int endGregorianMonth = lastDayOfMonth.Month;   // Gregorian month for the last day

            // List to hold Gregorian months
            List<int> gregorianMonths = new List<int> { startGregorianMonth };
            if (startGregorianMonth != endGregorianMonth)
            {
                gregorianMonths.Add(endGregorianMonth);
            }
            int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek; // 0 = Sunday, ..., 6 = Saturday
            //foreach (var item in gregorianMonths)
            //{
            //    Console.WriteLine($@"MOnth={item.ToString()}");

            //}





            // Adjust for Jalali week start on Saturday
            firstDayOfWeek = (firstDayOfWeek + 1) % 7; // 0 = Saturday, ..., 6 = Friday

            // Generate days and categorize them into weeks
            var monthData = new List<MonthData>();
            int currentWeek = 1;
            

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime currentDay = new DateTime(persianYear, persianMonth, day, persianCalendar);
                DateTime gregorianDate = persianCalendar.ToDateTime(persianYear, persianMonth, day, 0, 0, 0, 0);

                int currentWeekday = (int)currentDay.DayOfWeek;
                currentWeekday = (currentWeekday + 1) % 7; // Adjust for Saturday start

                if (currentWeekday == 0 && day > 1) // Start new week on Saturday
                {
                    currentWeek++;
                }

                monthData.Add(new MonthData
                {
                    JalaliDate = $"{persianYear}/{persianMonth:D2}/{day:D2}", // Format as YYYY/MM/DD
                    DayOfWeek = currentDay.ToString("dddd", new CultureInfo("fa-IR")), // Get Persian day name
                    Week = $"Week {currentWeek}",
                    DayofWeekNumber = currentWeekday,
                    MonthDay = gregorianDate.Day.ToString(),
                    MonthNumber=gregorianDate.Month.ToString(),
                    YearNumber=gregorianDate.Year.ToString(),
                    GregorianDate = gregorianDate.ToString("yyyy-MM-dd"),     // Format as YYYY/MM/DD
                    WeekNumber = currentWeek
                }); ;

            }
            string[] possibleFormats = { "yyyy-MM-dd", "MM/dd/yyyy" };
            string standardizedNow = now.ToString("yyyy-MM-dd");
            var CurrentDayCheck = monthData
                .Where(p =>
                {
                    // Try to parse GregorianDate into a DateTime object using possible formats
                    if (DateTime.TryParseExact(p.GregorianDate, possibleFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        // Standardize the parsed date and compare
                        return parsedDate.ToString("yyyy-MM-dd") == standardizedNow;
                    }
                    return false;
                })
                .FirstOrDefault();
            //var CurrentDayCheck = monthData.Where(p => p.GregorianDate == now.ToShortDateString()).FirstOrDefault();
            if (CurrentDayCheck == null)
            {
                return new ResultDto<OkrResult>
                {
                    IsSuccess = false,
                    Message = $@"Date={now.ToShortDateString()}"//اطلاعات تاریخ به درستی دریافت نشد
                };
            }
            var WeekDaysInfo = monthData.Where(p => p.WeekNumber == CurrentDayCheck.WeekNumber).ToList();
            var WeekPurchousAmount=new List<OKRData>();
            List<int?> In_Person = new List<int?> { 1, 3 };
            List<int?> Marketing = new List<int?> { 4,5,8,12,9,11 };
            List<int?> CyberSpace = new List<int?> { 2,6,7,10 };
            Dictionary<string, string> MonthName_Number = new Dictionary<string, string> {
                    { "01", "فروردین" },
                    {"02", "اردیبهشت" },
                    { "03", "خرداد" },
                    { "04", "تیر" },
                    { "05", "مرداد" },
                    { "06", "شهریور" },
                    { "07", "مهر" },
                    { "08", "آبان" },
                    { "09", "آذر" },
                    { "10", "دی" },
                    { "11", "بهمن" },
                    { "12", "اسفند" }
                };
            var UnmergedDataset = _context.MainFactors
            .Where(p => p.BranchCode == BranchCode  && !p.IsRemoved && gregorianMonths.Contains(Convert.ToInt32(p.month))).AsEnumerable();
            var DataSet = _context.MainFactors
            .Where(p => p.BranchCode == BranchCode && p.CustomerID != null && !p.IsRemoved && gregorianMonths.Contains(Convert.ToInt32(p.month))) // Filter out rows where CustomerId is null
            .Join(
                    _context.CustomerInfo,
                    mf => mf.CustomerID,
                    ci => ci.Id,
                    (mf, customerGroup) => new
                    {
                        Date = mf.LastConnectionTime,
                        Day =mf.day,
                        Month=mf.month,
                        Year=mf.year,
                        TotalAmount = mf.TotalAmount,
                        status = mf.status,
                        acquaintance = customerGroup.acquaintance,  // Select specific field from CustomerInfo
                    })
                .AsQueryable();





            var groupedByWeek = monthData
               .GroupBy(d => d.WeekNumber)
               .Select(g => new
               {

                   WeekNumber = g.Key,
                   StartDate = DateTime.Parse(g.Min(d => d.GregorianDate)),
                   EndDate = DateTime.Parse(g.Max(d => d.GregorianDate)),
                   ShamsiStart = g.Min(d => d.JalaliDate),
                   ShamsiEnd = g.Max(d => d.JalaliDate),
               });
            var MonthlyTarget = _context.MonthlyTargets.Where(p => p.BranchCode == BranchCode && p.month == persianMonth).FirstOrDefault();
            if (MonthlyTarget == null)
            {
                return new ResultDto<OkrResult>
                {
                    IsSuccess = false,
                    Message = " تارگت تعیین نشده است"
                };

            }
            Dictionary<int, float> Targets = new Dictionary<int, float> {
                {1,MonthlyTarget.week1 },
                {2,MonthlyTarget.week2 },
                { 3,MonthlyTarget.week3},
                {4,MonthlyTarget.week4 },
                { 5,MonthlyTarget.week5},
                { 6,MonthlyTarget.week6}
                        };
           

            List<object> AllWeeksCondition = new List<object>();
            foreach (var week in groupedByWeek)
            {

                // Query the second dataset based on the date range
                var ThisWeekPurchase = DataSet
                    .Where(record => record.Date >= week.StartDate && record.Date <= week.EndDate&& record.status)
                    .Sum(p => p.TotalAmount);
                //var ThisWeekDateMonth= $" {int.Parse(week.ShamsiStart.Split('/')[2])}-{int.Parse(week.ShamsiEnd.Split('/')[2])} {MonthName_Number[week.ShamsiStart.Split('/')[1]]}";
                var ThisWeekDateMonth = $"{ConvertToArabicNumbers($"{int.Parse(week.ShamsiEnd.Split('/')[2])}-{int.Parse(week.ShamsiStart.Split('/')[2])}")} {MonthName_Number[week.ShamsiStart.Split('/')[1]]}";
                var ThisWeekTarget = Targets[week.WeekNumber];
                var ThisWeekNumber = week.WeekNumber;
                var ThisWeeekColor = Environment.GetEnvironmentVariable("TARGET_NOT_ARRIVED");
                if (ThisWeekNumber <= CurrentDayCheck.WeekNumber)
                {
                    if (ThisWeekPurchase >= 0.8 * ThisWeekTarget && ThisWeekPurchase < ThisWeekTarget)
                    {
                        ThisWeeekColor = Environment.GetEnvironmentVariable("NEAR_TARGET");
                    }
                    else if (ThisWeekPurchase >= ThisWeekTarget)
                    {
                        ThisWeeekColor = Environment.GetEnvironmentVariable("PASS_TARGET");
                    }
                    else
                    {
                        ThisWeeekColor = Environment.GetEnvironmentVariable("FAIL_TARGET");
                    }
                }


                AllWeeksCondition.Add(new
                {
                    purchase = ThisWeekPurchase,
                    DateMonth = ThisWeekDateMonth,
                    Target = ThisWeekTarget,
                    Number = ThisWeekNumber,
                    Color = ThisWeeekColor

                });

            }





            //var DataSet =_context.MainFactors.Where(p=>p.BranchCode==BranchCode&& p.CustomerID!=null&&!p.IsRemoved&& gregorianMonths.Contains(Convert.ToInt32(p.month))).AsQueryable();

            var WeeklyTarget = Targets[CurrentDayCheck.WeekNumber];

            

            var InitialWeekPurchousAmount= new List<InitialOKRData>();
            foreach (var day in WeekDaysInfo)
            {
                //Console.WriteLine($" >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>False Amount = {DataSet.Where(p => !p.status && p.Day == day.MonthDay && p.Month == day.MonthNumber && p.Year == day.YearNumber).ToList().Count}");
                //Console.WriteLine($" >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>day= {day.MonthDay}");
                //Console.WriteLine($" >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>WeekNumber= {CurrentDayCheck.WeekNumber}");

                //Console.WriteLine($" >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>month= {day.MonthNumber}");
                //Console.WriteLine($" >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>year= {day.YearNumber}");

                InitialWeekPurchousAmount.Add(new InitialOKRData
                {
                    
                    DayNumber = day.DayofWeekNumber,
                    TrueTotalAmount = DataSet.Where(p => p.status && p.Day == day.MonthDay && p.Month == day.MonthNumber && p.Year==day.YearNumber).Sum(p => p.TotalAmount),
                    FalseTotalAmount = UnmergedDataset.Where(p => !p.status && p.day == day.MonthDay && p.month == day.MonthNumber && p.year == day.YearNumber).Sum(p => p.TotalAmount),
                    TrueCyberSpaceAmount = DataSet.Where(p => p.status && p.Day == day.MonthDay && p.Month == day.MonthNumber && p.Year == day.YearNumber && CyberSpace.Contains(p.acquaintance)).Sum(p => p.TotalAmount) ,
                    FalseCyberSpaceAmount = DataSet.Where(p => !p.status && p.Day == day.MonthDay && p.Month == day.MonthNumber && p.Year == day.YearNumber && CyberSpace.Contains(p.acquaintance)).Sum(p => p.TotalAmount) ,
                    TrueMarketingAmount = DataSet.Where(p => p.status && p.Day == day.MonthDay && p.Month == day.MonthNumber && p.Year == day.YearNumber && Marketing.Contains(p.acquaintance)).Sum(p => p.TotalAmount) ,
                    FalseMarketingAmount = DataSet.Where(p => !p.status && p.Day == day.MonthDay && p.Month == day.MonthNumber && p.Year == day.YearNumber && Marketing.Contains(p.acquaintance)).Sum(p => p.TotalAmount) ,
                    TrueInPersonAmount = DataSet.Where(p => p.status && p.Day == day.MonthDay && p.Month == day.MonthNumber && p.Year == day.YearNumber && In_Person.Contains(p.acquaintance)).Sum(p => p.TotalAmount) ,
                    FalseInPersonAmount = DataSet.Where(p => !p.status && p.Day == day.MonthDay && p.Month == day.MonthNumber && p.Year == day.YearNumber && In_Person.Contains(p.acquaintance)).Sum(p => p.TotalAmount) ,


                });


            }
            var SumOfWeeks = new InitialOKRData
            {
                DayNumber = 7,
                TrueTotalAmount = InitialWeekPurchousAmount.Sum(p => p.TrueTotalAmount),
                FalseTotalAmount = InitialWeekPurchousAmount.Sum(p => p.FalseTotalAmount),
                TrueCyberSpaceAmount = InitialWeekPurchousAmount.Sum(p => p.TrueCyberSpaceAmount),
                FalseCyberSpaceAmount = InitialWeekPurchousAmount.Sum(p => p.FalseCyberSpaceAmount),
                TrueMarketingAmount = InitialWeekPurchousAmount.Sum(p => p.TrueMarketingAmount),
                FalseMarketingAmount = InitialWeekPurchousAmount.Sum(p => p.FalseMarketingAmount),
                TrueInPersonAmount = InitialWeekPurchousAmount.Sum(p => p.TrueInPersonAmount),
                FalseInPersonAmount = InitialWeekPurchousAmount.Sum(p => p.FalseInPersonAmount),
            };
            InitialWeekPurchousAmount.Add(SumOfWeeks);

            //foreach (var day in InitialWeekPurchousAmount)
            //{
            //    Console.WriteLine(day.ToString());
            //    Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  Trueprice={day.TrueTotalAmount}");
            //    Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  Falseprice={day.FalseTotalAmount}");

            //}


            WeekPurchousAmount = InitialWeekPurchousAmount.Select(p => new OKRData
            {
                DayNumber= p.DayNumber,
                TrueTotalAmount=p.TrueTotalAmount !=0 ? p.TrueTotalAmount.ToString(): "",
                FalseTotalAmount=p.FalseTotalAmount !=0 ? p.FalseTotalAmount.ToString():"",
                TrueCyberSpaceAmount=p.TrueCyberSpaceAmount !=0 ? p.TrueCyberSpaceAmount.ToString():"",
                FalseCyberSpaceAmount = p.FalseCyberSpaceAmount != 0 ? p.FalseCyberSpaceAmount.ToString() : "",
                TrueMarketingAmount=p.TrueMarketingAmount !=0 ? p.TrueMarketingAmount.ToString(): "",
                FalseMarketingAmount = p.FalseMarketingAmount != 0 ? p.FalseMarketingAmount.ToString() : "",
                TrueInPersonAmount = p.TrueInPersonAmount != 0 ? p.TrueInPersonAmount.ToString() : "",
                FalseInPersonAmount = p.FalseInPersonAmount != 0 ? p.FalseInPersonAmount.ToString() : "",

            }).ToList();



            //foreach (var day in WeekDaysInfo)
            //{


            //    WeekPurchousAmount.Add(new OKRData
            //    {

            //        DayNumber=day.DayofWeekNumber,
            //        TrueTotalAmount= DataSet.Where(p => p.status&& p.Day==day.MonthDay.ToString()&& p.Month==day.MonthNumber).Sum(p => p.TotalAmount)!=0 ? DataSet.Where(p=>p.status && p.Day == day.MonthDay.ToString() && p.Month == day.MonthNumber).Sum(p => p.TotalAmount).ToString() : "",
            //        FalseTotalAmount = DataSet.Where(p => !p.status && p.Day == day.MonthDay.ToString() && p.Month == day.MonthNumber).Sum(p => p.TotalAmount)!=0 ? DataSet.Where(p => !p.status && p.Day == day.MonthDay.ToString() && p.Month == day.MonthNumber).Sum(p => p.TotalAmount).ToString() : "",
            //        TruCyberSpaceAmount = DataSet.Where(p => p.status && p.Day == day.MonthDay.ToString() && p.Month == day.MonthNumber && CyberSpace.Contains(p.acquaintance)).Sum(p => p.TotalAmount) != 0? DataSet.Where(p => p.status && p.Day == day.MonthDay.ToString() && p.Month == day.MonthNumber && CyberSpace.Contains(p.acquaintance)).Sum(p => p.TotalAmount).ToString() : "",
            //        FalseCyberSpaceAmount = DataSet.Where(p => !p.status && p.Day == day.MonthDay.ToString() && p.Month == day.MonthNumber && CyberSpace.Contains(p.acquaintance)).Sum(p => p.TotalAmount) != 0 ? DataSet.Where(p => !p.status && p.Day == day.MonthDay.ToString() && p.Month == day.MonthNumber && CyberSpace.Contains(p.acquaintance)).Sum(p => p.TotalAmount).ToString() : "",
            //        TrueMarketingAmount = DataSet.Where(p => p.status && p.Day == day.MonthDay.ToString() && p.Month == day.MonthNumber && Marketing.Contains(p.acquaintance)).Sum(p => p.TotalAmount) != 0? DataSet.Where(p => p.status && p.Day == day.MonthDay.ToString() && p.Month == day.MonthNumber && Marketing.Contains(p.acquaintance)).Sum(p => p.TotalAmount).ToString() : "",
            //        FalseMarketingAmount = DataSet.Where(p => !p.status && p.Day == day.MonthDay.ToString() && p.Month == day.MonthNumber && Marketing.Contains(p.acquaintance)).Sum(p => p.TotalAmount) != 0? DataSet.Where(p => !p.status && p.Day == day.MonthDay.ToString() && p.Month == day.MonthNumber && Marketing.Contains(p.acquaintance)).Sum(p => p.TotalAmount).ToString() : "",
            //        TrueInPersonAmount = DataSet.Where(p => p.status && p.Day == day.MonthDay.ToString() && p.Month == day.MonthNumber && In_Person.Contains(p.acquaintance)).Sum(p => p.TotalAmount) != 0? DataSet.Where(p => p.status && p.Day == day.MonthDay.ToString() && p.Month == day.MonthNumber && In_Person.Contains(p.acquaintance)).Sum(p => p.TotalAmount).ToString() : "",
            //        FalseInPersonAmount = DataSet.Where(p => !p.status && p.Day == day.MonthDay.ToString() && p.Month == day.MonthNumber && In_Person.Contains(p.acquaintance)).Sum(p => p.TotalAmount) != 0? DataSet.Where(p => !p.status && p.Day == day.MonthDay.ToString() && p.Month == day.MonthNumber && In_Person.Contains(p.acquaintance)).Sum(p => p.TotalAmount).ToString() : "",


            //    });


            //}
            //var WeeklyTarget = Targets[CurrentDayCheck.WeekNumber];

            var WeeklyTrueTotalAmount = WeekPurchousAmount.Sum(p => string.IsNullOrEmpty(p.TrueTotalAmount)
            ? 0f
            
            : float.Parse(p.TrueTotalAmount, CultureInfo.InvariantCulture)); 
            DateTime ConvertedcurrentDay = DateTime.Parse(CurrentDayCheck.GregorianDate);
            float targetPercentage = 0;
            //Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>(((    {WeeklyTrueTotalAmount}");
            if (WeeklyTrueTotalAmount != 0)
            {
                targetPercentage =  WeeklyTrueTotalAmount / WeeklyTarget/2*100;
            }
            else
            {
                targetPercentage = 0; // Or set to a default value
            }
            string targetColor = "#e14202";
            if (targetPercentage < 30) { targetColor = Environment.GetEnvironmentVariable("OKR_LOWER30"); }
            else if (targetPercentage >=30 && targetPercentage < 60) { targetColor = Environment.GetEnvironmentVariable("OKR_30_60"); }
            else if (targetPercentage >= 60 && targetPercentage < 100) { targetColor = Environment.GetEnvironmentVariable("OKR_60_100"); }
            else if (targetPercentage >= 100) { targetColor = Environment.GetEnvironmentVariable("OKR_Higher100"); }
            //var Result = _context.MainFactors.Where(p => p.InsertTime == ConvertedcurrentDay).Sum(p => p.TotalAmount);
            var OkrResult = new OkrResult
            {
                TargetPercentage= (int)targetPercentage,
                WeeklyTarget= WeeklyTarget,
                TargetColor = targetColor,
                oKRDatas = WeekPurchousAmount,
                AllWeeksCondition=AllWeeksCondition,

            };
            return new ResultDto<OkrResult>
            {
                Data = OkrResult,
                IsSuccess = true,
                Message = $@"Date={now.ToShortDateString()}"// WeeklyTrueTotalAmount.ToString()
            };

            // Return the data as JSON



        }










































        public ResultDto<List<object>> DateTest(int BranchCode)
        {

            DateTime now = DateTime.Now;

            // Convert to Persian (Jalali) date
            PersianCalendar persianCalendar = new PersianCalendar();
            int persianYear = persianCalendar.GetYear(now);
            int persianMonth = persianCalendar.GetMonth(now);

            // Get the number of days in the current Persian month
            int daysInMonth = persianCalendar.GetDaysInMonth(persianYear, persianMonth);

            // Determine the first day of the month
            DateTime firstDayOfMonth = new DateTime(persianYear, persianMonth, 1, persianCalendar);
            DateTime lastDayOfMonth = firstDayOfMonth.AddDays(daysInMonth - 1); // Last day of Jalali month
                                                                                // Extract Gregorian months
            int startGregorianMonth = firstDayOfMonth.Month; // Gregorian month for the first day






            int endGregorianMonth = lastDayOfMonth.Month;   // Gregorian month for the last day

            // List to hold Gregorian months
            List<int> gregorianMonths = new List<int> { startGregorianMonth };
            if (startGregorianMonth != endGregorianMonth)
            {
                gregorianMonths.Add(endGregorianMonth);
            }
            int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek; // 0 = Sunday, ..., 6 = Saturday
            //foreach (var item in gregorianMonths)
            //{
            //    Console.WriteLine($@"MOnth={item.ToString()}");

            //}





            // Adjust for Jalali week start on Saturday
            firstDayOfWeek = (firstDayOfWeek + 1) % 7; // 0 = Saturday, ..., 6 = Friday

            // Generate days and categorize them into weeks
            var monthData = new List<MonthData>();
            int currentWeek = 1;


            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime currentDay = new DateTime(persianYear, persianMonth, day, persianCalendar);
                DateTime gregorianDate = persianCalendar.ToDateTime(persianYear, persianMonth, day, 0, 0, 0, 0);

                int currentWeekday = (int)currentDay.DayOfWeek;
                currentWeekday = (currentWeekday + 1) % 7; // Adjust for Saturday start

                if (currentWeekday == 0 && day > 1) // Start new week on Saturday
                {
                    currentWeek++;
                }

                monthData.Add(new MonthData
                {
                    JalaliDate = $"{persianYear}/{persianMonth:D2}/{day:D2}", // Format as YYYY/MM/DD
                    DayOfWeek = currentDay.ToString("dddd", new CultureInfo("fa-IR")), // Get Persian day name
                    Week = $"Week {currentWeek}",
                    DayofWeekNumber = currentWeekday,
                    GregorianDate = gregorianDate.ToString("yyyy-MM-dd"),     // Format as YYYY/MM/DD
                    WeekNumber = currentWeek,
                    MonthDay = gregorianDate.Day.ToString(),
                    MonthNumber=gregorianDate.Month.ToString(),

                });

            }
            //return new ResultDto<object>
            //{
            //    IsSuccess = true,
            //    Message="",
            //    Data = monthData
            //};
            string[] possibleFormats = { "yyyy-MM-dd", "MM/dd/yyyy" };
            string standardizedNow = now.ToString("yyyy-MM-dd");
            var CurrentDayCheck = monthData
                .Where(p =>
                {
                    // Try to parse GregorianDate into a DateTime object using possible formats
                    if (DateTime.TryParseExact(p.GregorianDate, possibleFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        // Standardize the parsed date and compare
                        return parsedDate.ToString("yyyy-MM-dd") == standardizedNow;
                    }
                    return false;
                })
                .FirstOrDefault();
            //if (CurrentDayCheck == null)
            //{
            //    return new ResultDto<OkrResult>
            //    {
            //        IsSuccess = false,
            //        Message = $@"Date={now.ToShortDateString()}"//اطلاعات تاریخ به درستی دریافت نشد
            //    };
            //}
            var WeekDaysInfo = monthData.Where(p => p.WeekNumber == CurrentDayCheck.WeekNumber).ToList();
            var WeekPurchousAmount = new List<OKRData>();
            List<int?> In_Person = new List<int?> { 1, 3 };
            List<int?> Marketing = new List<int?> { 4, 5, 8, 12, 9, 11 };
            List<int?> CyberSpace = new List<int?> { 2, 6, 7, 10 };
            Dictionary<string, string> MonthName_Number = new Dictionary<string, string> {
                    { "01", "فروردین" },
                    {"02", "اردیبهشت" },
                    { "03", "خرداد" },
                    { "04", "تیر" },
                    { "05", "مرداد" },
                    { "06", "شهریور" },
                    { "07", "مهر" },
                    { "08", "آبان" },
                    { "09", "آذر" },
                    { "10", "دی" },
                    { "11", "بهمن" },
                    { "12", "اسفند" }
                };
            var DataSet = _context.MainFactors
            .Where(p => p.BranchCode == BranchCode && p.CustomerID != null && !p.IsRemoved && gregorianMonths.Contains(Convert.ToInt32(p.month))) // Filter out rows where CustomerId is null
            .Join(
                    _context.CustomerInfo,
                    mf => mf.CustomerID,
                    ci => ci.Id,
                    (mf, customerGroup) => new
                    {
                        Date=mf.LastConnectionTime,
                        TotalAmount = mf.TotalAmount,
                        status = mf.status,
                        acquaintance = customerGroup.acquaintance,  // Select specific field from CustomerInfo
                    })
                .AsQueryable();







            var groupedByWeek = monthData
               .GroupBy(d => d.WeekNumber)
               .Select(g => new
               {
                   
                   WeekNumber = g.Key,
                   StartDate = DateTime.Parse(g.Min(d => d.GregorianDate)),
                   EndDate = DateTime.Parse(g.Max(d => d.GregorianDate)),
                   ShamsiStart= g.Min(d => d.JalaliDate),
                   ShamsiEnd = g.Max(d => d.JalaliDate),
               });
            var MonthlyTarget = _context.MonthlyTargets.Where(p => p.BranchCode == BranchCode && p.month == persianMonth).FirstOrDefault();
            Dictionary<int, float> Targets = new Dictionary<int, float> {
                {1,MonthlyTarget.week1 },
                {2,MonthlyTarget.week2 },
                { 3,MonthlyTarget.week3},
                {4,MonthlyTarget.week4 },
                { 5,MonthlyTarget.week5},
                { 6,MonthlyTarget.week6}
                        };

            List<object> AllWeeksCondition = new List<object>();
            foreach (var week in groupedByWeek)
            {

                // Query the second dataset based on the date range
                var ThisWeekPurchase = DataSet
                    .Where(record => record.Date >= week.StartDate && record.Date <= week.EndDate)
                    .Sum(p=>p.TotalAmount);
                //var ThisWeekDateMonth= $" {int.Parse(week.ShamsiStart.Split('/')[2])}-{int.Parse(week.ShamsiEnd.Split('/')[2])} {MonthName_Number[week.ShamsiStart.Split('/')[1]]}";
                var ThisWeekDateMonth = $"{ConvertToArabicNumbers($"{int.Parse(week.ShamsiEnd.Split('/')[2])}-{int.Parse(week.ShamsiStart.Split('/')[2])}")} {MonthName_Number[week.ShamsiStart.Split('/')[1]]}";
                var ThisWeekTarget = Targets[week.WeekNumber];
                var ThisWeekNumber = week.WeekNumber;
                var ThisWeeekColor = "#0000";
                if (ThisWeekNumber <= CurrentDayCheck.WeekNumber)
                {
                    if(ThisWeekPurchase>=0.8*ThisWeekTarget && ThisWeekPurchase < ThisWeekTarget) 
                    {
                        ThisWeeekColor = "#FF00";
                    }
                    else if(ThisWeekPurchase>= ThisWeekTarget)
                    {
                        ThisWeeekColor = "#00AF";
                    }
                    else
                    {
                        ThisWeeekColor = "#0AFF";
                    }
                }
                
                
                AllWeeksCondition.Add (new 
                {
                    purchase= ThisWeekPurchase,
                    DateMonth=ThisWeekDateMonth,
                    Target= ThisWeekTarget,
                    Number= ThisWeekNumber,
                    Color= ThisWeeekColor

                });
                
            }


            return new ResultDto<List<object>>
            {
                IsSuccess = true,
                Message = "",
                Data = AllWeeksCondition
            };
            


        }















































        public ResultDto<OkrResult> Test(int BranchCode)
        {
            DateTime now=DateTime.Now;
            PersianCalendar persianCalendar = new PersianCalendar();

            // Get the Jalali year and month
            
            var WeekDatas = GetWeeksData(persianCalendar.GetYear(now), persianCalendar.GetMonth(now), now);/// total jalali weeks in month
            var CurrentWeekDatas= WeekDatas.WeeksData[WeekDatas.CurrentWeekNumber-1];//// the current week
            //Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Number  {WeekDatas.CurrentWeekNumber}");
            //Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Number  {WeekDatas.WeeksData[3].WeekStartDate}");

            int daysCount = (CurrentWeekDatas.WeekEndDate - CurrentWeekDatas.WeekStartDate).Days+1;/// number of current week days



            var Targets = _context.MonthlyTargets.FirstOrDefault(p => p.BranchCode == BranchCode && p.year == persianCalendar.GetYear(now) && p.month == persianCalendar.GetMonth(now));
            for (int i = 0; i < WeekDatas.WeeksData.Count; i++)
            {

                var weekData = new WeekData
                {
                    WeekDateRange = WeekDatas.WeeksData[i].WeekDateRange,
                    WeekTarget = (Targets != null) ? (i == 0 ? Targets.week1 :
                                   i == 1 ? Targets.week2 :
                                   i == 2 ? Targets.week3 :
                                   i == 3 ? Targets.week4 :
                                   i == 4 ? Targets.week5 :
                                   i == 5 ? Targets.week6 : null) : null,
                    WeekEndDate = WeekDatas.WeeksData[i].WeekEndDate,
                    WeekStartDate = WeekDatas.WeeksData[i].WeekStartDate,
                };

                // Dynamically assign the week data to the Result object based on the index.
                




            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var WeekPurchousAmount = new List<OKRData>();

            List<int?> In_Person = new List<int?> { 1, 3 };
            List<int?> Marketing = new List<int?> { 4, 5, 8, 12, 9, 11 };
            List<int?> CyberSpace = new List<int?> { 2, 6, 7, 10 };

            var UnmergedWeekFactors = _context.MainFactors.Where(p =>p.BranchCode==BranchCode&& !p.IsRemoved && p.LastConnectionTime <= CurrentWeekDatas.WeekEndDate && p.LastConnectionTime >= CurrentWeekDatas.WeekStartDate).ToList();

            var MergedWeekFactors = _context.MainFactors
            .Where(p => p.BranchCode == BranchCode && p.CustomerID != null && !p.IsRemoved && p.LastConnectionTime <= CurrentWeekDatas.WeekEndDate && p.LastConnectionTime >= CurrentWeekDatas.WeekStartDate) // Filter out rows where CustomerId is null
            .Join(
                    _context.CustomerInfo,
                    mf => mf.CustomerID,
                    ci => ci.Id,
                    (mf, customerGroup) => new
                    {
                        factorid=mf.Id,
                        Date = mf.LastConnectionTime,
                        Day = mf.day,
                        Month = mf.month,
                        Year = mf.year,
                        TotalAmount = mf.TotalAmount,
                        status = mf.status,
                        acquaintance = customerGroup.acquaintance,  // Select specific field from CustomerInfo
                    })
                .ToList();


            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var InitialWeekPurchousAmount = new List<InitialOKRData>();

            var day = CurrentWeekDatas.WeekStartDate;// تاریخ نخستین روز هفته
            
            
            for (int i = 0; i < daysCount; i++)
            {

                int dayNumber = ((int)day.DayOfWeek + 1) % 7;/// شماره روز هفته مثلا شنبه برابر با ۰
                //Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>   {day}");
                InitialWeekPurchousAmount.Add(new InitialOKRData
                {

                    DayNumber = dayNumber,
                    TrueTotalAmount = MergedWeekFactors.Where(p => p.status && p.Day == day.Day.ToString() && p.Month == day.Month.ToString() && p.Year == day.Year.ToString()).Sum(p => p.TotalAmount),
                    FalseTotalAmount = UnmergedWeekFactors.Where(p => !p.status && p.day == day.Day.ToString() && p.month == day.Month.ToString() && p.year == day.Year.ToString()).Sum(p => p.TotalAmount),
                    TrueCyberSpaceAmount = MergedWeekFactors.Where(p => p.status && p.Day == day.Day.ToString() && p.Month == day.Month.ToString() && p.Year == day.Year.ToString() && CyberSpace.Contains(p.acquaintance)).Sum(p => p.TotalAmount),
                    FalseCyberSpaceAmount = MergedWeekFactors.Where(p => !p.status && p.Day == day.Day.ToString() && p.Month == day.Month.ToString() && p.Year == day.Year.ToString() && CyberSpace.Contains(p.acquaintance)).Sum(p => p.TotalAmount),
                    TrueMarketingAmount = MergedWeekFactors.Where(p => p.status && p.Day == day.Day.ToString() && p.Month == day.Month.ToString() && p.Year == day.Year.ToString() && Marketing.Contains(p.acquaintance)).Sum(p => p.TotalAmount),
                    FalseMarketingAmount = MergedWeekFactors.Where(p => !p.status && p.Day == day.Day.ToString() && p.Month == day.Month.ToString() && p.Year == day.Year.ToString() && Marketing.Contains(p.acquaintance)).Sum(p => p.TotalAmount),
                    TrueInPersonAmount = MergedWeekFactors.Where(p => p.status && p.Day == day.Day.ToString() && p.Month == day.Month.ToString() && p.Year == day.Year.ToString() && In_Person.Contains(p.acquaintance)).Sum(p => p.TotalAmount),
                    FalseInPersonAmount = MergedWeekFactors.Where(p => !p.status && p.Day == day.Day.ToString() && p.Month == day.Month.ToString() && p.Year == day.Year.ToString() && In_Person.Contains(p.acquaintance)).Sum(p => p.TotalAmount),


                });
                day = day.AddDays(1);

            };

            var SumOfWeeks = new InitialOKRData
            {
                DayNumber = 7,
                TrueTotalAmount = InitialWeekPurchousAmount.Sum(p => p.TrueTotalAmount),
                FalseTotalAmount = InitialWeekPurchousAmount.Sum(p => p.FalseTotalAmount),
                TrueCyberSpaceAmount = InitialWeekPurchousAmount.Sum(p => p.TrueCyberSpaceAmount),
                FalseCyberSpaceAmount = InitialWeekPurchousAmount.Sum(p => p.FalseCyberSpaceAmount),
                TrueMarketingAmount = InitialWeekPurchousAmount.Sum(p => p.TrueMarketingAmount),
                FalseMarketingAmount = InitialWeekPurchousAmount.Sum(p => p.FalseMarketingAmount),
                TrueInPersonAmount = InitialWeekPurchousAmount.Sum(p => p.TrueInPersonAmount),
                FalseInPersonAmount = InitialWeekPurchousAmount.Sum(p => p.FalseInPersonAmount),
            };
            InitialWeekPurchousAmount.Add(SumOfWeeks);

            WeekPurchousAmount = InitialWeekPurchousAmount.Select(p => new OKRData
            {
                DayNumber = p.DayNumber,
                TrueTotalAmount = p.TrueTotalAmount != 0 ? p.TrueTotalAmount.ToString() : "",
                FalseTotalAmount = p.FalseTotalAmount != 0 ? p.FalseTotalAmount.ToString() : "",
                TrueCyberSpaceAmount = p.TrueCyberSpaceAmount != 0 ? p.TrueCyberSpaceAmount.ToString() : "",
                FalseCyberSpaceAmount = p.FalseCyberSpaceAmount != 0 ? p.FalseCyberSpaceAmount.ToString() : "",
                TrueMarketingAmount = p.TrueMarketingAmount != 0 ? p.TrueMarketingAmount.ToString() : "",
                FalseMarketingAmount = p.FalseMarketingAmount != 0 ? p.FalseMarketingAmount.ToString() : "",
                TrueInPersonAmount = p.TrueInPersonAmount != 0 ? p.TrueInPersonAmount.ToString() : "",
                FalseInPersonAmount = p.FalseInPersonAmount != 0 ? p.FalseInPersonAmount.ToString() : "",

            }).ToList();



            Dictionary<int, float> TargetsbyWeek = new Dictionary<int, float> {
                {1,Targets.week1 },
                {2,Targets.week2 },
                { 3,Targets.week3},
                {4,Targets.week4 },
                { 5,Targets.week5},
                { 6,Targets.week6}
                        };

            List<object> AllWeeksCondition = new List<object>();
            int TargetNumber = 1;
           
            foreach (var week in WeekDatas.WeeksData)
            {
                // Query the second dataset based on the date range
                var ThisWeekPurchase = _context.MainFactors
                    .Where(p =>p.BranchCode==BranchCode&& p.LastConnectionTime <= week.WeekEndDate&& p.LastConnectionTime >= week.WeekStartDate && p.status&& !p.IsRemoved)
                    .Sum(t => t.TotalAmount);

                var ThisWeekDateMonth =  week.WeekDateRange;
                var ThisWeekTarget = TargetsbyWeek[TargetNumber];
                var ThisWeekNumber = TargetNumber;
                var ThisWeeekColor = Environment.GetEnvironmentVariable("TARGET_NOT_ARRIVED");
                if (ThisWeekNumber <= WeekDatas.CurrentWeekNumber)
                {
                    if (ThisWeekPurchase >= 0.8 * ThisWeekTarget && ThisWeekPurchase < ThisWeekTarget)
                    {
                        ThisWeeekColor = Environment.GetEnvironmentVariable("NEAR_TARGET");
                    }
                    else if (ThisWeekPurchase >= ThisWeekTarget)
                    {
                        ThisWeeekColor = Environment.GetEnvironmentVariable("PASS_TARGET");
                    }
                    else
                    {
                        ThisWeeekColor = Environment.GetEnvironmentVariable("FAIL_TARGET");
                    }
                }


                AllWeeksCondition.Add(new
                {
                    purchase = ThisWeekPurchase,
                    DateMonth = ThisWeekDateMonth,
                    Target = ThisWeekTarget,
                    Number = ThisWeekNumber,
                    Color = ThisWeeekColor

                });
                TargetNumber++;

            }



            var WeeklyTarget = TargetsbyWeek[WeekDatas.CurrentWeekNumber];
            var WeeklyTrueTotalAmount = WeekPurchousAmount.Sum(p => string.IsNullOrEmpty(p.TrueTotalAmount)
            ? 0f
            : float.Parse(p.TrueTotalAmount, CultureInfo.InvariantCulture));


            float targetPercentage = 0;
            if (WeeklyTrueTotalAmount != 0)
            {
                targetPercentage = WeeklyTrueTotalAmount / WeeklyTarget / 2 * 100;
            }
            else
            {
                targetPercentage = 0; // Or set to a default value
            }
            string targetColor = "#e14202";
            if (targetPercentage < 30) { targetColor = Environment.GetEnvironmentVariable("OKR_LOWER30"); }
            else if (targetPercentage >= 30 && targetPercentage < 60) { targetColor = Environment.GetEnvironmentVariable("OKR_30_60"); }
            else if (targetPercentage >= 60 && targetPercentage < 100) { targetColor = Environment.GetEnvironmentVariable("OKR_60_100"); }
            else if (targetPercentage >= 100) { targetColor = Environment.GetEnvironmentVariable("OKR_Higher100"); }
            var OkrResult = new OkrResult
            {
                TargetPercentage = (int)targetPercentage,
                WeeklyTarget = WeeklyTarget,
                TargetColor = targetColor,
                oKRDatas = WeekPurchousAmount,
                AllWeeksCondition = AllWeeksCondition,

            };
            return new ResultDto<OkrResult>
            {
                Data = OkrResult,
                IsSuccess = true,
                Message = $@"Date={now.ToShortDateString()}"// WeeklyTrueTotalAmount.ToString()
            };


        }

        private static DateTime ConvertToDateTime(string year, string month,string day)
        {
            DateTime gregorianDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
            return gregorianDate;
        }

        private static string ConvertToWeekDateRange(DateTime startDate, DateTime endDate)
        {
            var JalaliStart = ConvertToJalali(startDate);
            var JalaliEnd= ConvertToJalali(endDate);
            var jalalimonth = monthofjalali(startDate);
            string DateRange= $"{ConvertToArabicNumbers($"{int.Parse(JalaliEnd.Split('/')[2])}-{int.Parse(JalaliStart.Split('/')[2])}")} {GetMonthName(jalalimonth)}";
            return DateRange;
        }
        
        private static string ConvertToJalali(DateTime GregorianDate)
        {
            PersianCalendar persianCalendar = new PersianCalendar();

            // Convert to Jalali (Persian) year, month, and day
            int persianYear = persianCalendar.GetYear(GregorianDate);
            int persianMonth = persianCalendar.GetMonth(GregorianDate);
            int persianDay = persianCalendar.GetDayOfMonth(GregorianDate);
            var shamsi = $"{persianYear}/{persianMonth:D2}/{persianDay:D2}";
            return shamsi;
        }
        private static int monthofjalali(DateTime GregorianDate)
        {
            PersianCalendar persianCalendar = new PersianCalendar();

            // Convert to Jalali (Persian) year, month, and day
            int persianYear = persianCalendar.GetYear(GregorianDate);
            int persianMonth = persianCalendar.GetMonth(GregorianDate);
            int persianDay = persianCalendar.GetDayOfMonth(GregorianDate);
            var shamsi = $"{persianYear}/{persianMonth:D2}/{persianDay:D2}";
            return persianMonth;
        }


        private static (int CurrentWeekNumber, List<Week> WeeksData) GetWeeksData(int year, int month, DateTime currentDate)
        {
            var weeks = new List<Week>();
            PersianCalendar persianCalendar = new PersianCalendar();

            // Get the first day of the month
            DateTime firstDayOfMonth = persianCalendar.ToDateTime(year, month, 1, 0, 0, 0, 0);

            // Get the number of days in the month
            int daysInMonth = persianCalendar.GetDaysInMonth(year, month);

            // Determine the day of the week for the first day (adjust for Jalali week starting on Saturday)
            int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            startDayOfWeek = (startDayOfWeek + 1) % 7; // Shift to make Saturday = 0 in Jalali

            // Start iterating from the first day of the month
            int currentDay = 1;

            int currentWeekNumber = 0; // Initialize current week number

            // Handle the first week separately
            int firstWeekEndDay = Math.Min(7 - startDayOfWeek, daysInMonth);
            DateTime firstWeekStartGregorian = persianCalendar.ToDateTime(year, month, currentDay, 0, 0, 0, 0);
            DateTime firstWeekEndGregorian = persianCalendar.ToDateTime(year, month, firstWeekEndDay, 0, 0, 0, 0);
            firstWeekEndGregorian = firstWeekEndGregorian.AddHours(23);
            firstWeekEndGregorian = firstWeekEndGregorian.AddMinutes(59);
            firstWeekEndGregorian = firstWeekEndGregorian.AddMilliseconds(59000);
            weeks.Add(new Week
            {
                WeekDateRange = ConvertToWeekDateRange(firstWeekStartGregorian, firstWeekEndGregorian),
                WeekStartDate = firstWeekStartGregorian,
                WeekEndDate = firstWeekEndGregorian,
            });

            // Check if the current date falls in the first week
            if (currentDate >= firstWeekStartGregorian && currentDate <= firstWeekEndGregorian)
            {
                currentWeekNumber = 1; // First week
            }

            currentDay = firstWeekEndDay + 1; // Start the next week

            int weekNumber = 2; // Starting week number for the remaining weeks

            // Handle the remaining weeks
            while (currentDay <= daysInMonth)
            {
                int startDay = currentDay;
                int endDay = Math.Min(currentDay + 6, daysInMonth);
                DateTime weekStartGregorian = persianCalendar.ToDateTime(year, month, startDay, 0, 0, 0, 0);
                DateTime weekEndGregorian = persianCalendar.ToDateTime(year, month, endDay, 0, 0, 0, 0);
                weekEndGregorian = weekEndGregorian.AddHours(23);
                weekEndGregorian = weekEndGregorian.AddMinutes(59);
                weekEndGregorian = weekEndGregorian.AddMilliseconds(59000);
                weeks.Add(new Week
                {
                    WeekDateRange = ConvertToWeekDateRange(weekStartGregorian, weekEndGregorian), 
                    WeekStartDate = weekStartGregorian,
                    WeekEndDate = weekEndGregorian,
                }) ;
                
                // Check if the current date falls within this week
                if (currentDate >= weekStartGregorian && currentDate <= weekEndGregorian)
                {

                    currentWeekNumber = weekNumber;
                }

                weekNumber++;
                currentDay = endDay + 1;
            }

            return (currentWeekNumber, weeks);
        }
        private static string ConvertToPersianNumber(int number)
        {
            string persianNumbers = "٠١٢٣٤٥٦٧٨٩";
            char[] persianDigits = number.ToString().ToCharArray();
            for (int i = 0; i < persianDigits.Length; i++)
            {
                if (char.IsDigit(persianDigits[i]))
                {
                    persianDigits[i] = persianNumbers[persianDigits[i] - '0'];
                }
            }
            return new string(persianDigits);
        }

        private static string GetMonthName(int month)
        {
            string[] jalaliMonthNames = {
            "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
            "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
        };
            return jalaliMonthNames[month - 1];
        }
        static string ConvertToArabicNumbers(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return input
                .Replace('0', '٠')
                .Replace('1', '١')
                .Replace('2', '٢')
                .Replace('3', '٣')
                .Replace('4', '٤')
                .Replace('5', '٥')
                .Replace('6', '٦')
                .Replace('7', '٧')
                .Replace('8', '٨')
                .Replace('9', '٩');
        }
    }
    
    public class Week
    {

        public string WeekDateRange { get; set; }
        public float? WeekTarget { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public float? AcheivedTarget { get; set; }
    }

    public class MonthData
    {
        public string JalaliDate { get; set; }
        public string Week { get; set; }
        public string DayOfWeek { get; set; }
        public string GregorianDate { get; set; }
        public int DayofWeekNumber { get; set; }
        public int WeekNumber { get; set; }
        public string MonthDay {  get; set; }
        public string MonthNumber {  get; set; }
        public string YearNumber { get; set; }     
    }

    public class InitialOKRData
    {
        public int DayNumber { get; set; }
        public float? TrueTotalAmount { get; set; } = 0;
        public float? FalseTotalAmount { get; set; } = 0;
        public float? TrueCyberSpaceAmount { get; set; } = 0;
        public float? FalseCyberSpaceAmount { get; set; } = 0;
        public float? TrueMarketingAmount { get; set; } = 0;
        public float? FalseMarketingAmount { get; set; } = 0;
        public float? TrueInPersonAmount { get; set; } = 0;
        public float? FalseInPersonAmount { get; set; } = 0;


    }

    public class OKRData
    {
        public int DayNumber { get; set; }
        public string TrueTotalAmount { get; set; }
        public string FalseTotalAmount { get; set; }
        public string TrueCyberSpaceAmount { get; set; }
        public string FalseCyberSpaceAmount { get; set; }
        public string TrueMarketingAmount { get; set; }
        public string FalseMarketingAmount { get; set; }
        public string TrueInPersonAmount { get; set; }
        public string FalseInPersonAmount { get; set; }


    }
    public class OkrResult
    {
        public int TargetPercentage { get; set;}
        public float WeeklyTarget { get; set; }  
        public string TargetColor { get; set; }
        public List<OKRData> oKRDatas { get; set; }
        public List<object> AllWeeksCondition { get; set; }
    }
}