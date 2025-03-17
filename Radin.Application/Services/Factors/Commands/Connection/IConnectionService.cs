using Microsoft.EntityFrameworkCore;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Factors.Commands.Customer;
using Radin.Application.Services.Factors.Commands.NegotiationSet;
using Radin.Common.Dto;
using Radin.Common.StaticClass;
using Radin.Domain.Entities.Customers;
using Radin.Domain.Entities.Factors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Radin.Application.Services.Factors.Commands.Customer.CustomerService;

namespace Radin.Application.Services.Factors.Commands.SetConnection
{
    public interface IConnectionService
    {
        Task<ResultDto> AddConnection(AddConnectionRequest request, HttpClient client);
        Task<ResultDto> RemoveConnection(RemoveConnectionRequest request, HttpClient client);

    }

    public class ConnectionService : IConnectionService
    {
        private readonly IDataBaseContext _context;
        private static readonly HttpClient client = new HttpClient();
        public ConnectionService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> AddConnection(AddConnectionRequest request, HttpClient client)
        {


            try
            {
                
                DateTime dateTime = SimpleMethods.InsertDateTime(request.ConnectionTime);
                var Factor=_context.MainFactors.Include(m => m.CustomerConnections).FirstOrDefault(p=>p.Id==request.FactorId&& !p.IsRemoved );
                if (Factor==null)
                {
                    return new ResultDto
                    {
                        IsSuccess = false,
                        Message="چنین فاکتوری وجود ندارد"
                    };
                }
                var PreviousConnections= Factor.CustomerConnections.ToList();
                var PreviousConnectionDuration= PreviousConnections.Sum(p=>p.ConnectionDuration);
                var PreviousCount= PreviousConnections.Count();


                var newConnection = new CustomerConnection
                {
                    FactorID=request.FactorId,
                    ConnectionDuration= Convert.ToInt32(request.ConnectionDuration.TotalMinutes),
                    ConnectinTime= dateTime,
                    ContactType=request.ContactType.id,
                    ContactTypeName=request.ContactType.label
                };
                Factor.CustomerConnections.Add(newConnection);
                Factor.ConnectionCount = PreviousCount + 1;
                Factor.ConnectionDuration = PreviousConnectionDuration + newConnection.ConnectionDuration;
                Factor.ContactType= request.ContactType.id;
                
                
                
                
                var LastConnectionTime = Factor.CustomerConnections
                .Max(p => p.ConnectinTime);
                Factor.LastConnectionTime = LastConnectionTime;
                var TatilatResult = IsHolidayAsync(LastConnectionTime);
                DayOfWeek dayOfWeek = LastConnectionTime.DayOfWeek;

                int month = LastConnectionTime.Month;
                int year = LastConnectionTime.Year;
                int day = LastConnectionTime.Day;
                Factor.day = day.ToString();
                Factor.month = month.ToString();
                Factor.year = year.ToString();
                Factor.dayofweek=dayOfWeek.ToString();




                if (Factor.state != 0 && Factor.CustomerID!=null&& Factor.RecommandedDesign!=null)
                {
                    Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>{1}");
                    var Customer = _context.CustomerInfo.FirstOrDefault(p => p.CustomerID == Factor.CustomerID);
                    if (Customer == null)
                    {
                        return new ResultDto
                        {
                            Message = "مشتری با چنین مشخصاتی وجود ندارد",
                            IsSuccess = false
                        };
                    }
                    string url = Environment.GetEnvironmentVariable("PROBABILITY_API");
                    var InitialConnectionTime = $"{Factor.InitialConnectionTime:hh:mm:ss tt}";
                    var Input = new
                    {
                        Gender = Customer.Gender,
                        AgeCategory = Customer.AgeCategory,
                        CharacterType = Customer.CharacterType,
                        dayofweek = Factor.dayofweek,
                        ConnectionCount = Factor.ConnectionCount,
                        ConnectionDuration = Factor.ConnectionDuration,
                        ContactType = Factor.ContactType != 0 ? Factor.ContactType : 3,
                        RecommandedDesign = Factor.RecommandedDesign,
                        TotalAmount = Factor.TotalAmount,
                        InitialConnectionTime = InitialConnectionTime
                    };
                    // Send the request to the Python API
                    string serializedInput = JsonSerializer.Serialize(Input);
                    Console.WriteLine($"Serialized JSON: {serializedInput}");

                    var content = new StringContent(serializedInput, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);


                    
                    if (!response.IsSuccessStatusCode)
                    {
                        return new ResultDto
                        {
                            Message = "اشکال در محاسبه احتمال",
                            IsSuccess = false
                        };
                    }

                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> {responseContent}");

                    // Deserialize the Python API's JSON response
                    var pythonApiResponse = JsonSerializer.Deserialize<Probability>(responseContent);
                    //Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> {pythonApiResponse.probabilities[1]}");
                    Factor.PurchaseProbability = (float)Math.Round(pythonApiResponse.probabilities[1] * 100, 1);
                }


                _context.MainFactors.Update(Factor);

                _context.SaveChanges();

                return new ResultDto
                {
                    
                    IsSuccess = true,
                    Message = "ارتباط ثبت شد"
                };
            }
            catch
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "خطا در ثبت ارتباط"
                };

            }
        }







        public async Task<ResultDto> RemoveConnection(RemoveConnectionRequest request, HttpClient client)
        {


            try
            {
                
                var Factor = _context.MainFactors.Include(m => m.CustomerConnections).Where(p => p.Id == request.FactorId && !p.IsRemoved).FirstOrDefault();
                if (Factor == null)
                {
                    return new ResultDto
                    {
                        IsSuccess = false,
                        Message = "چنین فاکتوری وجود ندارد"
                    };
                }

                var Connection = Factor.CustomerConnections.FirstOrDefault(p => p.Id == request.CustomerConnectionId);
                if (Connection == null)
                {
                    return new ResultDto
                    {
                        IsSuccess = false,
                        Message = "چنین ارتباطی وجود ندارد"
                    };
                }


                if (Factor.ConnectionCount > 1) { Factor.ConnectionCount = Factor.ConnectionCount - 1; }
                Factor.ConnectionDuration = Factor.ConnectionDuration - Connection.ConnectionDuration;
                if (Factor.CustomerConnections.ToList().Count>1)
                {
                    Factor.ContactType = Factor.CustomerConnections.Last().ContactType;
                }

                var LastConnectionTime = Factor.CustomerConnections
                .Max(p => p.ConnectinTime);
                Factor.LastConnectionTime = LastConnectionTime;
                var TatilatResult = IsHolidayAsync(LastConnectionTime);
                DayOfWeek dayOfWeek = LastConnectionTime.DayOfWeek;

                int month = LastConnectionTime.Month;
                int year = LastConnectionTime.Year;
                int day = LastConnectionTime.Day;
                Factor.day = day.ToString();
                Factor.month = month.ToString();
                Factor.year = year.ToString();
                Factor.dayofweek = dayOfWeek.ToString();


                if (Factor.state != 0 && Factor.CustomerID != null && Factor.RecommandedDesign != null)
                {
                    var Customer = _context.CustomerInfo.FirstOrDefault(p => p.CustomerID == Factor.CustomerID);
                    if (Customer == null)
                    {
                        return new ResultDto
                        {
                            Message = "مشتری با چنین مشخصاتی وجود ندارد",
                            IsSuccess = false
                        };
                    }
                    string url = Environment.GetEnvironmentVariable("PROBABILITY_API");
                    var InitialConnectionTime = $"{Factor.InitialConnectionTime:hh:mm:ss tt}";
                    var Input = new
                    {
                        Gender = Customer.Gender,
                        AgeCategory = Customer.AgeCategory,
                        CharacterType = Customer.CharacterType,
                        dayofweek = Factor.dayofweek,
                        ConnectionCount = Factor.ConnectionCount,
                        ConnectionDuration = Factor.ConnectionDuration,
                        ContactType = Factor.ContactType != 0 ? Factor.ContactType : 3,
                        RecommandedDesign = Factor.RecommandedDesign,
                        TotalAmount = Factor.TotalAmount,
                        InitialConnectionTime = InitialConnectionTime
                    };
                    // Send the request to the Python API
                    string serializedInput = JsonSerializer.Serialize(Input);
                    Console.WriteLine($"Serialized JSON: {serializedInput}");

                    var content = new StringContent(serializedInput, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);



                    if (!response.IsSuccessStatusCode)
                    {
                        return new ResultDto
                        {
                            Message = "اشکال در محاسبه احتمال",
                            IsSuccess = false
                        };
                    }

                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> {responseContent}");

                    // Deserialize the Python API's JSON response
                    var pythonApiResponse = JsonSerializer.Deserialize<Probability>(responseContent);
                    //Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> {pythonApiResponse.probabilities[1]}");
                    Factor.PurchaseProbability = (float)Math.Round(pythonApiResponse.probabilities[1] * 100, 1);
                }
                Factor.CustomerConnections.Remove(Connection);

                _context.SaveChanges();

                return new ResultDto
                {
                    
                    IsSuccess = true,
                    Message = "ارتباط حذف شد"
                };
            }
            catch
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "خطا در حذف ارتباط"
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
                    Data = isHoliday,
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

    public class AddConnectionRequest
    {
        public long FactorId { get; set; }
        public string ConnectionTime { get; set; }
        public TimeSpan ConnectionDuration { get; set; }
        public IdLabelDto ContactType { get; set; }
    }
    public class RemoveConnectionRequest
    {
        public long FactorId { get; set; }
        public long CustomerConnectionId { get; set; }
       
    }
}