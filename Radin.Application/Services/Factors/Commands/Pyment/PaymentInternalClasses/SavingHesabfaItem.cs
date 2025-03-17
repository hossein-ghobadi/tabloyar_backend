using Radin.Common.Dto;
using Radin.Domain.Entities.Branches;
using Radin.Domain.Entities.Customers;
using Radin.Domain.Entities.Factors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Radin.Domain.Entities.Factors;
using System.Linq.Expressions;

namespace Radin.Application.Services.Factors.Commands.Pyment.PaymentInternalClasses
{
    internal class SavingHesabfaItem
    {


        internal async Task<ResultDto<HesabfaItemResponse>> SaveItem(HesabfaItemSave request, HttpClient client, string url1)
        {
            //string url1 = Environment.GetEnvironmentVariable("HESABFA_ITEM_BATCH_SAVE");
            HttpResponseMessage response1 = await client.PostAsJsonAsync(url1, request);
            if (!response1.IsSuccessStatusCode)
            {
                return new ResultDto<HesabfaItemResponse>
                {
                    Message = "اطلاعات کالاها در سیستم حسابداری شعبه ثبت نشد",
                    IsSuccess = false
                };
            }
            string responseItemsSave = await response1.Content.ReadAsStringAsync();
            var HesabfaApiResponseSaveItems = JsonSerializer.Deserialize<HesabfaItemResponse>(responseItemsSave);
            return new ResultDto<HesabfaItemResponse>
            {
                Data = HesabfaApiResponseSaveItems,
                Message = "ثبت کالاها با موفقیت انجام شد",
                IsSuccess = true
            };
        }



        internal async Task<ResultDto<HesabfaSaveContactResponse>> SaveContact(HesabfaSaveContactDto request, HttpClient client, string url1)
        {
            //string url1 = Environment.GetEnvironmentVariable("HESABFA_ITEM_BATCH_SAVE");
            HttpResponseMessage response1 = await client.PostAsJsonAsync(url1, request);
            if (!response1.IsSuccessStatusCode)
            {
                return new ResultDto<HesabfaSaveContactResponse>
                {
                    Message = "اطلاعات مشتری در سیستم حسابداری شعبه ثبت نشد",
                    IsSuccess = false
                };
            }
            string responseItemsSave = await response1.Content.ReadAsStringAsync();
            var HesabfaApiResponseSaveItems = JsonSerializer.Deserialize<HesabfaSaveContactResponse>(responseItemsSave);
            return new ResultDto<HesabfaSaveContactResponse>
            {
                Data = HesabfaApiResponseSaveItems,
                Message = "ثبت مشتری با موفقیت انجام شد",
                IsSuccess = true
            };
        }



        internal PreparedProductsResult PrepareProductsForSaving(IEnumerable<ProductFactor> Products, BranchINFO branch)
        {

            var BranchProducts = new List<HesabfaItemDto>();
            foreach (var product in Products)
            {
                BranchProducts.Add(new HesabfaItemDto
                {
                    code = product.Id.ToString(),
                    //code = $"A{product.Id.ToString()}",
                    name = product.Name,
                    itemType = 0,///// کالا 0.....خدمات 1
                    sellPrice = product.fee * 10,//product.fee*10 اگر نیاز به تبدیل تومان به ریا ل باشد
                    buyPrice = product.fee * 10 / 2
                });
            }
            var Input1 = new HesabfaItemSave
            {
                apiKey = branch.apiKey,
                userId = branch.HesabfaUserId,
                password = branch.HesabfaPass,
                loginToken = branch.loginToken,

                items = BranchProducts
            };
            var RadinProducts = BranchProducts.Select(item => new HesabfaItemDto
            {
                code = item.code,
                name = item.name,
                itemType = 1,///// کالا 0.....خدمات 1
                sellPrice = item.buyPrice
            }).ToList();

            var Input2 = new HesabfaItemSave
            {
                apiKey = "snoTL1UnT7KBb6LYTGjztaQhpVIVTVOX",
                userId = "09101050112",
                password = "hossein50112",
                loginToken = "836677609bf81efb0004e70a3041af884a799d2115cde9d17a28c8707662c269fbb7240531bbf2f937dac8bad3933ec4",
                items = RadinProducts
            };
            if (branch.BranchCode != 7792)
            {
                Input2.apiKey = "aL89NbQsJqckxBBjFVdKVO0n6rR0bta3";
                Input2.loginToken = "a803e6769d3ccbdfa9075fd0d0babe1642778bd046716cfe98d78a59103ec043d2d51062d8e3a74c9e57488326a40a6c";

            }
            var Result = new PreparedProductsResult
            {
                RadinPreparedProduct = Input2,
                BranchPreparedProduct = Input1,
            };
            return Result;
        }


        public class PreparedProductsResult
        {
            public HesabfaItemSave RadinPreparedProduct { get; set; }
            public HesabfaItemSave BranchPreparedProduct { get; set; }
        }




        internal PreparedContactsResult PrepareContactsForSaving(BranchINFO branch, CustomerInfo Customer)
        {


            string fullName = Customer.Name + " " + Customer.LastName;
            /////////////////////////////////////////                      اطلاعات مشتری برای شعبه

            var Input3 = new HesabfaSaveContactDto
            {
                apiKey = branch.apiKey,
                userId = branch.HesabfaUserId,
                password = branch.HesabfaPass,
                loginToken = branch.loginToken,

                contact = new ContactInfo
                {
                    code = Customer.CustomerID.ToString(),
                    name = fullName,
                    firstName = Customer.Name,
                    lastName = Customer.LastName,
                    contactType = 1,//نامشخص 0.....حقیقی 1....حقوقی 2 
                    mobile = Customer.phone,
                }
            };
            /////////////////////////////////////////                      اطلاعات مشتری برای رادین
            var Input4 = new HesabfaSaveContactDto
            {
                apiKey = "snoTL1UnT7KBb6LYTGjztaQhpVIVTVOX",
                userId = "09101050112",
                password = "hossein50112",
                loginToken = "836677609bf81efb0004e70a3041af884a799d2115cde9d17a28c8707662c269fbb7240531bbf2f937dac8bad3933ec4",
                contact = new ContactInfo
                {
                    code = branch.BranchCode.ToString(),
                    name = branch.BranchName,
                    firstName = "",
                    lastName = "",
                    contactType = 1,//نامشخص 0.....حقیقی 1....حقوقی 2
                    mobile = branch.BranchPhone1,
                }
            };
            if (branch.BranchCode != 7792)
            {
                Input4.apiKey = "aL89NbQsJqckxBBjFVdKVO0n6rR0bta3";
                Input4.loginToken = "a803e6769d3ccbdfa9075fd0d0babe1642778bd046716cfe98d78a59103ec043d2d51062d8e3a74c9e57488326a40a6c";

            }

            var Result = new PreparedContactsResult
            {
                RadinPreparedContact = Input4,
                BranchPreparedContact = Input3

            };
            return Result;

        }


        public class PreparedContactsResult
        {
            public HesabfaSaveContactDto RadinPreparedContact { get; set; }
            public HesabfaSaveContactDto BranchPreparedContact { get; set; }
        }



        internal List<HesabfaInvoiceItem> PrepareProductForFactor(IEnumerable<ProductFactor> Products, HesabfaItemResponse response, float CashOrChecDiscount, int feeEffect,float BranchTotalCheckAdd)
        {
            List<HesabfaInvoiceItem> Result = new List<HesabfaInvoiceItem>();
            foreach (var item in response.Result)
            {
                float fee;
                float discount;

                int ProductId = Convert.ToInt32(item.Code);
                var matchingProduct = Products.FirstOrDefault(p => p.Id == ProductId);
                float transformDiscount = CashOrChecDiscount + (matchingProduct.Discount * (100 - CashOrChecDiscount));
                switch (feeEffect)
                {
                    case 1://// شعبه
                        fee = item.SellPrice;
                        discount = (transformDiscount) * item.SellPrice  / 100;
                        break;
                    case 2://// رادین
                        fee = item.SellPrice/2;
                        discount = 0;
                        break;
                    case 3://// شعبه تمام چک
                        fee = item.SellPrice*(100+ BranchTotalCheckAdd)/100;
                        discount = 0;
                        break;
                    
                    default:
                        fee = item.SellPrice;
                        discount = (transformDiscount) * item.SellPrice / 100;
                        break;

                }
                Result.Add(new HesabfaInvoiceItem
                {
                    itemCode = item.Code,
                    description = item.Name,
                    quantity = matchingProduct.count,
                    unitPrice = fee,
                    discount = discount,
                    tax = 0
                });
            }
            return Result;

        }




        internal List<HesabfaInvoiceItem> PrepareAcessoryForFactor(IEnumerable<ProductFactor> Products,float CashOrChecDiscount,int feeEffect,float BranchTotalCheckAdd)
        {
            List < HesabfaInvoiceItem > Result=new List<HesabfaInvoiceItem> ();
            foreach (var item in Products)
            {
                float fee;
                float discount;
                float transformDiscount = CashOrChecDiscount + (item.Discount * (100 - CashOrChecDiscount)) / 100;

                switch (feeEffect)
                {
                    case 1://// شعبه
                        fee = item.fee;
                        discount = (transformDiscount) * item.fee / 100;
                        break;
                    case 2://// رادین
                        fee = item.PurchaseFee;
                        discount = 0;
                        break;
                    case 3://// شعبه تمام چک
                        fee = item.fee * (100 + BranchTotalCheckAdd) / 100;
                        discount = 0;
                        break;
                    default:
                        fee = item.fee;
                        discount = (transformDiscount) * item.fee / 100;
                        break;

                }
                Result.Add(new HesabfaInvoiceItem
                {
                    itemCode = item.AcessoryCode.ToString(),
                    description = item.Name,
                    quantity = item.count,
                    unitPrice = fee*10,
                    discount = discount*10,
                    tax = 0
                });
            }
            return Result;

        }



        internal List<HesabfaInvoiceItem> PrepareServiceForFactor(IEnumerable<ProductFactor> Products, float CashOrChecDiscount,float BranchTotalCheckAdd)
        {
            List<HesabfaInvoiceItem> Result = new List<HesabfaInvoiceItem>();
            foreach (var item in Products)
            {
                float fee;
                float transformDiscount = CashOrChecDiscount + (item.Discount * (100 - CashOrChecDiscount)) / 100;

               
                
                Result.Add(new HesabfaInvoiceItem
                {
                    itemCode = item.ServiceCode.ToString(),
                    description = item.Name,
                    quantity = item.count,
                    unitPrice = item.fee*10*(100+ BranchTotalCheckAdd)/100,
                    discount = transformDiscount * 10,
                    tax = 0
                });
            }
            return Result;

        }






        internal HesabfaFactorSave PrepareFactor(List<HesabfaInvoiceItem> Items, MainFactor factor,int InvoiceType,string ContactCode, string apikey, string UserId, string Password, string LoginToken)
        {

            var Result = new HesabfaFactorSave
            {
                apiKey = apikey,
                userId = UserId,
                password = Password,
                loginToken = LoginToken,

                invoice = new HesabfaInvoiceDto
                {
                    number = factor.Id.ToString(),
                    date = DateTime.Now.ToString(),
                    dueDate = DateTime.Now.ToString(),
                    contactCode = ContactCode,// 
                    contactTitle = factor.WorkName,
                    invoiceType = InvoiceType,///فروش 0.....خرید 1....برگشت از فروش 2 ....برگشت از خرید 3 ....ضایعات 4 
                    status = 1,// تایید شده 1 ....پیش نویس 0
                    project = factor.WorkName,
                    InvoiceItems = Items
                }
            };

            return Result;

        }



        

        }
}
