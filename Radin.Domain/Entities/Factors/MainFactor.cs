using Radin.Domain.Entities.Commons;
using Radin.Domain.Entities.Factors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Factors
{
    public class MainFactor:BaseEntity
    {
        public long Id { get; set; }
        public long BranchCode { get; set; }
        public int ConnectionCount { get; set; } = 0;
        public int? ConnectionDuration { get; set; } = 0;
        public int ContactType { get; set; }= 0;
        public DateTime InitialConnectionTime { get; set; }
        public DateTime LastConnectionTime { get; set; }

        public bool TatilRasmi { get; set; }
        public string dayofweek { get; set; }
        public string year {  get; set; }
        public string month { get; set; }
        public string day { get; set; }
        public string? ReasonStatus { get; set; }
        public string WorkName { get; set; }
        public int? RecommandedDesign { get; set; }
        public string? SelectedDesign { get; set; }
        public string MainsellerID { get; set; }
        public string? AssistantSellerID { get; set; }
        public int? AssistantSellerPercent { get; set; }
        public float? TotalDiscount { get; set; } = 0;
        public float? TotalPackingCost { get; set; } = 0;
        public int? count { get; set; } = 1;
        public float? fee { get; set; } = 0;
        public float? TotalAmount { get; set; } = 0;
        public long? CustomerID { get; set; }
        public bool position {  get; set; }
        public DateTime ExpireTime { get; set; }
        public int state { get; set; }
        public bool status { get; set; }
        public float PurchaseProbability { get; set; } = 0;
        public string? description { get; set; }
        public bool FinantialAgrrement { get; set; } = false;
        public int PaymentType { get; set; } = 0;

        public ICollection<SubFactor> SubFactors { get; set; }
        public PaymentReport PaymentReports { get; set; }
        public ICollection<CustomerConnection> CustomerConnections { get; set; }

    }
}


