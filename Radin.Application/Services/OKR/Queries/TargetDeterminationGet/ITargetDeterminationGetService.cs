using Radin.Application.Interfaces.Contexts;
using Radin.Common.Dto;
using Radin.Domain.Entities.Factors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.OKR.Queries.TargetDeterminationGet
{
    public interface ITargetDeterminationGetService
    {
        ResultDto<Year_Month_Result> Year_Month_Get();
        ResultDto<List<BranchResult>> Branch_List(int year,int month);
        ResultDto<TargetsOfWeeks> BranchTargets(int year, int month, long branchCode);
        ResultDto<List<TargetsOfWeeksHistory>> BranchTargetsHistory(long branchCode);

    }



    public class TargetDeterminationGetService : ITargetDeterminationGetService
    {
        private readonly IDataBaseContext _context;

        public TargetDeterminationGetService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<Year_Month_Result> Year_Month_Get()
        {

            try
            {
                PersianCalendar persianCalendar = new PersianCalendar();

                // Get the current Gregorian date
                DateTime currentDate = DateTime.Now;
                //currentDate= currentDate.AddMonths(2);
                // Extract the Jalali year and month
                int currentYear = persianCalendar.GetYear(currentDate);
                int currentMonth = persianCalendar.GetMonth(currentDate);

                // Initialize the list of months
                var Data = new List<Year_Month>();

                // Initialize a temporary list for months that belong to the same year
                var temp = new List<IdLabelDto>();

                for (int i = 0; i < 3; i++)
                {
                    int month = currentMonth + i;
                    int year = currentYear;

                    // Handle wrapping after Esfand (month 12)
                    if (month > 12)
                    {
                        month -= 12;
                        year += 1;
                    }


                    Data.Add(new Year_Month { year = year, month = month ,Text=$"{year} {GetMonthName(month)}" });
                }

               

                var Result = new Year_Month_Result

                {
                    Year_Months = Data
                };

                return new ResultDto<Year_Month_Result>
                {
                    Data = Result,
                    IsSuccess = true,
                    Message="دریافت موفق"

                };
            }
            catch 
            {
                return new ResultDto<Year_Month_Result>
                {
                    IsSuccess = false,
                    Message = "خطای دریافت"

                };

            }


        }


        public ResultDto<List<BranchResult>> Branch_List(int year, int month)
        {

            try
            {
                var DeterminedMonth = _context.MonthlyTargets.Where(p => p.year == year && p.month == month).Select(p => p.BranchCode).ToList().Distinct()
                            .ToHashSet();

               
                var BranchList=_context.BranchINFOs.Select(p => new BranchResult
                {

                    Id=p.BranchCode,
                    BranchName = p.BranchName,
                    Determination = DeterminedMonth.Contains(p.BranchCode)

                }).ToList();



                return new ResultDto<List<BranchResult>>
                {
                    Data= BranchList,
                    IsSuccess = true,
                    Message = "دریافت موفق"

                };
            }
            catch
            {
                return new ResultDto<List<BranchResult>>
                {
                    IsSuccess = false,
                    Message = "خطای دریافت"

                };

            }


        }


        public ResultDto<TargetsOfWeeks> BranchTargets(int year, int month,long branchCode)
        {

            try
            {

                var weeks = WeeksDatas(year, month);

                var Result = new TargetsOfWeeks();
                var Targets = _context.MonthlyTargets.FirstOrDefault(p => p.BranchCode == branchCode && p.year == year && p.month == month);
                for (int i = 0; i < weeks.Count; i++)
                {
                    
                    var weekData = new WeekData
                    {
                        WeekDateRange = weeks[i].WeekDateRange,
                        WeekTarget = (Targets != null) ? (i == 0 ? Targets.week1 :
                                       i == 1 ? Targets.week2 :
                                       i == 2 ? Targets.week3 :
                                       i == 3 ? Targets.week4 :
                                       i == 4 ? Targets.week5 :
                                       i == 5 ? Targets.week6 : null) : null,
                        WeekEndDate = weeks[i].WeekEndDate,
                        WeekStartDate = weeks[i].WeekStartDate,
                    };

                    // Dynamically assign the week data to the Result object based on the index.
                    switch (i)
                    {
                        case 0: Result.Week1 = weekData; break;
                        case 1: Result.Week2 = weekData; break;
                        case 2: Result.Week3 = weekData; break;
                        case 3: Result.Week4 = weekData; break;
                        case 4: Result.Week5 = weekData; break;
                        case 5: Result.Week6 = weekData; break;
                    }



                    
                }
                Result.dailyMid = Targets!=null?Targets.DailyMid:0;
                Result.dailyMin = Targets != null ? Targets.DailyMin:0;
                Result.dailyMax = Targets != null ? Targets.DailyMax:0;

                return new ResultDto<TargetsOfWeeks>
                {
                    Data = Result,
                    IsSuccess = true,
                    Message = "دریافت موفق"

                };
            }
            catch
            {
                return new ResultDto<TargetsOfWeeks>
                {
                    IsSuccess = false,
                    Message = "خطای دریافت"

                };

            }


        }


        public ResultDto<List<TargetsOfWeeksHistory>> BranchTargetsHistory(long branchCode)
        {




            try
            {
                // Step 1: Retrieve monthly targets and map to TargetsOfWeeksHistory
                var TargetsList = _context.MonthlyTargets
                    .Where(p => p.BranchCode == branchCode)
                    .Select(p => new TargetsOfWeeksHistory
                    {
                        year = p.year,
                        month = p.month,
                        Week1 = new WeekData { WeekTarget = p.week1 },
                        Week2 = new WeekData { WeekTarget = p.week2 },
                        Week3 = new WeekData { WeekTarget = p.week3 },
                        Week4 = new WeekData { WeekTarget = p.week4 },
                        Week5 = p.week5 != 0 ? new WeekData { WeekTarget = p.week5 } : null,
                        Week6 = p.week6 != 0 ? new WeekData { WeekTarget = p.week6 } : null,
                    })
                    .ToList();

                // Step 2: Pre-fetch all the main factors for the given branchCode in one query
                var mainFactors = _context.MainFactors
                    .Where(p => p.BranchCode == branchCode && p.status == true&&!p.IsRemoved)
                    .ToList();

                // Step 3: Process each target list entry and populate weeks data
                foreach (var target in TargetsList)
                {
                    var weeks = WeeksDatas(target.year, target.month);

                    // Process each week
                    ProcessWeek(target, weeks, 0, mainFactors);
                    ProcessWeek(target, weeks, 1, mainFactors);
                    ProcessWeek(target, weeks, 2, mainFactors);
                    ProcessWeek(target, weeks, 3, mainFactors);

                    if (target.Week5 != null)
                    {
                        ProcessWeek(target, weeks, 4, mainFactors);
                    }

                    if (target.Week6 != null)
                    {
                        ProcessWeek(target, weeks, 5, mainFactors);
                    }
                }

                return new ResultDto<List<TargetsOfWeeksHistory>> { Data = TargetsList ,
                    IsSuccess = true,
                    Message = "دریافت موفق"
                };
            }
            catch
            {
                return new ResultDto<List<TargetsOfWeeksHistory>>
                {
                    IsSuccess = false,
                    Message = "خطای دریافت"

                };

            }










            //try
            //{
            //    var TargetsList = _context.MonthlyTargets.Where(p => p.BranchCode == branchCode).Select(p => new TargetsOfWeeksHistory
            //    {

            //        year=p.year, month=p.month,
            //        Week1 = new WeekData { WeekTarget = p.week1 },
            //        Week2 = new WeekData { WeekTarget = p.week1 },
            //        Week3 = new WeekData { WeekTarget = p.week1 },
            //        Week4 = new WeekData { WeekTarget = p.week1 },
            //        Week5 = p.week5 != 0 ? new WeekData { WeekTarget = p.week5 } : null,
            //        Week6 = p.week6 != 0 ? new WeekData { WeekTarget = p.week6 } : null,

            //    }).ToList();

            //for (int i = 0; i < TargetsList.Count; i++)
            //{

            //    var year = TargetsList[i].year; var month = TargetsList[i].month;
            //    var weeks = WeeksDatas(year, month);

            //    TargetsList[i].Week1.WeekDateRange = weeks[0].WeekDateRange;
            //    TargetsList[i].Week1.WeekStartDate = weeks[0].WeekStartDate;
            //    TargetsList[i].Week1.WeekEndDate = weeks[0].WeekEndDate;
            //    var week1Acheived = _context.MainFactors.Where(p => p.BranchCode == branchCode && p.status == true && p.InitialConnectionTime >= weeks[0].WeekStartDate && p.InitialConnectionTime <= weeks[0].WeekEndDate)
            //            .Sum(p => p.TotalAmount);
            //    TargetsList[i].Week1.AcheivedTarget = week1Acheived;



            //    TargetsList[i].Week2.WeekDateRange = weeks[1].WeekDateRange;
            //    TargetsList[i].Week2.WeekStartDate = weeks[1].WeekStartDate;
            //    TargetsList[i].Week2.WeekEndDate = weeks[1].WeekEndDate;
            //    var week2Acheived = _context.MainFactors.Where(p => p.BranchCode == branchCode && p.status == true && p.InitialConnectionTime >= weeks[1].WeekStartDate && p.InitialConnectionTime <= weeks[1].WeekEndDate)
            //        .Sum(p => p.TotalAmount);
            //    TargetsList[i].Week2.AcheivedTarget = week2Acheived;

            //    TargetsList[i].Week3.WeekDateRange = weeks[2].WeekDateRange;
            //    TargetsList[i].Week3.WeekStartDate = weeks[2].WeekStartDate;
            //    TargetsList[i].Week3.WeekEndDate = weeks[2].WeekEndDate;
            //    var week3Acheived = _context.MainFactors.Where(p => p.BranchCode == branchCode && p.status == true && p.InitialConnectionTime >= weeks[2].WeekStartDate && p.InitialConnectionTime <= weeks[2].WeekEndDate)
            //            .Sum(p => p.TotalAmount);
            //    TargetsList[i].Week3.AcheivedTarget = week3Acheived;


            //    TargetsList[i].Week4.WeekDateRange = weeks[3].WeekDateRange;
            //    TargetsList[i].Week4.WeekStartDate = weeks[3].WeekStartDate;
            //    TargetsList[i].Week4.WeekEndDate = weeks[3].WeekEndDate;
            //    var week4Acheived = _context.MainFactors.Where(p => p.BranchCode == branchCode && p.status == true && p.InitialConnectionTime >= weeks[3].WeekStartDate && p.InitialConnectionTime <= weeks[3].WeekEndDate)
            //        .Sum(p => p.TotalAmount);
            //    TargetsList[i].Week4.AcheivedTarget = week4Acheived;


            //    if (TargetsList[i].Week5 != null)
            //    {

            //        TargetsList[i].Week5.WeekDateRange = weeks[4].WeekDateRange;
            //        TargetsList[i].Week5.WeekStartDate = weeks[4].WeekStartDate;
            //        TargetsList[i].Week5.WeekEndDate = weeks[4].WeekEndDate;
            //        var week5Acheived = _context.MainFactors.Where(p => p.BranchCode == branchCode && p.status == true && p.InitialConnectionTime >= weeks[4].WeekStartDate && p.InitialConnectionTime <= weeks[4].WeekEndDate)
            //            .Sum(p => p.TotalAmount);
            //        TargetsList[i].Week5.AcheivedTarget = week5Acheived;
            //        }
            //    if (TargetsList[i].Week6 != null)
            //    {

            //        TargetsList[i].Week6.WeekDateRange = weeks[5].WeekDateRange;
            //        TargetsList[i].Week6.WeekStartDate = weeks[5].WeekStartDate;
            //        TargetsList[i].Week6.WeekEndDate = weeks[5].WeekEndDate;
            //        var week6Acheived = _context.MainFactors.Where(p => p.BranchCode == branchCode && p.status == true && p.InitialConnectionTime >= weeks[5].WeekStartDate && p.InitialConnectionTime <= weeks[5].WeekEndDate)
            //            .Sum(p => p.TotalAmount);
            //        TargetsList[i].Week6.AcheivedTarget = week6Acheived;
            //        }
            //};


            //return new ResultDto<List<TargetsOfWeeksHistory>>
            //    {
            //        Data = TargetsList,
            //        IsSuccess = true,
            //        Message = "دریافت موفق"

            //    };
            //}
            //catch
            //{
            //    return new ResultDto<List<TargetsOfWeeksHistory>>
            //    {
            //        IsSuccess = false,
            //        Message = "خطای دریافت"

            //    };

            //}

        }
















        private void ProcessWeek(TargetsOfWeeksHistory target, List<WeekData> weeks, int weekIndex, List<MainFactor> mainFactors)
        {
            if (weeks.Count > weekIndex)
            {
                var week = weeks[weekIndex];
                var weekData = GetWeekData(target, weekIndex, week);

                // Set the week data
                weekData.WeekDateRange = week.WeekDateRange;
                weekData.WeekStartDate = week.WeekStartDate;
                weekData.WeekEndDate = week.WeekEndDate;

                // Calculate the achieved target
                var achievedTarget = mainFactors
                    .Where(p => p.InitialConnectionTime >= week.WeekStartDate && p.InitialConnectionTime <= week.WeekEndDate)
                    .Sum(p => p.TotalAmount);

                weekData.AcheivedTarget = achievedTarget;
            }
        }

        private WeekData GetWeekData(TargetsOfWeeksHistory target, int weekIndex, WeekData week)
        {
            switch (weekIndex)
            {
                case 0: return target.Week1;
                case 1: return target.Week2;
                case 2: return target.Week3;
                case 3: return target.Week4;
                case 4: return target.Week5;
                case 5: return target.Week6;
                default: return null;
            }
        }






        private static List<WeekData> WeeksDatas(int year, int month)
        {

            var weeks = new List<WeekData>();
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

            // Handle the first week separately
            int firstWeekEndDay = Math.Min(7 - startDayOfWeek, daysInMonth);
            DateTime firstWeekStartGregorian = persianCalendar.ToDateTime(year, month, currentDay, 0, 0, 0, 0);
            DateTime firstWeekEndGregorian = persianCalendar.ToDateTime(year, month, firstWeekEndDay, 0, 0, 0, 0);
            //Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> StartDAy= {firstWeekStartGregorian}");
            //Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> StartDAy= {firstWeekEndGregorian}");
            weeks.Add(new WeekData
            {
                WeekDateRange = $"{ConvertToPersianNumber(currentDay)}-{ConvertToPersianNumber(firstWeekEndDay)} {GetMonthName(month)}",
                WeekStartDate = firstWeekStartGregorian,
                WeekEndDate = firstWeekEndGregorian,
                
            });

            currentDay = firstWeekEndDay + 1; // Start the next week

            // Handle the remaining weeks
            while (currentDay <= daysInMonth)
            {
                int startDay = currentDay;
                int endDay = Math.Min(currentDay + 6, daysInMonth);
                DateTime weekStartGregorian = persianCalendar.ToDateTime(year, month, startDay, 0, 0, 0, 0);
                DateTime weekEndGregorian = persianCalendar.ToDateTime(year, month, endDay, 0, 0, 0, 0);

                weeks.Add(new WeekData
                {
                    WeekDateRange = $"{ConvertToPersianNumber(startDay)}-{ConvertToPersianNumber(endDay)} {GetMonthName(month)}",
                    WeekStartDate = weekStartGregorian,
                    WeekEndDate = weekEndGregorian,
                });
                //Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> StartDAy= {weekStartGregorian}");
                //Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> StartDAy= {weekEndGregorian}");
                currentDay = endDay + 1;
            }

            return weeks;


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

    }

    public class Year_Month_Result
    {
        
        public List<Year_Month> Year_Months { get; set; }

    }


    public class Year_Month 
    {
        public int year { get; set; }
        public int month { get; set; }
        public string Text {  get; set; }
    
    
    }


    public class BranchResult
    {

        public  long Id { get; set; }    
        public  string BranchName { get; set; }
        public  bool Determination { get; set; }
    }

    public class TargetsOfWeeks
    {

        public WeekData? Week1 { get; set; }
        public WeekData? Week2 { get; set; }
        public WeekData? Week3 { get; set; }
        public WeekData? Week4 { get; set;}
        public WeekData? Week5 { get; set;}
        public WeekData? Week6 { get; set;}
        public float dailyMid { get; set; } = 0;
        public float dailyMin { get; set; } = 0;
        public float dailyMax { get; set; } = 0;


    }

    public class TargetsOfWeeksHistory
    {
        public int year { get; set; }
        public int month { get; set; }
        public WeekData? Week1 { get; set; }
        public WeekData? Week2 { get; set; }
        public WeekData? Week3 { get; set; }
        public WeekData? Week4 { get; set; }
        public WeekData? Week5 { get; set; }
        public WeekData? Week6 { get; set; }

    }

    public class WeekData
    {

        public string WeekDateRange { get; set; }
        public float? WeekTarget { get; set; }
        public DateTime? WeekStartDate { get; set; }
        public DateTime? WeekEndDate { get; set; }
        public float? AcheivedTarget { get; set; }
    }
   
}

