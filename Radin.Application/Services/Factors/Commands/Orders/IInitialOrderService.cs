using Microsoft.AspNetCore.Identity;
using Radin.Application.Interfaces.Contexts;
using Radin.Common.Dto;
using Radin.Domain.Entities.Branches;
using Radin.Domain.Entities.Customers;
using Radin.Domain.Entities.Factors;
using Radin.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.Json;
using Radin.Common.StaticClass;

namespace Radin.Application.Services.Factors.Commands.Orders
{
    public interface IInitialOrderService
    {
        ResultDto<ResultInitialOrderDto> Execute(RequestInitialOrderDto result);
    }

    public class InitialOrderService : IInitialOrderService
    {
        private readonly IDataBaseContext _context;
        private readonly UserManager<User> _userManager;
        private static readonly HttpClient client = new HttpClient();

        public InitialOrderService(IDataBaseContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        public ResultDto<ResultInitialOrderDto> Execute(RequestInitialOrderDto request)
        {

            var Errors = new List<IdLabelDto>();
           
            DateTime dateTime = SimpleMethods.InsertDateTime(request.InitialConnectionTime);
            DateTime now=SimpleMethods.TimeToTehran(DateTime.Now);
            if (dateTime > now.AddMinutes(30))
            {
                return new ResultDto<ResultInitialOrderDto>()
                {
                    Data = new ResultInitialOrderDto()
                    {
                        Id = 0,
                        Errors = Errors,
                    },
                    IsSuccess = false,
                    Message = "زمان خرید باید قبل از زمان فعلی باشد"
                };
            }
            var TatilatResult=  IsHolidayAsync(dateTime);
            if (TatilatResult.Result == null)
            {
                return new ResultDto<ResultInitialOrderDto>()
                {
                    Data = new ResultInitialOrderDto()
                    {
                        Id = 0,
                        Errors = Errors,
                    },
                    IsSuccess = false,
                    Message = "Tatilat Api have problem !"
                };
            }
            //Console.WriteLine(dateTime);
            //DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
            try
            {
                
                var user = _userManager.FindByIdAsync(request.UserId).Result;
                string SellerId = user.Id;
                long BranchCode = user.BranchCode;
                //long SalerCode = _context.SellerINFOs.FirstOrDefault(s => s.UserId == SalerId).SellerCode;
                int id = 0;
                if (BranchCode == null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!کد شعبه را وارد نمایید"
                    });
                }

                //if (SalerCode == null)
                //{
                //    id = id + 1;
                //    Errors.Add(new IdLabelDto
                //    {
                //        id = id,
                //        label = "!کد فروشنده در سیستم ثبت نمی باشد"
                //    });
                //}

                if (string.IsNullOrWhiteSpace(request.InitialConnectionTime.ToString()))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!تاریخ ثبت را وارد نمایید"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.WorkName))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!نام کار را وارد نمایید"
                    });
                }


                if (Errors.Count() < 1)
                {
                    

                    
                    DayOfWeek dayOfWeek = dateTime.DayOfWeek;

                    int month = dateTime.Month;
                    int year = dateTime.Year;
                    int day = dateTime.Day;
                    MainFactor factor = new MainFactor()
                    {
                        BranchCode = BranchCode,
                        ConnectionCount = 1,
                        InitialConnectionTime = dateTime,
                        LastConnectionTime = dateTime,
                        dayofweek = dayOfWeek.ToString(),
                        month = month.ToString(),
                        year = year.ToString(),
                        day = day.ToString(),
                        TatilRasmi = TatilatResult.Result.Data,
                        WorkName = request.WorkName,
                        MainsellerID = SellerId,
                        position = false,
                        ExpireTime = dateTime.AddDays(30),
                        state = 0,
                        status = false
                    };
                    


                    if (request.FactorId != null && request.FactorId!=0)
                    {
                        // Assuming 'factor' is the object populated with the new values you want to update
                        var existingFactor = _context.MainFactors.FirstOrDefault(f => f.Id == request.FactorId);
                        

                        if (existingFactor != null)
                        {
                            existingFactor.WorkName = request.WorkName;
                            existingFactor.InitialConnectionTime = dateTime;
                            existingFactor.LastConnectionTime = dateTime;
                            existingFactor.dayofweek = dayOfWeek.ToString();
                            existingFactor.month = month.ToString();
                            existingFactor.year = year.ToString();
                            existingFactor.day = day.ToString();
                            existingFactor.TatilRasmi = TatilatResult.Result.Data;




                            _context.SaveChanges();

                            return new ResultDto<ResultInitialOrderDto>()
                            {
                                Data = new ResultInitialOrderDto()
                                {
                                    Id = existingFactor.Id,
                                    Errors = Errors,
                                },
                                IsSuccess = true,
                                Message = "ثبت اولیه ویرایش شد",
                            };
                        }
                        // Update the existing entity
                        else
                        {
                            return new ResultDto<ResultInitialOrderDto>()
                            {
                                Data = new ResultInitialOrderDto()
                                {
                                    Id = 0,
                                    Errors = Errors,
                                },
                                IsSuccess = false,
                                Message = "ثبت فاکتور انجام نشد !"
                            };
                        }
                    }
                    else
                    {
                       
                        //factor.CustomerID = _context.CustomerInfo.OrderByDescending(c => c.Id).FirstOrDefault().Id;
                        _context.MainFactors.Add(factor);
                        _context.SaveChanges();
                    }

                    _context.SaveChanges();

                    return new ResultDto<ResultInitialOrderDto>()
                    {
                        Data = new ResultInitialOrderDto()
                        {
                            Id = factor.Id,
                            Errors = Errors,
                        },
                        IsSuccess = true,
                        Message = "ثبت اولیه فاکتور شد",
                    };
                }
                else
                {
                    return new ResultDto<ResultInitialOrderDto>()
                    {
                        Data = new ResultInitialOrderDto()
                        {
                            Id = 0,
                            Errors = Errors,
                        },
                        IsSuccess = false,
                        Message = "ثبت فاکتور انجام نشد !"
                    };

                }
            }
            catch (Exception)
            {
                return new ResultDto<ResultInitialOrderDto>()
                {
                    Data = new ResultInitialOrderDto()
                    {
                        Id = 0,
                        Errors = Errors,
                    },
                    IsSuccess = false,
                    Message = "ثبت فاکتور با مشکل شد !"
                };


            }

        }


        private async Task<ResultDto<bool>> IsHolidayAsync(DateTime date
        )
        {

            // Format the URL based on the provided date
            string url = $"https://holidayapi.ir/gregorian/{date.Year}/{date.Month:D2}/{date.Day:D2}";

            try
            {
                // Send GET request to the API
                HttpResponseMessage response = await client.GetAsync(url);

                // Ensure successful response
                response.EnsureSuccessStatusCode();

                // Read and parse the JSON response
                string jsonResponse = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);

                // Extract `is_holiday` field from the JSON
                bool isHoliday = doc.RootElement.GetProperty("is_holiday").GetBoolean();

                return new ResultDto<bool>
                {
                    Data= isHoliday,
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error checking holiday: {ex.Message}");
                return null;
            }
        }

    }

    



    public class RequestInitialOrderDto
    {
        public long? FactorId { get; set; }    
        public string UserId { get; set; }
        public long InitialConnectionTime { get; set; }
        public string WorkName { get; set; } 
    }

    public class ResultInitialOrderDto
    {
        public long Id { get; set; }
        public List<IdLabelDto> Errors { get; set; }
    }
}
