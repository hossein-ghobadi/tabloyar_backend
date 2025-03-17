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
    public class ColorCostsLoading
    {
        public ExcelTablesResultDto ReadDataFromExcel(string filePath, int WorksheetNumber)
        {
            var colorCosts = new List<ColorCost>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var ColorCostsTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the third worksheet
                for (int row = 2; row <= ColorCostsTable.Dimension.End.Row; row++)
                {
                    if (string.IsNullOrWhiteSpace(ColorCostsTable.Cells[row, 2].Value?.ToString()))
                    {
                        continue;
                    }
                    var colorCost = new ColorCost
                    {
                        Title = ColorCostsTable.Cells[row, 2].Value.ToString(),

                    };
                    
                    float colorHardness;
                    ExcelHelper.TryConvertToFloat(ColorCostsTable.Cells[row, 3].Value, out colorHardness, "en-US", "de-DE");
                    colorCost.ColorHardness = colorHardness;

                    float colorFee1;
                    ExcelHelper.TryConvertToFloat(ColorCostsTable.Cells[row, 4].Value, out colorFee1, "en-US", "de-DE");
                    colorCost.ColorFee1 = colorFee1;

                    float colorFee2;
                    ExcelHelper.TryConvertToFloat(ColorCostsTable.Cells[row, 5].Value, out colorFee2, "en-US", "de-DE");
                    colorCost.ColorFee2 = colorFee2;



                    colorCosts.Add(colorCost);





                    
                }



                return new ExcelTablesResultDto
                {
                    ColorCosts = colorCosts

                };
            }
        }
        public class ExcelTablesResultDto
        {
            public List<ColorCost> ColorCosts { get; set; }
        }
        }
}
