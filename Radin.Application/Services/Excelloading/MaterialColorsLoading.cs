//using OfficeOpenXml;
//using Radin.Domain.Entities.Products;
//using Radin.Domain.Entities.Products.Aditional;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Excelloading
//{
//    public class MaterialColorsLoading
//    {
//        public ExcelTablesResultDto ReadDataFromExcel(string filePath, int WorksheetNumber)
//        {
//            var materialColors = new List<MaterialColor>();

//            using (var package = new ExcelPackage(new FileInfo(filePath)))
//            {
//                var MaterialColorsTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the third worksheet
//                for (int row = 2; row <= MaterialColorsTable.Dimension.End.Row; row++)
//                {
//                    if (string.IsNullOrWhiteSpace(MaterialColorsTable.Cells[row, 2].Value?.ToString()))
//                    {
//                        continue;
//                    }
//                    var materialColor = new MaterialColor
//                    {
//                        Title = MaterialColorsTable.Cells[row, 2].Value.ToString(),
//                        MaterialName = MaterialColorsTable.Cells[row, 3].Value.ToString(),

//                        Color = MaterialColorsTable.Cells[row, 4].Value.ToString(),

//                        //PowerType = PowersTable.Cells[row, 2].Value.ToString(),

//                    };
//                    var V_IsDefault = MaterialColorsTable.Cells[row, 5].Value;
//                    materialColor.IsDefault = V_IsDefault != null ? Convert.ToBoolean(V_IsDefault) : false;




//                    materialColors.Add(materialColor);
//                }



//                return new ExcelTablesResultDto
//                {
//                    MaterialColors = materialColors

//                };
//            }
//        }
//        public class ExcelTablesResultDto
//        {
//            public List<MaterialColor> MaterialColors { get; set; }
//        }
//    }
//}