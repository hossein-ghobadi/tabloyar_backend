using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Radin.Application.Interfaces.Contexts;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static Radin.Application.Services.OtherExcelloading.QuestionService;

namespace Radin.Application.Services.OtherExcelloading
{
    public class QuestionService
    {
        private readonly IDataBaseContext _dbContext;

        public QuestionService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Object GetHighDiscCharacter(DISCResult discResult)
        {
            // Create a list of DISC values with their corresponding labels
            var discValues = new Dictionary<string, int>
        {
            { "D", discResult.D },
            { "I", discResult.I },
            { "S", discResult.S },
            { "C", discResult.C }
        };

            // Find which labels have values greater than 50
            var highLabels = discValues
                .Where(kv => kv.Value > 50)
                .Select(kv => kv.Key)
                .OrderBy(label => label) // Ensure consistent order (alphabetical)
                .ToList();

            // Combine the labels to create the "Type" string (e.g., "CD" for "C" and "D" > 50)
            string combinedType = string.Join("", highLabels);
            var Tip= _dbContext.PersonalityCharacterType
                .FirstOrDefault(ct => ct.Type == combinedType);
            if(Tip == null)
            {
                return new
                {
                    id = 14,
                    label = "N",
                    DValue = discResult.D,
                    IValue = discResult.I,
                    SValue = discResult.S,
                    CValue = discResult.C,
                };
            }
            // Query the database for the matching type
            return new 
            {
                id=Tip.Id ,
                label=Tip.Type ,
                DValue = discResult.D,
                IValue = discResult.I,
                SValue = discResult.S,
                CValue = discResult.C,


            };
        }
        public DISCResult CalculateDiscResult(Dictionary<int, int> answers, List<DISCQuestion> questions)
        {
            int totalD = 0, totalI = 0, totalS = 0, totalC = 0;
            int maxD = 0, maxI = 0, maxS = 0, maxC = 0;

            foreach (var question in questions)
            {
                // Find max scores for this question
                int questionMaxD = question.Answers.Max(a => a.DScore);
                int questionMaxI = question.Answers.Max(a => a.IScore);
                int questionMaxS = question.Answers.Max(a => a.SScore);
                int questionMaxC = question.Answers.Max(a => a.CScore);

                maxD += questionMaxD;
                maxI += questionMaxI;
                maxS += questionMaxS;
                maxC += questionMaxC;

                // Check if user answered this question
                if (answers.TryGetValue(question.Qid, out int selectedAid))
                {
                    // Get the selected answer
                    var selectedAnswer = question.Answers.FirstOrDefault(a => a.Aid == selectedAid);
                    if (selectedAnswer != null)
                    {
                        totalD += selectedAnswer.DScore;
                        totalI += selectedAnswer.IScore;
                        totalS += selectedAnswer.SScore;
                        totalC += selectedAnswer.CScore;
                    }
                }
            }

            if (maxD == 0 || maxI == 0 || maxS == 0 || maxC == 0)
            {
                throw new InvalidOperationException("Invalid question or answer data.");
            }

            // Normalize scores (convert to percentage)
            double normalizedD = (double)totalD / maxD * 100;
            double normalizedI = (double)totalI / maxI * 100;
            double normalizedS = (double)totalS / maxS * 100;
            double normalizedC = (double)totalC / maxC * 100;

            // Prepare result
            return new DISCResult
            {
                D = (int)normalizedD,
                I = (int)normalizedI,
                S = (int)normalizedS,
                C = (int)normalizedC
            };
        }


        public List<DISCQuestion> LoadQuestionsFromExcel(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var questions = new List<DISCQuestion>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int row = 2;

                while (worksheet.Cells[row, 1].Value != null) // Continue until Qid is null
                {
                    int qid = Convert.ToInt32(worksheet.Cells[row, 1].Value); // Qid column
                    string qtext = worksheet.Cells[row, 2].Text;             // Qtext column

                    var question = questions.FirstOrDefault(q => q.Qid == qid);
                    if (question == null)
                    {
                        question = new DISCQuestion { Qid = qid, Qtext = qtext, Answers = new List<DISCAnswer>() };
                        questions.Add(question);
                    }

                    int aid = Convert.ToInt32(worksheet.Cells[row, 3].Value); // Aid column
                    string atext = worksheet.Cells[row, 4].Text;              // Atext column
                    int dScore = int.TryParse(worksheet.Cells[row, 5].Text, out int d) ? d : 0;
                    int iScore = int.TryParse(worksheet.Cells[row, 6].Text, out int i) ? i : 0;
                    int sScore = int.TryParse(worksheet.Cells[row, 7].Text, out int s) ? s : 0;
                    int cScore = int.TryParse(worksheet.Cells[row, 8].Text, out int c) ? c : 0;

                    var answer = new DISCAnswer
                    {
                        Aid = aid,
                        Atext = atext,
                        DScore = dScore,
                        IScore = iScore,
                        SScore = sScore,
                        CScore = cScore
                    };

                    question.Answers.Add(answer);

                    row++;
                }
            }

            return questions;
        }
        public class DISCQuestion
        {
            public int Qid { get; set; }
            public string Qtext { get; set; }
            public List<DISCAnswer> Answers { get; set; } = new List<DISCAnswer>();
        }
        public class DISCAnswer
        {
            public int Aid { get; set; }
            public string Atext { get; set; }
            public int DScore { get; set; }
            public int IScore { get; set; }
            public int SScore { get; set; }
            public int CScore { get; set; }
        }

        public class DISCResult
        {
            public int D { get; set; }
            public int I { get; set; }
            public int S { get; set; }
            public int C { get; set; }
        }
    }
}
