using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.OKR
{
    public class MonthlyTarget
    {
        public long Id { get; set; }
        public long BranchCode { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public float week1 { get; set; }
        public float week2 { get; set; }
        public float week3 { get; set; }
        public float week4 { get; set; }
        public float week5 { get; set; }
        public float week6 { get; set; }
        public float Sum { get; set; }
        public float DailyMin { get; set; } = 0;
        public float DailyMax { get; set; } = 0;
        public float DailyMid { get; set; } = 0;
    }
}
