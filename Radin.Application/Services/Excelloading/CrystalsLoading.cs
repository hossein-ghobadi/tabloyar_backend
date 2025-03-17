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
    public class CrystalsLoading
    {
        public ExcelTablesResultDto ReadDataFromExcel(string filePath, int WorksheetNumber)
        {
            var crystals = new List<Crystal>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var CrystalsTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the third worksheet
                for (int row = 2; row <= CrystalsTable.Dimension.End.Row; row++)
                {
                    if (string.IsNullOrWhiteSpace(CrystalsTable.Cells[row, 2].Value?.ToString()))
                    {
                        continue;
                    }
                    var crystal = new Crystal
                    {
                        CrystalModel = CrystalsTable.Cells[row, 2].Value.ToString(),
                        CrystalColor = CrystalsTable.Cells[row, 3].Value?.ToString(),
                        QualityFactor = CrystalsTable.Cells[row, 5].Value.ToString(),



                    };
                    var V_IsDefault = CrystalsTable.Cells[row, 6].Value;
                    crystal.IsDefault = V_IsDefault != null ? Convert.ToBoolean(V_IsDefault) : false;

                    float crystalFee;
                    ExcelHelper.TryConvertToFloat(CrystalsTable.Cells[row, 4].Value, out crystalFee, "en-US", "de-DE");
                    crystal.CrystalFee = crystalFee;



                    crystals.Add(crystal);
                }



                return new ExcelTablesResultDto
                {
                    Crystals = crystals

                };
            }
        }
        public class ExcelTablesResultDto
        {
            public List<Crystal> Crystals { get; set; }
        }
    }
}
