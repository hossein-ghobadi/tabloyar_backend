//using Endpoint.Site.Models.ViewModels.CharacterTypeCalculationModels;
//using Microsoft.AspNetCore.Mvc;
//using OfficeOpenXml;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using OfficeOpenXml;
//namespace Endpoint.Site.Controllers
//{
//    public class TestController : Controller
//    {
//        private List<Question> Questions = new List<Question>();
//        public TestController()
//        {
//            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CT2.xlsx");
//            Questions = LoadQuestionsFromExcel(filePath);
//        }
//        // Action to display questions
//        public IActionResult Index()
//        {
//            return View(Questions);

//        }

//        [HttpPost]
//        public IActionResult Index(Dictionary<int, int> answers)
//        {
//            int totalD = 0, totalI = 0, totalS = 0, totalC = 0;
//            int maxD = 0, maxI = 0, maxS = 0, maxC = 0;

//            // Calculate user scores and max possible scores
//            foreach (var question in Questions)
//            {
//                // Find max scores for this question
//                int questionMaxD = question.Answers.Max(a => a.DScore);
//                int questionMaxI = question.Answers.Max(a => a.IScore);
//                int questionMaxS = question.Answers.Max(a => a.SScore);
//                int questionMaxC = question.Answers.Max(a => a.CScore);

//                // Accumulate max scores
//                maxD += questionMaxD;
//                maxI += questionMaxI;
//                maxS += questionMaxS;
//                maxC += questionMaxC;

//                // Check if user answered this question
//                if (answers.TryGetValue(question.Qid, out int selectedAid))
//                {
//                    // Get the selected answer
//                    var selectedAnswer = question.Answers.FirstOrDefault(a => a.Aid == selectedAid);
//                    if (selectedAnswer != null)
//                    {
//                        totalD += selectedAnswer.DScore;
//                        totalI += selectedAnswer.IScore;
//                        totalS += selectedAnswer.SScore;
//                        totalC += selectedAnswer.CScore;
//                    }
//                }
//            }

//            // Normalize scores (convert to percentage)
//            double normalizedD = (double)totalD / maxD * 100;
//            double normalizedI = (double)totalI / maxI * 100;
//            double normalizedS = (double)totalS / maxS * 100;
//            double normalizedC = (double)totalC / maxC * 100;

//            // Prepare result
//            var result = new DiscResult
//            {
//                D = (int)normalizedD,
//                I = (int)normalizedI,
//                S = (int)normalizedS,
//                C = (int)normalizedC
//            };

//            return View("Result", result);
//        }


//        private List<Question> LoadQuestionsFromExcel(string filePath)
//{
//    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

//    var questions = new List<Question>();

//    using (var package = new ExcelPackage(new FileInfo(filePath)))
//    {
//        var worksheet = package.Workbook.Worksheets[0];
//        int row = 2;

//        while (worksheet.Cells[row, 1].Value != null) // Continue until Qid is null
//        {
//            int qid = Convert.ToInt32(worksheet.Cells[row, 1].Value); // Qid column
//            string qtext = worksheet.Cells[row, 2].Text;             // Qtext column

//            // Check if this question already exists in the list
//            var question = questions.FirstOrDefault(q => q.Qid == qid);
//            if (question == null)
//            {
//                question = new Question { Qid = qid, Qtext = qtext };
//                questions.Add(question);
//            }

//            // Read answer details
//            int aid = Convert.ToInt32(worksheet.Cells[row, 3].Value); // Aid column
//            string atext = worksheet.Cells[row, 4].Text;              // Atext column
//            int dScore = int.TryParse(worksheet.Cells[row, 5].Text, out int d) ? d : 0;
//            int iScore = int.TryParse(worksheet.Cells[row, 6].Text, out int i) ? i : 0;
//            int sScore = int.TryParse(worksheet.Cells[row, 7].Text, out int s) ? s : 0;
//            int cScore = int.TryParse(worksheet.Cells[row, 8].Text, out int c) ? c : 0;

//            // Create answer and add it to the question
//            var answer = new Answer
//            {
//                Aid = aid,
//                Atext = atext,
//                DScore = dScore,
//                IScore = iScore,
//                SScore = sScore,
//                CScore = cScore
//            };

//            question.Answers.Add(answer);

//            row++;
//        }
//    }

//    return questions;
//}

//    }
//}
