using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Radin.Application.Interfaces.Contexts;
using Radin.Domain.Entities.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.CRM.Commands.CheckExpiration
{
    public class CheckExpirationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CheckExpirationService> _logger;
        public CheckExpirationService(IServiceProvider serviceProvider, ILogger<CheckExpirationService> logger
       )
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ExpireTimeUpdaterService is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Wait until midnight (00:00) of the next day
                    DateTime now = DateTime.Now;
                    DateTime nextRun = now.Date.AddDays(1).AddHours(0).AddMinutes(0).AddSeconds(0);
                    TimeSpan delay = nextRun - now;
                    DateTime tomorrow = now.AddDays(5);

                    _logger.LogInformation($"Next update scheduled for: {nextRun}");

                    await Task.Delay(delay, stoppingToken); // Wait until midnight

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var _context = scope.ServiceProvider.GetRequiredService<IDataBaseContext>();
                        // Update all MainFactors' ExpireTime by adding 1 day
                        var mainFactors = _context.MainFactors
                                                  .Where(f => f.ExpireTime >= now && f.ExpireTime < tomorrow&& !f.position)
                                                    .ToList();
                        
                        var ExpiredFactors = _context.MainFactors
                           .Where(f => f.ExpireTime < now && f.position == false && !f.status&& f.CustomerID != null && f.RecommandedDesign != null && f.ReasonStatus != null)
                           .ToList();
                        
                        ExpiredFactors.ForEach(f => f.position = true);
                        var newMessages = new List<ProxyNotification>();
                        var PreviousMesages = _context.ProxyNotifications.Where(p=>!p.IsRemoved).ToList();
                        if (PreviousMesages.Any()) 
                        {
                            foreach (var message in PreviousMesages)
                            {
                                message.IsRemoved = true; // Set status to false
                            }
                            _context.ProxyNotifications.UpdateRange(PreviousMesages);
                        }
                        if(mainFactors.Any()) 
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
                            
                            await _context.ProxyNotifications.AddRangeAsync(newMessages);



                        }
                        mainFactors.ForEach(p=>p.ExpireTime = p.ExpireTime.AddDays(1));
                        _context.MainFactors.UpdateRange(mainFactors);
                        _context.MainFactors.UpdateRange(ExpiredFactors);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation($"Updated {mainFactors.Count} records at {DateTime.Now}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating ExpireTime.");
                }
            }

            _logger.LogInformation("ExpireTimeUpdaterService is stopping.");
        }
    }
}