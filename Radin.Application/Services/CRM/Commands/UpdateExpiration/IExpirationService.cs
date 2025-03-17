using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.CRM.Queries.CrmGet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Factors;
using Radin.Domain.Entities.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Radin.Application.Services.CRM.Commands.UpdateExpiration.ExpirationService;

namespace Radin.Application.Services.CRM.Commands.UpdateExpiration
{
    public interface IExpirationService
    {
        ResultDto UpdateExpireTime(ExpireRequest request);
        ResultDto UpdateExpireTimeWithDaysInput(ExpireRequest request);
        ResultDto CheckExpireMessages();

        ResultDto CycleExpireTime(ExpireRequest request);
    }
    public class ExpirationService : IExpirationService
    {
        private readonly IDataBaseContext _context;
        private readonly ILogger<ExpirationService> _logger;

        public ExpirationService(IDataBaseContext context, ILogger<ExpirationService> logger)
        {
            _context = context;
            _logger = logger;
        }


        public ResultDto UpdateExpireTime(ExpireRequest request)
        {
            try
            {
                var factor=_context.MainFactors.FirstOrDefault(m=>m.Id==request.factorId&& !m.IsRemoved);
                // Check if the factor exists and ExpireTime needs updating
                if (factor == null)
                {
                    _logger.LogWarning("Factor is null. Cannot update ExpireTime.");
                    return new ResultDto
                    {

                        IsSuccess = false,
                        Message = " چنین فاکتوری وجود ندارد"
                    };

                }

                // Add 30 days to the ExpireTime

                //var InitialConnectionTime=factor.InitialConnectionTime;
                //var InitialExpireTime=InitialConnectionTime.AddDays(30);
                //var CurrentExpireTime = InitialExpireTime;
                //if (factor.ExpireTime > InitialConnectionTime || (InitialConnectionTime-factor.ExpireTime).Days<10)
                //{
                //    CurrentExpireTime = factor.ExpireTime.AddDays(-9);


                //}
                //else
                //{
                //    CurrentExpireTime = InitialExpireTime;

                //}
                //factor.ExpireTime = CurrentExpireTime;





                factor.ExpireTime = factor.ExpireTime.AddDays(30);

                // Save changes to the database
                _context.SaveChangesAsync();

                _logger.LogInformation($"Successfully updated ExpireTime for Factor ID: {factor.Id}");
                return new ResultDto
                {

                    IsSuccess = true,
                    Message = " عملیات موفق"
                }; // Indicate success
              }
            catch (Exception ex)
           {
                // Log the exception and provide useful details for troubleshooting
                _logger.LogError(ex, $"Error updating ExpireTime for Factor ID: {request.factorId }");
                return new ResultDto
                {

                    IsSuccess = false,
                    Message = " خطا در برنامه "
                }; // Indicate failure
            }
        }








        public ResultDto UpdateExpireTimeWithDaysInput(ExpireRequest request)
        {
            try
            {
                var factor = _context.MainFactors.FirstOrDefault(m => m.Id == request.factorId && !m.IsRemoved);
                // Check if the factor exists and ExpireTime needs updating
                if (factor == null)
                {
                    _logger.LogWarning("Factor is null. Cannot update ExpireTime.");
                    return new ResultDto
                    {

                        IsSuccess = false,
                        Message = " چنین فاکتوری وجود ندارد"
                    };

                }


                factor.ExpireTime = factor.ExpireTime.AddDays(request.DayNymber);
                var ProxyNotif = _context.ProxyNotifications.FirstOrDefault(f => f.FactorId == request.factorId&&!f.IsRemoved);
                if (ProxyNotif != null) { ProxyNotif.IsRemoved = true; }
                // Save changes to the database
                _context.SaveChangesAsync();

                _logger.LogInformation($"Successfully updated ExpireTime for Factor ID: {factor.Id}");
                return new ResultDto
                {

                    IsSuccess = true,
                    Message = " عملیات موفق"
                }; // Indicate success
            }
            catch (Exception ex)
            {
                // Log the exception and provide useful details for troubleshooting
                _logger.LogError(ex, $"Error updating ExpireTime for Factor ID: {request.factorId}");
                return new ResultDto
                {

                    IsSuccess = false,
                    Message = " خطا در برنامه "
                }; // Indicate failure
            }
        }







        public ResultDto CheckExpireMessages()
        {


            //try
            //{
                // Wait until midnight (00:00) of the next day
                DateTime now = DateTime.Now;
                DateTime nextRun = now.Date.AddDays(1).AddHours(0).AddMinutes(0).AddSeconds(0);
                TimeSpan delay = nextRun - now;
                 DateTime tomorrow = now.AddDays(1);

            // Update all MainFactors' ExpireTime by adding 1 day
            var mainFactors = _context.MainFactors
                                              .Where(f => f.ExpireTime >= now && f.ExpireTime < tomorrow)
                          .ToList();
            var mainFactor = _context.MainFactors
                                              .FirstOrDefault(f => f.Id == 11003);
            if (mainFactor.ExpireTime >= now && mainFactor.ExpireTime < tomorrow) { Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> {mainFactors.Count} "); }
            
            var newMessages = new List<ProxyNotification>();
                    var PreviousMesages = _context.ProxyNotifications.Where(p => !p.IsRemoved).ToList();
                    if (PreviousMesages.Any())
                    {
                        foreach (var message in PreviousMesages)
                        {
                            message.IsRemoved = true; // Set status to false
                        }
                        _context.ProxyNotifications.UpdateRange(PreviousMesages);
                    }
                    if (mainFactors.Any())
                    {
                    foreach (var factor in mainFactors)
                    {
                        newMessages.Add(new ProxyNotification
                        {
                            FactorId = factor.Id,
                            BranchCode = factor.BranchCode,
                            WorkName = factor.WorkName,
                            ExpirationTime = factor.ExpireTime,

                        });
                    }
                _context.ProxyNotifications.AddRange(newMessages);
                _context.SaveChangesAsync();
                    

                   

                    }
                return new ResultDto
                {

                    IsSuccess = true,
                    Message = " موفق "
                }; // Indicate failure
            //}
            
            //catch (Exception ex)
            //{
            //    return new ResultDto
            //    {

            //        IsSuccess = false,
            //        Message = " خطا در برنامه "
            //    }; // Indicate failure
            //}
        }






            public ResultDto CycleExpireTime(ExpireRequest request)
        {
            try
            {
                var factor = _context.MainFactors.FirstOrDefault(m => m.Id == request.factorId && !m.IsRemoved);
                // Check if the factor exists and ExpireTime needs updating
                if (factor == null)
                {
                    _logger.LogWarning("Factor is null. Cannot update ExpireTime.");
                    return new ResultDto
                    {

                        IsSuccess = false,
                        Message = " چنین فاکتوری وجود ندارد"
                    };

                }

                // Add 30 days to the ExpireTime

                var InitialConnectionTime = factor.InitialConnectionTime;
                var InitialExpireTime = InitialConnectionTime.AddDays(30);
                var CurrentExpireTime = InitialExpireTime;
                if (factor.ExpireTime > InitialConnectionTime || (InitialConnectionTime - factor.ExpireTime).Days < 10)
                {
                    CurrentExpireTime = factor.ExpireTime.AddDays(-9);


                }
                else
                {
                    CurrentExpireTime = InitialExpireTime;

                }
                factor.ExpireTime = CurrentExpireTime;





                //factor.ExpireTime = factor.ExpireTime.AddDays(30);

                // Save changes to the database
                _context.SaveChangesAsync();

                _logger.LogInformation($"Successfully updated ExpireTime for Factor ID: {factor.Id}");
                return new ResultDto
                {

                    IsSuccess = true,
                    Message = " عملیات موفق"
                }; // Indicate success
            }
            catch (Exception ex)
            {
                // Log the exception and provide useful details for troubleshooting
                _logger.LogError(ex, $"Error updating ExpireTime for Factor ID: {request.factorId}");
                return new ResultDto
                {

                    IsSuccess = false,
                    Message = " خطا در برنامه "
                }; // Indicate failure
            }
        }
        public class ExpireRequest
        {
            public long factorId { get; set; }
            public int DayNymber { get; set; } = 1;
        }
        
    }

}
