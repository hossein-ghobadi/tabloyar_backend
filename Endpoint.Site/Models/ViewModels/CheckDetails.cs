using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Endpoint.Site.Models.ViewModels
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public int Amount { get; set; } // مبلغ

        [BindProperty]
        public int Months { get; set; } // تعداد ماه

        [BindProperty]
        public int CheckCount { get; set; } // تعداد چک

        [BindProperty]
        public string StartDate { get; set; } // تاریخ شروع به فرمت شمسی

        public int GoldenNumber { get; set; } // عدد طلایی محاسبه‌شده
        public List<List<DateTime>> ValidChecks { get; set; } = new List<List<DateTime>>(); // لیست تاریخ‌های معتبر چک‌ها

        public void OnPost()
        {
            // محاسبه عدد طلایی
            GoldenNumber = (Months + 1) * 30 / 2;

            // تبدیل تاریخ شمسی به میلادی
            DateTime startDate = DateTime.ParseExact(StartDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);

            // حداکثر تعداد روزها
            int maxDays = Months * 30;

            // پیدا کردن ترکیب‌های تاریخ چک‌ها
            var validDateOffsets = FindDates(0, GoldenNumber, maxDays, CheckCount);

            // تبدیل اعداد روز به تاریخ میلادی
            ValidChecks = validDateOffsets.Select(comb =>
                comb.Select(offset => startDate.AddDays(offset)).ToList()).ToList();
        }

        private List<List<int>> FindDates(int startDate, int goldenNumber, int maxDays, int numDates)
        {
            int endDate = startDate + maxDays;
            int totalSumRequired = numDates * goldenNumber;

            // تولید همه ترکیب‌های ممکن
            var allCombinations = GetCombinations(Enumerable.Range(startDate, endDate - startDate + 1).ToList(), numDates);

            // فیلتر کردن ترکیب‌هایی که مجموع آن‌ها برابر عدد طلایی است
            return allCombinations.Where(comb => comb.Sum() == totalSumRequired).ToList();
        }

        private List<List<int>> GetCombinations(List<int> list, int length)
        {
            if (length == 1)
                return list.Select(t => new List<int> { t }).ToList();

            return GetCombinations(list, length - 1)
                .SelectMany(t => list.Where(e => e > t.Last()), (t1, t2) => t1.Concat(new List<int> { t2 }).ToList())
                .ToList();
        }
    }



}
