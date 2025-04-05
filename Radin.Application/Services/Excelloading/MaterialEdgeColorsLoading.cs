//using OfficeOpenXml;
//using Radin.Domain.Entities.Products.Aditional;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Excelloading
//{
//    public class MaterialEdgeColorsLoading
//    {
//        public ExcelTablesResultDto ReadDataFromExcel(string filePath, int WorksheetNumber)
//        {
//            var materialEdgeColors = new List<MaterialEdgeColor>();

//            using (var package = new ExcelPackage(new FileInfo(filePath)))
//            {
//                var MaterialEdgeColrsTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the third worksheet
//                for (int row = 2; row <= MaterialEdgeColrsTable.Dimension.End.Row; row++)
//                {
//                    if (string.IsNullOrWhiteSpace(MaterialEdgeColrsTable.Cells[row, 2].Value?.ToString()))
//                    {
//                        continue;
//                    }
//                    var materialEdgeColor = new MaterialEdgeColor
//                    {
//                        Title = MaterialEdgeColrsTable.Cells[row, 2].Value.ToString(),
//                        EdgeColor = MaterialEdgeColrsTable.Cells[row, 3].Value.ToString(),
//                        SecondEdgeColor = MaterialEdgeColrsTable?.Cells[row, 4].Value?.ToString(),

//                    };
//                    var V_IsDefault = MaterialEdgeColrsTable.Cells[row, 5].Value;
//                    materialEdgeColor.IsDefault = V_IsDefault != null ? Convert.ToBoolean(V_IsDefault) : false;



//                    materialEdgeColors.Add(materialEdgeColor);
//                }



//                return new ExcelTablesResultDto
//                {
//                    MaterialEdgeColors = materialEdgeColors

//                };
//            }
//        }
//        public class ExcelTablesResultDto
//        {
//            public List<MaterialEdgeColor> MaterialEdgeColors { get; set; }
//        }
//    }
//}
