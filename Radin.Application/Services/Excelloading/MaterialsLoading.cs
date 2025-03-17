using OfficeOpenXml;
using Radin.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Radin.Application.Services.Excelloading;



    public class MaterialsLoading
    {
        public ExcelTablesResultDto ReadDataFromExcel(string filePath, int WorksheetNumber)
        {

            var materials = new List<Material>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {


                ////////////////////////////////////////////////////////____________________________________________________________________________________________________
                var MaterialTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the first worksheet
                for (int row = 2; row <= MaterialTable.Dimension.End.Row; row++)
                {
                if (string.IsNullOrWhiteSpace(MaterialTable.Cells[row, 3].Value?.ToString()))
                {
                    continue;
                }
                var material = new Material
                    {
                        //Id = Convert.ToInt32(MaterialTable.Cells[row, 0].Value.ToString()),
                        MaterialName = MaterialTable?.Cells[row, 2].Value?.ToString(),
                        QualityFactor = MaterialTable.Cells[row, 3].Value.ToString(),
                        MaterialColor = MaterialTable?.Cells[row, 4].Value?.ToString(),

                    };
                    // nullable float
                    //......................................................................................................
                    var V_MaterialThickness = MaterialTable.Cells[row, 5].Value;
                    if (V_MaterialThickness == null || string.IsNullOrWhiteSpace(V_MaterialThickness.ToString()))
                    {
                        material.MaterialThickness = null;
                    }
                    else
                    {
                        // Proceed with conversion
                        ExcelHelper.TryConvertToFloat(V_MaterialThickness, out float materialThickness, "en-US", "de-DE");
                        material.MaterialThickness = materialThickness;
                    }
                    //non nullable float
                    //......................................................................................................
                    float materialSizeX;
                    ExcelHelper.TryConvertToFloat(MaterialTable.Cells[row, 6].Value, out materialSizeX, "en-US", "de-DE");
                    material.MaterialSizeX = materialSizeX;
                    //......................................................................................................
                    float materialSizeY;
                    ExcelHelper.TryConvertToFloat(MaterialTable.Cells[row, 7].Value, out materialSizeY, "en-US", "de-DE");
                    material.MaterialSizeY = materialSizeY;
                    //......................................................................................................
                    float materialFee;
                    ExcelHelper.TryConvertToFloat(MaterialTable.Cells[row, 8].Value, out materialFee, "en-US", "de-DE"); // Ensure this is the correct column
                    material.MaterialFee = materialFee;
                    //......................................................................................................
                    materials.Add(material);
                }



                return new ExcelTablesResultDto
                {
                    Materials = materials

                };
            }
        }


    }

        public class ExcelTablesResultDto
        {
            public List<Material> Materials { get; set; }

        }
   
