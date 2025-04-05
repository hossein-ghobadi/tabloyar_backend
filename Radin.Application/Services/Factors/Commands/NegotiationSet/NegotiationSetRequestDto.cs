//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Factors.Commands.NegotiationSet
//{
//    public class NegotiationSetRequestDto
//    {
//        public long FactorId { get; set; }
//        public long CustomerId { get; set; }
//        public string? AssistantSellerID { get; set; } = null;
//        public int RecommandedDesign { get; set; }
//        //public string? SelectedDesign { get; set; }
//        public string? description { get; set; }
//        public string? ReasonStatus { get; set; }
//    }

//    public class ProbabilityRequestDto
//    {

//        public int gender   { get; set; }
//        public int ageCategory { get; set; }
//        public int CharacterType { get; set; }
//        public string dayofweek { get; set; }
//        public int ConnectionCount { get; set; } = 1;
//        public int ConnectionDuration { get; set; } = 0; 
//        public int ContactType { get; set; }
//        public int RecommandedDesign { get; set; }
//        public float TotalAmount { get; set; } 
//        public DateTime InitialConnectionTime { get; set; }


//    }

//    public class Probability
//    {
//        public int prediction { get; set;}
//        public List<float> probabilities { get; set; }
//    }


//    //public class HesabfaSaveContactDto
//    //{
//    //    public string apiKey { get; set; }
//    //    public string userId { get; set; }
//    //    public string password { get; set; }
//    //    public string loginToken { get; set; }
//    //    public ContactInfo contact {  get; set; }
//    //}

//    //public class ContactInfo
//    //{
//    //    public string code { get; set; }
//    //    public string name { get; set; }
//    //    public string firstName { get; set; }
//    //    public string lastName { get; set; }
//    //    public int contactType { get; set; }
//    //    public string mobile { get; set; }
//    //}

//    //public class HesabfaSaveContactResponse
//    //{
//    //    public bool Success { get; set; }
//    //    public int ErrorCode { get; set; }
//    //    public ContactInfoResult Result { get; set; }
//    //}


//    //public class ContactInfoResult
//    //{
//    //    public string Code { get; set; }
//    //    public string Name { get; set; }
//    //}

//    //public class HesabfaFactorSave
//    //{
//    //    public string apiKey { get; set; }
//    //    public string userId { get; set; }
//    //    public string password { get; set; }
//    //    public string loginToken { get; set; }
//    //    public HesabfaInvoiceDto invoice { get; set; }

//    //}

//    //public class HesabfaInvoiceDto
//    //{
//    //    public string number { get; set; }
//    //    public string date { get; set; }
//    //    public string dueDate { get; set; }
//    //    public string contactCode { get; set; }
//    //    public string contactTitle { get; set; }
//    //    public int invoiceType { get; set; }
//    //    public string project { get; set; }
//    //    public List<HesabfaInvoiceItem> InvoiceItems { get; set; }

//    //}

//    //public class HesabfaInvoiceItem
//    //{
//    //    public string description { get; set; }
//    //    public string itemCode { get; set;}
//    //    public int quantity { get; set; }
//    //    public float unitPrice { get; set; }
//    //    public float discount { get; set; }
//    //    public int tax { get; set;}
//    //}


//    //public class HesabfaItemSave
//    //{
//    //    public string apiKey { get; set; }
//    //    public string userId { get; set; }
//    //    public string password { get; set; }
//    //    public string loginToken { get; set; }
//    //    public List<HesabfaItemDto> items { get; set; }

//    //}

//    //public class HesabfaItemDto
//    //{
//    //    public string code { get; set; }
//    //    public string name { get; set; }
//    //    public int itemType { get; set; }
//    //    public float sellPrice { get; set; }



//    //}

//    //public class HesabfaItemDtoResponse
//    //{
//    //    public string Code { get; set; }
//    //    public string Name { get; set; }
//    //    public int ItemType { get; set; }
//    //    public float SellPrice { get; set; }



//    //}

//    //public class HesabfaItemResponse
//    //{
//    //    public bool Success { get; set; }
//    //    public int ErrorCode { get; set; }
//    //    public string ErrorMessage { get; set; }
//    //    public List<HesabfaItemDtoResponse> Result { get; set; }
//    //}

//    //public class HesabfaFactorSaveResponse
//    //{
//    //    public bool Success { get; set; }
//    //    public int ErrorCode { get; set; }
//    //    public string ErrorMessage { get; set; }
//    //    public List<HesabfaItemDto> Result { get; set; }
//    //}


//}
