using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Factors.Queries.AccessoryGet;
using Radin.Common.Dto;
using Radin.Common.StaticClass;
using Radin.Domain.Entities.Customers;
using Radin.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Factors.Queries.CustomerGet
{
    public interface ICustomerGetService
    {
        ResultDto<CustomerItems> GetNeededDataList();
        ResultDto<CustomerItemsEdit> GetForEdit(long CustomerId, long branchCode);
        ResultDto<List<CustomerAbstractData>> GetBySearch(string search, long branchCode);
        Task<ResultDto<CustomersData>> BranchCustomersAsync(string userEmail, int pageNumber, int pageSize, string search);//پنل ادمین/سفارشات/مشتریان


    }



    public class CustomerGetService : ICustomerGetService
    {
        private readonly IDataBaseContext _context;
        private readonly UserManager<User> _userManager;

        public CustomerGetService(IDataBaseContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;


        }
        public ResultDto<CustomerItems> GetNeededDataList()
        {
            try
            {

                var AgeCategoryList = _context.AgeCategories.Select(p => new IdLabelDto
                {
                    id = Convert.ToInt32(p.Id),
                    label = p.category

                }).ToList();


                var CharacterTypeList = _context.PersonalityCharacterType.Select(p => new IdLabelDto
                {
                    id = Convert.ToInt32(p.Id),
                    label = p.Type

                }).ToList();
                //if (CharacterTypeList.Count > 0)
                //{
                //    var firstItem = CharacterTypeList[0]; // Get the first item
                //    CharacterTypeList.RemoveAt(0);       // Remove the first item
                //    CharacterTypeList.Add(firstItem);    // Add it to the end
                //}



                var acquaintanceList = _context.acquaintances.Select(p => new IdLabelDto
                {
                    id = Convert.ToInt32(p.Id),
                    label = p.type

                }).ToList();

                var MarketOrientedList = _context.marketOrients.Select(p => new IdLabelDto
                {
                    id = Convert.ToInt32(p.Id),
                    label = p.type

                }).ToList();

                var JobCategoryList = _context.JobCategoryInfo.Select(p => new IdLabelIsDefault
                {
                    id = Convert.ToInt32(p.Id),
                    label = p.Name,
                    isDefault = p.IsDefault

                }).ToList();


                if (JobCategoryList.Count > 0)
                {
                    var firstItem = JobCategoryList[0]; // Get the first item
                    JobCategoryList.RemoveAt(0);       // Remove the first item
                    JobCategoryList.Add(firstItem);    // Add it to the end
                }

                var GendersList = _context.Genders.Select(p => new IdLabelDto
                {
                    id = Convert.ToInt32(p.Id),
                    label = p.type

                }).ToList();


                return new ResultDto<CustomerItems>
                {
                    Data = new CustomerItems
                    {
                        AgeCategoryList = AgeCategoryList,
                        CharacterTypeList = CharacterTypeList,
                        MarketOrientedList = MarketOrientedList,
                        JobCategoryList = JobCategoryList,
                        acquaintanceList = acquaintanceList,
                        Genders = GendersList


                    },
                    IsSuccess = true,
                    Message = "دریافت موفق"

                };



            }
            catch (Exception ex)
            {


                return new ResultDto<CustomerItems>
                {

                    IsSuccess = false,
                    Message = "خطا در دریافت"

                };

            }

        }








        public ResultDto<CustomerItemsEdit> GetForEdit(long CustomerId, long branchCode)
        {
            try
            {

                var customersWithBranchFactor = _context.MainFactors
                        .FirstOrDefault(f => f.BranchCode == branchCode && f.CustomerID == CustomerId);
                // Using HashSet for fast lookup
                if (customersWithBranchFactor == null)
                {
                    return new ResultDto<CustomerItemsEdit>
                    {
                        IsSuccess = false,
                        Message = "شما دسترسی به این مشتری ندارید"
                    };
                }
                var customer = _context.CustomerInfo.Where(p => p.CustomerID == CustomerId)
                    .FirstOrDefault();
                if (customer == null)
                {
                    return new ResultDto<CustomerItemsEdit>
                    {
                        IsSuccess = false,
                        Message = "آیتمی موجود نیست"
                    };
                }


                var Gender = _context.Genders.Where(p => p.Id == customer.Gender).Select(p => new IdLabelDto
                {
                    id = Convert.ToInt32(p.Id),
                    label = p.type

                }).FirstOrDefault();
                var JobCategory = _context.JobCategoryInfo.Where(p => p.Id == customer.JobCategory).Select(p => new IdLabelIsDefault
                {
                    id = Convert.ToInt32(p.Id),
                    label = p.Name,
                    isDefault = p.IsDefault,

                }).FirstOrDefault();

                var AgeCategory = _context.AgeCategories.Where(p => p.Id == customer.AgeCategory).Select(p => new IdLabelDto
                {
                    id = Convert.ToInt32(p.Id),
                    label = p.category

                }).FirstOrDefault();

                var CharacterType = _context.PersonalityCharacterType.Where(p => p.Id == customer.CharacterType).Select(p => new IdLabelDto
                {
                    id = Convert.ToInt32(p.Id),
                    label = p.Type

                }).FirstOrDefault();

                var acquaintance = _context.acquaintances.Where(p => p.Id == customer.acquaintance).Select(p => new IdLabelDto
                {
                    id = Convert.ToInt32(p.Id),
                    label = p.type

                }).FirstOrDefault();

                var MarketOriented = _context.marketOrients.Where(p => p.Id == customer.MarketOriented).Select(p => new IdLabelDto
                {
                    id = Convert.ToInt32(p.Id),
                    label = p.type

                }).FirstOrDefault();


                var Result = new CustomerItemsEdit
                {
                    Name = customer.Name,
                    LastName = customer.LastName,
                    Gender = Gender,
                    JobCategory = JobCategory,
                    //Birthday = customer.Birtday,
                    AgeCategory = AgeCategory,
                    CharacterType = CharacterType,
                    acquaintance = acquaintance,
                    MarketOriented = MarketOriented,
                    Country = new IdLabelDto
                    {
                        id = customer.Country ?? 0,
                        label = _context.Cities.FirstOrDefault(c => c.CountryId == customer.Country)?.Country ?? null

                    },

                    Province = new IdLabelDto
                    {
                        id = customer.Province ?? 0,
                        label = _context.Cities.FirstOrDefault(c => c.ProvinceId == customer.Province)?.province ?? null
                    },

                    city = new IdLabelDto
                    {
                        id = customer.city ?? 0,
                        label = _context.Cities.FirstOrDefault(c => c.Id == customer.city)?.city ?? null
                    },

                    CustomerId = customer.CustomerID,
                    phone = customer.phone,
                    Address = customer.Address,
                    Latitude = customer.Longitude,
                    Longitude = customer.Latitude,
                    CharacterTypeDetails = new CharacterTypesNumbers
                    {
                        D = customer.CharacterTypeDetails?.D ?? 0,
                        I = customer.CharacterTypeDetails?.I ?? 0,
                        S = customer.CharacterTypeDetails?.S ?? 0,
                        C = customer.CharacterTypeDetails?.C ?? 0,
                    }

                };
                if (customer.Birtday != null) { Result.Birthday = SimpleMethods.DateTimeToTimeStamp(customer.Birtday ?? DateTime.Now); };

                return new ResultDto<CustomerItemsEdit>
                {
                    Data = Result,
                    IsSuccess = true,
                    Message = "دریافت موفق"

                };
            }
            catch (Exception ex)
            {
                return new ResultDto<CustomerItemsEdit>
                {

                    IsSuccess = false,
                    Message = " خطا در دریافت"

                };
            }


        }





        public ResultDto<List<CustomerAbstractData>> GetBySearch(string search, long branchCode)
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    return new ResultDto<List<CustomerAbstractData>>
                    {

                        IsSuccess = false,
                        Message = "خطا در دریافت"

                    };
                }

                var CustomerList = new List<CustomerAbstractData>();
                if (!string.IsNullOrWhiteSpace(search))
                {
                    // Fetch customers and join cities directly in one query
                    //CustomerList = (from customer in _context.CustomerInfo
                    //                join city in _context.Cities
                    //                on customer.city equals city.Id into cityGroup
                    //                from city in cityGroup.DefaultIfEmpty() // Left join to handle nulls
                    //                where customer.phone.Contains(search) ||
                    //                      customer.Name.Contains(search) ||
                    //                      customer.LastName.Contains(search)
                    //                select new CustomerAbstractData
                    //                {
                    //                    Id = customer.Id,
                    //                    Name = $"{customer.Name} {customer.LastName}",
                    //                    phone = customer.phone,
                    //                    city = city != null ? city.city : null // Handle null cities
                    //                })
                    //                .ToList();




                    var customersWithBranchFactor = _context.MainFactors
                        .Where(f => f.BranchCode == branchCode)
                        .Select(f => f.CustomerID)
                        .Distinct()
                        .ToHashSet(); // Using HashSet for fast lookup

                    // Query the customer list
                    CustomerList = (from customer in _context.CustomerInfo
                                    join city in _context.Cities
                                    on customer.city equals city.Id into cityGroup
                                    from city in cityGroup.DefaultIfEmpty() // Left join to handle nulls
                                    where customer.phone.Contains(search) ||
                                          customer.Name.Contains(search) ||
                                          customer.LastName.Contains(search)
                                    select new CustomerAbstractData
                                    {
                                        Id = customer.CustomerID,
                                        Name = $"{customer.Name} {customer.LastName}",
                                        phone = customersWithBranchFactor.Contains(customer.CustomerID)
                                            ? customer.phone // Show full phone number if customer has a factor in branch
                                            : MaskingNumber(customer.phone), // Mask phone otherwise
                                        city = city != null ? city.city : null // Handle null cities
                                    })
                                    .ToList();
                }





                return new ResultDto<List<CustomerAbstractData>>
                {
                    Data = CustomerList,
                    IsSuccess = true,
                    Message = "دریافت موفق"

                };


            }
            catch (Exception ex)
            {


                return new ResultDto<List<CustomerAbstractData>>
                {

                    IsSuccess = false,
                    Message = "خطا در  برنامه"

                };

            }

        }


        public async Task<ResultDto<CustomersData>> BranchCustomersAsync(string userEmail, int pageNumber, int pageSize, string search)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return new ResultDto<CustomersData> { IsSuccess = false, Message = "کاربر یافت نشد" };
            }

            var query = _context.CustomerInfo.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.phone.Contains(search) || p.LastName.Contains(search) || p.Name.Contains(search));
            }

            int count = await query.CountAsync();
            int remainder = count % pageSize;

            int pageCount = 0;

            if (remainder > 0)
            {
                pageCount = (count / pageSize) + 1;
            }
            else
            {
                pageCount = count / pageSize;
            }
            int skip = (pageNumber - 1) * pageSize;

            var customersWithBranchFactor = _context.MainFactors
                .Where(f => f.BranchCode == user.BranchCode)
                .Select(f => f.CustomerID)
                .Distinct()
                .ToHashSet();

            var customers = await query
                .Skip(skip)
                .Take(pageSize)
                .Select(p => new CustomersListItems
                {
                    Id = p.CustomerID ?? 0,
                    Name = p.Name,
                    LastName = p.LastName,
                    Phone = customersWithBranchFactor.Contains(p.CustomerID) ? p.phone : MaskingNumber(p.phone),
                    City = _context.Cities.Where(q => q.Id == p.city).Select(q => q.city).FirstOrDefault(),
                    Province = _context.Cities.Where(q => q.ProvinceId == p.Province).Select(q => q.province).FirstOrDefault()
                })
                .ToListAsync();

            var result = new CustomersData
            {
                Customers = customers,
                PageCount = pageCount,
                Count = count
            };

            return new ResultDto<CustomersData> { Data = result, IsSuccess = true, Message = "دریافت موفق" };
        }

        private static string MaskingNumber(string phonenumber)
        {
            if (phonenumber.Length == 11)
            {
                var maskednumber = phonenumber.Substring(0, 4) + "XXXX" + phonenumber.Substring(8);
                return maskednumber;
            }
            else { return null; }
        }
    }





   


    public class CustomersData
    {
        public List<CustomersListItems> Customers { get; set; }
        public int PageCount { get; set; }
        public int Count { get; set; }
    }

    public class CustomersListItems
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Province { get; set; }


    }









    public class CustomerAbstractData
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string? phone { get; set; }
        public string? city { get; set; }


    }
    public class CustomerItems
    {
        public List<IdLabelDto> AgeCategoryList { get; set; }
        public List<IdLabelDto> CharacterTypeList { get; set; }
        public List<IdLabelDto> acquaintanceList { get; set; }
        public List<IdLabelDto> MarketOrientedList { get; set; }
        public List<IdLabelIsDefault> JobCategoryList { get; set; }
        public List<IdLabelDto> Genders { get; set; }


    }
    public class CustomerItemsEdit
    {
        public long? CustomerId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public IdLabelDto Gender { set; get; }
        public IdLabelIsDefault JobCategory { set; get; }
        public long? Birthday { get; set; }
        public IdLabelDto AgeCategory { set; get; }
        public IdLabelDto CharacterType { set; get; }
        public IdLabelDto acquaintance { set; get; }
        public IdLabelDto MarketOriented { set; get; }
        public IdLabelDto Country { set; get; }
        public IdLabelDto Province { get; set; }//Province
        public IdLabelDto city { get; set; }//Province
        public string? phone { get; set; }
        public string? Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public CharacterTypesNumbers CharacterTypeDetails { set; get; }
    }
    public class CharacterTypesNumbers
    {
        public float D { set; get; }
        public float I { set; get; }
        public float S { set; get; }
        public float C { set; get; }
    }


}

