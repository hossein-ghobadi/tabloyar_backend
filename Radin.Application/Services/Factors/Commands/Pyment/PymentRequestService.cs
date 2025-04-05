//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Factors.Commands.Pyment
//{

//    public class NonCashRequestService
//    {
//        public long FactorId { get; set; }
//        public bool FinancialAgreement { get; set; } = false;

//        public float amount { get; set; }
      
//        public long CustomerId { get; set; }
//        public List<RecievedCheckInfo>? RecievedCheckInfos { get; set; }
//    }
//    public class PymentRequestService
//    {
//        public long FactorId { get; set; }
//        public int bankCode { get; set; }
//        public string bankName { get; set; }
//        public int bankType { get; set; }
//        public string amount { get; set; }
//        /// <summary>
//        /// /
//        /// </summary>
//        public long CustomerId { get; set; }
//        public bool FinancialAgreement { get; set; } = false;
//        public bool IsCash {  get; set; }
//        public string? ReceiptImage { get; set; }
//        public List<RecievedCheckInfo>? RecievedCheckInfos { get; set; }
//    }
//    public class RecievedCheckInfo
//    {
//        public int number { get; set; }
//        public long date { get; set; }
//        public string CheckImage { get; set; }
//        public float amount { get; set; }

//    }

//    public class PaymentInitialRequestDto
//    {
//        public string apiKey { get; set; }
//        public string userId { get; set; }
//        public string password { get; set; }
//        public string loginToken { get; set; }
//        public long number { get; set; }
//        public int type { get; set; }
//        public long? contactCode { get; set; } = null;
//        public float amount { get; set; }
//        public string description { get; set; } = null;
//        public string? transactionFee { get; set; }
//        public string transactionNumber { get; set; }
//    }

    
//    public class BankPaymentRequestDto : PaymentInitialRequestDto
//    {
//        public int bankCode { get; set; }
//    }

//    public class CashPaymentRequestDto : PaymentInitialRequestDto
//    {
//        public int cashCode { get; set; }
//    }

//    public class PettyCashPaymentRequestDto : PaymentInitialRequestDto
//    {
//        public int pettyCashCode { get; set; }
//    }
//    public class CashPymentResponse
//    {
//        public bool Success { get; set; }
//        public int ErrorCode { get; set; }
//        public string ErrorMessage { get; set; }
//    }

//    public class CheckPymentRequestService
//    {
//        public long FactorId { get; set; }

//        public List<Check> checkList { get; set; }


//    }


//    public class CheckPymentRequestDto
//    {
//        public string apiKey { get; set; }
//        public string userId { get; set; }
//        public string password { get; set; }
//        public string loginToken { get; set; }
//        public string description { get; set; } = null;
//        public int type { get; set; }
//        public List<ItemObject> items { get; set; }
//        public List<CheckDto> transactions { get; set; }
//    }

//    public class ItemObject
//    {
//        public string contactCode { get; set; }
//        public float amount { get; set; }
//    }


//    public class CheckDto
//    {
//        public Check Check { get; set; }
//    }

//    public class Check
//    {
//        public int number { get; set; }
//        public string date { get; set; }
//        public float amount { get; set; }
//        public string bankName { get; set; }
//        public long payerCode { get; set; }
//    }

//    public class CheckPymentResponse
//    {
//        public bool Success { get; set; }
//        public int ErrorCode { get; set; }
//        public string ErrorMessage { get; set; }
//    }




























//    public class HesabfaSaveContactDto
//    {
//        public string apiKey { get; set; }
//        public string userId { get; set; }
//        public string password { get; set; }
//        public string loginToken { get; set; }
//        public ContactInfo contact { get; set; }
//    }

//    public class ContactInfo
//    {
//        public string code { get; set; }
//        public string name { get; set; }
//        public string firstName { get; set; }
//        public string lastName { get; set; }
//        public int contactType { get; set; }
//        public string mobile { get; set; }
//    }

//    public class HesabfaSaveContactResponse
//    {
//        public bool Success { get; set; }
//        public int ErrorCode { get; set; }
//        public ContactInfoResult Result { get; set; }
//    }
   


//    public class ContactInfoResult
//    {
//        public string Code { get; set; }
//        public string Name { get; set; }
//    }

//    public class HesabfaFactorSave
//    {
//        public string apiKey { get; set; }
//        public string userId { get; set; }
//        public string password { get; set; }
//        public string loginToken { get; set; }
//        public HesabfaInvoiceDto invoice { get; set; }

//    }

//    public class HesabfaInvoiceDto
//    {
//        public string number { get; set; }
//        public string date { get; set; }
//        public string dueDate { get; set; }
//        public string contactCode { get; set; }
//        public string contactTitle { get; set; }
//        public int invoiceType { get; set; }
//        public int status { get; set; }
//        public string project { get; set; }
//        public List<HesabfaInvoiceItem> InvoiceItems { get; set; }

//    }

//    public class HesabfaInvoiceItem
//    {
//        public string description { get; set; }
//        public string itemCode { get; set; }
//        public int quantity { get; set; }
//        public float unitPrice { get; set; }
//        public float discount { get; set; }
//        public int tax { get; set; }
//    }


//    public class HesabfaItemSave
//    {
//        public string apiKey { get; set; }
//        public string userId { get; set; }
//        public string password { get; set; }
//        public string loginToken { get; set; }
//        public List<HesabfaItemDto> items { get; set; }

//    }

//    public class HesabfaItemDto
//    {
//        public string code { get; set; }
//        public string name { get; set; }
//        public int itemType { get; set; }
//        public float? sellPrice { get; set; } = null;
//        public float? buyPrice { get; set; } = null;

//    }
//    public class HesabfaItemDto2
//    {
//        public string code { get; set; }
//        public string name { get; set; }
//        public int itemType { get; set; }
//        public float? sellPrice { get; set; } = null;
//        public float? buyPrice { get; set; } = null;

//    }

//    public class HesabfaItemDtoResponse
//    {
//        public string Code { get; set; }
//        public string Name { get; set; }
//        public int ItemType { get; set; }
//        public float SellPrice { get; set; }
//        public float buyPrice { get; set; }



//    }

//    public class HesabfaItemResponse
//    {
//        public bool Success { get; set; }
//        public int ErrorCode { get; set; }
//        public string ErrorMessage { get; set; }
//        public List<HesabfaItemDtoResponse> Result { get; set; }
//    }

//    public class HesabfaFactorSaveResponse
//    {
//        public bool Success { get; set; }
//        public int ErrorCode { get; set; }
//        public string ErrorMessage { get; set; }
//        public List<HesabfaItemDto> Result { get; set; }
//    }
//}
