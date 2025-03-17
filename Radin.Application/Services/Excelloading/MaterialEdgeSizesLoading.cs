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
    public class MaterialEdgeSizesLoading
    {
        public ExcelTablesResultDto ReadDataFromExcel(string filePath, int WorksheetNumber)
        {
            var materialEdgeSizes = new List<MaterialEdgeSize>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var MaterialEdgeSizesTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the third worksheet
                for (int row = 2; row <= MaterialEdgeSizesTable.Dimension.End.Row; row++)
                {

                    if (string.IsNullOrWhiteSpace(MaterialEdgeSizesTable.Cells[row, 2].Value?.ToString()))
                    {
                        continue;
                    }
                    var materialEdgeSize = new MaterialEdgeSize
                    {
                        Title = MaterialEdgeSizesTable.Cells[row, 2].Value.ToString(),

                    };
                    var V_IsDefault = MaterialEdgeSizesTable.Cells[row, 4].Value;
                    materialEdgeSize.IsDefault = V_IsDefault != null ? Convert.ToBoolean(V_IsDefault) : false;

                    float edgeSize;
                    ExcelHelper.TryConvertToFloat(MaterialEdgeSizesTable.Cells[row, 3].Value, out edgeSize, "en-US", "de-DE");
                    materialEdgeSize.EdgeSize = edgeSize;



                    materialEdgeSizes.Add(materialEdgeSize);
                }



                return new ExcelTablesResultDto
                {
                    MaterialEdgeSizes = materialEdgeSizes

                };
            }
        }
        public class ExcelTablesResultDto
        {
            public List<MaterialEdgeSize> MaterialEdgeSizes { get; set; }
        }
    }
}
