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
    public class SecondLayerMaterialLoading
    {
        public ExcelTablesResultDto ReadDataFromExcel(string filePath, int WorksheetNumber)
        {
            var secondLayerMaterials = new List<SecondLayerMaterial>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var SecondLayerMaterialTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the third worksheet
                for (int row = 2; row <= SecondLayerMaterialTable.Dimension.End.Row; row++)
                {
                    if (string.IsNullOrWhiteSpace(SecondLayerMaterialTable.Cells[row, 2].Value?.ToString()))
                    {
                        continue;
                    }
                    var secondLayerMaterial = new SecondLayerMaterial
                    {
                        Title = SecondLayerMaterialTable.Cells[row, 2].Value.ToString(),
                        MaterialName = SecondLayerMaterialTable.Cells[row, 3].Value.ToString()


                    };
                    var V_IsDefault = SecondLayerMaterialTable.Cells[row, 4].Value;
                    secondLayerMaterial.IsDefault = V_IsDefault != null ? Convert.ToBoolean(V_IsDefault) : false;



                    secondLayerMaterials.Add(secondLayerMaterial);
                }



                return new ExcelTablesResultDto
                {
                    SecondLayerMaterials = secondLayerMaterials

                };
            }
        }
        public class ExcelTablesResultDto
        {
            public List<SecondLayerMaterial> SecondLayerMaterials { get; set; }
        }
    }
}
