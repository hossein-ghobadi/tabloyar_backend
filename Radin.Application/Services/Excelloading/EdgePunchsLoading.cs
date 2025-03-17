using OfficeOpenXml;
using Radin.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Excelloading
{
    public class EdgePunchsLoading
    {
        public ExcelTablesResultDto ReadDataFromExcel(string filePath, int WorksheetNumber)
        {
            var edgePunchs = new List<EdgePunch>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var EdgePunchTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the third worksheet
                for (int row = 2; row <= EdgePunchTable.Dimension.End.Row; row++)
                {
                    if (string.IsNullOrWhiteSpace(EdgePunchTable.Cells[row, 2].Value?.ToString()))
                    {
                        continue;
                    }
                    var edgePunch = new EdgePunch
                    {
                        EdgePunchTitle = EdgePunchTable.Cells[row, 2].Value.ToString(),
                        EdgePunchModel = EdgePunchTable.Cells[row, 3].Value.ToString(),
                        QualityFactor = EdgePunchTable.Cells[row, 5].Value.ToString(),

                    };
                    var V_IsDefault = EdgePunchTable.Cells[row, 6].Value;
                    edgePunch.IsDefault = V_IsDefault != null ? Convert.ToBoolean(V_IsDefault) : false;


                    float edgePunchFee;
                    ExcelHelper.TryConvertToFloat(EdgePunchTable.Cells[row, 4].Value, out edgePunchFee, "en-US", "de-DE");
                    edgePunch.EdgePunchFee = edgePunchFee;

                    edgePunchs.Add(edgePunch);
                }



                return new ExcelTablesResultDto
                {
                    EdgePunchs = edgePunchs

                };
            }
        }
        public class ExcelTablesResultDto
        {
            public List<EdgePunch> EdgePunchs { get; set; }
        }

    }
}
