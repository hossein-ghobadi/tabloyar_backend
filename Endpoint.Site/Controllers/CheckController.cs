//using Endpoint.Site.Models.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using System.Globalization;

//namespace Endpoint.Site.Controllers
//{
//    public class CheckController : Controller
//    {
//        public ActionResult Index()
//        {
//            return View();
//        }

//        [HttpPost]
//        public ActionResult CalculateDueDates(decimal price, int numChecks, int maxMonths, DateTime purchaseDate)
//        {
//            if (numChecks <= 0 || maxMonths <= 0)
//            {
//                ViewBag.Error = "Number of checks and max months must be greater than zero.";
//                return View("Index");
//            }

//            decimal goldenNumber = (maxMonths + 1) / 2.0M;
//            var result = GenerateFlexibleCheckDueDates(numChecks, maxMonths, goldenNumber, purchaseDate);

//            ViewBag.Price = price;
//            ViewBag.NumChecks = numChecks;
//            ViewBag.MaxMonths = maxMonths;
//            ViewBag.GoldenNumber = goldenNumber;
//            ViewBag.PurchaseDate = purchaseDate;
//            ViewBag.DueDates = result;

//            return View("Result");

//        }

//        //private List<string> GenerateCheckDueDatesByDate(int numChecks, int maxMonths, decimal goldenNumber, DateTime purchaseDate)
//        //{
//        //    var results = new List<string>();

//        //    // Convert maxMonths to maxDays
//        //    int maxDays = maxMonths * 30;
//        //    int goldenDays = (int)(goldenNumber * 30); // Convert golden number to days

//        //    // Generate all combinations of possible due dates
//        //    for (int firstCheckDay = 1; firstCheckDay <= maxDays; firstCheckDay++)
//        //    {
//        //        var dueDates = new List<DateTime> { purchaseDate.AddDays(firstCheckDay) };

//        //        int remainingChecks = numChecks - 1;
//        //        int remainingDays = maxDays - firstCheckDay;

//        //        // Skip if not enough days left for remaining checks
//        //        if (remainingDays < remainingChecks) continue;

//        //        // Generate subsequent due dates
//        //        for (int i = 1; i <= remainingChecks; i++)
//        //        {
//        //            // Calculate evenly spaced days for remaining checks
//        //            int nextCheckDay = firstCheckDay + (i * remainingDays / remainingChecks);

//        //            // Ensure the next check day is within maxDays
//        //            if (nextCheckDay > maxDays) break;

//        //            dueDates.Add(purchaseDate.AddDays(nextCheckDay));
//        //        }

//        //        // Calculate the average days difference
//        //        decimal averageDays = dueDates
//        //            .Select(d => (decimal)(d - purchaseDate).TotalDays)
//        //            .Average();

//        //        // Validate against the golden number in days with a tolerance
//        //        if (Math.Abs(averageDays - goldenDays) <= 1)
//        //        {
//        //            // Add the valid configuration
//        //            results.Add(string.Join(", ", dueDates.Select((d, i) => $"Check {i + 1}: {d:yyyy-MM-dd}")));
//        //        }
//        //    }

//        //    return results;



//        //}


//        private List<string> GenerateFlexibleCheckDueDates(int numChecks, int maxMonths, decimal goldenNumber, DateTime purchaseDate)
//        {
//            var allResults = new List<List<DateTime>>();

//            // Convert maxMonths to maxDays
//            int maxDays = maxMonths * 30;
//            int goldenDays = (int)(goldenNumber * 30); // Convert golden number to days

//            // Iterate through all possible first check days (up to maxDays)
//            for (int firstCheckDay = 30; firstCheckDay <= maxDays; firstCheckDay++) // Start from 30 days after purchase
//            {
//                var dueDates = new List<DateTime> { purchaseDate.AddDays(firstCheckDay) };

//                int remainingChecks = numChecks - 1;
//                int remainingDays = maxDays - firstCheckDay;

//                // Skip if not enough days left for remaining checks
//                if (remainingDays < remainingChecks) continue;

//                // Generate all subsequent due dates
//                for (int i = 1; i <= remainingChecks; i++)
//                {
//                    int nextCheckDay = firstCheckDay + (i * remainingDays / remainingChecks);
//                    if (nextCheckDay > maxDays) break;

//                    dueDates.Add(purchaseDate.AddDays(nextCheckDay));
//                }

//                // Validate the configuration
//                decimal averageDays = dueDates
//                    .Select(d => (decimal)(d - purchaseDate).TotalDays)
//                    .Average();

//                if (Math.Abs(averageDays - goldenDays) <= 1) // Golden number tolerance
//                {
//                    allResults.Add(dueDates);
//                }
//            }

//            // Filter for distinct configurations
//            var distinctResults = FilterDistinctResults(allResults);

//            // Package the distinct results as offers
//            return distinctResults.Select((dueDates, index) =>
//                $"Offer {index + 1}: {string.Join(", ", dueDates.Select((d, i) => $"Check {i + 1}: {d:yyyy-MM-dd}"))}"
//            ).ToList();
//        }

//        private List<List<DateTime>> FilterDistinctResults(List<List<DateTime>> allResults)
//        {
//            var distinctResults = new List<List<DateTime>>();

//            foreach (var result in allResults)
//            {
//                bool isDistinct = true;

//                // Compare against existing distinct results
//                foreach (var distinct in distinctResults)
//                {
//                    if (AreDueDatesSimilar(result, distinct))
//                    {
//                        isDistinct = false;
//                        break;
//                    }
//                }

//                if (isDistinct)
//                {
//                    distinctResults.Add(result);
//                }
//            }

//            return distinctResults;
//        }

//        private bool AreDueDatesSimilar(List<DateTime> result1, List<DateTime> result2)
//        {
//            // Check if two results are too similar based on spacing or dates
//            const int dayTolerance = 10; // Allowable difference in days for "similarity"

//            if (result1.Count != result2.Count) return false;

//            for (int i = 0; i < result1.Count; i++)
//            {
//                if (Math.Abs((result1[i] - result2[i]).TotalDays) > dayTolerance)
//                {
//                    return false; // Dates are significantly different
//                }
//            }

//            return true;
//        }


//    }
//}
