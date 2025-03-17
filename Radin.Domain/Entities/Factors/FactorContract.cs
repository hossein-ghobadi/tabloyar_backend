using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Factors
{
    public class FactorContract
    {
        public long Id { get; set; }
        public long factorId { get; set; }
        public string sellerName { get; set; }
        public string sellerId { get; set; }
        public string customerNationalNumber { get; set; }
        public string customerId { get; set; }
        public string customerPhone { get; set; }
        public string customerName { get; set; }
        public string customerAdress { get; set; }
        public DateTime contractDate { get; set; }
        public int deliveryDate { get; set; }
        public bool scaffoldCost { get; set; }
        public bool craneCost { get; set; }
        public bool transportationCost { get; set; }
        public int warrantyMonthNumber { get; set; }
        public int attachedNumber { get; set; }
    }
}
