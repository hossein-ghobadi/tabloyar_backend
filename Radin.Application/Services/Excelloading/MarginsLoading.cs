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
    public class MarginsLoading
    {

        public ExcelTablesResultDto ReadDataFromExcel(string filePath, int WorksheetNumber)
        {
            var margins = new List<Margin>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var MarginTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the third worksheet
                for (int row = 2; row <= MarginTable.Dimension.End.Row; row++)
                {
                    if (string.IsNullOrWhiteSpace(MarginTable.Cells[row, 2].Value?.ToString()))
                    {
                        continue;
                    }
                    var margin = new Margin
                    {
                        MarginTitle = MarginTable.Cells[row, 2].Value.ToString(),
                       

                    };
                    var V_IsDefault = MarginTable.Cells[row,4].Value;
                    margin.IsDefault = V_IsDefault != null ? Convert.ToBoolean(V_IsDefault) : false;


                    float marginNumber;
                    ExcelHelper.TryConvertToFloat(MarginTable.Cells[row, 3].Value, out marginNumber, "en-US", "de-DE");
                    margin.MarginNumber = marginNumber;

                    margins.Add(margin);
                }



                return new ExcelTablesResultDto
                {
                    Margins = margins

                };
            }
        }
        public class ExcelTablesResultDto
        {
            public List<Margin> Margins { get; set; }
        }
    }
}
