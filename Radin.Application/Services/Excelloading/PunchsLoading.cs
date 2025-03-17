using OfficeOpenXml;
using Radin.Domain.Entities.Products;
using Radin.Domain.Entities.Products.Aditional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Excelloading
{
    public class PunchsLoading
    {
        public ExcelTablesResultDto ReadDataFromExcel(string filePath, int WorksheetNumber)
        {
            var punchs = new List<Punch>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var PunchTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the third worksheet
                for (int row = 2; row <= PunchTable.Dimension.End.Row; row++)
                {

                    if (string.IsNullOrWhiteSpace(PunchTable.Cells[row, 2].Value?.ToString()))
                    {
                        continue;
                    }
                    var punch = new Punch
                    {
                        PunchTitle = PunchTable.Cells[row, 2].Value.ToString(),
                        PunchModel = PunchTable.Cells[row, 3].Value.ToString(),
                        QualityFactor = PunchTable?.Cells[row, 5].Value?.ToString(),


                    };
                    var V_IsDefault = PunchTable.Cells[row, 6].Value;
                    punch.IsDefault = V_IsDefault != null ? Convert.ToBoolean(V_IsDefault) : false;

                    float punchFee;
                    ExcelHelper.TryConvertToFloat(PunchTable.Cells[row, 4].Value, out punchFee, "en-US", "de-DE");
                    punch.PunchFee = punchFee;



                    punchs.Add(punch);
                }



                return new ExcelTablesResultDto
                {
                    Punchs = punchs

                };
            }
        }
        public class ExcelTablesResultDto
        {
            public List<Punch> Punchs { get; set; }
        }
    }
}
