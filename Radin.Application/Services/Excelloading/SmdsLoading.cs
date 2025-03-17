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
    public class SmdsLoading
    {
        public ExcelTablesResultDto ReadDataFromExcel(string filePath, int WorksheetNumber)
        {
            var smds = new List<Smd>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var SmdsTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the third worksheet
                for (int row = 2; row <= SmdsTable.Dimension.End.Row; row++)
                {
                    if (string.IsNullOrWhiteSpace(SmdsTable.Cells[row, 2].Value?.ToString()))
                    {
                        continue;
                    }
                    var smd = new Smd
                    {
                        SmdTitle = SmdsTable.Cells[row, 2].Value.ToString(),
                        SmdModel = SmdsTable.Cells[row, 3].Value.ToString(),
                        SmdColor = SmdsTable.Cells[row, 4].Value?.ToString(),
                        SmdSecondColor = SmdsTable.Cells[row, 5].Value?.ToString(),
                        QualityFactor = SmdsTable.Cells[row, 10].Value.ToString(),

                    };
                    var V_IsDefault = SmdsTable.Cells[row, 11].Value;
                    smd.IsDefault = V_IsDefault != null ? Convert.ToBoolean(V_IsDefault) : false;

                    float smdFee;
                    ExcelHelper.TryConvertToFloat(SmdsTable.Cells[row, 6].Value, out smdFee, "en-US", "de-DE");
                    smd.SmdFee = smdFee;

                    float smdWorkerFee;
                    ExcelHelper.TryConvertToFloat(SmdsTable.Cells[row, 7].Value, out smdWorkerFee, "en-US", "de-DE");
                    smd.SmdWorkerFee = smdWorkerFee;

                    float fSmdGoldNumber;
                    ExcelHelper.TryConvertToFloat(SmdsTable.Cells[row, 8].Value, out fSmdGoldNumber, "en-US", "de-DE");
                    smd.FSmdGoldNumber = fSmdGoldNumber;

                    float bSmdGoldNumber;
                    ExcelHelper.TryConvertToFloat(SmdsTable.Cells[row, 9].Value, out bSmdGoldNumber, "en-US", "de-DE");
                    smd.BSmdGoldNumber = bSmdGoldNumber;



                    smds.Add(smd);
                }



                return new ExcelTablesResultDto
                {
                    Smds = smds

                };
            }
        }
        public class ExcelTablesResultDto
        {
            public List<Smd> Smds { get; set; }
        }
    }
}
