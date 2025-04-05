//using OfficeOpenXml;
//using Radin.Domain.Entities.Products;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Excelloading
//{
//    public class PowersLoading
//    {
//        public ExcelTablesResultDto ReadDataFromExcel(string filePath, int WorksheetNumber)
//        {
//            var powers = new List<Power>();

//            using (var package = new ExcelPackage(new FileInfo(filePath)))
//            {
//                var PowersTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the third worksheet
//                for (int row = 2; row <= PowersTable.Dimension.End.Row; row++)
//                {

                    
//                    if (string.IsNullOrWhiteSpace(PowersTable.Cells[row, 5].Value?.ToString()))
//                    {
//                        continue;
//                    }
//                    var power = new Power
//                    {
//                        QualityFactor = PowersTable.Cells[row, 5].Value.ToString(),
//                        //PowerType = PowersTable.Cells[row, 2].Value.ToString(),

//                    };
//                    var V_IsDefault = PowersTable.Cells[row, 6].Value;
//                    power.IsDefault = V_IsDefault != null ? Convert.ToBoolean(V_IsDefault) : false;



//                    //var V_powerFee = PowersTable.Cells[row, 5].Value;
//                    //if (V_powerFee == null || string.IsNullOrWhiteSpace(V_powerFee.ToString()))
//                    //{
//                    //    power.PowerFee = null;
//                    //}
//                    //else
//                    //{
//                    //    // Proceed with conversion
//                    //    ExcelHelper.TryConvertToFloat(V_powerFee, out float powerFee, "en-US", "de-DE");
//                    //    power.PowerFee = powerFee;
//                    //}
//                    int powerType;
//                    ExcelHelper.TryConvertToInt(PowersTable.Cells[row, 2].Value, out powerType, "en-US", "de-DE");
//                    power.PowerType = powerType;

//                    int maxSmd;
//                    ExcelHelper.TryConvertToInt(PowersTable.Cells[row, 3].Value, out maxSmd, "en-US", "de-DE");
//                    power.MaxSmd = maxSmd;

//                    float powerFee;
//                    ExcelHelper.TryConvertToFloat(PowersTable.Cells[row, 4].Value, out powerFee, "en-US", "de-DE");
//                    power.PowerFee = powerFee;

//                    powers.Add(power);
//                }



//                return new ExcelTablesResultDto
//                {
//                    Powers = powers

//                };
//            }
//        }
//        public class ExcelTablesResultDto
//        {
//            public List<Power> Powers { get; set; }
//        }
//    }
//}
