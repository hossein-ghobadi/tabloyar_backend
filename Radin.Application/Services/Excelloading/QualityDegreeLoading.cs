//using OfficeOpenXml;
//using Radin.Domain.Entities.Products.Aditional;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Excelloading
//{
//    public class QualityDegreeLoading
//    {
//        public ExcelTablesResultDto ReadDataFromExcel(string filePath, int WorksheetNumber)
//        {
//            var qualitydegrees = new List<QualityDegree>();

//            using (var package = new ExcelPackage(new FileInfo(filePath)))
//            {
//                var QualityDegreeTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the 10th worksheet
//                for (int row = 2; row <= QualityDegreeTable.Dimension.End.Row; row++)
//                {
//                    if (string.IsNullOrWhiteSpace(QualityDegreeTable.Cells[row, 2].Value?.ToString()))
//                    {
//                        continue;
//                    }
//                    var qualityDegree = new QualityDegree
//                    {
//                        QualityFactor = QualityDegreeTable.Cells[row, 2].Value.ToString(),


//                    };
                    
//                    qualitydegrees.Add(qualityDegree);
//                }


//                return new ExcelTablesResultDto
//                {
//                    QualityDegrees = qualitydegrees

//                };
//            }
//        }
//        public class ExcelTablesResultDto
//        {
//            public List<QualityDegree> QualityDegrees { get; set; }
//        }
//    }
//}
