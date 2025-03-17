using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.OKR.Queries.TargetDeterminationGet;
using Radin.Common.Dto;
using Radin.Domain.Entities.OKR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.OKR.Commands.TargetDeterminationSet
{
    public interface ITargetDeterminationSetService
    {
        ResultDto<long> SetMonthlyTarget(MonthlyTargetRequestSet request);
    }


    public class TargetDeterminationSetService : ITargetDeterminationSetService
    {
        private readonly IDataBaseContext _context;

        public TargetDeterminationSetService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<long> SetMonthlyTarget(MonthlyTargetRequestSet request)
        {

            try
            {
                var previousTarget = _context.MonthlyTargets.FirstOrDefault(p => p.year == request.year && p.month == request.month && p.BranchCode == request.branchCode);
                if (previousTarget != null)
                {
                    previousTarget.year = request.year;
                    previousTarget.month = request.month;
                    previousTarget.BranchCode = request.branchCode;
                    previousTarget.week1 = request.week1;
                    previousTarget.week2 = request.week2;
                    previousTarget.week3 = request.week3;
                    previousTarget.week4 = request.week4;
                    if (request.week5 != null) { previousTarget.week5 = request.week5 ?? 0; };
                    if (request.week6 != null) { previousTarget.week6 = request.week6 ?? 0; };
                    previousTarget.Sum = previousTarget.week1 + previousTarget.week2 + previousTarget.week3 + previousTarget.week4 + previousTarget.week5 + previousTarget.week6;


                    List<float?> floatValues = previousTarget.GetType()
                               .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                               .Where(p => p.PropertyType == typeof(float) || p.PropertyType == typeof(float?))
                               .Select(p => (float?)p.GetValue(previousTarget))
                               .ToList().Where(p => p.HasValue && p.Value != 0)
                                .Distinct()                               // Remove duplicates
                                .OrderByDescending(p => p)

                               .ToList();// Ensure p has value and it's not 0
                    //previousTarget.DailyMin = floatValues.Last() / 4 ?? 0;
                    //previousTarget.DailyMax = floatValues[1] / 7 ?? 0;
                    //previousTarget.DailyMid = previousTarget.Sum / 30;

                    previousTarget.DailyMid = request.dailyMid;
                    previousTarget.DailyMin = request.dailyMin;
                    previousTarget.DailyMax = request.dailyMax;



                    _context.MonthlyTargets.Update(previousTarget);
                    _context.SaveChanges();
                    return new ResultDto<long>
                    {
                        Data = previousTarget.BranchCode,
                        IsSuccess = true,
                        Message = $"تارگت {previousTarget.month}-{previousTarget.year} ثبت شد"
                    };

                }
                else
                {
                    var newTarget = new MonthlyTarget
                    {
                        year = request.year,
                        month = request.month,
                        BranchCode = request.branchCode,
                        week1 = request.week1,
                        week2 = request.week2,
                        week3 = request.week3,
                        week4 = request.week4,
                    };
                    if (request.week5 != null) { newTarget.week5 = request.week5 ?? 0; };
                    if (request.week6 != null) { newTarget.week6 = request.week6 ?? 0; };
                    newTarget.Sum=newTarget.week1 + newTarget.week2 + newTarget.week3 + newTarget.week4 + newTarget.week5 + newTarget.week6;

                    List<float?> floatValues = newTarget.GetType()
                               .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                               .Where(p => p.PropertyType == typeof(float) || p.PropertyType == typeof(float?))
                               .Select(p => (float?)p.GetValue(newTarget))
                               .ToList().Where(p => p.HasValue && p.Value != 0)
                                .Distinct()                               // Remove duplicates
                                .OrderByDescending(p => p)


                               .ToList();// Ensure p has value and it's not 0
                    //newTarget.DailyMin = floatValues.Last()/4 ?? 0;
                    //newTarget.DailyMax = floatValues[1] / 7 ?? 0;
                    //newTarget.DailyMid = newTarget.Sum / 30;

                    newTarget.DailyMid = request.dailyMid;
                    newTarget.DailyMin = request.dailyMin;
                    newTarget.DailyMax = request.dailyMax;



                    _context.MonthlyTargets.Add(newTarget);
                   
                    
                    _context.SaveChanges();
                    return new ResultDto<long>
                    {
                        Data = newTarget.BranchCode,
                        IsSuccess = true,
                        Message = $" تارگت  {newTarget.month}-{newTarget.year} ثبت شد"
                    };

                };
            }
            catch  {
                return new ResultDto<long>
                {
                    IsSuccess = false,
                    Message = "خطای دریافت"

                };
            }
            




        }
    }


    public class MonthlyTargetRequestSet
    {
        public int year {  get; set; }
        public int month { get; set; }
        public long branchCode { get; set; } = 0;
        public float week1 { get; set; }
        public float week2 { get; set; }
        public float week3 { get; set; }
        public float week4 { get; set; }
        public float? week5 { get; set; }
        public float? week6 { get; set; }
        public float dailyMax { get; set; } = 0;
        public float dailyMin { get; set; } = 0;
        public float dailyMid{ get; set;} = 0;

    }
}
