//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Factors.Commands.Pyment.PaymentInternalClasses;
//using Radin.Application.Services.Operations.Check;
//using Radin.Application.Services.SMS.Commands;
//using Radin.Common.Dto;
//using Radin.Common.HesabfaItems;
//using Radin.Common.StaticClass;
//using Radin.Domain.Entities.Factors;
//using Radin.Domain.Entities.Users;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http.Json;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;
//using static Radin.Application.Services.Factors.Commands.Pyment.PaymentInternalClasses.SavingHesabfaItem;

//namespace Radin.Application.Services.Factors.Commands.Pyment
//{
//    public interface IPaymentService
//    {
//        Task<ResultDto> Payment(PymentRequestService request, HttpClient client);
//        Task<ResultDto> TotalCheckPayment(NonCashRequestService request, HttpClient client);
//        Task<ResultDto> AcessoryServiceAddItems(HttpClient client,long branchCode);

//    }
//    public class PaymentService: IPaymentService
//    {
//        private readonly IDataBaseContext _context;
//        private readonly IBanckCheckService _CheckService;
//        private static readonly HttpClient client = new HttpClient();

//        public PaymentService(IDataBaseContext context,
//            IBanckCheckService CheckService
//            )
//        {
//            _context = context;
//            _CheckService = CheckService;
//        }


//        public async Task<ResultDto> AcessoryServiceAddItems(HttpClient client, long branchCode)
//        {
//            try
//            {
//                var Acessories=_context.Accessories.ToList();
//                var Services=_context.Services.ToList();
//                var branch = _context.BranchINFOs.First(p => p.BranchCode == branchCode);
//                if (branch == null) {
//                    return new ResultDto
//                    {
//                        Message = "چنین شعبه ای وجود ندارد",
//                        IsSuccess = false
//                    };
//                }
//                if (string.IsNullOrEmpty(branch.apiKey) || string.IsNullOrEmpty(branch.loginToken))
//                {
//                    return new ResultDto
//                    {
//                        Message = "حسابداری متصل نیست",
//                        IsSuccess = false
//                    };
//                }
//                string url1 = Environment.GetEnvironmentVariable("HESABFA_ITEM_BATCH_SAVE"); SavingHesabfaItem itemSaver = new SavingHesabfaItem();

//                var BranchProducts = new List<HesabfaItemDto>();
//                foreach (var product in Acessories)
//                {
//                    BranchProducts.Add(new HesabfaItemDto
//                    {
//                        code = product.Id.ToString(),
//                        //code = $"A{product.Id.ToString()}",
//                        name = product.Name,
//                        itemType = 0,///// کالا 0.....خدمات 1
//                        sellPrice = product.fee * 10 ,//product.fee*10 اگر نیاز به تبدیل تومان به ریا ل باشد
//                        buyPrice = product.purchaseFee*10
//                    });
//                }
//                foreach (var product in Services)
//                {
//                    BranchProducts.Add(new HesabfaItemDto
//                    {
//                        code = product.Id.ToString(),
//                        //code = $"A{product.Id.ToString()}",
//                        name = product.ServiceName,
//                        itemType = 0,///// کالا 0.....خدمات 1
//                        //sellPrice = product,//product.fee*10 اگر نیاز به تبدیل تومان به ریا ل باشد
//                        //buyPrice = product.fee
//                    });
//                }
//                var Input1 = new HesabfaItemSave
//                {
//                    apiKey = branch.apiKey,
//                    userId = branch.HesabfaUserId,
//                    password = branch.HesabfaPass,
//                    loginToken = branch.loginToken,

//                    items = BranchProducts
//                };
//                var RadinProducts = BranchProducts.Select(item => new HesabfaItemDto
//                {
//                    code = item.code,
//                    name = item.name,
//                    itemType = 0,///// کالا 0.....خدمات 1
//                    sellPrice = item.buyPrice
//                }).ToList();

//                var Input2 = new HesabfaItemSave
//                {
//                    apiKey = "snoTL1UnT7KBb6LYTGjztaQhpVIVTVOX",
//                    userId = "09101050112",
//                    password = "hossein50112",
//                    loginToken = "836677609bf81efb0004e70a3041af884a799d2115cde9d17a28c8707662c269fbb7240531bbf2f937dac8bad3933ec4",
//                    items = RadinProducts
//                };
//                //if (branch.BranchCode != 7792)
//                //{
//                //    Input2.apiKey = "aL89NbQsJqckxBBjFVdKVO0n6rR0bta3";
//                //    Input2.loginToken = "a803e6769d3ccbdfa9075fd0d0babe1642778bd046716cfe98d78a59103ec043d2d51062d8e3a74c9e57488326a40a6c";

//                //}
//                var Result = new PreparedProductsResult
//                {
//                    RadinPreparedProduct = Input2,
//                    BranchPreparedProduct = Input1,
//                };
//                var RadinItemsSavedResult = await itemSaver.SaveItem(Result.RadinPreparedProduct, client, url1);
//                var BranchItemsSavedResult = await itemSaver.SaveItem(Result.BranchPreparedProduct, client, url1);
//                if(!RadinItemsSavedResult.IsSuccess || !BranchItemsSavedResult.IsSuccess)
//                {
//                    return new ResultDto
//                    {
//                        Message = "ثبت ناموفق",
//                        IsSuccess = false
//                    };
//                }
//                return new ResultDto
//                {
//                    Message = "ثبت موفق",
//                    IsSuccess = true
//                };
//            }
//            catch  
//            {
//                return new ResultDto
//                {
//                    IsSuccess = false,
//                    Message = "!خطا"
//                };

//            }
//        }


//            public async Task<ResultDto> Payment(PymentRequestService request, HttpClient client)
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
//                if(!request.IsCash) { factor.PaymentType = 1; }
//                if (factor == null) { return new ResultDto { Message = "چنین فاکتوری وجود ندارد", IsSuccess = false }; }
//                if (request.CustomerId == null) { return new ResultDto { Message = "اطلاعات مشتری ثبت نشده است", IsSuccess = false }; }
//                factor.CustomerID = request.CustomerId;
//                _context.SaveChanges();
//                ////////////////////////////////////////////////////////////////////////////////////

//                float CashOrChecDiscount = 0;

//                var Customer = _context.CustomerInfo.FirstOrDefault(c => c.CustomerID == factor.CustomerID);
//                var branch = _context.BranchINFOs.FirstOrDefault(b => b.BranchCode == factor.BranchCode);
//                if (branch==null) { return new ResultDto { IsSuccess = false, Message = "چنین شعبه ای وجود ندارد", }; }
//                if(branch.loginToken==null || branch.apiKey == null) { return new ResultDto { IsSuccess = false, Message = "به حسابداری متصل نیستید", }; }
//                long SubFactorId = _context.SubFactors.FirstOrDefault(f => f.FactorID == request.FactorId & f.status == true).Id;
//                var ProductList = _context.ProductFactors.Where(f => f.FactorID == request.FactorId & f.SubFactorID == SubFactorId && !f.IsRemoved).ToList();
//                if (request.IsCash) { CashOrChecDiscount = branch.BranchDiscount; };

//                if (ProductList.Count == 0) { return new ResultDto { IsSuccess = false, Message = "محصولی وجود ندارد", }; }
//                if (Customer == null) { return new ResultDto { IsSuccess = false, Message = "مشتری یافت شند", }; }
//                if (factor.CustomerID == null) { return new ResultDto { IsSuccess = false, Message = "اطلاعات مشتری ثبت نشده است" }; }

//                float? factorPrice = factor.TotalAmount;
//                if (request.IsCash)//// دریافت درصد تخفیف در صورت نقدی بودن خرید
//                {
//                    factorPrice = factorPrice * (100 - branch.BranchDiscount) * 0.01f;
//                } 
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
//                    if (string.IsNullOrEmpty(request.ReceiptImage)) { return new ResultDto { IsSuccess = false,
//                        Message = "رسید پرداخت بارگذاری شود",
//                    }; }
//                }


//                ////////////////////////////////////////////////////////////////////////////////////


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

//                ////////////////////////////////////////////////////////////////////////////////////

//                if (!request.IsCash && !request.FinancialAgreement)
//                {
//                    var factorInitialPrice = factor.fee * factor.count;

//                    var remaining = factorInitialPrice - float.Parse(request.amount);
//                    var CheckPrice = request.RecievedCheckInfos.Sum(item => item.amount);
//                    if ((remaining - CheckPrice) > 1000 && (remaining - CheckPrice) < -1000)
//                    {
//                        return new ResultDto
//                        {
//                            Message = "مجموع مبالغ وارد شده اشتباه است",
//                            IsSuccess = false
//                        };


//                    }
//                }

//                ////////////////////////////////////////////////////////////////////////////////////

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
//                if (!request.IsCash)
//                {

//                    report.CheckPayments = new List<CheckPayment>(); // Initialize the list
//                    if (request.RecievedCheckInfos != null && request.RecievedCheckInfos.Any())
//                    {
//                        foreach (var checkInfo in request.RecievedCheckInfos)
//                        {
//                            var checkPayment = new CheckPayment
//                            {
//                                CheckDueDate = SimpleMethods.InsertDateTime(checkInfo.date),
//                                CheckPrice = (float)checkInfo.amount,
//                                PaymentId = report.Id,
//                                CheckImage = checkInfo.CheckImage,
//                            };

//                            report.CheckPayments.Add(checkPayment); // Add the check to the PaymentReport
//                            _context.PaymentReports.Update(report);
//                            _context.CheckPayments.UpdateRange(report.CheckPayments);
//                            await _context.SaveChangesAsync();
//                        }
//                    }


//                }
//                ////////////////////////////////////////////////////////////////////////////////////

//                factor.status = true;
//                factor.state = 3;
//                factor.FinantialAgrrement = request.FinancialAgreement;
//                //Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  1");
//                await _context.SaveChangesAsync();
//                ////////////////////////////////////////////////////////////////////////////////////
//                var MainProducts = ProductList.Where(p =>   !p.IsAccessory && !p.IsService && !p.IsRemoved);
//                var AcessoryList = ProductList.Where(p => !p.IsUndefinedProduct && p.IsAccessory && !p.IsRemoved);
//                var ServiceList= ProductList.Where(p =>  p.IsService && !p.IsRemoved);
//                SavingHesabfaItem itemSaver = new SavingHesabfaItem();
//                ////////////////////////////////////////////////                                        آماده سازی کالا برای ارسال به شعبه و رادین
//                PreparedProductsResult PreparedProducts = itemSaver.PrepareProductsForSaving(MainProducts, branch);
//                string url1 = Environment.GetEnvironmentVariable("HESABFA_ITEM_BATCH_SAVE");
//                ////////////////////////////////////////////////////////////////////                ارسال کالا  به شعبه و رادین و دریافت نتیجه آن
//                var RadinItemsSavedResult =await itemSaver.SaveItem(PreparedProducts.RadinPreparedProduct,client, url1);
//                var BranchItemsSavedResult = await itemSaver.SaveItem(PreparedProducts.BranchPreparedProduct, client, url1);
//                if (!BranchItemsSavedResult.IsSuccess||!RadinItemsSavedResult.IsSuccess)
//                {
//                    if (!RadinItemsSavedResult.IsSuccess) {return new ResultDto { Message = RadinItemsSavedResult.Message, IsSuccess = false };}

//                    else { return new ResultDto { Message = BranchItemsSavedResult.Message, IsSuccess = false }; }
//                }
//                ////////////////////////////////////////////////////////////////////           آماده سازی  مشتری برای ذخیره در شعبه و رادین
//                //Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  2");

//                PreparedContactsResult PreparedContacts = itemSaver.PrepareContactsForSaving(branch, Customer);
//                string url2 = Environment.GetEnvironmentVariable("HESABFA_CONTACT_SAVE");
//                var RadinContactSavedResult = await itemSaver.SaveContact(PreparedContacts.RadinPreparedContact, client, url2);
//                var BranchContactSavedResult = await itemSaver.SaveContact(PreparedContacts.BranchPreparedContact, client, url2);
//                if (!BranchContactSavedResult.IsSuccess || !RadinContactSavedResult.IsSuccess)
//                {
//                    if (!RadinContactSavedResult.IsSuccess) { return new ResultDto { Message = RadinContactSavedResult.Message, IsSuccess = false }; }

//                    else { return new ResultDto { Message = BranchContactSavedResult.Message, IsSuccess = false }; }
//                }
//                ////////////////////////////////////////////////                                        آماده سازی  سرویس ها و محصولات جانبی برای ذخیره در فاکتورهای  شعبه و رادین
//                List<HesabfaInvoiceItem> PreparedProductsForRadin = itemSaver.PrepareProductForFactor(MainProducts, RadinItemsSavedResult.Data, CashOrChecDiscount, 2,0);///  محصولات رادین
//                List<HesabfaInvoiceItem> PreparedProductsForBranchSelling = itemSaver.PrepareProductForFactor(MainProducts, BranchItemsSavedResult.Data, CashOrChecDiscount, 1,0);/// محصولات شعبه هنگام فروش به مشتری
//                List<HesabfaInvoiceItem> PreparedProductsForBranchBuying = itemSaver.PrepareProductForFactor(MainProducts, BranchItemsSavedResult.Data, CashOrChecDiscount, 2,0);/// محصولات شعبه هنگام خرید از رادین
//                List<HesabfaInvoiceItem> PreparedAcessoriesForRadin = itemSaver.PrepareAcessoryForFactor(AcessoryList, CashOrChecDiscount, 2,0);/// جانبی های رادین
//                List<HesabfaInvoiceItem> PreparedAcessoriesForBranchSelling = itemSaver.PrepareAcessoryForFactor(AcessoryList, CashOrChecDiscount, 1,0);/// جانبی های شعبه هنگام فروش به مشتری
//                List<HesabfaInvoiceItem> PreparedAcessoriesForBranchBuying = itemSaver.PrepareAcessoryForFactor(AcessoryList, CashOrChecDiscount, 2,0);///  جانبی های شعبه هنگام خرید از رادین
//                List<HesabfaInvoiceItem> PreparedServicesForBranch = itemSaver.PrepareServiceForFactor(ServiceList, CashOrChecDiscount,0);//// خدمات شعبه
//                //Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  3");

//                List<HesabfaInvoiceItem> RadinItemsList=new List<HesabfaInvoiceItem>();// مجموعه محصولات فروش رادین به شعبه
//                RadinItemsList.AddRange(PreparedAcessoriesForRadin);
//                RadinItemsList.AddRange(PreparedProductsForRadin);
//                List<HesabfaInvoiceItem> BranchItemsListSelling =new List<HesabfaInvoiceItem>();// مجموعه محصولات فروش شعبه به مشتری
//                BranchItemsListSelling.AddRange(PreparedProductsForBranchSelling);
//                BranchItemsListSelling.AddRange(PreparedAcessoriesForBranchSelling);
//                BranchItemsListSelling.AddRange(PreparedServicesForBranch);
//                List<HesabfaInvoiceItem> BranchItemsListBuying = new List<HesabfaInvoiceItem>();// مجموعه محصولات خرید شعبه از رادین
//                BranchItemsListBuying.AddRange(PreparedProductsForBranchBuying);
//                BranchItemsListBuying.AddRange(PreparedAcessoriesForBranchBuying);
               
//                /////////////////////////////////////////////////////
//                ///
//                var RadinApiKey = "snoTL1UnT7KBb6LYTGjztaQhpVIVTVOX";
//                var RadinUserId = "09101050112";
//                var RadinPassword = "H123456789";
//                var RadinLoginToken = "836677609bf81efb0004e70a3041af884a799d2115cde9d17a28c8707662c269fbb7240531bbf2f937dac8bad3933ec4";
//                string RadinContactCode = "000001";
//                string customerId = BranchContactSavedResult.Data.Result.Code;
//                if (branch.BranchCode != 7792)
//                {
//                    RadinApiKey = "aL89NbQsJqckxBBjFVdKVO0n6rR0bta3";
//                    RadinLoginToken = "a803e6769d3ccbdfa9075fd0d0babe1642778bd046716cfe98d78a59103ec043d2d51062d8e3a74c9e57488326a40a6c";

//                }

//                var Input5 = itemSaver.PrepareFactor(BranchItemsListSelling, factor, 0, customerId, branch.apiKey, branch.HesabfaUserId, branch.HesabfaPass, branch.loginToken);
//                var Input6 = itemSaver.PrepareFactor(BranchItemsListBuying, factor, 1, RadinContactCode, branch.apiKey, branch.HesabfaUserId, branch.HesabfaPass, branch.loginToken);
//                var Input7 = itemSaver.PrepareFactor(RadinItemsList, factor, 0, factor.BranchCode.ToString(), RadinApiKey, RadinUserId, RadinPassword, RadinLoginToken);
//                string url3 = Environment.GetEnvironmentVariable("HESABFA_SAVE_FACTOR");
//                HttpResponseMessage response5 = await client.PostAsJsonAsync(url3, Input5);
//                HttpResponseMessage response6 = await client.PostAsJsonAsync(url3, Input6);
//                HttpResponseMessage response7 = await client.PostAsJsonAsync(url3, Input7);
//                if (!response5.IsSuccessStatusCode) { return new ResultDto { IsSuccess = false, Message = "فاکتور فروش در سامانه حسابداری شعبه ثبت نشد", }; }
//                if (!response6.IsSuccessStatusCode) { return new ResultDto { IsSuccess = false, Message = "فاکتور خرید در سامانه حسابداری شعبه ثبت نشد" }; }
//                if (!response6.IsSuccessStatusCode) { return new ResultDto { IsSuccess = false, Message =  "فاکتور خرید در سامانه حسابداری رادین ثبت نشد"  }; }

//                string FactorBranchSell = await response5.Content.ReadAsStringAsync();
//                string FactorBranchBuy = await response5.Content.ReadAsStringAsync();
//                string FactorRadinSell = await response5.Content.ReadAsStringAsync();
//                var HesabfaApiResponse1 = JsonSerializer.Deserialize<CashPymentResponse>(FactorBranchSell);
//                var HesabfaApiResponse2 = JsonSerializer.Deserialize<CashPymentResponse>(FactorBranchBuy);
//                var HesabfaApiResponse3 = JsonSerializer.Deserialize<CashPymentResponse>(FactorRadinSell);
//                if (!HesabfaApiResponse1.Success) { return new ResultDto { IsSuccess = false, Message = $"خطای فاکتور خرید شعبه        {HesabfaApiResponse1.ErrorCode}      {HesabfaApiResponse1.ErrorMessage}" }; }
//                if (!HesabfaApiResponse2.Success) { return new ResultDto { IsSuccess = false, Message = $"خطای فاکتور فروش شعبه        {HesabfaApiResponse2.ErrorCode}        {HesabfaApiResponse2.ErrorMessage} " }; }
//                if (!HesabfaApiResponse3.Success) { return new ResultDto { IsSuccess = false, Message = $"خطای فاکتور فروش رادین        {HesabfaApiResponse3.ErrorCode}        {HesabfaApiResponse3.ErrorMessage}" }; }



//                /////////////////////////////////////////////////////////////////
//                ///
//                string url = Environment.GetEnvironmentVariable("HESABFA_SAVE_PYMENT");
//                HttpResponseMessage Paymentresponse = new HttpResponseMessage();
                

//                if (float.Parse(request.amount) == 0)
//                {
//                    return new ResultDto { IsSuccess = false, Message = "مبلغ پرداخت صفر است" }; }

                    
//                 if (float.Parse(request.amount) > 0) {

//                    if (request.bankType == ReceiptType.Bank)
//                    {
//                        Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 11");
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
//                        Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 22");

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
//                        Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 33");

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



//                    if (!Paymentresponse.IsSuccessStatusCode) { return new ResultDto { IsSuccess = false, Message = $"۱اطلاعات پرداخت در سیستم حسابداری ثبت نشد    {Paymentresponse.Content}" }; }
                    
//                    string responsePymentSave = await Paymentresponse.Content.ReadAsStringAsync();

//                    var HesabfaApiResponseSavePyment = JsonSerializer.Deserialize<CashPymentResponse>(responsePymentSave);

//                    if (!HesabfaApiResponseSavePyment.Success) { return new ResultDto { IsSuccess = false, Message = $"!اطلاعات پرداخت در سیستم حسابداری ثبت نشد۲    {HesabfaApiResponseSavePyment.ErrorCode}       {HesabfaApiResponseSavePyment.ErrorMessage}" }; }
                   
//                }

//                if (!request.IsCash)
//                {

//                    string url0 = Environment.GetEnvironmentVariable("HESABFA_SAVE_PYMENT_CHECK");


//                    var transactionCheck = new List<CheckDto>();
//                    float CheckAmount = 0;

//                    foreach (var obj in request.RecievedCheckInfos)
//                    {
//                        ///////////////////////////////////////////////////////////////////////////                                                              اعداد چک ها رند شدند، بعدا کل فرآیند مالی باید رند شود.



//                        decimal roundedAmount = (decimal)RoundAmount(obj.amount); // Centralized rounding method
//                        CheckAmount += (float)roundedAmount;

//                        //CheckAmount = CheckAmount + (float)Math.Round(obj.amount, 0, MidpointRounding.AwayFromZero);
//                        var CheckDtoObj = new CheckDto
//                        {
//                            Check = new Check
//                            {
//                                number = obj.number,
//                                date = SimpleMethods.InsertDateTime(obj.date).ToString(),
//                                amount = (float)roundedAmount * 10,//(float)Math.Round(obj.amount, 0, MidpointRounding.AwayFromZero) * 10
//                                bankName = "نامشخص",
//                                payerCode = Customer.Id
//                            }
//                        };




//                        ////float roundedAmount = (obj.amount);
//                        ////CheckAmount += roundedAmount;
//                        //CheckAmount = CheckAmount + (float)Math.Round(obj.amount, 0, MidpointRounding.AwayFromZero);
//                        //var CheckDtoObj = new CheckDto
//                        //{
//                        //    Check = new Check
//                        //    {
//                        //        number = obj.number,
//                        //        date = SimpleMethods.InsertDateTime(obj.date).ToString(),
//                        //        amount = (float)Math.Round(obj.amount, 0, MidpointRounding.AwayFromZero)*10,
                           
//                        //        bankName = request.bankName,
//                        //        payerCode = Customer.Id
//                        //    }
//                        //};
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
//                        description = $" دریافت چک از {Customer.Name} {Customer.LastName} بابت {factor.WorkName}",
//                        type = 1,
//                        items = ItemsCheck,
//                        transactions = transactionCheck
//                    };
                 
//                    HttpResponseMessage response = await client.PostAsJsonAsync(url0, Input0);

//                    if (!response.IsSuccessStatusCode) { return new ResultDto { IsSuccess = false, Message = "اطلاعات چک در سیستم حسابداری حاسبفا ثبت نشد ورودی مشکل دارد" }; }
//                    string responsePymentSave = await response.Content.ReadAsStringAsync();
//                    var HesabfaApiResponseSavePyment = JsonSerializer.Deserialize<CheckPymentResponse>(responsePymentSave);
//                    if (!HesabfaApiResponseSavePyment.Success) { return new ResultDto { IsSuccess = false, Message = "! ثبت ناموفق چک در سیستم حسابداری" }; }



//                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  6");


//                }
//                return new ResultDto
//                {

//                    IsSuccess = true,
//                    Message = " فاکتورها ، کالاها و پرداخت‌ها به درستی ثبت شدند"
//                };

//            }
//            catch (Exception ex)
//            {
//                return new ResultDto
//                {
//                    IsSuccess = false,
//                    Message = "!اطلاعات پرداخت در سیستم حسابداری با مشکل مواجه شد"
//                };
//            }
//        }


//        float RoundAmount(float amount) => (float)Math.Round(amount, 0, MidpointRounding.AwayFromZero);

//        private async Task<ResultDto<bool>> IsHolidayAsync(DateTime date
//       )
//            {

//                // Format the URL based on the provided date
//                string url = $"https://holidayapi.ir/gregorian/{date.Year}/{date.Month:D2}/{date.Day:D2}";

//                try
//                {
//                    // Send GET request to the API
//                    HttpResponseMessage response = await client.GetAsync(url);

//                    // Ensure successful response
//                    response.EnsureSuccessStatusCode();

//                    // Read and parse the JSON response
//                    string jsonResponse = await response.Content.ReadAsStringAsync();
//                    using JsonDocument doc = JsonDocument.Parse(jsonResponse);

//                    // Extract `is_holiday` field from the JSON
//                    bool isHoliday = doc.RootElement.GetProperty("is_holiday").GetBoolean();

//                    return new ResultDto<bool>
//                    {
//                        Data = isHoliday,
//                        IsSuccess = true,
//                    };
//                }
//                catch (Exception ex)
//                {
//                    //Console.WriteLine($"Error checking holiday: {ex.Message}");
//                    return null;
//                }
//            }















//        public async Task<ResultDto> TotalCheckPayment(NonCashRequestService request, HttpClient client)
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
//                factor.PaymentType = 2;
//                _context.SaveChanges();
//                //////////////////////////////////////////
//                var Customer = _context.CustomerInfo.FirstOrDefault(c => c.CustomerID == factor.CustomerID);
//                var branch = _context.BranchINFOs.FirstOrDefault(b => b.BranchCode == factor.BranchCode);
//                if (branch == null) { return new ResultDto { IsSuccess = false, Message = "چنین شعبه ای وجود ندارد", }; }
//                if (branch.loginToken == null || branch.apiKey == null) { return new ResultDto { IsSuccess = false, Message = "به حسابداری متصل نیستید", }; }
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
//                float? factorPrice = factor.TotalAmount * (100 + branch.NonCashAddingPayment) / 100;//?????????????????????????????????????????????????????????
                


//                if (request.RecievedCheckInfos.Any(item => string.IsNullOrWhiteSpace(item.CheckImage)))
//                {
//                    return new ResultDto
//                    {
//                        IsSuccess = false,
//                        Message = "تصویر چک نمی‌تواند خالی باشد"
//                    };
//                }

//                var CheckValidation = _CheckService.AverageDueDateValidation(new CheckRequestDto
//                {
//                    MaxPaymentMonth = 6,
//                    PurchaseDate = DateTime.Now,
//                    CheckItems = request.RecievedCheckInfos.Select(p => new CheckItem
//                    {
//                        Amount = p.amount,
//                        DueDate = SimpleMethods.InsertDateTime(p.date)


//                    }).ToList(),

//                });
//                if (!CheckValidation.IsSuccess && !request.FinancialAgreement)
//                {
//                    return new ResultDto
//                    {
//                        Message = CheckValidation.Message,
//                        IsSuccess = false
//                    };
//                }


//                //}


//                var CheckPrice = request.RecievedCheckInfos.Sum(item => item.amount);
//                if (((factorPrice - CheckPrice) > 1000 && !request.FinancialAgreement) || (factorPrice - CheckPrice) < -1000)
//                {
//                    return new ResultDto
//                    {
//                        Message = "مجموع مبالغ وارد شده اشتباه است",
//                        IsSuccess = false
//                    };


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
//                        CashInitialPayment = 0,
//                        TotalPrice = (float)Math.Floor(Convert.ToDouble(factorPrice)),
//                        IsCash = false,
//                        IsFinancialAgreement = false
//                    };


//                    _context.PaymentReports.Add(report); // Add the new PaymentReport to the context
//                    await _context.SaveChangesAsync(); // Save changes to generate the ID

//                }
//                else
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
//                }


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
//                        _context.PaymentReports.Update(report);
//                        _context.CheckPayments.UpdateRange(report.CheckPayments);
//                        await _context.SaveChangesAsync();

//                    }
//                }



//                // Save changes to the database
//                factor.status = true;
//                factor.state = 3;
//                factor.FinantialAgrrement = request.FinancialAgreement;

//                await _context.SaveChangesAsync();
//                float CashOrChecDiscount = 0;
//                float BranchTotalCheckAdd= branch.NonCashAddingPayment;

//                var MainProducts = ProductList.Where(p => !p.IsAccessory && !p.IsService && !p.IsRemoved);
//                var AcessoryList = ProductList.Where(p => !p.IsUndefinedProduct && p.IsAccessory && !p.IsRemoved);
//                var ServiceList = ProductList.Where(p => p.IsService && !p.IsRemoved);
//                SavingHesabfaItem itemSaver = new SavingHesabfaItem();
//                ////////////////////////////////////////////////                                        آماده سازی کالا برای ارسال به شعبه و رادین
//                PreparedProductsResult PreparedProducts = itemSaver.PrepareProductsForSaving(MainProducts, branch);
//                string url1 = Environment.GetEnvironmentVariable("HESABFA_ITEM_BATCH_SAVE");
//                ////////////////////////////////////////////////////////////////////                ارسال کالا  به شعبه و رادین و دریافت نتیجه آن
//                var RadinItemsSavedResult = await itemSaver.SaveItem(PreparedProducts.RadinPreparedProduct, client, url1);
//                var BranchItemsSavedResult = await itemSaver.SaveItem(PreparedProducts.BranchPreparedProduct, client, url1);
//                if (!BranchItemsSavedResult.IsSuccess || !RadinItemsSavedResult.IsSuccess)
//                {
//                    if (!RadinItemsSavedResult.IsSuccess) { return new ResultDto { Message = RadinItemsSavedResult.Message, IsSuccess = false }; }

//                    else { return new ResultDto { Message = BranchItemsSavedResult.Message, IsSuccess = false }; }
//                }
//                ////////////////////////////////////////////////////////////////////           آماده سازی  مشتری برای ذخیره در شعبه و رادین
//                //Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  2");

//                PreparedContactsResult PreparedContacts = itemSaver.PrepareContactsForSaving(branch, Customer);
//                string url2 = Environment.GetEnvironmentVariable("HESABFA_CONTACT_SAVE");
//                var RadinContactSavedResult = await itemSaver.SaveContact(PreparedContacts.RadinPreparedContact, client, url2);
//                var BranchContactSavedResult = await itemSaver.SaveContact(PreparedContacts.BranchPreparedContact, client, url2);
//                if (!BranchContactSavedResult.IsSuccess || !RadinContactSavedResult.IsSuccess)
//                {
//                    if (!RadinContactSavedResult.IsSuccess) { return new ResultDto { Message = RadinContactSavedResult.Message, IsSuccess = false }; }

//                    else { return new ResultDto { Message = BranchContactSavedResult.Message, IsSuccess = false }; }
//                }
//                ////////////////////////////////////////////////                                        آماده سازی  سرویس ها و محصولات جانبی برای ذخیره در فاکتورهای  شعبه و رادین
//                List<HesabfaInvoiceItem> PreparedProductsForRadin = itemSaver.PrepareProductForFactor(MainProducts, RadinItemsSavedResult.Data, CashOrChecDiscount, 2, BranchTotalCheckAdd);///  محصولات رادین
//                List<HesabfaInvoiceItem> PreparedProductsForBranchSelling = itemSaver.PrepareProductForFactor(MainProducts, BranchItemsSavedResult.Data, CashOrChecDiscount, 3, BranchTotalCheckAdd);/// محصولات شعبه هنگام فروش به مشتری
//                List<HesabfaInvoiceItem> PreparedProductsForBranchBuying = itemSaver.PrepareProductForFactor(MainProducts, BranchItemsSavedResult.Data, CashOrChecDiscount, 2, BranchTotalCheckAdd);/// محصولات شعبه هنگام خرید از رادین
//                List<HesabfaInvoiceItem> PreparedAcessoriesForRadin = itemSaver.PrepareAcessoryForFactor(AcessoryList, CashOrChecDiscount, 2, BranchTotalCheckAdd);/// جانبی های رادین
//                List<HesabfaInvoiceItem> PreparedAcessoriesForBranchSelling = itemSaver.PrepareAcessoryForFactor(AcessoryList, CashOrChecDiscount, 3, BranchTotalCheckAdd);/// جانبی های شعبه هنگام فروش به مشتری
//                List<HesabfaInvoiceItem> PreparedAcessoriesForBranchBuying = itemSaver.PrepareAcessoryForFactor(AcessoryList, CashOrChecDiscount, 2, BranchTotalCheckAdd);///  جانبی های شعبه هنگام خرید از رادین
//                List<HesabfaInvoiceItem> PreparedServicesForBranch = itemSaver.PrepareServiceForFactor(ServiceList, CashOrChecDiscount, BranchTotalCheckAdd);//// خدمات شعبه
//                //Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  3");

//                List<HesabfaInvoiceItem> RadinItemsList = new List<HesabfaInvoiceItem>();// مجموعه محصولات فروش رادین به شعبه
//                RadinItemsList.AddRange(PreparedAcessoriesForRadin);
//                RadinItemsList.AddRange(PreparedProductsForRadin);
//                List<HesabfaInvoiceItem> BranchItemsListSelling = new List<HesabfaInvoiceItem>();// مجموعه محصولات فروش شعبه به مشتری
//                BranchItemsListSelling.AddRange(PreparedProductsForBranchSelling);
//                BranchItemsListSelling.AddRange(PreparedAcessoriesForBranchSelling);
//                BranchItemsListSelling.AddRange(PreparedServicesForBranch);
//                List<HesabfaInvoiceItem> BranchItemsListBuying = new List<HesabfaInvoiceItem>();// مجموعه محصولات خرید شعبه از رادین
//                BranchItemsListBuying.AddRange(PreparedProductsForBranchBuying);
//                BranchItemsListBuying.AddRange(PreparedAcessoriesForBranchBuying);

//                /////////////////////////////////////////////////////
//                ///
//                var RadinApiKey = "snoTL1UnT7KBb6LYTGjztaQhpVIVTVOX";
//                var RadinUserId = "09101050112";
//                var RadinPassword = "H123456789";
//                var RadinLoginToken = "836677609bf81efb0004e70a3041af884a799d2115cde9d17a28c8707662c269fbb7240531bbf2f937dac8bad3933ec4";
//                string RadinContactCode = "000001";
//                string customerId = BranchContactSavedResult.Data.Result.Code;

//                if (branch.BranchCode != 7792)
//                {
//                    RadinApiKey = "aL89NbQsJqckxBBjFVdKVO0n6rR0bta3";
//                    RadinLoginToken = "a803e6769d3ccbdfa9075fd0d0babe1642778bd046716cfe98d78a59103ec043d2d51062d8e3a74c9e57488326a40a6c";

//                }
//                var Input5 = itemSaver.PrepareFactor(BranchItemsListSelling, factor, 0, customerId, branch.apiKey, branch.HesabfaUserId, branch.HesabfaPass, branch.loginToken);
//                var Input6 = itemSaver.PrepareFactor(BranchItemsListBuying, factor, 1, RadinContactCode, branch.apiKey, branch.HesabfaUserId, branch.HesabfaPass, branch.loginToken);
//                var Input7 = itemSaver.PrepareFactor(RadinItemsList, factor, 0, factor.BranchCode.ToString(), RadinApiKey, RadinUserId, RadinPassword, RadinLoginToken);
//                string url3 = Environment.GetEnvironmentVariable("HESABFA_SAVE_FACTOR");
//                HttpResponseMessage response5 = await client.PostAsJsonAsync(url3, Input5);
//                HttpResponseMessage response6 = await client.PostAsJsonAsync(url3, Input6);
//                HttpResponseMessage response7 = await client.PostAsJsonAsync(url3, Input7);
//                if (!response5.IsSuccessStatusCode) { return new ResultDto { IsSuccess = false, Message = "فاکتور فروش در سامانه حسابداری شعبه ثبت نشد", }; }
//                if (!response6.IsSuccessStatusCode) { return new ResultDto { IsSuccess = false, Message = "فاکتور خرید در سامانه حسابداری شعبه ثبت نشد" }; }
//                if (!response6.IsSuccessStatusCode) { return new ResultDto { IsSuccess = false, Message = "فاکتور خرید در سامانه حسابداری رادین ثبت نشد" }; }

//                string FactorBranchSell = await response5.Content.ReadAsStringAsync();
//                string FactorBranchBuy = await response5.Content.ReadAsStringAsync();
//                string FactorRadinSell = await response5.Content.ReadAsStringAsync();
//                var HesabfaApiResponse1 = JsonSerializer.Deserialize<CashPymentResponse>(FactorBranchSell);
//                var HesabfaApiResponse2 = JsonSerializer.Deserialize<CashPymentResponse>(FactorBranchBuy);
//                var HesabfaApiResponse3 = JsonSerializer.Deserialize<CashPymentResponse>(FactorRadinSell);
//                if (!HesabfaApiResponse1.Success) { return new ResultDto { IsSuccess = false, Message = $"خطای فاکتور خرید شعبه        {HesabfaApiResponse1.ErrorCode}      {HesabfaApiResponse1.ErrorMessage}" }; }
//                if (!HesabfaApiResponse2.Success) { return new ResultDto { IsSuccess = false, Message = $"خطای فاکتور فروش شعبه        {HesabfaApiResponse2.ErrorCode}        {HesabfaApiResponse2.ErrorMessage} " }; }
//                if (!HesabfaApiResponse3.Success) { return new ResultDto { IsSuccess = false, Message = $"خطای فاکتور فروش رادین        {HesabfaApiResponse3.ErrorCode}        {HesabfaApiResponse3.ErrorMessage}" }; }


//                string url0 = Environment.GetEnvironmentVariable("HESABFA_SAVE_PYMENT_CHECK");


//                var transactionCheck = new List<CheckDto>();
//                float CheckAmount = 0;

//                foreach (var obj in request.RecievedCheckInfos)
//                {
//                    ///////////////////////////////////////////////////////////////////////////                                                              اعداد چک ها رند شدند، بعدا کل فرآیند مالی باید رند شود.


//                    decimal roundedAmount = (decimal) RoundAmount(obj.amount); // Centralized rounding method
//                    CheckAmount += (float)roundedAmount;

//                    //CheckAmount = CheckAmount + (float)Math.Round(obj.amount, 0, MidpointRounding.AwayFromZero);
//                    var CheckDtoObj = new CheckDto
//                    {
//                        Check = new Check
//                        {
//                            number = obj.number,
//                            date = SimpleMethods.InsertDateTime(obj.date).ToString(),
//                            amount = (float)roundedAmount * 10,//(float)Math.Round(obj.amount, 0, MidpointRounding.AwayFromZero) * 10
//                            bankName = "نامشخص",
//                            payerCode = Customer.Id
//                        }
//                    };
//                    //Console.WriteLine($"TransactionAmount =      {CheckDtoObj.Check.amount}");
//                    transactionCheck.Add(CheckDtoObj);

//                }
                
//                var ItemsCheck = new List<ItemObject>();
//                ItemsCheck.Add(new ItemObject
//                {
//                    contactCode = customerId,
//                    amount = transactionCheck.Sum(o => o.Check.amount),//CheckAmount
//                });
//                //Console.WriteLine($"ItemAmount =      {ItemsCheck[0].amount}");

//                var Input0 = new CheckPymentRequestDto
//                {
//                    apiKey = branch.apiKey,
//                    userId = branch.HesabfaUserId,
//                    password = branch.HesabfaPass,
//                    loginToken = branch.loginToken,
//                    description = $" دریافت چک از {Customer.Name} {Customer.LastName} بابت {factor.WorkName }",
//                    type = 1,
//                    items = ItemsCheck,
//                    transactions = transactionCheck
//                };

//                HttpResponseMessage response = await client.PostAsJsonAsync(url0, Input0);

//                if (!response.IsSuccessStatusCode) { return new ResultDto { IsSuccess = false, Message = "اطلاعات چک در سیستم حسابداری حاسبفا ثبت نشد ورودی مشکل دارد" }; }
//                string responsePymentSave = await response.Content.ReadAsStringAsync();
//                var HesabfaApiResponseSavePyment = JsonSerializer.Deserialize<CheckPymentResponse>(responsePymentSave);
//                if (!HesabfaApiResponseSavePyment.Success) { return new ResultDto { IsSuccess = false, Message = $"! ثبت ناموفق چک در سیستم حسابداری                 {HesabfaApiResponseSavePyment.ErrorMessage}" }; }



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




















//    }

//}

