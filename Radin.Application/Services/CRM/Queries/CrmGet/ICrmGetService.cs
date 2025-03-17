using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Radin.Application.Interfaces.Contexts;
using Radin.Common.Dto;
using Radin.Domain.Entities.Factors;
using System;
using System.Collections.Generic;
using System.Linq;

using static Radin.Application.Services.CRM.Queries.CrmGet.CrmGetService;

namespace Radin.Application.Services.CRM.Queries.CrmGet
{
    public interface ICrmGetService
    {
        ResultDto<List<ResultForCrm>> GetForCrm(long BranchCode);


    }

    public class CrmGetService : ICrmGetService
    {
        private readonly IDataBaseContext _context;
        private readonly ILogger<CrmGetService> _logger;

        public CrmGetService(IDataBaseContext context , ILogger<CrmGetService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public ResultDto<List<ResultForCrm>> GetForCrm(long BranchCode)
        {
            try
            {
                var Branch = _context.BranchINFOs.FirstOrDefault(p => p.BranchCode == BranchCode);
                if (Branch == null)
                {
                    return new ResultDto<List<ResultForCrm>>
                    {
                        IsSuccess = false,
                        Message = "چنین شعبه ای وجود ندارد",
                    };
                }

                var result = Enumerable.Range(0, 4) // States 0 to 4
                    .Select(state => new ResultForCrm
                    {
                        state = state,
                        Factors = _context.MainFactors
                            .Where(m => m.state >= 0 && m.state <= 3 && m.state == state && m.BranchCode == BranchCode && !m.IsRemoved && !m.position)
                            .Select(m => new
                            {
                                id =m.Id,
                                InitialConnectionTime = (long)(m.InitialConnectionTime - new DateTime(1970, 1, 1)).TotalMilliseconds,
                                WorkName = m.WorkName,
                                TotalAmount = m.TotalAmount,
                                PurchaseProbability = m.PurchaseProbability,
                                CustomerID = m.CustomerID,
                                ExpireTime = ExpirationColor(m.ExpireTime),
                                UpdateTime = m.UpdateTime,

                            })
                            .ToList().OrderByDescending(n => n.UpdateTime) // Materialize the query here
                            .Select(n => new DataList
                            {
                                id=n.id,
                                InitialConnectionTime = n.InitialConnectionTime,
                                WorkName = n.WorkName,
                                TotalAmount =n.TotalAmount>0 ? Convert.ToString(n.TotalAmount) : "بدون قیمت",
                                PurchaseProbability = n.PurchaseProbability > 0  ? $"{Convert.ToString(n.PurchaseProbability)}%" : "احتمال نامعین",
                                CustomerName = GetCustomerName(n.CustomerID),
                                CustomerCharacterType = GetCharacterTypeColors(n.CustomerID),
                                ExpirationColor = n.ExpireTime.IsSuccess ? n.ExpireTime.Message : Environment.GetEnvironmentVariable("NEXPIRED_COLOR")
                            })
                            .ToList()
                    })
                    .ToList();

                return new ResultDto<List<ResultForCrm>>
                {
                    IsSuccess = true,
                    Message = "دریافت موفق",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                // Log the exception if needed (use logging framework)

                return new ResultDto<List<ResultForCrm>>
                {
                    IsSuccess = false,
                    Message = "دریافت ناموفق",
                };
            }
        }
        

        private static ResultDto ExpirationColor(DateTime ExpireTime)
        {
            try
            {
                string ColorCode = null ;
               

                var currentTime = DateTime.Now;
                if (ExpireTime < currentTime)
                {
                    ColorCode= Environment.GetEnvironmentVariable("EXPIRED_COLOR"); // Expired

                }
                else if ((ExpireTime - currentTime).TotalDays <= 10)
                {
                    ColorCode= Environment.GetEnvironmentVariable("CLOSETOEXPIRATION_COLOR"); // Expires within the next 3 days

                }
                else
                {
                    ColorCode= Environment.GetEnvironmentVariable("NOTEXPIRED_COLOR"); // More than 3 days until expiration

                }
                return new ResultDto
                {

                    IsSuccess = true,
                    Message = ColorCode

                };

            }
            catch (Exception ex)
            {
                return new ResultDto
                {

                    IsSuccess = false,
                    Message = ex.Message

                };
            }
        }
        


        private string GetCustomerName(long? customerId)
        {
            if (customerId == null)
            {
                return "نام نامعین";
            }
            var TCustomer = _context.CustomerInfo
                .Where(c => c.CustomerID == customerId)
                .Select(c => new { name=c.Name,family=c.LastName }
                )
                .FirstOrDefault();
            if (TCustomer == null)
            {
                return "نام نامعین";
                    };
            return $"{TCustomer.name} {TCustomer.family}";
        }


        private List<ColorModel> GetCharacterTypeColors(long? customerId)
        {
            if (customerId == null)
            {
                return new List<ColorModel> {
            new ColorModel {
                id = "0",
                label = "N",
                Color = Environment.GetEnvironmentVariable("COLOR_N"),
            }
        };
            }

            // Define character type and color mappings
            var characterColorMapping = new Dictionary<string, string>
    {
        { "C", Environment.GetEnvironmentVariable("COLOR_C") },
        { "S", Environment.GetEnvironmentVariable("COLOR_S") },
        { "I", Environment.GetEnvironmentVariable("COLOR_I") },
        { "D", Environment.GetEnvironmentVariable("COLOR_D") },
        { "N", Environment.GetEnvironmentVariable("COLOR_N") }
    };

            // Define integer to character types mapping
            var characterMapping = new Dictionary<int, List<string>>
    {
        { 14, new List<string> { "N" } },
        { 1, new List<string> { "D" } },
        { 2, new List<string> { "I" } },
        { 3, new List<string> { "S" } },
        { 4, new List<string> { "C" } },
        { 5, new List<string> { "D", "I" } }, // CS
        { 6, new List<string> { "D", "C" } },
        { 7, new List<string> { "I", "S" } },// CS
        { 8, new List<string> { "S", "C" } },// CS
        { 9, new List<string> { "D", "I", "S" } }, // CSD
        { 10, new List<string> { "I", "S", "C" } },
        { 11, new List<string> { "S", "C", "D" } },
        { 12, new List<string> { "C", "D", "I" } },
        { 13, new List<string> { "D", "I", "S","C" } },
        // Add more mappings as necessary...
    };

            // Fetch character type for the customer
            var characterTypeValue = _context.CustomerInfo
                .Where(c => c.CustomerID == customerId)
                .Select(c => c.CharacterType)
                .FirstOrDefault() ?? 0; // Default to 0 if no value found

            var colorModels = new List<ColorModel>();

            // Interpret the character type integer
            if (characterMapping.TryGetValue(characterTypeValue, out var characterTypes))
            {
                // Create a ColorModel for each character type associated with this integer
                for (int i = 0; i < characterTypes.Count; i++)
                {
                    var characterType = characterTypes[i];

                    if (characterColorMapping.TryGetValue(characterType, out var color))
                    {
                        colorModels.Add(new ColorModel
                        {
                            id = characterType,
                            label = characterType,
                            Color = color
                        });
                    }
                }
                colorModels.Reverse();

            }

            else
            {
                return new List<ColorModel>
        {
            new ColorModel
            {
                id = "0",
                label = "N",
                Color = Environment.GetEnvironmentVariable("COLOR_N"),
            }
        };
            }

            // Return colorModels with an index prefix in the desired format
            return colorModels.Select((cm, index) => new ColorModel
            {
                id = index.ToString(),
                label = $"{cm.label}",
                Color = cm.Color
            }).ToList();
        }



        public class ColorModel
        {
            public string id { get; set; }
            public string label { get; set; }
            public string Color { get; set; }
        }

        public class DataList
        {
            public long id { get; set; }
            public long InitialConnectionTime { get; set; }
            public string WorkName { get; set; }
            public string TotalAmount { get; set; }
            public string PurchaseProbability { get; set; }
            public string CustomerName { get; set; }
            public string ExpirationColor { get; set; }   
            public List<ColorModel> CustomerCharacterType { get; set; }
        }

        public class ResultForCrm
        {
            public int state { get; set; }
            public List<DataList> Factors { get; set; }
        }
    }
}
