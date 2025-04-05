//using CsvHelper;
//using Microsoft.EntityFrameworkCore;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Branch.Commands.BranchInfoSetService;
//using Radin.Application.Services.Claims.Queries;
//using Radin.Application.Services.Factors.Commands.RecordProduct;
//using Radin.Application.Services.Operations.Check;
//using Radin.Common.Dto;
//using Radin.Common.HesabfaItems;
//using Radin.Common.StaticClass;
//using Radin.Domain.Entities.Branches;
//using Radin.Domain.Entities.Factors;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Net.Http.Json;
//using System.Numerics;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Factors.Commands.Pyment
//{
//    public interface ICashPymentSaveService
//    {
//        Task<ResultDto> Execute(PymentRequestService request, HttpClient client);
//        Task<ResultDto> ExecuteCheck(CheckPymentRequestService request, HttpClient client);
//        Task<ResultDto> NonCashPayment(NonCashRequestService request, HttpClient client);

//    }

//    public class CashPymentSaveService : ICashPymentSaveService
//    {
//        private readonly IDataBaseContext _context;
//        private readonly IBanckCheckService _CheckService;
//        private static readonly HttpClient client = new HttpClient();

//        public CashPymentSaveService(IDataBaseContext context,
//            IBanckCheckService CheckService
//            )
//        {
//            _context = context;
//            _CheckService = CheckService;
//        }
//        public async Task<ResultDto> Execute(PymentRequestService request, HttpClient client)
//        {

//            try
//            {
                
//                var factor = _context.MainFactors.FirstOrDefault(f => f.Id == request.FactorId);


                
//                DateTime purhaseTime = SimpleMethods.TimeToTehran(DateTime.Now);
//                factor.LastConnectionTime = purhaseTime;
//                var TatilatResult = IsHolidayAsync(purhaseTime);
//                DayOfWeek dayOfWeek = purhaseTime.DayOfWeek;
//                int month = purhaseTime.Month;
//                int year = purhaseTime.Year;
//                int day = purhaseTime.Day;
//                factor.day = day.ToString();
//                factor.month = month.ToString();
//                factor.year = year.ToString();
//                factor.dayofweek = dayOfWeek.ToString();





//                if (factor == null)
//                {
//                    return new ResultDto
//                    {
//                        Message = "چنین فاکتوری وجود ندارد",
//                        IsSuccess = false
//                    };
//                }
//                float CashOrChecDiscount = 0;
//                //////////////////////////////
//                if (request.CustomerId == null)
//                {
//                    return new ResultDto
//                    {
//                        Message = "اطلاعات مشتری ثبت نشده است",
//                        IsSuccess = false
//                    };
//                }
//                factor.CustomerID = request.CustomerId;
//                _context.SaveChanges();
//                //////////////////////////////////////////
//                var Customer = _context.CustomerInfo.FirstOrDefault(c => c.CustomerID == factor.CustomerID);
//                var branch = _context.BranchINFOs.FirstOrDefault(b => b.BranchCode == factor.BranchCode);
//                long SubFactorId = _context.SubFactors.FirstOrDefault(f => f.FactorID == request.FactorId & f.status == true).Id;
//                var ProductList = _context.ProductFactors.Where(f => f.FactorID == request.FactorId & f.SubFactorID == SubFactorId&& !f.IsRemoved).ToList();
//                if (request.IsCash) { CashOrChecDiscount = branch.BranchDiscount; };

//                if (ProductList.Count == 0)
//                {

//                    return new ResultDto
//                    {
//                        Message = "محصولی وجود ندارد",
//                        IsSuccess = false
//                    };

//                }
//                if (factor.CustomerID == null) {
//                    return new ResultDto
//                    {
//                        Message = "اطلاعات مشتری ثبت نشده است",
//                        IsSuccess = false
//                    };
//                }
//                float? factorPrice = factor.TotalAmount;
//                if (request.IsCash) { 
//                    factorPrice= factorPrice * (100 - branch.BranchDiscount) * 0.01f; } //// دریافت درصد تخفیف در صورت نقدی بودن خرید
//                if (!request.FinancialAgreement)
//                {
//                    if (float.Parse(request.amount) < factorPrice * branch.InitialPayment / 100)
//                    {
//                        return new ResultDto
//                        {
//                            Message = "حداقل مبلغ پرداختی رعایت نشده است",
//                            IsSuccess = false
//                        };

//                    }
//                    if (string.IsNullOrEmpty(request.ReceiptImage))
//                    {
//                        return new ResultDto
//                        {
//                            Message = "رسید پرداخت بارگذاری شود",
//                            IsSuccess = false
//                        };

//                    }



//                }

//                if (!request.IsCash)
//                {
//                    if (request.RecievedCheckInfos.Any(item => string.IsNullOrWhiteSpace(item.CheckImage)))
//                    {
//                        return new ResultDto
//                        {
//                            IsSuccess = false,
//                            Message = "تصویر چک نمی‌تواند خالی باشد"
//                        };
//                    }

//                    var CheckValidation = _CheckService.AverageDueDateValidation(new CheckRequestDto
//                    {
//                        MaxPaymentMonth = 6,
//                        PurchaseDate = DateTime.Now,
//                        CheckItems = request.RecievedCheckInfos.Select(p => new CheckItem
//                        {
//                            Amount = p.amount,
//                            DueDate = SimpleMethods.InsertDateTime(p.date)


//                        }).ToList(),

//                    });
//                    if (!CheckValidation.IsSuccess && !request.FinancialAgreement)
//                    {
//                        return new ResultDto
//                        {
//                            Message = CheckValidation.Message,
//                            IsSuccess = false
//                        };
//                    }


//                }

//                if (!request.IsCash && !request.FinancialAgreement)
//                {
//                    var factorInitialPrice = factor.fee * factor.count;

//                    var remaining = factorInitialPrice - float.Parse(request.amount);
//                    var CheckPrice = request.RecievedCheckInfos.Sum(item => item.amount);
//                    if((remaining-CheckPrice) > 1000 && (remaining - CheckPrice) <-1000 ) 
//                    {
//                        return new ResultDto
//                        {
//                            Message = "مجموع مبالغ وارد شده اشتباه است",
//                            IsSuccess = false
//                        };


//                    }
//                }




//                var report = await _context.PaymentReports
//                         .Include(r => r.CheckPayments) // Include related CheckPayments
//                         .FirstOrDefaultAsync(p => p.FactorId == request.FactorId);

//                if (report == null)
//                {
//                    // Create a new PaymentReport if none exists
//                    report = new PaymentReport
//                    {
//                        FactorId = request.FactorId,
//                        PaymentReceiptImage = request.ReceiptImage,
//                        CashInitialPayment = float.Parse(request.amount),
//                        TotalPrice = (float)factorPrice,
//                        IsCash = request.IsCash,
//                        IsFinancialAgreement = request.FinancialAgreement
//                    };

//                    _context.PaymentReports.Add(report); // Add the new PaymentReport to the context
//                    await _context.SaveChangesAsync();
//                }
//                else
//                {
//                    // Update the existing PaymentReport
//                    report.FactorId = request.FactorId;
//                    report.PaymentReceiptImage = request.ReceiptImage;
//                    report.CashInitialPayment = float.Parse(request.amount);
//                    report.TotalPrice = (float)factorPrice;
//                    report.IsCash = request.IsCash;
//                    report.IsFinancialAgreement = request.FinancialAgreement;

//                    // Remove related CheckPayments
//                    _context.CheckPayments.RemoveRange(report.CheckPayments);
//                    _context.PaymentReports.Update(report);
//                    await _context.SaveChangesAsync();
//                }
//                if (!request.IsCash )
//                {
                   
//                    report.CheckPayments = new List<CheckPayment>(); // Initialize the list
//                    if (request.RecievedCheckInfos != null && request.RecievedCheckInfos.Any())
//                        {
//                            foreach (var checkInfo in request.RecievedCheckInfos)
//                            {
//                                var checkPayment = new CheckPayment
//                                {
//                                    CheckDueDate = SimpleMethods.InsertDateTime(checkInfo.date),
//                                    CheckPrice = (float)checkInfo.amount,
//                                    PaymentId = report.Id,
//                                    CheckImage = checkInfo.CheckImage,
//                                };

//                            report.CheckPayments.Add(checkPayment); // Add the check to the PaymentReport
//                            _context.PaymentReports.Update(report);
//                            _context.CheckPayments.UpdateRange(report.CheckPayments);
//                            await _context.SaveChangesAsync();
//                        }
//                        }
               

//                }
//                // Save changes to the database
//                factor.status = true;
//                factor.state = 3;
//                factor.FinantialAgrrement = request.FinancialAgreement;

//                await _context.SaveChangesAsync();
//                //if (1 == 1)
//                //{
//                //    return new ResultDto
//                //    {
//                //        Message = "ثبت موفق",
//                //        IsSuccess = true
//                //    };

//                //}
//                /////////////////////////////////////////                      اضافه کردن محصولات

//                var MainProducts = ProductList.Where(p => !p.IsUndefinedProduct && !p.IsAccessory&& !p.IsRemoved);
//                var AcessoryList= ProductList.Where(p => !p.IsUndefinedProduct && p.IsAccessory && !p.IsRemoved);
//                var UndefinedProductList= ProductList.Where(p => p.IsUndefinedProduct && !p.IsRemoved);

//                var BranchMainProducts = new List<HesabfaItemDto>();
//                var BranchAcessoryList = new List<HesabfaItemDto>();
//                var BranchUndefinedProductList = new List<HesabfaItemDto>();

//                foreach (var product in MainProducts)
//                {
//                    BranchMainProducts.Add(new HesabfaItemDto
//                    {
//                        code = product.Id.ToString(),
//                        //code = $"A{product.Id.ToString()}",
//                        name = product.Name,
//                        itemType = 0,///// کالا 0.....خدمات 1
//                        sellPrice = product.fee*10,//product.fee*10 اگر نیاز به تبدیل تومان به ریا ل باشد
//                        buyPrice = product.fee * 10 / 2
//                    });
//                }
//                //foreach (var product in AcessoryList)
//                //{
//                //    BranchAcessoryList.Add(new HesabfaItemDto
//                //    {
//                //        code = product.Id.ToString(),
//                //        //code = $"A{product.Id.ToString()}",
//                //        name = product.Name,
//                //        itemType = 0,///// کالا 0.....خدمات 1
//                //        sellPrice = product.fee* 10,//product.fee*10 اگر نیاز به تبدیل تومان به ریا ل باشد
//                //        buyPrice = product.PurchaseFee * 10 
//                //    });
//                //}
//                /////////////////////////////////////////                      ثبت کالا برای حسابفا شعبه
//                var Input1 = new HesabfaItemSave
//                {
//                    apiKey = branch.apiKey,
//                    userId = branch.HesabfaUserId,
//                    password = branch.HesabfaPass,
//                    loginToken = branch.loginToken,

//                    items = BranchMainProducts
//                };
//                /////////////////////////////////////////                      ثبت کالا برای حسابفا رادین
                
//                var RadinMainProducts =  BranchMainProducts.Select(item => new HesabfaItemDto
//                {
//                    code = item.code,
//                    name = item.name,
//                    itemType = 1,///// کالا 0.....خدمات 1
//                    sellPrice = item.buyPrice
//                }).ToList();
//                var Input2 = new HesabfaItemSave
//                {
//                    apiKey = "snoTL1UnT7KBb6LYTGjztaQhpVIVTVOX",
//                    userId = "09101050112",
//                    password = "hossein50112",
//                    loginToken = "836677609bf81efb0004e70a3041af884a799d2115cde9d17a28c8707662c269fbb7240531bbf2f937dac8bad3933ec4",
//                    items = RadinMainProducts
//                };
                 
//                string url1 = Environment.GetEnvironmentVariable("HESABFA_ITEM_BATCH_SAVE");
//                HttpResponseMessage response1 = await client.PostAsJsonAsync(url1, Input1);
//                HttpResponseMessage response2 = await client.PostAsJsonAsync(url1, Input2);
//                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  1");
//                if (!response1.IsSuccessStatusCode)
//                {
//                    return new ResultDto
//                    {
//                        Message = "اطلاعات کالاها در سیستم حسابداری شعبه ثبت نشد",
//                        IsSuccess = false
//                    };
//                }
//                if (!response2.IsSuccessStatusCode)
//                {
//                    return new ResultDto
//                    {
//                        Message = "اطلاعات کالاها در سیستم حسابداری رادین ثبت نشد",
//                        IsSuccess = false
//                    };
//                }
//                /////////////////////////////////////////                      دریافت نتیجه ذخیره سازی کالاها

//                string responseItemsSave = await response1.Content.ReadAsStringAsync();
//                var HesabfaApiResponseSaveItems = JsonSerializer.Deserialize<HesabfaItemResponse>(responseItemsSave);

//                if (Customer == null)
//                {
//                    return new ResultDto
//                    {
//                        Message = "مشتری با چنین مشخصاتی وجود ندارد",
//                        IsSuccess = false
//                    };
//                }
//                string fullName = Customer.Name + " " + Customer.LastName;
//                /////////////////////////////////////////                      اطلاعات مشتری برای شعبه

//                var Input3 = new HesabfaSaveContactDto
//                {
//                    apiKey = branch.apiKey,
//                    userId = branch.HesabfaUserId,
//                    password = branch.HesabfaPass,
//                    loginToken = branch.loginToken,

//                    contact = new ContactInfo
//                    {
//                        code = Customer.CustomerID.ToString(),
//                        name = fullName,
//                        firstName = Customer.Name,
//                        lastName = Customer.LastName,
//                        contactType = 1,//نامشخص 0.....حقیقی 1....حقوقی 2 
//                        mobile = Customer.phone,
//                    }
//                };
//                /////////////////////////////////////////                      اطلاعات مشتری برای رادین
//                var Input4 = new HesabfaSaveContactDto
//                {
//                    apiKey = "snoTL1UnT7KBb6LYTGjztaQhpVIVTVOX",
//                    userId = "09101050112",
//                    password = "hossein50112",
//                    loginToken = "836677609bf81efb0004e70a3041af884a799d2115cde9d17a28c8707662c269fbb7240531bbf2f937dac8bad3933ec4",
//                    contact = new ContactInfo
//                    {
//                        code = branch.BranchCode.ToString(),
//                        name = branch.BranchName,
//                        firstName = "",
//                        lastName = "",
//                        contactType = 1,//نامشخص 0.....حقیقی 1....حقوقی 2
//                        mobile = branch.BranchPhone1,
//                    }
//                };

//                string url2 = Environment.GetEnvironmentVariable("HESABFA_CONTACT_SAVE");
//                HttpResponseMessage response3 = await client.PostAsJsonAsync(url2, Input3);
//                HttpResponseMessage response4 = await client.PostAsJsonAsync(url2, Input4);
//                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  2");

//                if (!response3.IsSuccessStatusCode)
//                {
//                    return new ResultDto
//                    {
//                        Message = "اطلاعات مشتری در سیستم حسابداری شعبه ثبت نشد",
//                        IsSuccess = false
//                    };
//                }
//                if (!response4.IsSuccessStatusCode)
//                {
//                    return new ResultDto
//                    {
//                        Message = "اطلاعات مشتری در سیستم حسابداری رادین ثبت نشد",
//                        IsSuccess = false
//                    };
//                }

//                /////////////////////////////////////////                      دریافت نتیجه ذخیره سازی اطلاعات مشتری

//                string responseSaveContact = await response3.Content.ReadAsStringAsync();
//                // Deserialize the Python API's JSON response
//                var HesabfaApiResponseSaveContact = JsonSerializer.Deserialize<HesabfaSaveContactResponse>(responseSaveContact);
//                string customerId = HesabfaApiResponseSaveContact.Result.Code;
//                Console.WriteLine($"     >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>   Customerid=     {customerId}");

//                /////////////////////////////////////////                      ثبت فاکتور خرید و فروش برای شعبه و ثبت فاکتور فروش برای رادین

//                var ItemsListResult = new List<HesabfaInvoiceItem>();
//                foreach (var item in HesabfaApiResponseSaveItems.Result)
//                {
//                    int ProductId = Convert.ToInt32(item.Code);
//                    var matchingProduct = ProductList.FirstOrDefault(p => p.Id == ProductId);
//                    float transformDiscount =  CashOrChecDiscount + (matchingProduct.Discount * (100 - CashOrChecDiscount)) / 100;
//                    ItemsListResult.Add(new HesabfaInvoiceItem
//                    {
//                        itemCode = item.Code,
//                        description = item.Name,
//                        quantity = matchingProduct.count,
//                        unitPrice = item.SellPrice/2,
//                        discount = (transformDiscount)* item.SellPrice/2/100,
//                        tax = 0
//                    });
//                }
//                var SellingItemsListResult = new List<HesabfaInvoiceItem>();
//                foreach (var item in HesabfaApiResponseSaveItems.Result)
//                {
//                    int ProductId = Convert.ToInt32(item.Code);
//                    var matchingProduct = ProductList.FirstOrDefault(p => p.Id == ProductId);
//                    float transformDiscount = CashOrChecDiscount + (matchingProduct.Discount * (100 - CashOrChecDiscount)) / 100;
//                    SellingItemsListResult.Add(new HesabfaInvoiceItem
//                    {
//                        itemCode = item.Code,
//                        description = item.Name,
//                        quantity = matchingProduct.count,
//                        unitPrice = item.SellPrice,
//                        discount = (transformDiscount) * item.SellPrice / 100,
//                        tax = 0
//                    });
//                }


//                var Input5 = new HesabfaFactorSave
//                {
//                    apiKey = branch.apiKey,
//                    userId = branch.HesabfaUserId,
//                    password = branch.HesabfaPass,
//                    loginToken = branch.loginToken,

//                    invoice = new HesabfaInvoiceDto
//                    {
//                        number = factor.Id.ToString(),
//                        date = DateTime.Now.ToString(),
//                        dueDate = DateTime.Now.ToString() ,
//                        contactCode = customerId,//  فروش شعبه به مشتری
//                        contactTitle = factor.WorkName,
//                        invoiceType = 0,///فروش 0.....خرید 1....برگشت از فروش 2 ....برگشت از خرید 3 ....ضایعات 4 
//                        status = 1,// تایید شده 1 ....پیش نویس 0
//                        project = factor.WorkName,
//                        InvoiceItems = SellingItemsListResult
//                    }
//                };

//                var Input6 = new HesabfaFactorSave
//                {
//                    apiKey = branch.apiKey,
//                    userId = branch.HesabfaUserId,
//                    password = branch.HesabfaPass,
//                    loginToken = branch.loginToken,

//                    invoice = new HesabfaInvoiceDto
//                    {
//                        number = factor.Id.ToString(),
//                        date = DateTime.Now.ToString(),
//                        dueDate = DateTime.Now.ToString(),
//                        contactCode = "000001",/// خرید شعبه از رادین
//                        contactTitle = factor.WorkName,
//                        invoiceType = 1,///فروش 0.....خرید 1....برگشت از فروش 2 ....برگشت از خرید 3 ....ضایعات 4
//                        status = 1,// تایید شده 1 ....پیش نویس 0
//                        project = factor.WorkName,
//                        InvoiceItems = ItemsListResult
//                    }
//                };

//                var Input7 = new HesabfaFactorSave
//                {
//                    apiKey = "snoTL1UnT7KBb6LYTGjztaQhpVIVTVOX",
//                    userId = "09101050112",
//                    password = "hossein50112",
//                    loginToken = "836677609bf81efb0004e70a3041af884a799d2115cde9d17a28c8707662c269fbb7240531bbf2f937dac8bad3933ec4",

//                    invoice = new HesabfaInvoiceDto
//                    {
//                        number = factor.Id.ToString(),
//                        date = DateTime.Now.ToString(),
//                        dueDate = DateTime.Now.ToString(),
//                        contactCode = factor.BranchCode.ToString(),// فروش رادین به شعبه
//                        contactTitle = factor.WorkName,
//                        invoiceType = 0,///فروش 0.....خرید 1....برگشت از فروش 2 ....برگشت از خرید 3 ....ضایعات 4
//                        status = 1,// تایید شده 1 ....پیش نویس 0
//                        project = factor.WorkName,
//                        InvoiceItems = ItemsListResult
//                    }
//                };

//                string url3 = Environment.GetEnvironmentVariable("HESABFA_SAVE_FACTOR");
//                HttpResponseMessage response5 = await client.PostAsJsonAsync(url3, Input5);
//                HttpResponseMessage response6 = await client.PostAsJsonAsync(url3, Input6);
//                HttpResponseMessage response7 = await client.PostAsJsonAsync(url3, Input7);
//                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  3");

//                if (!response5.IsSuccessStatusCode)
//                {
//                    return new ResultDto
//                    {
//                        Message = "فاکتور فروش در سامانه حسابداری شعبه ثبت نشد",
//                        IsSuccess = false
//                    };
//                }

                

//                if (!response6.IsSuccessStatusCode)
//                {
//                    return new ResultDto
//                    {
//                        Message = "فاکتور خرید در سامانه حسابداری شعبه ثبت نشد",
//                        IsSuccess = false
//                    };
//                }

//                if (!response6.IsSuccessStatusCode)
//                {
//                    return new ResultDto
//                    {
//                        Message = "فاکتور خرید در سامانه حسابداری رادین ثبت نشد",
//                        IsSuccess = false
//                    };
//                }
//                //return new ResultDto
//                //{
//                //    Message = "فاکتورها ثبت شدند",
//                //    IsSuccess = true
//                //};
//                //////////////////////////////////////////////////////////////////////////////
//                ///

//                string url = Environment.GetEnvironmentVariable("HESABFA_SAVE_PYMENT");
//                HttpResponseMessage Paymentresponse = new HttpResponseMessage();
                

//                if (float.Parse(request.amount) >0) 
                
//                {

//                    if (request.bankType == ReceiptType.Bank)
//                    {
//                        var BankInput = new BankPaymentRequestDto
//                        {
//                            apiKey = branch.apiKey,
//                            userId = branch.HesabfaUserId,
//                            password = branch.HesabfaPass,
//                            loginToken = branch.loginToken,
//                            type = 0,
//                            number = factor.Id,
//                            bankCode = request.bankCode,
//                            //contactCode = Customer.CustomerID.Value,

//                            transactionNumber = "255496387",
//                            description = $" پرداخت توسط {Customer.Name} {Customer.LastName} ",
//                            amount = float.Parse(request.amount) * 10,
//                        };
//                        Paymentresponse = await client.PostAsJsonAsync(url, BankInput);

//                    }
//                    else if (request.bankType == ReceiptType.Cash)
//                    {
//                        var CashInput = new CashPaymentRequestDto
//                        {
//                            apiKey = branch.apiKey,
//                            userId = branch.HesabfaUserId,
//                            password = branch.HesabfaPass,
//                            loginToken = branch.loginToken,
//                            type = 0,
//                            number = factor.Id,
//                            //contactCode = Customer.CustomerID.Value,
//                            cashCode = request.bankCode,
//                            transactionNumber = "255496387",
//                            description = $" پرداخت توسط {Customer.Name} {Customer.LastName} ",
//                            amount = float.Parse(request.amount) * 10,
//                        };
//                        Paymentresponse = await client.PostAsJsonAsync(url, CashInput);

//                    }
//                    else if (request.bankType == ReceiptType.PettyCash)
//                    {
//                        var PettyCashInput = new PettyCashPaymentRequestDto
//                        {
//                            apiKey = branch.apiKey,
//                            userId = branch.HesabfaUserId,
//                            password = branch.HesabfaPass,
//                            loginToken = branch.loginToken,
//                            type = 0,
//                            number = factor.Id,
//                            //contactCode = Customer.CustomerID.Value,
//                            pettyCashCode = request.bankCode,
//                            transactionNumber = "255496387",
//                            description = $" پرداخت توسط {Customer.Name} {Customer.LastName} ",
//                            amount = float.Parse(request.amount) * 10,
//                        };
//                        Paymentresponse = await client.PostAsJsonAsync(url, PettyCashInput);


//                    }
//                    else { return new ResultDto { Message = " نوع پرداخت امکان پذیر نیست", IsSuccess = false }; }

//                    if (!Paymentresponse.IsSuccessStatusCode)
//                    {
//                        return new ResultDto
//                        {
//                            Message = "اطلاعات پرداخت در سیستم حسابداری ثبت نشد",
//                            IsSuccess = false
//                        };
//                    }
//                    string responsePymentSave = await Paymentresponse.Content.ReadAsStringAsync();

//                    var HesabfaApiResponseSavePyment = JsonSerializer.Deserialize<CashPymentResponse>(responsePymentSave);

//                    if (!HesabfaApiResponseSavePyment.Success)
//                    {
//                        return new ResultDto
//                        {

//                            IsSuccess = false,
//                            Message = "!اطلاعات پرداخت در سیستم حسابداری ثبت نشد"
//                        };
//                    }
//                }
//                if (!request.IsCash)
//                {

//                    string url0 = Environment.GetEnvironmentVariable("HESABFA_SAVE_PYMENT_CHECK");


//                    var transactionCheck = new List<CheckDto>();
//                    float CheckAmount = 0;

//                    foreach (var obj in request.RecievedCheckInfos)
//                    {
//                        ///////////////////////////////////////////////////////////////////////////                                                              اعداد چک ها رند شدند، بعدا کل فرآیند مالی باید رند شود.
//                        CheckAmount = CheckAmount + (float)Math.Round(obj.amount, 0, MidpointRounding.AwayFromZero);
//                        var CheckDtoObj = new CheckDto { Check = new Check { number = obj.number, date =SimpleMethods.InsertDateTime(obj.date).ToString(), amount = (float)Math.Round(obj.amount, 0, MidpointRounding.AwayFromZero)
//                            , bankName = request.bankName, payerCode = Customer.Id } };
//                        Console.WriteLine($"AMOUNT=>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> {(float)Math.Round(obj.amount, 0, MidpointRounding.AwayFromZero)}");
//                        transactionCheck.Add(CheckDtoObj);

//                    }

//                    var ItemsCheck = new List<ItemObject>();
//                    ItemsCheck.Add(new ItemObject
//                    {
//                        contactCode = customerId,
//                        amount = transactionCheck.Sum(o => o.Check.amount),//CheckAmount
//                    });

//                    var Input0 = new CheckPymentRequestDto
//                    {
//                        apiKey = branch.apiKey,
//                        userId = branch.HesabfaUserId,
//                        password = branch.HesabfaPass,
//                        loginToken = branch.loginToken,
//                        type = 1,
//                        items = ItemsCheck,
//                        transactions = transactionCheck
//                    };
//                    //Console.WriteLine($"ObjectsAmount=     >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>{ItemsCheck.First().amount}");
//                    //Console.WriteLine($"TransactionsAmount=     >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>{transactionCheck.Sum(o => o.Check.amount)}");

//                    HttpResponseMessage response = await client.PostAsJsonAsync(url0, Input0);

//                    if (!response.IsSuccessStatusCode)
//                    {
//                        return new ResultDto
//                        {
//                            Message = "اطلاعات چک در سیستم حسابداری حاسبفا ثبت نشد ورودی مشکل دارد",
//                            IsSuccess = false
//                        };
//                    }
//                    string responsePymentSave = await response.Content.ReadAsStringAsync();
//                    Console.WriteLine($"result = >>>>>>>>>>>>>>>>>>>>> {responsePymentSave}");
//                    var HesabfaApiResponseSavePyment = JsonSerializer.Deserialize<CheckPymentResponse>(responsePymentSave);
//                    Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  detected= {HesabfaApiResponseSavePyment.ErrorCode}");
//                    if (!HesabfaApiResponseSavePyment.Success)
//                    {
//                        return new ResultDto
//                        {

//                            IsSuccess = false,
//                            Message = "! ثبت ناموفق چک در سیستم حسابداری"
//                        };
//                    }




//                }
//                return new ResultDto
//                {

//                    IsSuccess = true,
//                    Message = " فاکتورها ، کالاها و پرداخت‌ها به درستی ثبت شدند"
//                };


//            }

//            catch (Exception)
//            {
//                return new ResultDto
//                { 
//                    IsSuccess = false,
//                    Message = "!اطلاعات پرداخت در سیستم حسابداری با مشکل مواجه شد"
//                };


//            }

//        }
//        //________________________________________________________________________________________________________//









//        public async Task<ResultDto> NonCashPayment(NonCashRequestService request, HttpClient client)
//        {


//            try
//            {
//                var ItemsList = new List<HesabfaItemDto>();
//                var factor = _context.MainFactors.FirstOrDefault(f => f.Id == request.FactorId);
//                if (factor == null)
//                {
//                    return new ResultDto
//                    {
//                        Message = "چنین فاکتوری وجود ندارد",
//                        IsSuccess = false
//                    };
//                }

//                //////////////////////////////
//                if (request.CustomerId == null)
//                {
//                    return new ResultDto
//                    {
//                        Message = "اطلاعات مشتری ثبت نشده است",
//                        IsSuccess = false
//                    };
//                }

//                DateTime purhaseTime = SimpleMethods.TimeToTehran(DateTime.Now);
//                factor.LastConnectionTime = purhaseTime;
//                var TatilatResult = IsHolidayAsync(purhaseTime);
//                DayOfWeek dayOfWeek = purhaseTime.DayOfWeek;
//                int month = purhaseTime.Month;
//                int year = purhaseTime.Year;
//                int day = purhaseTime.Day;
//                factor.day = day.ToString();
//                factor.month = month.ToString();
//                factor.year = year.ToString();
//                factor.dayofweek = dayOfWeek.ToString();

//                factor.CustomerID = request.CustomerId;

//                _context.SaveChanges();
//                //////////////////////////////////////////
//                var Customer = _context.CustomerInfo.FirstOrDefault(c => c.CustomerID == factor.CustomerID);
//                var branch = _context.BranchINFOs.FirstOrDefault(b => b.BranchCode == factor.BranchCode);
//                long SubFactorId = _context.SubFactors.FirstOrDefault(f => f.FactorID == request.FactorId & f.status == true).Id;
//                var ProductList = _context.ProductFactors.Where(f => f.FactorID == request.FactorId & f.SubFactorID == SubFactorId).ToList();

//                if (ProductList.Count == 0)
//                {

//                    return new ResultDto
//                    {
//                        Message = "محصولی وجود ندارد",
//                        IsSuccess = false
//                    };

//                }
//                if (factor.CustomerID == null)
//                {
//                    return new ResultDto
//                    {
//                        Message = "اطلاعات مشتری ثبت نشده است",
//                        IsSuccess = false
//                    };
//                }
//                float? factorPrice = factor.TotalAmount*(100+branch.NonCashAddingPayment)/100;//?????????????????????????????????????????????????????????
                
                
                
//                    if (request.RecievedCheckInfos.Any(item => string.IsNullOrWhiteSpace(item.CheckImage)))
//                    {
//                        return new ResultDto
//                        {
//                            IsSuccess = false,
//                            Message = "تصویر چک نمی‌تواند خالی باشد"
//                        };
//                    }

//                    var CheckValidation = _CheckService.AverageDueDateValidation(new CheckRequestDto
//                    {
//                        MaxPaymentMonth = 6,
//                        PurchaseDate = DateTime.Now,
//                        CheckItems = request.RecievedCheckInfos.Select(p => new CheckItem
//                        {
//                            Amount = p.amount,
//                            DueDate = SimpleMethods.InsertDateTime(p.date)


//                        }).ToList(),

//                    });
//                    if (!CheckValidation.IsSuccess && !request.FinancialAgreement)
//                    {
//                        return new ResultDto
//                        {
//                            Message = CheckValidation.Message,
//                            IsSuccess = false
//                        };
//                    }


//                //}

                
//                    var CheckPrice = request.RecievedCheckInfos.Sum(item => item.amount);
//                    if (((factorPrice - CheckPrice) > 1000&& !request.FinancialAgreement) || (factorPrice - CheckPrice) < -1000)
//                    {
//                        return new ResultDto
//                        {
//                            Message = "مجموع مبالغ وارد شده اشتباه است",
//                            IsSuccess = false
//                        };


//                    }
                




//                var report = await _context.PaymentReports
//                         .Include(r => r.CheckPayments) // Include related CheckPayments
//                         .FirstOrDefaultAsync(p => p.FactorId == request.FactorId);

//                if (report == null)
//                {
//                    // Create a new PaymentReport if none exists
//                    report = new PaymentReport
//                    {
//                        FactorId = request.FactorId,
//                        CashInitialPayment = 0,
//                        TotalPrice = (float)Math.Floor(Convert.ToDouble(factorPrice)),
//                        IsCash = false,
//                        IsFinancialAgreement = false
//                    };


//                    _context.PaymentReports.Add(report); // Add the new PaymentReport to the context
//                await _context.SaveChangesAsync(); // Save changes to generate the ID

//            }
//            else
//                {
//                    // Update the existing PaymentReport
//                    report.FactorId = request.FactorId;
//                    report.CashInitialPayment = 0;
//                    report.TotalPrice = (float)Math.Floor(Convert.ToDouble(factorPrice));
//                    report.IsCash = false;
//                    report.IsFinancialAgreement = false;

//                    // Remove related CheckPayments
//                    _context.CheckPayments.RemoveRange(report.CheckPayments);
//                    _context.PaymentReports.Update(report);
//                    await _context.SaveChangesAsync();
//            }
                

//                report.CheckPayments = new List<CheckPayment>(); // Initialize the list
//                if (request.RecievedCheckInfos != null && request.RecievedCheckInfos.Any())
//                {
//                    foreach (var checkInfo in request.RecievedCheckInfos)
//                    {
//                        var checkPayment = new CheckPayment
//                        {
//                            CheckDueDate = SimpleMethods.InsertDateTime(checkInfo.date),
//                            CheckPrice = (float)checkInfo.amount,
//                            PaymentId = report.Id,
//                            CheckImage = checkInfo.CheckImage,
//                        };

//                        report.CheckPayments.Add(checkPayment); // Add the check to the PaymentReport
//                    _context.PaymentReports.Update(report);
//                    _context.CheckPayments.UpdateRange(report.CheckPayments);
//                    await _context.SaveChangesAsync();

//                }
//            }


                
//                // Save changes to the database
//                factor.status = true;
//                factor.state = 3;
//                factor.FinantialAgrrement = request.FinancialAgreement;

//                await _context.SaveChangesAsync();
//                //if (1 == 1)
//                //{
//                //    return new ResultDto
//                //    {
//                //        Message = "ثبت موفق",
//                //        IsSuccess = true
//                //    };

//                //}
//                ///////////////////////////////////////////                      اضافه کردن محصولات
//                //foreach (var product in ProductList)
//                //{
//                //    ItemsList.Add(new HesabfaItemDto
//                //    {
//                //        code = product.Id.ToString(),
//                //        //code = $"A{product.Id.ToString()}",
//                //        name = product.Name,
//                //        itemType = 0,///// کالا 0.....خدمات 1
//                //        sellPrice = product.fee * 10//product.fee*10 اگر نیاز به تبدیل تومان به ریا ل باشد
//                //    });
//                //}
//                ////var BranchItemsList = ItemsList.Select(item => new HesabfaItemDto
//                ////{
//                ////    code = item.code,
//                ////    name = item.name,
//                ////    itemType = item.itemType,
//                ////    sellPrice = item.sellPrice * 1.7f
//                ////}).ToList();                /////////////////////////////////////////                      ثبت کالا برای حسابفا شعبه
//                //var Input1 = new HesabfaItemSave
//                //{
//                //    apiKey = branch.apiKey,
//                //    userId = branch.HesabfaUserId,
//                //    password = branch.HesabfaPass,
//                //    loginToken = branch.loginToken,

//                //    items = ItemsList
//                //};
//                ///////////////////////////////////////////                      ثبت کالا برای حسابفا رادین

//                //var Input2 = new HesabfaItemSave
//                //{
//                //    apiKey = "snoTL1UnT7KBb6LYTGjztaQhpVIVTVOX",
//                //    userId = "09101050112",
//                //    password = "hossein50112",
//                //    loginToken = "836677609bf81efb0004e70a3041af884a799d2115cde9d17a28c8707662c269fbb7240531bbf2f937dac8bad3933ec4",
//                //    items = ItemsList
//                //};

//                //string url1 = Environment.GetEnvironmentVariable("HESABFA_ITEM_BATCH_SAVE");
//                //HttpResponseMessage response1 = await client.PostAsJsonAsync(url1, Input1);
//                //HttpResponseMessage response2 = await client.PostAsJsonAsync(url1, Input2);
//                //Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  1");
//                //if (!response1.IsSuccessStatusCode)
//                //{
//                //    return new ResultDto
//                //    {
//                //        Message = "اطلاعات کالاها در سیستم حسابداری شعبه ثبت نشد",
//                //        IsSuccess = false
//                //    };
//                //}
//                //if (!response2.IsSuccessStatusCode)
//                //{
//                //    return new ResultDto
//                //    {
//                //        Message = "اطلاعات کالاها در سیستم حسابداری رادین ثبت نشد",
//                //        IsSuccess = false
//                //    };
//                //}
//                ///////////////////////////////////////////                      دریافت نتیجه ذخیره سازی کالاها

//                //string responseItemsSave = await response1.Content.ReadAsStringAsync();
//                //var HesabfaApiResponseSaveItems = JsonSerializer.Deserialize<HesabfaItemResponse>(responseItemsSave);

//                //if (Customer == null)
//                //{
//                //    return new ResultDto
//                //    {
//                //        Message = "مشتری با چنین مشخصاتی وجود ندارد",
//                //        IsSuccess = false
//                //    };
//                //}
//                //string fullName = Customer.Name + " " + Customer.LastName;
//                ///////////////////////////////////////////                      اطلاعات مشتری برای شعبه

//                //var Input3 = new HesabfaSaveContactDto
//                //{
//                //    apiKey = branch.apiKey,
//                //    userId = branch.HesabfaUserId,
//                //    password = branch.HesabfaPass,
//                //    loginToken = branch.loginToken,

//                //    contact = new ContactInfo
//                //    {
//                //        code = Customer.CustomerID.ToString(),
//                //        name = fullName,
//                //        firstName = Customer.Name,
//                //        lastName = Customer.LastName,
//                //        contactType = 1,//نامشخص 0.....حقیقی 1....حقوقی 2 
//                //        mobile = Customer.phone,
//                //    }
//                //};
//                ///////////////////////////////////////////                      اطلاعات مشتری برای رادین
//                //var Input4 = new HesabfaSaveContactDto
//                //{
//                //    apiKey = "snoTL1UnT7KBb6LYTGjztaQhpVIVTVOX",
//                //    userId = "09101050112",
//                //    password = "hossein50112",
//                //    loginToken = "836677609bf81efb0004e70a3041af884a799d2115cde9d17a28c8707662c269fbb7240531bbf2f937dac8bad3933ec4",
//                //    contact = new ContactInfo
//                //    {
//                //        code = branch.BranchCode.ToString(),
//                //        name = branch.BranchName,
//                //        firstName = "",
//                //        lastName = "",
//                //        contactType = 1,//نامشخص 0.....حقیقی 1....حقوقی 2
//                //        mobile = branch.BranchPhone1,
//                //    }
//                //};

//                //string url2 = Environment.GetEnvironmentVariable("HESABFA_CONTACT_SAVE");
//                //HttpResponseMessage response3 = await client.PostAsJsonAsync(url2, Input3);
//                //HttpResponseMessage response4 = await client.PostAsJsonAsync(url2, Input4);
//                //Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  2");

//                //if (!response3.IsSuccessStatusCode)
//                //{
//                //    return new ResultDto
//                //    {
//                //        Message = "اطلاعات مشتری در سیستم حسابداری شعبه ثبت نشد",
//                //        IsSuccess = false
//                //    };
//                //}
//                //if (!response4.IsSuccessStatusCode)
//                //{
//                //    return new ResultDto
//                //    {
//                //        Message = "اطلاعات مشتری در سیستم حسابداری رادین ثبت نشد",
//                //        IsSuccess = false
//                //    };
//                //}

//                ///////////////////////////////////////////                      دریافت نتیجه ذخیره سازی اطلاعات مشتری

//                //string responseSaveContact = await response3.Content.ReadAsStringAsync();
//                //// Deserialize the Python API's JSON response
//                //var HesabfaApiResponseSaveContact = JsonSerializer.Deserialize<HesabfaSaveContactResponse>(responseSaveContact);
//                //string customerId = HesabfaApiResponseSaveContact.Result.Code;
//                //Console.WriteLine($"     >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>   Customerid=     {customerId}");

//                ///////////////////////////////////////////                      ثبت فاکتور خرید و فروش برای شعبه و ثبت فاکتور فروش برای رادین

//                //var ItemsListResult = new List<HesabfaInvoiceItem>();
//                //foreach (var item in HesabfaApiResponseSaveItems.Result)
//                //{
//                //    int ProductId = Convert.ToInt32(item.Code);
//                //    var matchingProduct = ProductList.FirstOrDefault(p => p.Id == ProductId);
//                //    float transformDiscount = CashOrChecDiscount + (matchingProduct.Discount * (100 - CashOrChecDiscount)) / 100;
//                //    ItemsListResult.Add(new HesabfaInvoiceItem
//                //    {
//                //        itemCode = item.Code,
//                //        description = item.Name,
//                //        quantity = matchingProduct.count,
//                //        unitPrice = item.SellPrice / 2,
//                //        discount = (transformDiscount) * item.SellPrice / 2 / 100,
//                //        tax = 0
//                //    });
//                //}
//                //var SellingItemsListResult = new List<HesabfaInvoiceItem>();
//                //foreach (var item in HesabfaApiResponseSaveItems.Result)
//                //{
//                //    int ProductId = Convert.ToInt32(item.Code);
//                //    var matchingProduct = ProductList.FirstOrDefault(p => p.Id == ProductId);
//                //    float transformDiscount = CashOrChecDiscount + (matchingProduct.Discount * (100 - CashOrChecDiscount)) / 100;
//                //    SellingItemsListResult.Add(new HesabfaInvoiceItem
//                //    {
//                //        itemCode = item.Code,
//                //        description = item.Name,
//                //        quantity = matchingProduct.count,
//                //        unitPrice = item.SellPrice,
//                //        discount = (transformDiscount) * item.SellPrice / 100,
//                //        tax = 0
//                //    });
//                //}


//                //var Input5 = new HesabfaFactorSave
//                //{
//                //    apiKey = branch.apiKey,
//                //    userId = branch.HesabfaUserId,
//                //    password = branch.HesabfaPass,
//                //    loginToken = branch.loginToken,

//                //    invoice = new HesabfaInvoiceDto
//                //    {
//                //        number = factor.Id.ToString(),
//                //        date = DateTime.Now.ToString(),
//                //        dueDate = DateTime.Now.ToString(),
//                //        contactCode = customerId,//  فروش شعبه به مشتری
//                //        contactTitle = factor.WorkName,
//                //        invoiceType = 0,///فروش 0.....خرید 1....برگشت از فروش 2 ....برگشت از خرید 3 ....ضایعات 4 
//                //        status = 1,// تایید شده 1 ....پیش نویس 0
//                //        project = factor.WorkName,
//                //        InvoiceItems = SellingItemsListResult
//                //    }
//                //};

//                //var Input6 = new HesabfaFactorSave
//                //{
//                //    apiKey = branch.apiKey,
//                //    userId = branch.HesabfaUserId,
//                //    password = branch.HesabfaPass,
//                //    loginToken = branch.loginToken,

//                //    invoice = new HesabfaInvoiceDto
//                //    {
//                //        number = factor.Id.ToString(),
//                //        date = DateTime.Now.ToString(),
//                //        dueDate = DateTime.Now.ToString(),
//                //        contactCode = "000001",/// خرید شعبه از رادین
//                //        contactTitle = factor.WorkName,
//                //        invoiceType = 1,///فروش 0.....خرید 1....برگشت از فروش 2 ....برگشت از خرید 3 ....ضایعات 4
//                //        status = 1,// تایید شده 1 ....پیش نویس 0
//                //        project = factor.WorkName,
//                //        InvoiceItems = ItemsListResult
//                //    }
//                //};

//                //var Input7 = new HesabfaFactorSave
//                //{
//                //    apiKey = "snoTL1UnT7KBb6LYTGjztaQhpVIVTVOX",
//                //    userId = "09101050112",
//                //    password = "hossein50112",
//                //    loginToken = "836677609bf81efb0004e70a3041af884a799d2115cde9d17a28c8707662c269fbb7240531bbf2f937dac8bad3933ec4",

//                //    invoice = new HesabfaInvoiceDto
//                //    {
//                //        number = factor.Id.ToString(),
//                //        date = DateTime.Now.ToString(),
//                //        dueDate = DateTime.Now.ToString(),
//                //        contactCode = factor.BranchCode.ToString(),// فروش رادین به شعبه
//                //        contactTitle = factor.WorkName,
//                //        invoiceType = 0,///فروش 0.....خرید 1....برگشت از فروش 2 ....برگشت از خرید 3 ....ضایعات 4
//                //        status = 1,// تایید شده 1 ....پیش نویس 0
//                //        project = factor.WorkName,
//                //        InvoiceItems = ItemsListResult
//                //    }
//                //};

//                //string url3 = Environment.GetEnvironmentVariable("HESABFA_SAVE_FACTOR");
//                //HttpResponseMessage response5 = await client.PostAsJsonAsync(url3, Input5);
//                //HttpResponseMessage response6 = await client.PostAsJsonAsync(url3, Input6);
//                //HttpResponseMessage response7 = await client.PostAsJsonAsync(url3, Input7);
//                //Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  3");

//                //if (!response5.IsSuccessStatusCode)
//                //{
//                //    return new ResultDto
//                //    {
//                //        Message = "فاکتور فروش در سامانه حسابداری شعبه ثبت نشد",
//                //        IsSuccess = false
//                //    };
//                //}



//                //if (!response6.IsSuccessStatusCode)
//                //{
//                //    return new ResultDto
//                //    {
//                //        Message = "فاکتور خرید در سامانه حسابداری شعبه ثبت نشد",
//                //        IsSuccess = false
//                //    };
//                //}

//                //if (!response6.IsSuccessStatusCode)
//                //{
//                //    return new ResultDto
//                //    {
//                //        Message = "فاکتور خرید در سامانه حسابداری رادین ثبت نشد",
//                //        IsSuccess = false
//                //    };
//                //}
//                ////return new ResultDto
//                ////{
//                ////    Message = "فاکتورها ثبت شدند",
//                ////    IsSuccess = true
//                ////};
//                ////////////////////////////////////////////////////////////////////////////////
//                /////

//                //string url = Environment.GetEnvironmentVariable("HESABFA_SAVE_PYMENT");

//                //var Input = new CashPymentRequestDto
//                //{
//                //    apiKey = branch.apiKey,
//                //    userId = branch.HesabfaUserId,
//                //    password = branch.HesabfaPass,
//                //    loginToken = branch.loginToken,
//                //    type = 1,
//                //    bankCode = request.bankCode,
//                //    contactCode = Customer.CustomerID.Value,
//                //    amount = request.amount,
//                //};


//                //if (request.amount > 0)

//                //{
//                //    HttpResponseMessage response = await client.PostAsJsonAsync(url, Input);
//                //    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  4");

//                //    if (!response.IsSuccessStatusCode)
//                //    {
//                //        return new ResultDto
//                //        {
//                //            Message = "اطلاعات پرداخت در سیستم حسابداری ثبت نشد",
//                //            IsSuccess = false
//                //        };
//                //    }
//                //    string responsePymentSave = await response.Content.ReadAsStringAsync();

//                //    var HesabfaApiResponseSavePyment = JsonSerializer.Deserialize<CashPymentResponse>(responsePymentSave);

//                //    if (!HesabfaApiResponseSavePyment.Success)
//                //    {
//                //        return new ResultDto
//                //        {

//                //            IsSuccess = false,
//                //            Message = "!اطلاعات پرداخت در سیستم حسابداری ثبت نشد"
//                //        };
//                //    }
//                //}
//                //if (!request.IsCash)
//                //{

//                //    string url0 = Environment.GetEnvironmentVariable("HESABFA_SAVE_PYMENT_CHECK");


//                //    var transactionCheck = new List<CheckDto>();
//                //    float CheckAmount = 0;

//                //    foreach (var obj in request.RecievedCheckInfos)
//                //    {
//                //        ///////////////////////////////////////////////////////////////////////////                                                              اعداد چک ها رند شدند، بعدا کل فرآیند مالی باید رند شود.
//                //        CheckAmount = CheckAmount + (float)Math.Round(obj.amount, 0, MidpointRounding.AwayFromZero);
//                //        var CheckDtoObj = new CheckDto
//                //        {
//                //            Check = new Check
//                //            {
//                //                number = obj.number,
//                //                date = SimpleMethods.InsertDateTime(obj.date).ToString(),
//                //                amount = (float)Math.Round(obj.amount, 0, MidpointRounding.AwayFromZero)
//                //            ,
//                //                bankName = request.bankName,
//                //                payerCode = Customer.Id
//                //            }
//                //        };
//                //        Console.WriteLine($"AMOUNT=>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> {(float)Math.Round(obj.amount, 0, MidpointRounding.AwayFromZero)}");
//                //        transactionCheck.Add(CheckDtoObj);

//                //    }

//                //    var ItemsCheck = new List<ItemObject>();
//                //    ItemsCheck.Add(new ItemObject
//                //    {
//                //        contactCode = customerId,
//                //        amount = transactionCheck.Sum(o => o.Check.amount),//CheckAmount
//                //    });

//                //    var Input0 = new CheckPymentRequestDto
//                //    {
//                //        apiKey = branch.apiKey,
//                //        userId = branch.HesabfaUserId,
//                //        password = branch.HesabfaPass,
//                //        loginToken = branch.loginToken,
//                //        type = 1,
//                //        items = ItemsCheck,
//                //        transactions = transactionCheck
//                //    };
//                //    //Console.WriteLine($"ObjectsAmount=     >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>{ItemsCheck.First().amount}");
//                //    //Console.WriteLine($"TransactionsAmount=     >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>{transactionCheck.Sum(o => o.Check.amount)}");

//                //    HttpResponseMessage response = await client.PostAsJsonAsync(url0, Input0);

//                //    if (!response.IsSuccessStatusCode)
//                //    {
//                //        return new ResultDto
//                //        {
//                //            Message = "اطلاعات چک در سیستم حسابداری حاسبفا ثبت نشد ورودی مشکل دارد",
//                //            IsSuccess = false
//                //        };
//                //    }
//                //    string responsePymentSave = await response.Content.ReadAsStringAsync();
//                //    Console.WriteLine($"result = >>>>>>>>>>>>>>>>>>>>> {responsePymentSave}");
//                //    var HesabfaApiResponseSavePyment = JsonSerializer.Deserialize<CheckPymentResponse>(responsePymentSave);
//                //    Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  detected= {HesabfaApiResponseSavePyment.ErrorCode}");
//                //    if (!HesabfaApiResponseSavePyment.Success)
//                //    {
//                //        return new ResultDto
//                //        {

//                //            IsSuccess = false,
//                //            Message = "! ثبت ناموفق چک در سیستم حسابداری"
//                //        };
//                //    }




//                //}
//                return new ResultDto
//                {

//                    IsSuccess = true,
//                    Message = " فاکتورها ، کالاها و پرداخت‌ها به درستی ثبت شدند"
//                };


//            }

//            catch (Exception)
//            {
//                return new ResultDto
//                {
//                    IsSuccess = false,
//                    Message = "!اطلاعات پرداخت در سیستم حسابداری با مشکل مواجه شد"
//                };


//            }




//        }
















































//        //________________________________________________________________________________________________________//
//        public async Task<ResultDto> ExecuteCheck(CheckPymentRequestService request, HttpClient client)
//        {

//            try
//            {
//                var ItemsList = new List<HesabfaItemDto>();
//                var factor = _context.MainFactors.FirstOrDefault(f => f.Id == request.FactorId);
//                var Customer = _context.CustomerInfo.FirstOrDefault(c => c.CustomerID == factor.CustomerID);
//                var branch = _context.BranchINFOs.FirstOrDefault(b => b.BranchCode == factor.BranchCode);
//                long SubFactorId = _context.SubFactors.FirstOrDefault(f => f.FactorID == request.FactorId & f.status == true).Id;
//                var ProductList = _context.ProductFactors.Where(f => f.FactorID == request.FactorId & f.SubFactorID == SubFactorId).ToList();

//                if (ProductList.Count == 0)
//                {

//                    return new ResultDto
//                    {
//                        Message = "محصولی وجود ندارد",
//                        IsSuccess = false
//                    };

//                }
//                foreach (var product in ProductList)
//                {
//                    ItemsList.Add(new HesabfaItemDto
//                    {
//                        code = product.Id.ToString(),
//                        name = product.Name,
//                        itemType = 0,
//                        sellPrice = product.fee
//                    });
//                }
//                var Input1 = new HesabfaItemSave
//                {
//                    apiKey = branch.apiKey,
//                    userId = branch.HesabfaUserId,
//                    password = branch.HesabfaPass,
//                    loginToken = branch.loginToken,

//                    items = ItemsList
//                };

//                var Input2 = new HesabfaItemSave
//                {
//                    apiKey = "snoTL1UnT7KBb6LYTGjztaQhpVIVTVOX",
//                    userId = "09101050112",
//                    password = "hossein50112",
//                    loginToken = "836677609bf81efb0004e70a3041af884a799d2115cde9d17a28c8707662c269fbb7240531bbf2f937dac8bad3933ec4",
//                    items = ItemsList
//                };

//                string url1 = Environment.GetEnvironmentVariable("HESABFA_ITEM_BATCH_SAVE");
//                HttpResponseMessage response1 = await client.PostAsJsonAsync(url1, Input1);
//                HttpResponseMessage response2 = await client.PostAsJsonAsync(url1, Input2);

//                if (!response1.IsSuccessStatusCode)
//                {
//                    return new ResultDto
//                    {
//                        Message = "اطلاعات کالاها در سیستم حسابداری شعبه ثبت نشد",
//                        IsSuccess = false
//                    };
//                }

//                string responseItemsSave = await response1.Content.ReadAsStringAsync();
//                var HesabfaApiResponseSaveItems = JsonSerializer.Deserialize<HesabfaItemResponse>(responseItemsSave);

//                if (Customer == null)
//                {
//                    return new ResultDto
//                    {
//                        Message = "مشتری با چنین مشخصاتی وجود ندارد",
//                        IsSuccess = false
//                    };
//                }
//                string fullName = Customer.Name + " " + Customer.LastName;
//                var Input3 = new HesabfaSaveContactDto
//                {
//                    apiKey = branch.apiKey,
//                    userId = branch.HesabfaUserId,
//                    password = branch.HesabfaPass,
//                    loginToken = branch.loginToken,

//                    contact = new ContactInfo
//                    {
//                        code = Customer.CustomerID.ToString(),
//                        name = fullName,
//                        firstName = Customer.Name,
//                        lastName = Customer.LastName,
//                        contactType = 1,
//                        mobile = Customer.phone,
//                    }
//                };

//                var Input4 = new HesabfaSaveContactDto
//                {
//                    apiKey = "snoTL1UnT7KBb6LYTGjztaQhpVIVTVOX",
//                    userId = "09101050112",
//                    password = "hossein50112",
//                    loginToken = "836677609bf81efb0004e70a3041af884a799d2115cde9d17a28c8707662c269fbb7240531bbf2f937dac8bad3933ec4",
//                    contact = new ContactInfo
//                    {
//                        code = branch.BranchCode.ToString(),
//                        name = branch.BranchName,
//                        firstName = "",
//                        lastName = "",
//                        contactType = 1,
//                        mobile = branch.BranchPhone1,
//                    }
//                };

//                string url2 = Environment.GetEnvironmentVariable("HESABFA_CONTACT_SAVE");
//                HttpResponseMessage response3 = await client.PostAsJsonAsync(url2, Input3);
//                HttpResponseMessage response4 = await client.PostAsJsonAsync(url2, Input4);

//                if (!response3.IsSuccessStatusCode)
//                {
//                    return new ResultDto
//                    {
//                        Message = "اطلاعات مشتری در سیستم حسابداری ثبت نشد",
//                        IsSuccess = false
//                    };
//                }
//                string responseSaveContact = await response3.Content.ReadAsStringAsync();

//                // Deserialize the Python API's JSON response
//                var HesabfaApiResponseSaveContact = JsonSerializer.Deserialize<HesabfaSaveContactResponse>(responseSaveContact);
//                string customerId = HesabfaApiResponseSaveContact.Result.Code;

//                var ItemsListResult = new List<HesabfaInvoiceItem>();
//                foreach (var item in HesabfaApiResponseSaveItems.Result)
//                {
//                    int ProductId = Convert.ToInt32(item.Code);
//                    var matchingProduct = ProductList.FirstOrDefault(p => p.Id == ProductId);
//                    float transformDiscount = matchingProduct.Discount + ((branch.BranchDiscount * (100 - matchingProduct.Discount)) / 100);
//                    ItemsListResult.Add(new HesabfaInvoiceItem
//                    {
//                        itemCode = item.Code,
//                        description = item.Name,
//                        quantity = matchingProduct.count,
//                        unitPrice = item.SellPrice,
//                        discount = transformDiscount,
//                        tax = 0
//                    });
//                }
//                var Input5 = new HesabfaFactorSave
//                {
//                    apiKey = branch.apiKey,
//                    userId = branch.HesabfaUserId,
//                    password = branch.HesabfaPass,
//                    loginToken = branch.loginToken,

//                    invoice = new HesabfaInvoiceDto
//                    {
//                        number = factor.Id.ToString(),
//                        date = DateTime.Now.ToString(),
//                        dueDate = DateTime.Now.ToString(),
//                        contactCode = customerId,
//                        contactTitle = factor.WorkName,
//                        invoiceType = 0,
//                        project = factor.WorkName,
//                        InvoiceItems = ItemsListResult
//                    }
//                };

//                var Input6 = new HesabfaFactorSave
//                {
//                    apiKey = branch.apiKey,
//                    userId = branch.HesabfaUserId,
//                    password = branch.HesabfaPass,
//                    loginToken = branch.loginToken,

//                    invoice = new HesabfaInvoiceDto
//                    {
//                        number = factor.Id.ToString(),
//                        date = DateTime.Now.ToString(),
//                        dueDate = DateTime.Now.ToString(),
//                        contactCode = "000001",
//                        contactTitle = factor.WorkName,
//                        invoiceType = 1,
//                        project = factor.WorkName,
//                        InvoiceItems = ItemsListResult
//                    }
//                };

//                var Input7 = new HesabfaFactorSave
//                {
//                    apiKey = "snoTL1UnT7KBb6LYTGjztaQhpVIVTVOX",
//                    userId = "09101050112",
//                    password = "hossein50112",
//                    loginToken = "836677609bf81efb0004e70a3041af884a799d2115cde9d17a28c8707662c269fbb7240531bbf2f937dac8bad3933ec4",

//                    invoice = new HesabfaInvoiceDto
//                    {
//                        number = factor.Id.ToString(),
//                        date = DateTime.Now.ToString(),
//                        dueDate = DateTime.Now.ToString(),
//                        contactCode = factor.BranchCode.ToString(),
//                        contactTitle = factor.WorkName,
//                        invoiceType = 0,
//                        project = factor.WorkName,
//                        InvoiceItems = ItemsListResult
//                    }
//                };

//                string url3 = Environment.GetEnvironmentVariable("HESABFA_SAVE_FACTOR");
//                HttpResponseMessage response5 = await client.PostAsJsonAsync(url3, Input5);
//                HttpResponseMessage response6 = await client.PostAsJsonAsync(url3, Input6);
//                HttpResponseMessage response7 = await client.PostAsJsonAsync(url3, Input7);

//                if (!response5.IsSuccessStatusCode)
//                {
//                    return new ResultDto
//                    {
//                        Message = "فاکتور فروش در سامانه حسابداری ثبت نشد",
//                        IsSuccess = false
//                    };
//                }



//                if (!response6.IsSuccessStatusCode)
//                {
//                    return new ResultDto
//                    {
//                        Message = "فاکتور خرید در سامانه حسابداری ثبت نشد",
//                        IsSuccess = false
//                    };
//                }




//                string url = Environment.GetEnvironmentVariable("HESABFA_SAVE_PYMENT_CHECK");


//                var transactionCheck = new List<CheckDto>();
//                float CheckAmount = 0;

//                foreach (var obj in request.checkList)
//                {
//                    CheckAmount = CheckAmount + obj.amount;
//                    var CheckDtoObj = new CheckDto { Check = new Check { number = obj.number ,date = obj.date,amount = obj.amount,bankName=obj.bankName,payerCode = obj.payerCode} };
//                    transactionCheck.Add(CheckDtoObj);

//                }

//                var ItemsCheck = new List<ItemObject>();
//                ItemsCheck.Add(new ItemObject
//                {
//                    contactCode = customerId,
//                    amount = CheckAmount
//                });

//                var Input = new CheckPymentRequestDto
//                {
//                    apiKey = branch.apiKey,
//                    userId = branch.HesabfaUserId,
//                    password = branch.HesabfaPass,
//                    loginToken = branch.loginToken,
//                    type = 1,
//                    items = ItemsCheck,
//                    transactions = transactionCheck
//                };

//                HttpResponseMessage response = await client.PostAsJsonAsync(url, Input);

//                if (!response.IsSuccessStatusCode)
//                {
//                    return new ResultDto
//                    {
//                        Message = "اطلاعات چک در سیستم حسابداری حاسبفا ثبت نشد ورودی مشکل دارد",
//                        IsSuccess = false
//                    };
//                }
//                string responsePymentSave = await response.Content.ReadAsStringAsync();
//                var HesabfaApiResponseSavePyment = JsonSerializer.Deserialize<CheckPymentResponse>(responsePymentSave);
//                if (HesabfaApiResponseSavePyment.Success)
//                {
//                    return new ResultDto
//                    {
//                        IsSuccess = true,
//                        Message = "اطلاعات چک در سیستم حسابداری با موفقیت ثبت شد",
//                    };
//                }
//                else
//                {
//                    return new ResultDto
//                    {

//                        IsSuccess = false,
//                        Message = "! ریسپانس اطلاعات چک در سیستم حسابداری ثبت نشد"
//                    };

//                }
//            }

//            catch (Exception)
//            {
//                return new ResultDto
//                {
//                    IsSuccess = false,
//                    Message = "!اطلاعات چک در سیستم حسابداری با مشکل مواجه شد"
//                };


//            }

//        }





//        private async Task<ResultDto<bool>> IsHolidayAsync(DateTime date
//       )
//        {

//            // Format the URL based on the provided date
//            string url = $"https://holidayapi.ir/gregorian/{date.Year}/{date.Month:D2}/{date.Day:D2}";

//            try
//            {
//                // Send GET request to the API
//                HttpResponseMessage response = await client.GetAsync(url);

//                // Ensure successful response
//                response.EnsureSuccessStatusCode();

//                // Read and parse the JSON response
//                string jsonResponse = await response.Content.ReadAsStringAsync();
//                using JsonDocument doc = JsonDocument.Parse(jsonResponse);

//                // Extract `is_holiday` field from the JSON
//                bool isHoliday = doc.RootElement.GetProperty("is_holiday").GetBoolean();

//                return new ResultDto<bool>
//                {
//                    Data = isHoliday,
//                    IsSuccess = true,
//                };
//            }
//            catch (Exception ex)
//            {
//                //Console.WriteLine($"Error checking holiday: {ex.Message}");
//                return null;
//            }
//        }

//    }


//    //public class PymentRequestService
//    //{
//    //    public long FactorId { get; set; }
//    //    public int bankCode { get; set; }
//    //    public float amount { get; set; }
//    //}

//    //public class CashPymentRequestDto
//    //{
//    //    public string apiKey { get; set; }
//    //    public string userId { get; set; }
//    //    public string password { get; set; }
//    //    public string loginToken { get; set; }
//    //    public int type { get; set; }
//    //    public int bankCode { get; set; }
//    //    public long contactCode { get; set; }
//    //    public float amount { get; set; }

//    //}

//    //public class CashPymentResponse
//    //{
//    //    public bool Success { get; set; }
//    //    public int ErrorCode { get; set; }
//    //    public string ErrorMessage { get; set; }
//    //}

//    //public class CheckPymentRequestService
//    //{
//    //    public long FactorId { get; set; }

//    //    public List<Check> checkList { get; set; }


//    //}


//    //public class CheckPymentRequestDto
//    //{
//    //    public string apiKey { get; set; }
//    //    public string userId { get; set; }
//    //    public string password { get; set; }
//    //    public string loginToken { get; set; }
//    //    public int type { get; set; }
//    //    public List<ItemObject> items { get; set; }
//    //    public List<CheckDto> transactions { get; set; }
//    //}

//    //public class ItemObject
//    //{
//    //    public string contactCode { get; set; }
//    //    public float amount { get; set; }
//    //}


//    //public class CheckDto
//    //{
//    //    public Check Check { get; set; }
//    //}

//    //public class Check
//    //{
//    //   public int number { get; set; }
//    //   public string date { get; set; }
//    //   public float amount { get; set; }
//    //   public string bankName { get; set; }
//    //    public long payerCode { get; set; }
//    //}

//    //public class CheckPymentResponse
//    //{
//    //    public bool Success { get; set; }
//    //    public int ErrorCode { get; set; }
//    //    public string ErrorMessage { get; set; }
//    //}

//}
