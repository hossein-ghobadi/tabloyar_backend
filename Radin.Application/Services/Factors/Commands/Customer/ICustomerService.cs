using Microsoft.EntityFrameworkCore;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Factors.Commands.StatusReason;
using Radin.Common.Dto;
using Radin.Common.StaticClass;
using Radin.Domain.Entities.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Radin.Application.Services.Factors.Commands.Customer.CustomerService;

namespace Radin.Application.Services.Factors.Commands.Customer
{
    public interface ICustomerService
    {
        Task<ResultDto<long>> AddCustomer(RequestAddCustomerDto request);
        Task<ResultDto<string>> EditCustomer(RequestAddCustomerDto request);
    }

    public class CustomerService : ICustomerService
    {
        private readonly IDataBaseContext _context;

        public CustomerService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<long>> AddCustomer(RequestAddCustomerDto request)
        {
           

            try
            {
                // Check if a customer with the same phone number already exists
                var existingCustomer = await _context.CustomerInfo
                    .FirstOrDefaultAsync(c => c.phone == request.phone);
                if (existingCustomer != null)
                {
                    // Return a failure response indicating the duplicate
                    return new ResultDto<long>
                    {
                        IsSuccess = false,
                        Message = "این شماره تلفن قبلا ثبت شده است"
                    };
                }

                // Create CustomerInfo object and map properties
                var customer = new CustomerInfo
                {
                    Name = request.Name,
                    LastName = request.LastName,
                    Gender = request.Gender,
                    JobCategory = request.JobCategory,
                    AgeCategory = request.AgeCategory,
                    CharacterType = request.CharacterTypeInfo.Id,
                    acquaintance = request.acquaintance,
                    MarketOriented = request.MarketOriented,
                    Country = 1,
                    Province = request.Province,
                    city = request.city,
                    phone = request.phone,
                    Address = request.Address,
                    Description = request.Description,
                    
                };
                if (request.Birtday != null) { customer.Birtday = SimpleMethods.InsertDateTime(request.Birtday ?? 11111); };
                    
                if(request.Latitude!=null && request.Longitude != null)
                {
                    customer.Longitude = request.Longitude;
                    customer.Latitude = request.Latitude;   
                }

                // Add and save the customer
                CharacterTypeDetails Details = new CharacterTypeDetails();
                Details.D = 0;// request.CharacterTypeInfo.DValue;
                Details.I = 0;// request.CharacterTypeInfo.IValue;
                Details.S = 0;// request.CharacterTypeInfo.SValue;
                Details.C = 0;// request.CharacterTypeInfo.CValue;
                if (request.CharacterTypeInfo.DValue !=null && request.CharacterTypeInfo.DValue != null
            && request.CharacterTypeInfo.IValue != null
                    && request.CharacterTypeInfo.SValue != null && request.CharacterTypeInfo.CValue != null)
                {
                    Details.D = request.CharacterTypeInfo.DValue;
                    Details.I = request.CharacterTypeInfo.IValue;
                    Details.S = request.CharacterTypeInfo.SValue;
                    Details.C = request.CharacterTypeInfo.CValue;

                }


                customer.CharacterTypeDetails = Details;
                
                _context.CustomerInfo.Add(customer);

                await _context.SaveChangesAsync();
                customer.CharacterTypeDetails.CustomerID = customer.Id;

                customer.CustomerID = customer.Id;
                await _context.SaveChangesAsync();




                return new ResultDto<long>
                {
                    Data=customer.CustomerID??0,
                    IsSuccess = true,
                    Message = "مشتری با موفقیت اضافه شد",
                };
            }
            catch (Exception ex)
            {
                return new ResultDto<long>
                {
                    IsSuccess = false,
                    Message = "خطا در اضافه کردن مشتری: " + ex.Message
                };
            }
        }



        public async Task<ResultDto<string>> EditCustomer(RequestAddCustomerDto request)
        {


            try
            {
                // Check if a customer with the same phone number already exists
                var existingCustomer = await _context.CustomerInfo
                            .Include(c => c.CharacterTypeDetails) // Ensure navigation property is loaded
                            .FirstOrDefaultAsync(c => c.CustomerID == request.CustomerId);
                var AnotherCUstomer = _context.CustomerInfo.FirstOrDefault(p => p.CustomerID != request.CustomerId && p.phone == request.phone);
                if (AnotherCUstomer != null) {

                    return new ResultDto<string>
                    {
                        IsSuccess = false,
                        Message = "این شماره به نام فرد دیگری ثبت شده است"
                    };

                }
                if (existingCustomer == null)
                {
                    // Return a failure response indicating the duplicate
                    return new ResultDto<string>
                    {
                        IsSuccess = false,
                        Message = "این شماره تلفن وجود ندارد"
                    };
                }

                
                existingCustomer.Name = request.Name;
                existingCustomer.LastName = request.LastName;
                existingCustomer.Gender = request.Gender;
                existingCustomer.JobCategory = request.JobCategory;
                if (request.Birtday != null) { existingCustomer.Birtday = SimpleMethods.InsertDateTime(request.Birtday ?? 11111); };
                existingCustomer.AgeCategory = request.AgeCategory;
                existingCustomer.CharacterType = request.CharacterTypeInfo.Id;
                existingCustomer.acquaintance = request.acquaintance;
                existingCustomer.MarketOriented = request.MarketOriented;
                existingCustomer.Country = 1;
                existingCustomer.Province = request.Province;
                existingCustomer.city = request.city;
                
                existingCustomer.Address = request.Address;
                existingCustomer.Description = request.Description;

                
                if (request.Latitude != null && request.Longitude != null)
                {
                    existingCustomer.Longitude = request.Longitude;
                    existingCustomer.Latitude = request.Latitude;
                }
                Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>{request.CharacterTypeInfo.DValue}");

            if (existingCustomer.CharacterTypeDetails != null && request.CharacterTypeInfo.DValue != null
            && request.CharacterTypeInfo.IValue != null
                    && request.CharacterTypeInfo.SValue != null && request.CharacterTypeInfo.CValue != null
                    ) {
                    existingCustomer.CharacterTypeDetails.D = request.CharacterTypeInfo.DValue;
                    existingCustomer.CharacterTypeDetails.I = request.CharacterTypeInfo.IValue;
                    existingCustomer.CharacterTypeDetails.S = request.CharacterTypeInfo.SValue;
                    existingCustomer.CharacterTypeDetails.C = request.CharacterTypeInfo.CValue;
                    existingCustomer.CharacterTypeDetails.CustomerID = existingCustomer.Id;

                }

                // Add and save the customer

                _context.CustomerInfo.Update(existingCustomer);
                await _context.SaveChangesAsync();

                // Now that Id is generated, set CustomerID to match Id
                
                return new ResultDto<string>
                {
                    Data = existingCustomer.Name,
                    IsSuccess = true,
                    Message = "اطلاعات مشتری با موفقیت ویرایش شد",

                };
        }
            catch (Exception ex)
            {
                return new ResultDto<string>
                {
                    IsSuccess = false,
                    Message = "خطا در ویرایش اطلاعات مشتری: " + ex.Message

    };
}




        }
        
        public class RequestAddCustomerDto
        {
            public long CustomerId { get; set; }
            [Required(ErrorMessage = "نام را وارد کنید")]
            public string Name { get; set; }

            [Required(ErrorMessage = "نام خانوادگی را وارد کنید")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "جنسیت را انتخاب کنید")]
            [Range(1, 3, ErrorMessage = "جنسیت معتبر نیست")]
            public int Gender { get; set; }

            [Required(ErrorMessage = "رسته شغلی را انتخاب کنید")]
            [Range(1, int.MaxValue, ErrorMessage = "رسته شغلی معتبر نیست")]
            public int JobCategory { get; set; }

            //[Required(ErrorMessage = "تاریخ تولد را وارد کنید")]
            public long? Birtday { get; set; }

            [Required(ErrorMessage = "گروه سنی را انتخاب کنید")]
            [Range(1, int.MaxValue, ErrorMessage = "گروه سنی معتبر نیست")]
            public int AgeCategory { get; set; }

            //[Required(ErrorMessage = "نوع شخصیت را انتخاب کنید")]
            //[Range(1, int.MaxValue, ErrorMessage = "نوع شخصیت معتبر نیست")]
            //public int CharacterType { get; set; }
            //[Required(ErrorMessage = "اطلاعات تیپ شخصیتی وارد شود")]
            public CharacterTypeInfo? CharacterTypeInfo { get; set; }    




            [Required(ErrorMessage = "نحوه آشنایی را انتخاب کنید")]
            [Range(1, int.MaxValue, ErrorMessage = "نحوه آشنایی معتبر نیست")]
            public int acquaintance { get; set; }

            [Required(ErrorMessage = "سمت بازار را انتخاب کنید")]
            [Range(1, int.MaxValue, ErrorMessage = "سمت بازار معتبر نیست")]
            public int MarketOriented { get; set; }

            //[Required(ErrorMessage = "کشور را انتخاب کنید")]
            //[Range(1, int.MaxValue, ErrorMessage = "کشور معتبر نیست")]
            //public int Country { get; set; }

            [Required(ErrorMessage = "استان را انتخاب کنید")]
            [Range(1, int.MaxValue, ErrorMessage = "استان معتبر نیست")]
            public int Province { get; set; }

            [Required(ErrorMessage = "شهر را انتخاب کنید")]
            [Range(1, int.MaxValue, ErrorMessage = "شهر معتبر نیست")]
            public int city { get; set; }

            [Required(ErrorMessage = "شماره تلفن را وارد کنید")]
            [Phone(ErrorMessage = "شماره تلفن معتبر نیست")]
            public string phone { get; set; }

            public string? Address { get; set; }

            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
            public string? Description { get; set; }
        }

        public class CharacterTypeInfo
        {
            public int Id { get; set; }
            public string label { get; set; }
            public float DValue { get; set; } = 0;
            public float IValue { get; set; } = 0;
            public float SValue { get; set; } = 0;
            public float CValue { get; set; } = 0;


        }



    }
}
