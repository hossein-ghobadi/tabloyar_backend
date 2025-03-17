using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Factors
{
    public class PaymentReport
    {
        public int Id { get; set; }
        public long FactorId {  get; set; }
        public float CashInitialPayment { get; set; } = 0;
        public float TotalPrice { get; set; }
        public string PaymentReceiptImage { get; set; } = "";
        public bool IsFinancialAgreement { get; set; } = false;

        public bool IsCash { get; set; } = true;
        public MainFactor MainFactors { get; set; }
        public ICollection<CheckPayment> CheckPayments { get; set; }


    }
}
