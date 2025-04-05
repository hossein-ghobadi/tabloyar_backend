//using Microsoft.AspNetCore.Identity;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Branch.Commands.BranchInfoSetService;
//using Radin.Application.Services.Factors.Commands.Orders;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Branches;
//using Radin.Domain.Entities.Users;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Net.Http.Json;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Factors.Commands.NegotiationSet
//{
//    public interface INegotiationSetService
//    {
//        Task<ResultDto> NegotiationSet(NegotiationSetRequestDto request, HttpClient client);
//        Task<ResultDto<float>> PurchaseProbability(ProbabilityRequestDto request,HttpClient client );

//    }

//    public class NegotiationSetService : INegotiationSetService
//    {
//        private readonly IDataBaseContext _context;


//        public NegotiationSetService(IDataBaseContext context)
//        {
//            _context = context;

//        }
//        public async Task<ResultDto> NegotiationSet(NegotiationSetRequestDto request, HttpClient client)
//        {

//            try 
//                    { 

//                    var Factor = _context.MainFactors.FirstOrDefault(p => p.Id == request.FactorId && !p.IsRemoved);
//                    if (Factor == null)
//                    {
//                        return new ResultDto
//                        {
//                            IsSuccess = false,
//                            Message = "فاکتور یافت نشد"
//                        };

//                    }
//                    int Counter = 0;

//                    Factor.CustomerID = request.CustomerId;
//                    if(request.AssistantSellerID != null)
//                    {
//                        Factor.AssistantSellerID = request.AssistantSellerID;
//                    }
//                    Factor.RecommandedDesign = request.RecommandedDesign;
//                    Factor.description = request.description;
//                    Factor.ReasonStatus = request.ReasonStatus;


//                //purchaseProbability
//                if ( Factor.state != 0)
//                {
//                    var Customer = _context.CustomerInfo.FirstOrDefault(p => p.CustomerID == request.CustomerId);
//                    if (Customer == null) {
//                        return new ResultDto
//                        {
//                            Message = "مشتری با چنین مشخصاتی وجود ندارد",
//                            IsSuccess = false
//                        };
//                    }
//                    string url = Environment.GetEnvironmentVariable("PROBABILITY_API");
//                    Console.WriteLine($"API URL: {url}");

//                    var InitialConnectionTime = $"{Factor.InitialConnectionTime:hh:mm:ss tt}";

//                    var Input = new
//                    {
//                        Gender = Customer.Gender,
//                        AgeCategory = Customer.AgeCategory,
//                        CharacterType = Customer.CharacterType,
//                        dayofweek = Factor.dayofweek,
//                        ConnectionCount = Factor.ConnectionCount,
//                        ConnectionDuration = Factor.ConnectionDuration,
//                        ContactType = Factor.ContactType != 0 ? Factor.ContactType : 3,
//                        RecommandedDesign = Factor.RecommandedDesign,
//                        TotalAmount = Factor.TotalAmount,
//                        InitialConnectionTime = InitialConnectionTime
//                    };

//                    // Log the serialized JSON
//                    string serializedInput = JsonSerializer.Serialize(Input);
//                    Console.WriteLine($"Serialized JSON: {serializedInput}");

//                    var content = new StringContent(serializedInput, Encoding.UTF8, "application/json");
//                    HttpResponseMessage response = await client.PostAsync(url, content);
//                    // Send the request

//                    if (!response.IsSuccessStatusCode)
//                    {
//                        string errorDetails = await response.Content.ReadAsStringAsync();
//                        Console.WriteLine($"Python API Error: {errorDetails}");
//                        return new ResultDto
//                        {
//                            Message = "اشکال در محاسبه احتمال",
//                            IsSuccess = false
//                        };
//                    }

//                    // Process the successful response
//                    string responseContent = await response.Content.ReadAsStringAsync();
//                    //Console.WriteLine($"API Response Content: {responseContent}");

//                    var pythonApiResponse = JsonSerializer.Deserialize<Probability>(responseContent);
//                    Factor.PurchaseProbability = (float)Math.Round(pythonApiResponse.probabilities[1] * 100, 1);
//                }

//                //if(Factor.state==3 && Factor.TotalAmount>0 && Factor.RecommandedDesign>0 && !string.IsNullOrEmpty(Factor.ReasonStatus ) && Factor.CustomerID != null&& Factor.ContactType != 0)
//                //{
//                //    Factor.status = true;
//                //    Factor.position = true;
//                //                 }

//                _context.MainFactors.Update(Factor);
//                _context.SaveChanges();
//                return new ResultDto
//                {
//                    Message = "اطلاعات مذاکره با موفقیت ثبت شد",
//                    IsSuccess = true
//                };

//            }
//            catch (Exception ex)
//            {
//                return new ResultDto
//                {
//                    Message = "اختلال در ثبت مذاکره",
//                    IsSuccess = false
//                };
//            }
                
//        }



//        public async Task<ResultDto<float>> PurchaseProbability(ProbabilityRequestDto request, HttpClient client)
//        {
//            string url = Environment.GetEnvironmentVariable("PROBABILITY_API");

//            // Format InitialConnectionTime as ISO 8601
//            var InitialConnectionTime = $"{request.InitialConnectionTime:hh:mm:ss tt}";

//            // Build the input object
//            var Input = new
//            {
//                Gender = request.gender,
//                AgeCategory = request.ageCategory,
//                CharacterType = request.CharacterType,
//                dayofweek = request.dayofweek,
//                ConnectionCount = request.ConnectionCount,
//                ConnectionDuration = request.ConnectionDuration,
//                ContactType = request.ContactType != 0 ? request.ContactType : 3,
//                RecommandedDesign = request.RecommandedDesign,
//                TotalAmount = request.TotalAmount,
//                InitialConnectionTime = InitialConnectionTime
//            };

//            // Serialize the input object
//            string serializedInput = JsonSerializer.Serialize(Input);
//            Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Serialized JSON: {serializedInput}");

//            // Prepare the HTTP content
//            var content = new StringContent(serializedInput, Encoding.UTF8, "application/json");

//            // Send the POST request
//            HttpResponseMessage response = await client.PostAsync(url, content);
//            Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>API Response Content: {response}");

//            // Check for success
//            if (!response.IsSuccessStatusCode)
//            {
//                return new ResultDto<float>
//                {
//                    Message = "Error from Python API",
//                    IsSuccess = false
//                };
//            }

//            // Read and deserialize the response
//            string responseContent = await response.Content.ReadAsStringAsync();
//            var pythonApiResponse = JsonSerializer.Deserialize<Probability>(responseContent);

//            return new ResultDto<float>
//            {
//                Data = (float)Math.Round(pythonApiResponse.probabilities[1] * 100, 1),
//                Message = "Success",
//                IsSuccess = true
//            };
//        }






//}
        

//}
