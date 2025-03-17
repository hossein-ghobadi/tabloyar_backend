using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Factors
{
    public class CheckPayment
    {
        public int Id { get; set; }
        public  int PaymentId { get; set; }
        public float CheckPrice { get; set; }
        public DateTime CheckDueDate { get; set; }
        public string CheckImage { get; set; }
        public PaymentReport PaymentReports { get; set; }
    }
}
